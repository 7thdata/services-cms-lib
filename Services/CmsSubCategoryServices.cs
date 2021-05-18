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
    public class CmsSubCategoryServices : ICmsSubCategoryServices
    {
        private readonly CmsDbContext _db;

        public CmsSubCategoryServices(CmsDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Get sub categories by perma names.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="channelPermaName"></param>
        /// <param name="categoryPermaName"></param>
        /// <param name="keyword"></param>
        /// <param name="culture"></param>
        /// <param name="curretPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        public PaginationModel<ArticleSubCategoryViewModel> GetSubCategoriesByCaetgoryPermaName(string ownerId, string channelPermaName, string categoryPermaName, string keyword, string culture, int curretPage = 1, int itemsPerPage = 50)
        {
            var items =
                from c in _db.SubCategories.AsEnumerable()
                join ch in _db.Channels on c.ChannelId equals ch.Id
                join ca in _db.Categories on c.CategoryId equals ca.Id
                join a in _db.Articles on c.Id equals a.SubCategoryId into g

                where
                c.OwnerId == ownerId &&
                ch.PermaName == channelPermaName &&
                ca.PermaName == categoryPermaName && ca.IsDeleted == false

                select new ArticleSubCategoryViewModel()
                {
                    CategoryId = c.CategoryId,
                    CategoryName = ca.Name,
                    CategoryPermaName = ca.PermaName,
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

            return new PaginationModel<ArticleSubCategoryViewModel>()
            {
                Items = items.ToList(),
                CurrentPage = curretPage,
                ItemsPerPage = itemsPerPage,
                TotalItems = totalItems,
                TotalPages = totalPages
            };

        }

     
        /// <summary>
        /// Get sub categories by ids.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="channelId"></param>
        /// <param name="categoryId"></param>
        /// <param name="keyword"></param>
        /// <param name="culture"></param>
        /// <param name="curretPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        public PaginationModel<ArticleSubCategoryViewModel> GetSubCategoriesByCaetgoryId(string ownerId, string channelId, string categoryId, string keyword, string culture, int curretPage = 1, int itemsPerPage = 50)
        {
            var items =
                from c in _db.SubCategories.AsEnumerable()
                join ch in _db.Channels on c.ChannelId equals ch.Id
                join ca in _db.Categories on c.CategoryId equals ca.Id
                join a in _db.Articles on c.Id equals a.SubCategoryId into g

                where
                c.OwnerId == ownerId &&
                ch.Id == channelId &&
                ca.Id == categoryId && c.IsDeleted == false

                select new ArticleSubCategoryViewModel()
                {
                    CategoryId = c.CategoryId,
                    CategoryName = ca.Name,
                    CategoryPermaName = ca.PermaName,
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

            return new PaginationModel<ArticleSubCategoryViewModel>()
            {
                Items = items.ToList(),
                CurrentPage = curretPage,
                ItemsPerPage = itemsPerPage,
                TotalItems = totalItems,
                TotalPages = totalPages
            };

        }


        /// <summary>
        /// Get sub category
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="permaName"></param>
        /// <param name="channelPermaName"></param>
        /// <param name="categoryPermaName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public ArticleSubCategoryViewModel GetSubCategoryByPermaName(string ownerId, string permaName, string channelPermaName, string categoryPermaName, string culture)
        {
            var item = (
                from sc in _db.SubCategories.AsEnumerable()
                join ch in _db.Channels on sc.ChannelId equals ch.Id
                join ca in _db.Categories on sc.CategoryId equals ca.Id
                join ar in _db.Articles on sc.Id equals ar.SubCategoryId into g

                where sc.OwnerId == ownerId &&
                sc.PermaName == permaName &&
                ch.PermaName == channelPermaName &&
                ca.PermaName == categoryPermaName &&
                sc.IsDeleted == false

                select new ArticleSubCategoryViewModel()
                {

                    CategoryId = sc.CategoryId,
                    CategoryName = ca.Name,
                    CategoryPermaName = ca.PermaName,
                    ChannelId = sc.ChannelId,
                    ChannelName = ch.Name,
                    ChannelPermaName = ch.PermaName,
                    Count = g.Select(g => g.Culture == culture).Count(),
                    Created = sc.Created,
                    Culture = culture,
                    Description = sc.Description,
                    DisplayOrder = sc.DisplayOrder,
                    Id = sc.Id,
                    IsDeleted = sc.IsDeleted,
                    IsPublished = sc.IsPublished,
                    OwnerId = sc.OwnerId,
                    Modified = sc.Modified,
                    Name = sc.Name,
                    PermaName = sc.PermaName,
                    Title = sc.Title
                }

                ).FirstOrDefault();


            return item;
        }

        /// <summary>
        /// Get sub category
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="id"></param>
        /// <param name="channelId"></param>
        /// <param name="categoryId"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public ArticleSubCategoryViewModel GetSubCategoryById(string ownerId, string id, string channelId, string categoryId, string culture)
        {
            var item = (
                from sc in _db.SubCategories.AsEnumerable()
                join ch in _db.Channels on sc.ChannelId equals ch.Id
                join ca in _db.Categories on sc.CategoryId equals ca.Id
                join ar in _db.Articles on sc.Id equals ar.SubCategoryId into g

                where sc.OwnerId == ownerId &&
                sc.Id == id &&
                ch.Id == channelId &&
                ca.Id == categoryId &&
                sc.IsDeleted == false

                select new ArticleSubCategoryViewModel()
                {

                    CategoryId = sc.CategoryId,
                    CategoryName = ca.Name,
                    CategoryPermaName = ca.PermaName,
                    ChannelId = sc.ChannelId,
                    ChannelName = ch.Name,
                    ChannelPermaName = ch.PermaName,
                    Count = g.Select(g => g.Culture == culture).Count(),
                    Created = sc.Created,
                    Culture = culture,
                    Description = sc.Description,
                    DisplayOrder = sc.DisplayOrder,
                    Id = sc.Id,
                    IsDeleted = sc.IsDeleted,
                    IsPublished = sc.IsPublished,
                    OwnerId = sc.OwnerId,
                    Modified = sc.Modified,
                    Name = sc.Name,
                    PermaName = sc.PermaName,
                    Title = sc.Title
                }

                ).FirstOrDefault();


            return item;
        }


        /// <summary>
        /// Get sub category by name.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="permaName"></param>
        /// <param name="channelPermaName"></param>
        /// <param name="categoryPermaName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public ArticleSubCategoryViewModel GetSubCategoryByPermaNameAdmin(string ownerId, string permaName, string channelPermaName, string categoryPermaName, string culture)
        {
            var item = (
                from sc in _db.SubCategories.AsEnumerable()
                join ch in _db.Channels on sc.ChannelId equals ch.Id
                join ca in _db.Categories on sc.CategoryId equals ca.Id
                join ar in _db.Articles on sc.Id equals ar.SubCategoryId into g

                where sc.OwnerId == ownerId &&
                sc.PermaName == permaName &&
                ch.PermaName == channelPermaName &&
                ca.PermaName == categoryPermaName 

                select new ArticleSubCategoryViewModel()
                {

                    CategoryId = sc.CategoryId,
                    CategoryName = ca.Name,
                    CategoryPermaName = ca.PermaName,
                    ChannelId = sc.ChannelId,
                    ChannelName = ch.Name,
                    ChannelPermaName = ch.PermaName,
                    Count = g.Select(g => g.Culture == culture).Count(),
                    Created = sc.Created,
                    Culture = culture,
                    Description = sc.Description,
                    DisplayOrder = sc.DisplayOrder,
                    Id = sc.Id,
                    IsDeleted = sc.IsDeleted,
                    IsPublished = sc.IsPublished,
                    OwnerId = sc.OwnerId,
                    Modified = sc.Modified,
                    Name = sc.Name,
                    PermaName = sc.PermaName,
                    Title = sc.Title
                }

                ).FirstOrDefault();


            return item;
        }

        /// <summary>
        /// Get sub category
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="id"></param>
        /// <param name="channelId"></param>
        /// <param name="categoryId"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public ArticleSubCategoryViewModel GetSubCategoryByIdAdmin(string ownerId, string id, string channelId, string categoryId, string culture)
        {
            var item = (
                from sc in _db.SubCategories.AsEnumerable()
                join ch in _db.Channels on sc.ChannelId equals ch.Id
                join ca in _db.Categories on sc.CategoryId equals ca.Id
                join ar in _db.Articles on sc.Id equals ar.SubCategoryId into g

                where sc.OwnerId == ownerId &&
                sc.Id == id &&
                ch.Id == channelId &&
                ca.Id == categoryId &&
                sc.IsDeleted == false

                select new ArticleSubCategoryViewModel()
                {

                    CategoryId = sc.CategoryId,
                    CategoryName = ca.Name,
                    CategoryPermaName = ca.PermaName,
                    ChannelId = sc.ChannelId,
                    ChannelName = ch.Name,
                    ChannelPermaName = ch.PermaName,
                    Count = g.Select(g => g.Culture == culture).Count(),
                    Created = sc.Created,
                    Culture = culture,
                    Description = sc.Description,
                    DisplayOrder = sc.DisplayOrder,
                    Id = sc.Id,
                    IsDeleted = sc.IsDeleted,
                    IsPublished = sc.IsPublished,
                    OwnerId = sc.OwnerId,
                    Modified = sc.Modified,
                    Name = sc.Name,
                    PermaName = sc.PermaName,
                    Title = sc.Title
                }

                ).FirstOrDefault();


            return item;
        }

        /// <summary>
        /// Upsert sub category.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<UpsertResponseModel<ArticleSubCategoryModel>> UpsertArticleSubCategoryAsync(ArticleSubCategoryModel item)
        {
            // We assume everything is OK first.
            var isValid = true;
            var responseStatus = "Success";
            var erros = new List<ErrorModel>();

            // Validate channel
            var channel = (from ch in _db.Channels where ch.Id == item.ChannelId select ch).FirstOrDefault();
            if (channel == null)
            {
                isValid = false;
                erros.Add(new ErrorModel()
                {
                    ErrorCode = 500,
                    ErrorMessage = "Channel is not found"
                });
            }

            // Validate category
            var category = (from ca in _db.Categories where ca.Id == item.CategoryId select ca).FirstOrDefault();
            if (category == null)
            {
                isValid = false;
                erros.Add(new ErrorModel()
                {
                    ErrorCode = 500,
                    ErrorMessage = "Category is not found"
                });
            }

            // If valid, then excute db operation
            if (isValid)
            {
                // Insert or Update?
                var original = (from o in _db.SubCategories where o.Id == item.Id select o).AsNoTracking().FirstOrDefault();

                if (original == null)
                {
                    // Insert
                    item.Created = DateTime.Now;
                    item.Modified = DateTime.Now;

                    // Make sure perma name is unique.
                    if (IsUniqueArticleSubCategoryPermaName(item.PermaName, item.OwnerId, item.ChannelId, item.CategoryId))
                    {
                        try
                        {
                            _db.SubCategories.Add(item);
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
                        if (IsUniqueArticleSubCategoryPermaName(item.PermaName, item.OwnerId, item.ChannelId, item.CategoryId))
                        {
                            try
                            {
                                _db.SubCategories.Update(item);
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
                            _db.SubCategories.Update(item);
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

            return new UpsertResponseModel<ArticleSubCategoryModel>()
            {
                ResponseStatus = responseStatus,
                Item = item,
                Errors = erros
            };
        }

        /// <summary>
        /// Check to see if sub category perma name is unique.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="permaName"></param>
        /// <param name="channelId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        private bool IsUniqueArticleSubCategoryPermaName(string ownerId, string permaName, string channelId, string categoryId)
        {
            var subCategoryWithPermaName = (from p in _db.SubCategories where p.PermaName == permaName && p.OwnerId == ownerId && p.ChannelId == channelId && p.CategoryId == categoryId select p).FirstOrDefault();

            if (subCategoryWithPermaName == null)
            {
                return true;
            }

            return false;
        }

    }
}
