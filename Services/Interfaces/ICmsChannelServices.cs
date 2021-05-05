using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wppSeventh.Services.Cms.Models;

namespace wppSeventh.Services.Cms.Services.Interfaces
{
   
    public interface ICmsChannelServices
    {

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
        /// Get channel by channel Id.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="id"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        ArticleChannelViewModel GetChannelById(string ownerId, string id, string culture);

        /// <summary>
        /// Get channel by channel Id for admin.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="id"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        ArticleChannelViewModel GetChannelByIdAdmin(string ownerId, string id, string culture);

        /// <summary>
        /// Get channel by channel's perma name for admin.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="permaName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        ArticleChannelViewModel GetChannelByPermaNameAdmin(string ownerId, string permaName, string culture);

        /// <summary>
        /// Get channel by perma name.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="keyword"></param>
        /// <param name="culture"></param>
        /// <param name="curretPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        ArticleChannelViewModel GetChannelByPermaName(string ownerId, string permaName, string culture);


        /// <summary>
        /// Upsert channel.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<UpsertResponseModel<ArticleChannelModel>> UpsertArticleChannelAsync(ArticleChannelModel item);
    }
}
