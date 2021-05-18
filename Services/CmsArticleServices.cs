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
using wppSeventh.Services.Cms.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace wppSeventh.Services.Cms.Services
{
    /// <summary>
    /// CMS service class.
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class CmsArticleServices : ICmsArticleServices
    {
        private readonly CmsDbContext _db;

        public CmsArticleServices(CmsDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Get artciels by channel.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="channelPermaName"></param>
        /// <param name="categoryPermaName"></param>
        /// <param name="subCategoryPermaName"></param>
        /// <param name="authorPermaName"></param>
        /// <param name="keyword"></param>
        /// <param name="culture"></param>
        /// <param name="currentPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        public PaginationModel<ArticleViewModel> GetArticlesByPermaName(
            string ownerId,
            string channelPermaName,
            string categoryPermaName,
            string subCategoryPermaName,
            string authorPermaName,
            string keyword,
            string culture,
            int currentPage = 1,
            int itemsPerPage = 10)
        {

            var articles = new PaginationModel<ArticleViewModel>()
            {
                Errors = new List<ErrorModel>()
            };

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
                        a.IsDeleted == false &&
                        a.OwnerId == ownerId

                        select new ArticleViewModel()
                        {
                            Id = a.Id,
                            AuthorImageUrl = au.IconImageUrl,
                            AuthorId = a.AuthorId,
                            AuthorIntroduction = au.Description,
                            AuthorName = au.Name,
                            AuthorPermaName = au.PermaName,
                            CategoryId = a.CategoryId,
                            CategoryName = ca.Name,
                            CategoryPermaName = ca.PermaName,
                            ChannelId = a.ChannelId,
                            ChannelName = ch.Name,
                            ChannelPermaName = ch.PermaName,
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
                            SubCategoryPermaName = sc.PermaName,
                            Tags = a.Tags,
                            Text = a.Text,
                            MarkdownText = a.MarkdownText,
                            Title = a.Title,
                            Url = a.Url
                        };

            if (!items.Any())
            {
                // There is no such channel,
                articles.Errors.Add(new ErrorModel()
                {
                    ErrorCode = 404,
                    ErrorMessage = "There is no such article."
                });

                return articles;
            }

            // Get channel, search by channel if specified
            var channel = new ArticleChannelModel();
            if (!string.IsNullOrEmpty(channelPermaName))
            {
                channel = (from r in _db.Channels where r.PermaName == channelPermaName && r.OwnerId == ownerId select r).FirstOrDefault();

                if (channel == null)
                {
                    // There is no such channel,
                    articles.Errors.Add(new ErrorModel()
                    {
                        ErrorCode = 404,
                        ErrorMessage = "There is no such channel."
                    });

                    return articles;
                }

                items = items.Where(a => a.ChannelId == channel.Id);

            }

            items = items.Where(a => a.ChannelId == channel.Id);

            // Get category, search by category if specified
            var category = new ArticleCategoryModel();
            if (!string.IsNullOrEmpty(categoryPermaName))
            {
                category = (from r in _db.Categories where r.PermaName == categoryPermaName && r.ChannelId == channel.Id select r).FirstOrDefault();
                items = items.Where(a => a.CategoryId == category.Id);
            }

            // Get sub category, search by sub category if specified
            var subCategory = new ArticleSubCategoryModel();
            if (!string.IsNullOrEmpty(subCategoryPermaName))
            {
                subCategory = (from s in _db.SubCategories where s.PermaName == subCategoryPermaName && s.ChannelId == channel.Id select s).FirstOrDefault();
                items = items.Where(a => a.SubCategoryId == subCategory.Id);
            }

            // Get author, search by author if specified
            var author = new AuthorModel();
            if (!string.IsNullOrEmpty(authorPermaName))
            {
                author = (from a in _db.Author where a.PermaName == authorPermaName && a.OwnerId == ownerId select a).FirstOrDefault();
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
        /// <param name="channelPermaName"></param>
        /// <param name="categoryPermaName"></param>
        /// <param name="subCategoryPermaName"></param>
        /// <param name="authorPermaName"></param>
        /// <param name="keyword"></param>
        /// <param name="culture"></param>
        /// <param name="currentPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        public PaginationModel<ArticleViewModel> GetArticlesByPermaNameAdmin(
             string ownerId,
             string channelPermaName,
             string categoryPermaName,
             string subCategoryPermaName,
             string authorPermaName,
             string keyword,
             string culture,
             int currentPage = 1,
             int itemsPerPage = 10)
        {

            var articles = new PaginationModel<ArticleViewModel>()
            {
                Errors = new List<ErrorModel>()
            };

            // Get list of valid articles
            var now = DateTimeHelpers.GetUnixTime(DateTime.Now);
            var items = from a in _db.Articles
                        join ch in _db.Channels on a.ChannelId equals ch.Id
                        join ca in _db.Categories on a.CategoryId equals ca.Id
                        join sc in _db.SubCategories on a.SubCategoryId equals sc.Id
                        join au in _db.Author on a.AuthorId equals au.Id

                        where
                        a.Culture == culture &&
                        a.OwnerId == ownerId

                        select new ArticleViewModel()
                        {
                            Id = a.Id,
                            AuthorImageUrl = au.IconImageUrl,
                            AuthorId = a.AuthorId,
                            AuthorIntroduction = au.Description,
                            AuthorName = au.Name,
                            AuthorPermaName = au.PermaName,
                            CategoryId = a.CategoryId,
                            CategoryName = ca.Name,
                            CategoryPermaName = ca.PermaName,
                            ChannelId = a.ChannelId,
                            ChannelName = ch.Name,
                            ChannelPermaName = ch.PermaName,
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
                            SubCategoryPermaName = sc.PermaName,
                            Tags = a.Tags,
                            Text = a.Text,
                            MarkdownText = a.MarkdownText,
                            Title = a.Title,
                            Url = a.Url
                        };

            if (!items.Any())
            {
                // There is no such channel,
                articles.Errors.Add(new ErrorModel()
                {
                    ErrorCode = 404,
                    ErrorMessage = "There is no such article."
                });

                return articles;
            }

            // Get channel, search by channel if specified
            var channel = new ArticleChannelModel();
            if (!string.IsNullOrEmpty(channelPermaName))
            {
                channel = (from r in _db.Channels where r.PermaName == channelPermaName && r.OwnerId == ownerId select r).FirstOrDefault();

                if (channel == null)
                {
                    // There is no such channel,
                    articles.Errors.Add(new ErrorModel()
                    {
                        ErrorCode = 404,
                        ErrorMessage = "There is no such channel."
                    });

                    return articles;
                }

                items = items.Where(a => a.ChannelId == channel.Id);

            }

            // Get category, search by category if specified
            var category = new ArticleCategoryModel();
            if (!string.IsNullOrEmpty(categoryPermaName))
            {
                category = (from r in _db.Categories where r.PermaName == categoryPermaName && r.ChannelId == channel.Id select r).FirstOrDefault();
                items = items.Where(a => a.CategoryId == category.Id);
            }

            // Get sub category, search by sub category if specified
            var subCategory = new ArticleSubCategoryModel();
            if (!string.IsNullOrEmpty(subCategoryPermaName))
            {
                subCategory = (from s in _db.SubCategories where s.PermaName == subCategoryPermaName && s.ChannelId == channel.Id select s).FirstOrDefault();
                items = items.Where(a => a.SubCategoryId == subCategory.Id);
            }

            // Get author, search by author if specified
            var author = new AuthorModel();
            if (!string.IsNullOrEmpty(authorPermaName))
            {
                author = (from a in _db.Author where a.PermaName == authorPermaName && a.OwnerId == ownerId select a).FirstOrDefault();
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
        /// <param name="permaName"></param>
        /// <param name="channelPermaName"></param>
        /// <param name="categoryPermaName"></param>
        /// <param name="subCategoryPermaName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public ArticleViewModel GetArticleByPermaName(
            string ownerId,
            string channelPermaName,
            string categoryPermaName,
            string subCategoryPermaName,
            string permaName,
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
                        a.OwnerId == ownerId &&
                        a.PermaName == permaName &&
                        ch.PermaName == channelPermaName &&
                        ca.PermaName == categoryPermaName &&
                        sc.PermaName == subCategoryPermaName && 
                        a.IsDeleted == false

                        select new ArticleViewModel()
                        {
                            Id = a.Id,
                            AuthorImageUrl = au.IconImageUrl,
                            AuthorId = a.AuthorId,
                            AuthorIntroduction = au.Description,
                            AuthorName = au.Name,
                            AuthorPermaName = au.PermaName,
                            CategoryId = a.CategoryId,
                            CategoryName = ca.Name,
                            CategoryPermaName = ca.PermaName,
                            ChannelId = a.ChannelId,
                            ChannelName = ch.Name,
                            ChannelPermaName = ch.PermaName,
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
                            SubCategoryPermaName = sc.PermaName,
                            Tags = a.Tags,
                            Text = a.Text,
                            MarkdownText = a.MarkdownText,
                            Title = a.Title,
                            Url = a.Url
                        };


            return items.FirstOrDefault();
        }


        /// <summary>
        /// Get article by Id.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ArticleViewModel GetArticleById(string ownerId, string id)
        {

            // Get list of valid articles
            var now = DateTimeHelpers.GetUnixTime(DateTime.Now);
            var items = from a in _db.Articles
                        join ch in _db.Channels on a.ChannelId equals ch.Id
                        join ca in _db.Categories on a.CategoryId equals ca.Id
                        join sc in _db.SubCategories on a.SubCategoryId equals sc.Id
                        join au in _db.Author on a.AuthorId equals au.Id

                        where
                        a.OwnerId == ownerId &&
                        a.Id == id && a.IsDeleted == false

                        select new ArticleViewModel()
                        {
                            Id = a.Id,
                            AuthorImageUrl = au.IconImageUrl,
                            AuthorId = a.AuthorId,
                            AuthorIntroduction = au.Description,
                            AuthorName = au.Name,
                            AuthorPermaName = au.PermaName,
                            CategoryId = a.CategoryId,
                            CategoryName = ca.Name,
                            CategoryPermaName = ca.PermaName,
                            ChannelId = a.ChannelId,
                            ChannelName = ch.Name,
                            ChannelPermaName = ch.PermaName,
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
                            SubCategoryPermaName = sc.PermaName,
                            Tags = a.Tags,
                            Text = a.Text,
                            MarkdownText = a.MarkdownText,
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
                var original = (from o in _db.Articles where o.Id == item.Id select o).AsNoTracking().FirstOrDefault();

                if (original == null)
                {
                    // Insert

                    // Make sure perma name is unique.
                    if (IsUniqueArticlePermaName(item.OwnerId, item.ChannelId, item.CategoryId, item.SubCategoryId, item.PermaName, item.Culture))
                    {
                        try
                        {
                            // Set timestamp
                            item.ExpireUnixtime = DateTimeHelpers.GetUnixTime(item.Expire);
                            item.PublishUnixtime = DateTimeHelpers.GetUnixTime(item.Publish);
                            item.Modified = DateTime.Now;
                            item.Created = DateTime.Now;
                            
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
                        if (IsUniqueArticlePermaName(item.PermaName, item.ChannelId, item.CategoryId, item.SubCategoryId, item.OwnerId, item.Culture))
                        {
                            try
                            {
                                item.ExpireUnixtime = DateTimeHelpers.GetUnixTime(item.Expire);
                                item.PublishUnixtime = DateTimeHelpers.GetUnixTime(item.Publish);
                                item.Modified = DateTime.Now;
                              
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
                            item.ExpireUnixtime = DateTimeHelpers.GetUnixTime(item.Expire);
                            item.PublishUnixtime = DateTimeHelpers.GetUnixTime(item.Publish);
                            item.Modified = DateTime.Now;

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
        /// Delete article data (physically).
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ArticleModel> DeletePhysicalArticleAsync(string id)
        {
            var original = (from o in _db.Articles where o.Id == id select o).AsNoTracking().FirstOrDefault();

            _db.Articles.Remove(original);

            await _db.SaveChangesAsync();

            return original;

        }

        /// <summary>
        /// Delete article data (with flag, logical delete)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ArticleModel> DeleteLogicalArticleAsync(string id)
        {
            var original = (from o in _db.Articles where o.Id == id select o).AsNoTracking().FirstOrDefault();

            original.IsDeleted = true;
            original.Modified = DateTime.Now;

            _db.Articles.Update(original);

            await _db.SaveChangesAsync();

            return original;

        }

        /// <summary>
        /// Return true if permaName is unique.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="permaName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        private bool IsUniqueArticlePermaName(
            string ownerId, 
            string channelId, 
            string categoryId, 
            string subCategoryId, 
            string permaName, 
            string culture)
        {
            var articleWithPermaName = (from p in _db.Articles 
                                        where 
                                        p.PermaName == permaName && 
                                        p.OwnerId == ownerId && 
                                        p.ChannelId == channelId &&
                                        p.CategoryId == categoryId &&
                                        p.SubCategoryId == subCategoryId &&
                                        p.Culture == culture select p).FirstOrDefault();

            if (articleWithPermaName == null)
            {
                return true;
            }

            return false;
        }

    }
}
