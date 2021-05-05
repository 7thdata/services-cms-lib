using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wppSeventh.Services.Cms.Models;

namespace wppSeventh.Services.Cms.Services.Interfaces
{
    public interface ICmsAuthorServices
    {
        /// <summary>
        /// Get authors
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="keyword"></param>
        /// <param name="culture"></param>
        /// <param name="curretPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        PaginationModel<AuthorViewModel> GetAuthors(string ownerId, string keyword, string culture, int curretPage = 1, int itemsPerPage = 50);

        /// <summary>
        /// Get author by perma name.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="permaName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        AuthorViewModel GetAuthorByPermaName(string ownerId, string permaName, string culture);

        /// <summary>
        /// Get author by perma name for admin.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="permaName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        AuthorViewModel GetAuthorForByPermaNameAdmin(string ownerId, string permaName, string culture);

        /// <summary>
        /// Upsert author.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<UpsertResponseModel<AuthorModel>> UpsertAuthorAsync(AuthorModel item);
    }
}
