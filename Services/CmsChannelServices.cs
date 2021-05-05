using Microsoft.EntityFrameworkCore;
using SeventhData.Util.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wppSeventh.Services.Cms.Data;
using wppSeventh.Services.Cms.Models;
using wppSeventh.Services.Cms.Services.Interfaces;

namespace wppSeventh.Services.Cms
{
    public class CmsChannelServices : ICmsChannelServices
    {
        private readonly CmsDbContext _db;

        public CmsChannelServices(CmsDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Get channels by owner Id.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="keyword"></param>
        /// <param name="culture"></param>
        /// <param name="curretPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        public PaginationModel<ArticleChannelViewModel> GetChannels(string ownerId, string keyword, string culture, int curretPage = 1, int itemsPerPage = 50)
        {
            var channels = new PaginationModel<ArticleChannelViewModel>()
            {
                Errors = new List<ErrorModel>()
            };

            var items =
                from c in _db.Channels.AsEnumerable()
                join a in _db.Articles on c.Id equals a.ChannelId into g
                where c.OwnerId == ownerId

                select new ArticleChannelViewModel()
                {
                    Count = g.Count(g => g.Culture == culture),
                    Created = c.Created,
                    Culture = culture,
                    Description = c.Description,
                    DisplayOrder = c.DisplayOrder,
                    Id = c.Id,
                    IsDeleted = c.IsDeleted,
                    IsPublished = c.IsPublished,
                    Modified = c.Modified,
                    Name = c.Name,
                    OwnerId = c.OwnerId,
                    PermaName = c.PermaName,
                    Title = c.Title,
                   
                };

            if (items == null)
            {
                channels.Errors.Add(new ErrorModel()
                {
                    ErrorCode = 404,
                    ErrorMessage = "No channels."
                });
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                items = items.Where(c => c.Title.Contains(keyword));
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

            return new PaginationModel<ArticleChannelViewModel>()
            {
                Items = items.ToList(),
                CurrentPage = curretPage,
                ItemsPerPage = itemsPerPage,
                TotalItems = totalItems,
                TotalPages = totalPages
            };

        }


        /// <summary>
        /// Get specific channel by channel perma name.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="permaName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public ArticleChannelViewModel GetChannelByPermaName(string ownerId, string permaName, string culture)
        {
            var item = (
                from i in _db.Channels.AsEnumerable()
                join a in _db.Articles on i.Id equals a.ChannelId into g

                where i.OwnerId == ownerId && i.PermaName == permaName && i.IsDeleted == false

                select new ArticleChannelViewModel()
                {
                    Count = g.Select(g => g.Culture == culture).Count(),
                    Created = i.Created,
                    Culture = culture,
                    Description = i.Description,
                    DisplayOrder = i.DisplayOrder,
                    Id = i.Id,
                    IsDeleted = i.IsDeleted,
                    IsPublished = i.IsPublished,
                    Modified = i.Modified,
                    Name = i.Name,
                    OwnerId = i.OwnerId,
                    PermaName = i.PermaName,
                    Title = i.Title
                }

                ).FirstOrDefault();

            return item;
        }

        /// <summary>
        /// Get channel by channel Id.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="id"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public ArticleChannelViewModel GetChannelById(string ownerId, string id, string culture)
        {
            var item = (
                from i in _db.Channels.AsEnumerable()
                join a in _db.Articles on i.Id equals a.ChannelId into g

                where i.OwnerId == ownerId && i.Id == id && i.IsDeleted == false

                select new ArticleChannelViewModel()
                {
                    Count = g.Select(g => g.Culture == culture).Count(),
                    Created = i.Created,
                    Culture = culture,
                    Description = i.Description,
                    DisplayOrder = i.DisplayOrder,
                    Id = i.Id,
                    IsDeleted = i.IsDeleted,
                    IsPublished = i.IsPublished,
                    Modified = i.Modified,
                    Name = i.Name,
                    OwnerId = i.OwnerId,
                    PermaName = i.PermaName,
                    Title = i.Title
                }

                ).FirstOrDefault();

            return item;
        }

        /// <summary>
        /// Get specific channel by perma name for admin.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="permaName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public ArticleChannelViewModel GetChannelByPermaNameAdmin(string ownerId, string permaName, string culture)
        {
            var item = (
                from i in _db.Channels.AsEnumerable()
                join a in _db.Articles on i.Id equals a.ChannelId into g

                where i.OwnerId == ownerId && i.PermaName == permaName

                select new ArticleChannelViewModel()
                {
                    Count = g.Select(g => g.Culture == culture).Count(),
                    Created = i.Created,
                    Culture = culture,
                    Description = i.Description,
                    DisplayOrder = i.DisplayOrder,
                    Id = i.Id,
                    IsDeleted = i.IsDeleted,
                    IsPublished = i.IsPublished,
                    Modified = i.Modified,
                    Name = i.Name,
                    OwnerId = i.OwnerId,
                    PermaName = i.PermaName,
                    Title = i.Title
                }

                ).FirstOrDefault();

            return item;
        }

        /// <summary>
        /// Get channel by channel Id for admin.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="id"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public ArticleChannelViewModel GetChannelByIdAdmin(string ownerId, string id, string culture)
        {
            var item = (
                from i in _db.Channels.AsEnumerable()
                join a in _db.Articles on i.Id equals a.ChannelId into g

                where i.OwnerId == ownerId && i.Id == id

                select new ArticleChannelViewModel()
                {
                    Count = g.Select(g => g.Culture == culture).Count(),
                    Created = i.Created,
                    Culture = culture,
                    Description = i.Description,
                    DisplayOrder = i.DisplayOrder,
                    Id = i.Id,
                    IsDeleted = i.IsDeleted,
                    IsPublished = i.IsPublished,
                    Modified = i.Modified,
                    Name = i.Name,
                    OwnerId = i.OwnerId,
                    PermaName = i.PermaName,
                    Title = i.Title
                }

                ).FirstOrDefault();

            return item;
        }

        /// <summary>
        /// Upsert article channel.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<UpsertResponseModel<ArticleChannelModel>> UpsertArticleChannelAsync(ArticleChannelModel item)
        {
            // We assume everything is OK first.
            var isValid = true;
            var responseStatus = "Success";
            var erros = new List<ErrorModel>();

            // If valid, then excute db operation
            if (isValid)
            {
                // Insert or Update?
                var original = (from o in _db.Channels where o.Id == item.Id select o).AsNoTracking().FirstOrDefault();

                if (original == null)
                {
                    // Insert

                    // Make sure perma name is unique.
                    if (IsUniqueArticleChannelPermaName(item.PermaName, item.OwnerId))
                    {
                        try
                        {
                            item.Created = DateTime.Now;
                            item.Modified = DateTime.Now;
                            _db.Channels.Add(item);
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
                    // Make sure perma name is unique (if perma name is changed)
                    if (item.PermaName != original.PermaName)
                    {
                        if (IsUniqueArticleChannelPermaName(item.PermaName, item.OwnerId))
                        {
                            try
                            {
                                item.Modified = DateTime.Now;
                                _db.Channels.Update(item);
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
                            _db.Channels.Update(item);
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

            return new UpsertResponseModel<ArticleChannelModel>()
            {
                ResponseStatus = responseStatus,
                Item = item,
                Errors = erros
            };
        }

        /// <summary>
        /// Return true if permaName is unique.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="permaName"></param>
        /// <returns></returns>
        private bool IsUniqueArticleChannelPermaName(string ownerId, string permaName)
        {
            var categoryWithPermaName = (from p in _db.Channels where p.PermaName == permaName && p.OwnerId == ownerId select p).FirstOrDefault();

            if (categoryWithPermaName == null)
            {
                return true;
            }

            return false;
        }
    }
}
