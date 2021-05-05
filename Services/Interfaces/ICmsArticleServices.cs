using System.Threading.Tasks;
using wppSeventh.Services.Cms.Models;

namespace wppSeventh.Services.Cms.Services.Interfaces
{
    public interface ICmsArticleServices
    {

        /// <summary>
        /// Gete articles by channel's perma name.
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
        PaginationModel<ArticleViewModel> GetArticlesByPermaName(
            string ownerId,
            string channelPermaName,
            string categoryPermaName,
            string subCategoryPermaName,
            string authorPermaName,
            string keyword,
            string culture,
            int currentPage = 1,
            int itemsPerPage = 10);

        /// <summary>
        /// Get articles by channel's perma name for admin.
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
       PaginationModel<ArticleViewModel> GetArticlesByPermaNameAdmin(
            string ownerId,
            string channelPermaName,
            string categoryPermaName,
            string subCategoryPermaName,
            string authorPermaName,
            string keyword,
            string culture,
            int currentPage = 1,
            int itemsPerPage = 10);

        /// <summary>
        /// Get article by article's perma name.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="channelPermaName"></param>
        /// <param name="categoryPermaName"></param>
        /// <param name="subCategoryPermaName"></param>
        /// <param name="permaName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        ArticleViewModel GetArticleByPermaName(
            string ownerId,
            string channelPermaName,
            string categoryPermaName,
            string subCategoryPermaName,
            string permaName,
            string culture);

        /// <summary>
        /// Get article by id.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        ArticleViewModel GetArticleById(string ownerId, string id);

        /// <summary>
        /// Upsert article.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<UpsertResponseModel<ArticleModel>> UpsertArticleAsync(ArticleModel item);

        /// <summary>
        /// Delete article data (physically).
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ArticleModel> DeletePhysicalArticleAsync(string id);

        /// <summary>
        /// Delete article data (with flag, logical delete)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ArticleModel> DeleteLogicalArticleAsync(string id);
    }
}