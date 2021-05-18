using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wppSeventh.Services.Cms.Data;
using wppSeventh.Services.Cms.Models;
using wppSeventh.Services.Cms.Services.Interfaces;

namespace wppSeventh.Services.Cms.Services
{
    public class CmsAuthorServices : ICmsAuthorServices
    {
        private readonly CmsDbContext _db;

        public CmsAuthorServices(CmsDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Get list of authors.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="keyword"></param>
        /// <param name="culture"></param>
        /// <param name="curretPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        public PaginationModel<AuthorViewModel> GetAuthors(string ownerId, string keyword, string culture, int curretPage = 1, int itemsPerPage = 50)
        {
            var items =
                from au in _db.Author.AsEnumerable()
                join ar in _db.Articles on au.Id equals ar.AuthorId into g

                where au.OwnerId == ownerId && au.IsDeleted == false

                select new AuthorViewModel()
                {
                    AlterName = au.AlterName,
                    Counts = g.Select(g => g.Culture == culture).Count(),
                    Created = au.Created,
                    Description = au.Description,
                    IconImageUrl = au.IconImageUrl,
                    Id = au.Id,
                    IsDeleted = au.IsDeleted,
                    IsPublished = au.IsPublished,
                    Modified = au.Modified,
                    Name = au.Name,
                    OwnerId = au.OwnerId,
                    PermaName = au.PermaName
                };

            if (!string.IsNullOrEmpty(keyword))
            {
                items = items.Where(c => c.Name.Contains(keyword));
            }

            // Count
            int totalItems = items.Count();

            int totalPages = 0;

            if (totalItems > 0)
            {
                totalPages = (totalItems / itemsPerPage) + 1;

                // Shred
                items = items.Skip((curretPage - 1) * itemsPerPage);
                items = items.Take(itemsPerPage);
            }

            return new PaginationModel<AuthorViewModel>()
            {
                Items = items.ToList(),
                CurrentPage = curretPage,
                ItemsPerPage = itemsPerPage,
                TotalItems = totalItems,
                TotalPages = totalPages
            };

        }

        /// <summary>
        /// Get specific author.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="permaName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public AuthorViewModel GetAuthorByPermaName(string ownerId, string permaName, string culture)
        {
            var item = (
                from
                au in _db.Author.AsEnumerable()
                join
                ar in _db.Articles on au.Id equals ar.AuthorId into g

                where
                au.OwnerId == ownerId &&
                au.PermaName == permaName &&
                au.IsDeleted == false

                select new AuthorViewModel()
                {
                    AlterName = au.AlterName,
                    Counts = g.Select(g => g.Culture == culture).Count(),
                    Created = au.Created,
                    Description = au.Description,
                    IsDeleted = au.IsDeleted,
                    IconImageUrl = au.IconImageUrl,
                    Id = au.Id,
                    IsPublished = au.IsPublished,
                    OwnerId = au.OwnerId,
                    Modified = au.Modified,
                    Name = au.Name,
                    PermaName = au.PermaName
                }
                ).FirstOrDefault();

            return item;
        }

        /// <summary>
        /// Get specific author for admin.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="permaName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public AuthorViewModel GetAuthorForByPermaNameAdmin(string ownerId, string permaName, string culture)
        {
            var item = (
               from
               au in _db.Author
               join
               ar in _db.Articles on au.Id equals ar.AuthorId into g

               where
               au.OwnerId == ownerId &&
               au.PermaName == permaName &&
               au.IsDeleted == false

               select new AuthorViewModel()
               {
                   AlterName = au.AlterName,
                   Counts = g.Select(g => g.Culture == culture).Count(),
                   Created = au.Created,
                   Description = au.Description,
                   IsDeleted = au.IsDeleted,
                   IconImageUrl = au.IconImageUrl,
                   Id = au.Id,
                   IsPublished = au.IsPublished,
                   OwnerId = au.OwnerId,
                   Modified = au.Modified,
                   Name = au.Name,
                   PermaName = au.PermaName
               }
               ).FirstOrDefault();

            return item;
        }

        /// <summary>
        /// Upsert author.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<UpsertResponseModel<AuthorModel>> UpsertAuthorAsync(AuthorModel item)
        {
            // We assume everything is OK first.
            var isValid = true;
            var responseStatus = "Success";
            var erros = new List<ErrorModel>();

            // If valid, then excute db operation
            if (isValid)
            {
                // Insert or Update?
                var original = (from o in _db.Author where o.PermaName == item.PermaName && o.OwnerId == item.OwnerId select o).AsNoTracking().FirstOrDefault();

                if (original == null)
                {
                    item.Created = DateTime.Now;
                    item.Modified = DateTime.Now;

                    // Insert
                    if (IsUniqueAuthorPermaName(item.PermaName, item.OwnerId))
                    {
                        try
                        {
                            item.Created = DateTime.Now;
                            item.Modified = DateTime.Now;
                            
                            _db.Author.Add(item);
                            await _db.SaveChangesAsync();
                        }
                        catch (Exception e)
                        {
                            isValid = false;
                            erros.Add(new ErrorModel()
                            {
                                ErrorCode = 500,
                                ErrorMessage = e.Message
                            });
                        }
                    }
                    else
                    {
                        isValid = false;
                        erros.Add(new ErrorModel()
                        {
                            ErrorCode = 500,
                            ErrorMessage = "Perma name is not unique."
                        });
                    }
                }
                else
                {
                    // Upsert
                    item.Modified = DateTime.Now;

                    // Make sure perma name is unique.
                    if (item.PermaName != original.PermaName)
                    {
                        if (IsUniqueAuthorPermaName(item.PermaName, item.OwnerId))
                        {
                            try
                            {
                                item.Modified = DateTime.Now;

                                _db.Author.Update(item);
                                await _db.SaveChangesAsync();
                            }
                            catch (Exception e)
                            {
                                isValid = false;
                                erros.Add(new ErrorModel()
                                {
                                    ErrorCode = 500,
                                    ErrorMessage = e.Message
                                });
                            }
                        }
                        else
                        {
                            isValid = false;
                            erros.Add(new ErrorModel()
                            {
                                ErrorCode = 500,
                                ErrorMessage = "Perma name is not unique."
                            });
                        }
                    }
                    else
                    {
                        try
                        {
                            item.Modified = DateTime.Now;

                            _db.Author.Update(item);
                            await _db.SaveChangesAsync();
                        }
                        catch (Exception e)
                        {
                            isValid = false;
                            erros.Add(new ErrorModel()
                            {
                                ErrorCode = 500,
                                ErrorMessage = e.Message
                            });
                        }
                    }

                }
            }

            if (!isValid)
            {
                responseStatus = "Bad Request";
            }

            return new UpsertResponseModel<AuthorModel>()
            {
                ResponseStatus = responseStatus,
                Item = item,
                Errors = erros
            };
        }

        /// <summary>
        /// Return true if author permaname is unique.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="permaName"></param>
        /// <returns></returns>
        private bool IsUniqueAuthorPermaName(string ownerId, string permaName)
        {
            var authorPermaName = (from p in _db.Author where p.PermaName == permaName && p.OwnerId == ownerId select p).FirstOrDefault();

            if (authorPermaName == null)
            {
                return true;
            }

            return false;
        }
    }
}
