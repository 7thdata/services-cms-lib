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
    public class CmsCategoryServices : ICmsCategoryServices
    {
        private readonly CmsDbContext _db;

        public CmsCategoryServices(CmsDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Get categories with channel's permaname.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="channelPermanName"></param>
        /// <param name="keyword"></param>
        /// <param name="culture"></param>
        /// <param name="curretPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        public PaginationModel<ArticleCategoryViewModel> GetCategoriesByChannelPermaName(string ownerId, string channelPermaName, string keyword, string culture, int curretPage = 1, int itemsPerPage = 50)
        {
            var items =
                from c in _db.Categories.AsEnumerable()
                join ch in _db.Channels on c.ChannelId equals ch.Id
                join a in _db.Articles on c.Id equals a.CategoryId into g

                where c.OwnerId == ownerId && ch.PermaName == channelPermaName

                select new ArticleCategoryViewModel()
                {
                    ChannelId = c.ChannelId,
                    ChannelName = ch.Name,
                    ChannelPermaName = ch.PermaName,
                    Count = g.Select(g => g.Culture == culture).Count(),
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
                    Title = c.Title
                };

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

            return new PaginationModel<ArticleCategoryViewModel>()
            {
                Items = items.ToList(),
                CurrentPage = curretPage,
                ItemsPerPage = itemsPerPage,
                TotalItems = totalItems,
                TotalPages = totalPages
            };

        }

        /// <summary>
        /// Get categories by channel Id.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="channelId"></param>
        /// <param name="keyword"></param>
        /// <param name="culture"></param>
        /// <param name="curretPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        public PaginationModel<ArticleCategoryViewModel> GetCategoriesByChannelId(string ownerId, string channelId, string keyword, string culture, int curretPage = 1, int itemsPerPage = 50)
        {
            var items =
                from c in _db.Categories.AsEnumerable()
                join ch in _db.Channels on c.ChannelId equals ch.Id
                join a in _db.Articles on c.Id equals a.CategoryId into g

                where c.OwnerId == ownerId && ch.Id == channelId

                select new ArticleCategoryViewModel()
                {
                    ChannelId = c.ChannelId,
                    ChannelName = ch.Name,
                    ChannelPermaName = ch.PermaName,
                    Count = g.Select(g => g.Culture == culture).Count(),
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
                    Title = c.Title
                };

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

            return new PaginationModel<ArticleCategoryViewModel>()
            {
                Items = items.ToList(),
                CurrentPage = curretPage,
                ItemsPerPage = itemsPerPage,
                TotalItems = totalItems,
                TotalPages = totalPages
            };

        }

        /// <summary>
        /// Get specific category.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="permaName"></param>
        /// <param name="channelPermaName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public ArticleCategoryViewModel GetCategoryByPermaName(string ownerId, string permaName, string channelPermaName, string culture)
        {
            var item = (
                from ca in _db.Categories.AsEnumerable()
                join ch in _db.Channels on ca.ChannelId equals ch.Id
                join ar in _db.Articles on ca.Id equals ar.CategoryId into g

                where
                ca.OwnerId == ownerId &&
                ca.PermaName == permaName &&
                ca.IsDeleted == false &&
                ch.PermaName == channelPermaName

                select new ArticleCategoryViewModel()
                {
                    ChannelId = ca.ChannelId,
                    ChannelName = ch.Name,
                    ChannelPermaName = ch.PermaName,
                    Count = g.Select(g => g.Culture == culture).Count(),
                    Created = ca.Created,
                    Culture = culture,
                    Description = ca.Description,
                    DisplayOrder = ca.DisplayOrder,
                    Id = ca.Id,
                    IsDeleted = ca.IsDeleted,
                    IsPublished = ca.IsPublished,
                    Modified = ca.Modified,
                    Name = ca.Name,
                    OwnerId = ca.OwnerId,
                    PermaName = ca.PermaName,
                    Title = ca.Title
                }

                ).FirstOrDefault();

            return item;
        }

        /// <summary>
        /// Get specific category for admin,
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="permaName"></param>
        /// <param name="channelPermaName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public ArticleCategoryViewModel GetCategoryByPermaNameAdmin(string ownerId, string permaName, string channelPermaName, string culture)
        {
            var item = (
                 from ca in _db.Categories.AsEnumerable()
                 join ch in _db.Channels on ca.ChannelId equals ch.Id
                 join ar in _db.Articles on ca.Id equals ar.CategoryId into g

                 where
                 ca.OwnerId == ownerId &&
                 ca.PermaName == permaName &&
                 ch.PermaName == channelPermaName

                 select new ArticleCategoryViewModel()
                 {
                     ChannelId = ca.ChannelId,
                     ChannelName = ch.Name,
                     ChannelPermaName = ch.PermaName,
                     Count = g.Select(g => g.Culture == culture).Count(),
                     Created = ca.Created,
                     Culture = culture,
                     Description = ca.Description,
                     DisplayOrder = ca.DisplayOrder,
                     Id = ca.Id,
                     IsDeleted = ca.IsDeleted,
                     IsPublished = ca.IsPublished,
                     Modified = ca.Modified,
                     Name = ca.Name,
                     OwnerId = ca.OwnerId,
                     PermaName = ca.PermaName,
                     Title = ca.Title
                 }

                 ).FirstOrDefault();

            return item;
        }

        /// <summary>
        /// Get category by Id.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="id"></param>
        /// <param name="channelId"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public ArticleCategoryViewModel GetCategoryById(string ownerId, string id, string channelId, string culture)
        {
            var item = (
                from ca in _db.Categories.AsEnumerable()
                join ch in _db.Channels on ca.ChannelId equals ch.Id
                join ar in _db.Articles on ca.Id equals ar.CategoryId into g

                where
                ca.OwnerId == ownerId &&
                ca.Id == id &&
                ca.IsDeleted == false &&
                ch.Id == channelId

                select new ArticleCategoryViewModel()
                {
                    ChannelId = ca.ChannelId,
                    ChannelName = ch.Name,
                    ChannelPermaName = ch.PermaName,
                    Count = g.Select(g => g.Culture == culture).Count(),
                    Created = ca.Created,
                    Culture = culture,
                    Description = ca.Description,
                    DisplayOrder = ca.DisplayOrder,
                    Id = ca.Id,
                    IsDeleted = ca.IsDeleted,
                    IsPublished = ca.IsPublished,
                    Modified = ca.Modified,
                    Name = ca.Name,
                    OwnerId = ca.OwnerId,
                    PermaName = ca.PermaName,
                    Title = ca.Title
                }

                ).FirstOrDefault();

            return item;
        }

        /// <summary>
        /// Get category by Id for admin.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="id"></param>
        /// <param name="channelId"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public ArticleCategoryViewModel GetCategoryByIdAdmin(string ownerId, string id, string channelId, string culture)
        {
            var item = (
                from ca in _db.Categories.AsEnumerable()
                join ch in _db.Channels on ca.ChannelId equals ch.Id
                join ar in _db.Articles on ca.Id equals ar.CategoryId into g

                where
                ca.OwnerId == ownerId &&
                ca.Id == id &&
                ch.Id == channelId

                select new ArticleCategoryViewModel()
                {
                    ChannelId = ca.ChannelId,
                    ChannelName = ch.Name,
                    ChannelPermaName = ch.PermaName,
                    Count = g.Select(g => g.Culture == culture).Count(),
                    Created = ca.Created,
                    Culture = culture,
                    Description = ca.Description,
                    DisplayOrder = ca.DisplayOrder,
                    Id = ca.Id,
                    IsDeleted = ca.IsDeleted,
                    IsPublished = ca.IsPublished,
                    Modified = ca.Modified,
                    Name = ca.Name,
                    OwnerId = ca.OwnerId,
                    PermaName = ca.PermaName,
                    Title = ca.Title
                }

                ).FirstOrDefault();

            return item;
        }

        /// <summary>
        /// Upsert category.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<UpsertResponseModel<ArticleCategoryModel>> UpsertArticleCategoryAsync(ArticleCategoryModel item)
        {
            // We assume everything is OK first.
            var isValid = true;
            var responseStatus = "Success";
            var erros = new List<ErrorModel>();

            // Validate channel
            var channel = (from ch in _db.Channels where ch.Id == item.ChannelId select ch).AsNoTracking().FirstOrDefault();
            if (channel == null)
            {
                isValid = false;
                erros.Add(new ErrorModel()
                {
                    ErrorCode = 500,
                    ErrorMessage = "Channel is not found"
                });
            }

            // If valid, then excute db operation
            if (isValid)
            {
                // Insert or Update?
                var original = (from o in _db.Categories where o.Id == item.Id select o).AsNoTracking().FirstOrDefault();

                if (original == null)
                {
                    // Insert

                    // Make sure perma name is unique.
                    if (IsUniqueArticleCategoryPermaName(item.PermaName, item.OwnerId, item.ChannelId))
                    {
                        try
                        {
                            _db.Categories.Add(item);
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
                    if (item.PermaName != original.PermaName)
                    {
                        // Make sure perma name is unique.
                        if (IsUniqueArticleCategoryPermaName(item.PermaName, item.OwnerId, item.ChannelId))
                        {
                            try
                            {
                                _db.Categories.Update(item);
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
                            _db.Categories.Update(item);
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

            return new UpsertResponseModel<ArticleCategoryModel>()
            {
                ResponseStatus = responseStatus,
                Item = item,
                Errors = erros
            };
        }

        /// <summary>
        /// Check to see if category perma name is unique.
        /// </summary>
        /// <param name="permaName"></param>
        /// <param name="ownerId"></param>
        /// <param name="channelId"></param>
        /// <returns></returns>
        private bool IsUniqueArticleCategoryPermaName(string ownerId, string permaName, string channelId)
        {
            var categoryWithPermaName = (from p in _db.Categories where p.PermaName == permaName && p.OwnerId == ownerId && p.ChannelId == channelId select p).FirstOrDefault();

            if (categoryWithPermaName == null)
            {
                return true;
            }

            return false;
        }
    }
}
