using System;
using System.Collections.Generic;
using System.Text;
using SeventhData;
using SeventhData.Util.Helpers;
using wppSeventh.DataAccess.Data;
using System.Linq;
using wppSeventh.DataAccess.Models;
using wppSeventh.Services.Cms.Models;
using wppSeventh.Services.Cms.Data;
using System.Threading.Tasks;
using wppSeventh.Services.Cms.Interfaces;

namespace wppSeventh.Services.Cms
{
    /// <summary>
    /// CMS service class.
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    class CmsServices : ICmsServices
    {
        private readonly CmsDbContext _db;

        public CmsServices(CmsDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Get artciels by channel.
        /// </summary>
        /// <remarks>
        /// ownerId and channelName (permaName) is required.
        /// </remarks>
        /// <param name="ownerId"></param>
        /// <param name="channelName"></param>
        /// <param name="categoryName"></param>
        /// <param name="subCategoryName"></param>
        /// <param name="authorName"></param>
        /// <param name="keyword"></param>
        /// <param name="culture"></param>
        /// <param name="currentPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        public PaginationModel<ArticleViewModel> GetArticlesByChannelPermaName(
            string ownerId,
            string channelName,
            string categoryName,
            string subCategoryName,
            string authorName,
            string keyword,
            string culture,
            int currentPage = 1,
            int itemsPerPage = 10)
        {
            // Get list of valid articles
            var now = DateTimeHelpers.GetUnixTime(DateTime.Now);
            var items = from a in _db.Articles
                        join ch in _db.Channels on a.ChannelId equals ch.Id
                        join ca in _db.Categories on a.CategoryId equals ca.Id
                        join sc in _db.SubCategories on a.SubCategoryId equals sc.Id
                        join au in _db.Author on a.AuthorId equals au.Id

                        where
                        a.PublishUnixtime < now &&
                        a.ExpireUnixtime > now &&
                        a.Culture == culture &&
                        ch.PermaName == channelName &&
                        a.OwnerId == ownerId

                        select new ArticleViewModel()
                        {
                            Id = a.Id,
                            AuthorImageUrl = au.IconImageUrl,
                            AuthorId = a.AuthorId,
                            AuthorIntroduction = au.Description,
                            AuthorName = au.Name,
                            CategoryId = a.CategoryId,
                            CategoryName = ca.Name,
                            ChannelId = a.ChannelId,
                            ChannelName = ch.Name,
                            Culture = a.Culture,
                            Description = a.Description,
                            Expire = a.Expire,
                            ExpireUnixtime = a.ExpireUnixtime,
                            ImageId = a.ImageId,
                            ImageUrl = a.ImageUrl,
                            PermaName = a.PermaName,
                            Publish = a.Publish,
                            PublishUnixtime = a.PublishUnixtime,
                            SubCategoryId = a.SubCategoryId,
                            SubCategoryName = sc.Name,
                            Tags = a.Tags,
                            Text = a.Text,
                            Title = a.Title,
                            Url = a.Url
                        };

            // Get category, search by category if specified
            var category = new ArticleCategoryModel();
            if (!string.IsNullOrEmpty(categoryName))
            {
                category = (from r in _db.Categories where r.PermaName == categoryName && r.ChannelId == channel.Id select r).FirstOrDefault();
                items = items.Where(a => a.CategoryId == category.Id);
            }

            // Get sub category, search by sub category if specified
            var subCategory = new ArticleSubCategoryModel();
            if (!string.IsNullOrEmpty(subCategoryName))
            {
                subCategory = (from s in _db.SubCategories where s.PermaName == subCategoryName && s.ChannelId == channel.Id select s).FirstOrDefault();
                items = items.Where(a => a.SubCategoryId == subCategory.Id);
            }

            // Get author, search by author if specified
            var author = new AuthorModel();
            if (!string.IsNullOrEmpty(authorName))
            {
                author = (from a in _db.Author where a.PermaName == authorName && a.OwnerId == ownerId select a).FirstOrDefault();
                items = items.Where(a => a.AuthorId == author.Id);
            }

            // Search by keyword
            if (!string.IsNullOrEmpty(keyword))
            {
                items = items.Where(a => a.Title.Contains(keyword));
            }

            int totalItems = items.Count();
            int totalPages = 0;

            if (totalItems > 0)
            {
                totalPages = (totalItems / itemsPerPage) + 1;
            }

            // Order
            items = items.OrderByDescending(a => a.PublishUnixtime);

            items.Skip((currentPage - 1) * itemsPerPage);
            items.Take(itemsPerPage);

            return new PaginationModel<ArticleViewModel>()
            {
                Items = items.ToList(),
                CurrentPage = currentPage,
                ItemsPerPage = itemsPerPage,
                TotalItems = totalItems,
                TotalPages = totalPages
            };
        }

        /// <summary>
        /// Get articles by channel name for admin.
        /// </summary>
        /// <remarks>
        /// For admin, it shows everything, including delted.
        /// </remarks>
        /// <param name="ownerId"></param>
        /// <param name="channelName"></param>
        /// <param name="categoryName"></param>
        /// <param name="subCategoryName"></param>
        /// <param name="authorName"></param>
        /// <param name="keyword"></param>
        /// <param name="culture"></param>
        /// <param name="currentPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        public PaginationModel<ArticleViewModel> GetArticlesByChannelPermaNameAdmin(
            string ownerId,
            string channelName,
            string categoryName,
            string subCategoryName,
            string authorName,
            string keyword,
            string culture,
            int currentPage = 1,
            int itemsPerPage = 10)
        {
            // Get list of valid articles
            var now = DateTimeHelpers.GetUnixTime(DateTime.Now);
            var items = from a in _db.Articles
                        join ch in _db.Channels on a.ChannelId equals ch.Id
                        join ca in _db.Categories on a.CategoryId equals ca.Id
                        join sc in _db.SubCategories on a.SubCategoryId equals sc.Id
                        join au in _db.Author on a.AuthorId equals au.Id

                        where
                        ch.PermaName == channelName &&
                        a.OwnerId == ownerId &&
                        a.Culture == culture

                        select new ArticleViewModel()
                        {
                            Id = a.Id,
                            AuthorImageUrl = au.IconImageUrl,
                            AuthorId = a.AuthorId,
                            AuthorIntroduction = au.Description,
                            AuthorName = au.Name,
                            CategoryId = a.CategoryId,
                            CategoryName = ca.Name,
                            ChannelId = a.ChannelId,
                            ChannelName = ch.Name,
                            Culture = a.Culture,
                            Description = a.Description,
                            Expire = a.Expire,
                            ExpireUnixtime = a.ExpireUnixtime,
                            ImageId = a.ImageId,
                            ImageUrl = a.ImageUrl,
                            PermaName = a.PermaName,
                            Publish = a.Publish,
                            PublishUnixtime = a.PublishUnixtime,
                            SubCategoryId = a.SubCategoryId,
                            SubCategoryName = sc.Name,
                            Tags = a.Tags,
                            Text = a.Text,
                            Title = a.Title,
                            Url = a.Url
                        };

            // Get category, search by category if specified
            var category = new ArticleCategoryModel();
            if (!string.IsNullOrEmpty(categoryName))
            {
                category = (from r in _db.Categories where r.PermaName == categoryName && r.ChannelId == channel.Id select r).FirstOrDefault();
                items = items.Where(a => a.CategoryId == category.Id);
            }

            // Get sub category, search by sub category if specified
            var subCategory = new ArticleSubCategoryModel();
            if (!string.IsNullOrEmpty(subCategoryName))
            {
                subCategory = (from s in _db.SubCategories where s.PermaName == subCategoryName && s.ChannelId == channel.Id select s).FirstOrDefault();
                items = items.Where(a => a.SubCategoryId == subCategory.Id);
            }

            // Get author, search by author if specified
            var author = new AuthorModel();
            if (!string.IsNullOrEmpty(authorName))
            {
                author = (from a in _db.Author where a.PermaName == authorName && a.OwnerId == ownerId select a).FirstOrDefault();
                items = items.Where(a => a.AuthorId == author.Id);
            }

            // Search by keyword
            if (!string.IsNullOrEmpty(keyword))
            {
                items = items.Where(a => a.Title.Contains(keyword));
            }

            int totalItems = items.Count();
            int totalPages = 0;

            if (totalItems > 0)
            {
                totalPages = (totalItems / itemsPerPage) + 1;
            }

            // Order
            items = items.OrderByDescending(a => a.PublishUnixtime);

            items.Skip((currentPage - 1) * itemsPerPage);
            items.Take(itemsPerPage);

            return new PaginationModel<ArticleViewModel>()
            {
                Items = items.ToList(),
                CurrentPage = currentPage,
                ItemsPerPage = itemsPerPage,
                TotalItems = totalItems,
                TotalPages = totalPages
            };
        }

        /// <summary>
        /// Get specfic article.
        /// </summary>
        /// <remarks>
        /// OwnerId, DocPermaName, ChannePermaName, CategoryPermaName, SubCategoryPermaName, Culture are required.
        /// </remarks>
        /// <param name="ownerId"></param>
        /// <param name="docName"></param>
        /// <param name="channelName"></param>
        /// <param name="categoryName"></param>
        /// <param name="subCategoryName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public ArticleViewModel GetArticleByChannelName(
            string ownerId,
            string docName,
            string channelName,
            string categoryName,
            string subCategoryName,
            string culture)
        {
            // Get list of valid articles
            var now = DateTimeHelpers.GetUnixTime(DateTime.Now);
            var items = from a in _db.Articles
                        join ch in _db.Channels on a.ChannelId equals ch.Id
                        join ca in _db.Categories on a.CategoryId equals ca.Id
                        join sc in _db.SubCategories on a.SubCategoryId equals sc.Id
                        join au in _db.Author on a.AuthorId equals au.Id

                        where
                        a.PublishUnixtime < now &&
                        a.ExpireUnixtime > now &&
                        a.Culture == culture &&
                        ch.PermaName == channelName &&
                        ca.PermaName == categoryName &&
                        sc.PermaName == subCategoryName &&
                        a.OwnerId == ownerId &&
                        a.PermaName == docName

                        select new ArticleViewModel()
                        {
                            Id = a.Id,
                            AuthorImageUrl = au.IconImageUrl,
                            AuthorId = a.AuthorId,
                            AuthorIntroduction = au.Description,
                            AuthorName = au.Name,
                            CategoryId = a.CategoryId,
                            CategoryName = ca.Name,
                            ChannelId = a.ChannelId,
                            ChannelName = ch.Name,
                            Culture = a.Culture,
                            Description = a.Description,
                            Expire = a.Expire,
                            ExpireUnixtime = a.ExpireUnixtime,
                            ImageId = a.ImageId,
                            ImageUrl = a.ImageUrl,
                            PermaName = a.PermaName,
                            Publish = a.Publish,
                            PublishUnixtime = a.PublishUnixtime,
                            SubCategoryId = a.SubCategoryId,
                            SubCategoryName = sc.Name,
                            Tags = a.Tags,
                            Text = a.Text,
                            Title = a.Title,
                            Url = a.Url
                        };


            return items.FirstOrDefault();
        }

        /// <summary>
        /// Upsert article.
        /// </summary>
        /// <remarks>
        /// channelId, categoryId, subCategoryId, authorId must be valid.
        /// </remarks>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<UpsertResponseModel<ArticleModel>> UpsertArticleAsync(ArticleModel item)
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

            // Validate sub category
            var subCategory = (from sc in _db.SubCategories where sc.Id == item.SubCategoryId select sc).FirstOrDefault();
            if (subCategory == null)
            {
                isValid = false;
                erros.Add(new ErrorModel()
                {
                    ErrorCode = 500,
                    ErrorMessage = "Sub category is not found"
                });
            }

            // Validate author
            var author = (from au in _db.Author where au.Id == item.AuthorId select au).FirstOrDefault();
            if (author == null)
            {
                isValid = false;
                erros.Add(new ErrorModel()
                {
                    ErrorCode = 500,
                    ErrorMessage = "Author is not found"
                });
            }

            // If valid, then excute db operation
            if (isValid)
            {
                // Insert or Update?
                var original = (from o in _db.Articles where o.Id == item.Id select o).FirstOrDefault();

                if (original == null)
                {
                    // Insert

                    // Make sure perma name is unique.
                    if (IsUniqueArticlePermaName(item.OwnerId, item.PermaName, item.Culture))
                    {
                        try
                        {
                            _db.Articles.Add(item);
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
                    // Make sure perma name is unique.
                    if (item.PermaName != original.PermaName)
                    {
                        if (IsUniqueArticlePermaName(item.PermaName, item.OwnerId))
                        {
                            try
                            {
                                _db.Articles.Update(item);
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
                            _db.Articles.Update(item);
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

            return new UpsertResponseModel<ArticleModel>()
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
        /// <param name="culture"></param>
        /// <returns></returns>
        private bool IsUniqueArticlePermaName(string ownerId, string permaName, string culture)
        {
            var articleWithPermaName = (from p in _db.Articles where p.PermaName == permaName && p.OwnerId == ownerId && p.Culture == culture select p).FirstOrDefault();

            if (articleWithPermaName == null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get channels.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="keyword"></param>
        /// <param name="culture"></param>
        /// <param name="curretPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        public PaginationModel<ArticleChannelViewModel> GetChannels(string ownerId, string keyword, string culture, int curretPage = 1, int itemsPerPage = 50)
        {
            var items =
                from c in _db.Channels
                join a in _db.Articles on c.Id equals a.ChannelId into g

                where c.OwnerId == ownerId

                select new ArticleChannelViewModel()
                {
                    Count = g.Select(g => g.Culture == culture).Count(),
                    Created = c.Created,
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
        /// Get specific channel.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="permaName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public ArticleChannelViewModel GetChannel(string ownerId, string permaName, string culture)
        {
            var item = (
                from i in _db.Channels
                join a in _db.Articles on i.Id equals a.ChannelId into g

                where i.OwnerId == ownerId && i.PermaName == permaName && i.IsDeleted == false

                select new ArticleChannelViewModel()
                {
                    Count = g.Select(g => g.Culture == culture).Count(),
                    Created = i.Created,
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
        /// Get specific channel for admin.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="permaName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public ArticleChannelViewModel GetChannelAdmin(string ownerId, string permaName, string culture)
        {
            var item = (
                from i in _db.Channels
                join a in _db.Articles on i.Id equals a.ChannelId into g

                where i.OwnerId == ownerId && i.PermaName == permaName

                select new ArticleChannelViewModel()
                {
                    Count = g.Select(g => g.Culture == culture).Count(),
                    Created = i.Created,
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
                var original = (from o in _db.Channels where o.Id == item.Id select o).FirstOrDefault();

                if (original == null)
                {
                    // Insert

                    // Make sure perma name is unique.
                    if (IsUniqueArticleChannelPermaName(item.PermaName, item.OwnerId))
                    {
                        try
                        {
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

        /// <summary>
        /// Get categories.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="channelName"></param>
        /// <param name="keyword"></param>
        /// <param name="culture"></param>
        /// <param name="curretPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        public PaginationModel<ArticleCategoryViewModel> GetCategories(string ownerId, string channelName, string keyword, string culture, int curretPage = 1, int itemsPerPage = 50)
        {
            var items =
                from c in _db.Categories
                join ch in _db.Channels on c.ChannelId equals ch.Id
                join a in _db.Articles on c.Id equals a.CategoryId into g

                where c.OwnerId == ownerId && ch.PermaName == channelName

                select new ArticleCategoryViewModel()
                {
                    ChannelId = c.ChannelId,
                    ChannelName = ch.Name,
                    ChannelPermaName = ch.PermaName,
                    Count = g.Select(g => g.Culture == culture).Count(),
                    Created = c.Created,
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
        /// <param name="channelName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public ArticleCategoryViewModel GetCategory(string ownerId, string permaName, string channelName, string culture)
        {
            var item = (
                from ca in _db.Categories
                join ch in _db.Channels on ca.ChannelId equals ch.Id
                join ar in _db.Articles on ca.Id equals ar.CategoryId into g

                where
                ca.OwnerId == ownerId &&
                ca.PermaName == permaName &&
                ca.IsDeleted == false &&
                ch.PermaName == channelName

                select new ArticleCategoryViewModel()
                {
                    ChannelId = ca.ChannelId,
                    ChannelName = ch.Name,
                    ChannelPermaName = ch.PermaName,
                    Count = g.Select(g => g.Culture == culture).Count(),
                    Created = ca.Created,
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
        /// <param name="channelName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public ArticleCategoryModel GetCategoryAdmin(string ownerId, string permaName, string channelName, string culture)
        {
            var item = (
                from ca in _db.Categories
                join ch in _db.Channels on ca.ChannelId equals ch.Id
                join ar in _db.Articles on ca.Id equals ar.CategoryId into g

                where
                ca.OwnerId == ownerId &&
                ca.PermaName == permaName &&
                ch.PermaName == channelName

                select new ArticleCategoryViewModel()
                {
                    ChannelId = ca.ChannelId,
                    ChannelName = ch.Name,
                    ChannelPermaName = ch.PermaName,
                    Count = g.Select(g => g.Culture == culture).Count(),
                    Created = ca.Created,
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

            // If valid, then excute db operation
            if (isValid)
            {
                // Insert or Update?
                var original = (from o in _db.Categories where o.Id == item.Id select o).FirstOrDefault();

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

        /// <summary>
        /// Get sub categories
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="channelName"></param>
        /// <param name="categoryName"></param>
        /// <param name="keyword"></param>
        /// <param name="culture"></param>
        /// <param name="curretPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        public PaginationModel<ArticleSubCategoryViewModel> GetSubCategories(string ownerId, string channelName, string categoryName, string keyword, string culture, int curretPage = 1, int itemsPerPage = 50)
        {
            var items =
                from c in _db.SubCategories
                join ch in _db.Channels on c.ChannelId equals ch.Id
                join ca in _db.Categories on c.CategoryId equals ca.Id
                join a in _db.Articles on c.Id equals a.SubCategoryId into g

                where
                c.OwnerId == ownerId &&
                ch.PermaName == channelName &&
                ca.PermaName == categoryName

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
        /// <param name="channelName"></param>
        /// <param name="categoryName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public ArticleSubCategoryViewModel GetSubCategory(string ownerId, string permaName, string channelName, string categoryName, string culture)
        {
            var item = (
                from sc in _db.SubCategories
                join ch in _db.Channels on sc.ChannelId equals ch.Id
                join ca in _db.Categories on sc.CategoryId equals ca.Id
                join ar in _db.Articles on sc.Id equals ar.SubCategoryId into g

                where sc.OwnerId == ownerId &&
                sc.PermaName == permaName &&
                ch.PermaName == channelName &&
                ca.PermaName == categoryName &&
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
        /// Get specific sub category for admin.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ArticleSubCategoryModel GetSubCategoryAdmin(string ownerId, string permaName)
        {
            var item = (from i in _db.SubCategories where i.OwnerId == ownerId && i.PermaName == permaName select i).FirstOrDefault();

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
                var original = (from o in _db.SubCategories where o.Id == item.Id select o).FirstOrDefault();

                if (original == null)
                {
                    // Insert

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
                from au in _db.Author
                join ar in _db.Articles on au.Id equals ar.AuthorId into g

                where au.OwnerId == ownerId

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
        public AuthorViewModel GetAuthor(string ownerId, string permaName, string culture)
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
        /// Get specific author for admin.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="permaName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public AuthorViewModel GetAuthorForAdmin(string ownerId, string permaName, string culture)
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
                var original = (from o in _db.Author where o.Id == item.Id select o).FirstOrDefault();

                if (original == null)
                {
                    // Insert
                    if (IsUniqueAuthorPermaName(item.PermaName, item.OwnerId))
                    {
                        try
                        {
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
                    // Make sure perma name is unique.
                    if (item.PermaName != original.PermaName)
                    {
                        if (IsUniqueAuthorPermaName(item.PermaName, item.OwnerId))
                        {
                            try
                            {
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
