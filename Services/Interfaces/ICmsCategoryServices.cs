using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wppSeventh.Services.Cms.Models;

namespace wppSeventh.Services.Cms.Services.Interfaces
{
    public interface ICmsCategoryServices 
    {
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
        PaginationModel<ArticleCategoryViewModel> GetCategoriesByChannelPermaName(string ownerId, string channelPermaName, string keyword, string culture, int curretPage = 1, int itemsPerPage = 50);

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
        PaginationModel<ArticleCategoryViewModel> GetCategoriesByChannelId(string ownerId, string channelId, string keyword, string culture, int curretPage = 1, int itemsPerPage = 50);

        /// <summary>
        /// Get category.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="permaName"></param>
        /// <param name="channelPermaName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        ArticleCategoryViewModel GetCategoryByPermaName(string ownerId, string permaName, string channelPermaName, string culture);

        /// <summary>
        /// Get category (admin).
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="permaName"></param>
        /// <param name="channelPermaName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        ArticleCategoryViewModel GetCategoryByPermaNameAdmin(string ownerId, string permaName, string channelPermaName, string culture);

        /// <summary>
        /// Get category by Id.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="id"></param>
        /// <param name="channelId"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        ArticleCategoryViewModel GetCategoryById(string ownerId, string id, string channelId, string culture);

        /// <summary>
        /// Get category by Id for admin.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="id"></param>
        /// <param name="channelId"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        ArticleCategoryViewModel GetCategoryByIdAdmin(string ownerId, string id, string channelId, string culture);

        /// <summary>
        /// Upsert category.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<UpsertResponseModel<ArticleCategoryModel>> UpsertArticleCategoryAsync(ArticleCategoryModel item);

    }
}
