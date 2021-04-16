using System.Threading.Tasks;
using wppSeventh.Services.Cms.Models;

namespace wppSeventh.Services.Cms.Interfaces
{
    public interface ICmsServices
    {
        /// <summary>
        /// Get an article by channel name.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="docName"></param>
        /// <param name="channelName"></param>
        /// <param name="categoryName"></param>
        /// <param name="subCategoryName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        ArticleViewModel GetArticleByChannelName(string ownerId, string docName, string channelName, string categoryName, string subCategoryName, string culture);
        
        /// <summary>
        /// Get articles by channel name.
        /// </summary>
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
        PaginationModel<ArticleViewModel> GetArticlesByChannelPermaName(string ownerId, string channelName, string categoryName, string subCategoryName, string authorName, string keyword, string culture, int currentPage = 1, int itemsPerPage = 10);
        
        /// <summary>
        /// Get articles by channel name (for admin).
        /// </summary>
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
        PaginationModel<ArticleViewModel> GetArticlesByChannelPermaNameAdmin(string ownerId, string channelName, string categoryName, string subCategoryName, string authorName, string keyword, string culture, int currentPage = 1, int itemsPerPage = 10);
        
        /// <summary>
        /// Get author.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="permaName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        AuthorViewModel GetAuthor(string ownerId, string permaName, string culture);
        
        /// <summary>
        /// Get author (admin).
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="permaName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        AuthorViewModel GetAuthorForAdmin(string ownerId, string permaName, string culture);
        
        /// <summary>
        /// Get authors.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="keyword"></param>
        /// <param name="culture"></param>
        /// <param name="curretPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        PaginationModel<AuthorViewModel> GetAuthors(string ownerId, string keyword, string culture, int curretPage = 1, int itemsPerPage = 50);
        
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
        PaginationModel<ArticleCategoryViewModel> GetCategories(string ownerId, string channelName, string keyword, string culture, int curretPage = 1, int itemsPerPage = 50);
        
        /// <summary>
        /// Get category.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="permaName"></param>
        /// <param name="channelName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        ArticleCategoryViewModel GetCategory(string ownerId, string permaName, string channelName, string culture);
        
        /// <summary>
        /// Get category (admin).
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="permaName"></param>
        /// <param name="channelName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        ArticleCategoryModel GetCategoryAdmin(string ownerId, string permaName, string channelName, string culture);
        
        /// <summary>
        /// Get channel.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="permaName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        ArticleChannelViewModel GetChannel(string ownerId, string permaName, string culture);
        
        /// <summary>
        /// Get channel (admin).
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="permaName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        ArticleChannelViewModel GetChannelAdmin(string ownerId, string permaName, string culture);
        
        /// <summary>
        /// Get channels.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="keyword"></param>
        /// <param name="culture"></param>
        /// <param name="curretPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        PaginationModel<ArticleChannelViewModel> GetChannels(string ownerId, string keyword, string culture, int curretPage = 1, int itemsPerPage = 50);
        
        /// <summary>
        /// Get sub categories.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="channelName"></param>
        /// <param name="categoryName"></param>
        /// <param name="keyword"></param>
        /// <param name="culture"></param>
        /// <param name="curretPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        PaginationModel<ArticleSubCategoryViewModel> GetSubCategories(string ownerId, string channelName, string categoryName, string keyword, string culture, int curretPage = 1, int itemsPerPage = 50);
        
        /// <summary>
        /// Get sub category.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="permaName"></param>
        /// <param name="channelName"></param>
        /// <param name="categoryName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        
        ArticleSubCategoryViewModel GetSubCategory(string ownerId, string permaName, string channelName, string categoryName, string culture);
        
        /// <summary>
        /// Get sub category (admin).
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="permaName"></param>
        /// <returns></returns>
        ArticleSubCategoryModel GetSubCategoryAdmin(string ownerId, string permaName);
        
        /// <summary>
        /// Upsert article.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<UpsertResponseModel<ArticleModel>> UpsertArticleAsync(ArticleModel item);
        
        /// <summary>
        /// Upsert category.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<UpsertResponseModel<ArticleCategoryModel>> UpsertArticleCategoryAsync(ArticleCategoryModel item);
        
        /// <summary>
        /// Upsert channel.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<UpsertResponseModel<ArticleChannelModel>> UpsertArticleChannelAsync(ArticleChannelModel item);
        
        /// <summary>
        /// Upsert sub category.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<UpsertResponseModel<ArticleSubCategoryModel>> UpsertArticleSubCategoryAsync(ArticleSubCategoryModel item);
        
        /// <summary>
        /// Upsert author.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<UpsertResponseModel<AuthorModel>> UpsertAuthorAsync(AuthorModel item);
    }
}