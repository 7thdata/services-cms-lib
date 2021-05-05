using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wppSeventh.Services.Cms.Models;

namespace wppSeventh.Services.Cms.Services.Interfaces
{
    public interface ICmsSubCategoryServices
    {
        /// <summary>
        /// Get sub categories by category's perma name.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="channelPermaName"></param>
        /// <param name="categoryPermaName"></param>
        /// <param name="keyword"></param>
        /// <param name="culture"></param>
        /// <param name="curretPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        PaginationModel<ArticleSubCategoryViewModel> GetSubCategoriesByCaetgoryPermaName(string ownerId, string channelPermaName, string categoryPermaName, string keyword, string culture, int curretPage = 1, int itemsPerPage = 50);
        
        /// <summary>
        /// Get sub categories by category's Id.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="channelId"></param>
        /// <param name="categoryId"></param>
        /// <param name="keyword"></param>
        /// <param name="culture"></param>
        /// <param name="curretPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        PaginationModel<ArticleSubCategoryViewModel> GetSubCategoriesByCaetgoryId(string ownerId, string channelId, string categoryId, string keyword, string culture, int curretPage = 1, int itemsPerPage = 50);
        
        /// <summary>
        /// Get sub category by ca
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="permaName"></param>
        /// <param name="channelPermaName"></param>
        /// <param name="categoryPermaName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        ArticleSubCategoryViewModel GetSubCategoryByPermaName(string ownerId, string permaName, string channelPermaName, string categoryPermaName, string culture);
        
        /// <summary>
        /// Get sub 
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="id"></param>
        /// <param name="channelId"></param>
        /// <param name="categoryId"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        ArticleSubCategoryViewModel GetSubCategoryById(string ownerId, string id, string channelId, string categoryId, string culture);
        
        /// <summary>
        /// Get sub category by perma name for admin.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="permaName"></param>
        /// <param name="channelPermaName"></param>
        /// <param name="categoryPermaName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        ArticleSubCategoryViewModel GetSubCategoryByPermaNameAdmin(string ownerId, string permaName, string channelPermaName, string categoryPermaName, string culture);
        
        /// <summary>
        /// Get sub category by Id for admin.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="id"></param>
        /// <param name="channelId"></param>
        /// <param name="categoryId"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        ArticleSubCategoryViewModel GetSubCategoryByIdAdmin(string ownerId, string id, string channelId, string categoryId, string culture);


        /// <summary>
        /// Upsert sub category.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<UpsertResponseModel<ArticleSubCategoryModel>> UpsertArticleSubCategoryAsync(ArticleSubCategoryModel item);
    }
}
