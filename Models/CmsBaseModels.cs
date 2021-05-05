using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wppSeventh.Services.Cms.Models
{
    class CmsBaseModels
    {
    }
    /// <summary>
    /// For data upsert response.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UpsertResponseModel<T>
    {
        public string ResponseStatus { get; set; }
        public List<ErrorModel> Errors { get; set; }
        public T Item { get; set; }
    }

    /// <summary>
    /// Error.
    /// </summary>
    public class ErrorModel
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }

    /// <summary>
    /// Base, common model definition.
    /// </summary>
    public class ArticleBaseModel
    {
        [MaxLength(64)]
        public string OwnerId { get; set; }
        public DateTime Modified { get; set; }
        public DateTime Created { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPublished { get; set; }
       
    }

    /// <summary>
    /// Pagination.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginationModel<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalItems { get; set; }
        public List<T> Items { get; set; }
        public string Keyword { get; set; }

        public List<ErrorModel> Errors { get; set; }
    }
}
