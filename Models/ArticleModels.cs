using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace wppSeventh.Services.Cms.Models
{
    /// <summary>
    /// Article.
    /// </summary>
    [Table("Articles")]
    public class ArticleModel : ArticleBaseModel
    {
        [Key,MaxLength(64)]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string Text { get; set; }
        public string Url { get; set; }
        public DateTime Publish { get; set; }
        public long PublishUnixtime { get; set; }
        public DateTime Expire { get; set; }
        public long ExpireUnixtime { get; set; }
        [MaxLength(64)]
        public string ImageId { get; set; }
        public string ImageUrl { get; set; }
        [Required, MaxLength(64)]

        public string ChannelId { get; set; }
        [Required, MaxLength(64)]
        public string CategoryId { get; set; }
        [Required, MaxLength(64)]
        public string SubCategoryId { get; set; }
        public string Tags { get; set; }
        [Required]
        public string AuthorId { get; set; }
        [Required]
        public string Culture { get; set; }
        [Required]
        public string PermaName { get; set; }

    }

    /// <summary>
    /// Channel.
    /// </summary>
    [Table("ArticleChennels")]
    public class ArticleChannelModel : ArticleBaseModel
    {
        [Key, MaxLength(64)]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PermaName { get; set; }
        public int DisplayOrder { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
    /// <summary>
    /// Article view.
    /// </summary>
    public class ArticleViewModel : ArticleModel
    {
        public string ChannelName { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public string AuthorName { get; set; }
        public string AuthorImageUrl { get; set; }
        public string AuthorIntroduction { get; set; }
    }

    /// <summary>
    /// Channel view.
    /// </summary>
    public class ArticleChannelViewModel : ArticleChannelModel
    {
        public int Count { get; set; }
    }

    /// <summary>
    /// Category.
    /// </summary>
    [Table("ArticleCategories")]
    public class ArticleCategoryModel : ArticleBaseModel
    {
        [Key, MaxLength(64)]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PermaName { get; set; }
        [Required, MaxLength(64)]
        public string ChannelId { get; set; }
        public int DisplayOrder { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

    }

    /// <summary>
    /// Category view.
    /// </summary>
    public class ArticleCategoryViewModel : ArticleCategoryModel
    {
        public string ChannelName {get;set;}
        public string ChannelPermaName { get; set; }
        public int Count { get; set; }
    }

    /// <summary>
    /// Sub category.
    /// </summary>
    [Table("ArticleSubCategories")]
    public class ArticleSubCategoryModel : ArticleBaseModel
    {
        [Key, MaxLength(64)]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PermaName { get; set; }
        [Required, MaxLength(64)]
        public string ChannelId { get; set; }
        [Required, MaxLength(64)]
        public string CategoryId { get; set; }
        public int DisplayOrder { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// Sub category view.
    /// </summary>
    public class ArticleSubCategoryViewModel : ArticleSubCategoryModel
    {
        public string ChannelName { get; set; }
        public string ChannelPermaName { get; set; }
        public string CategoryName { get; set; }
        public string CategoryPermaName { get; set; }
        public int Count { get; set; }
    }

    /// <summary>
    /// Image
    /// </summary>
    [Table("Images")]
    public class ImageModel : ArticleBaseModel
    {
        [Key, MaxLength(64)]
        public string Id { get; set; }
        [Required]
        public string Url { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int DisplayOrder { get; set; }
    }

    /// <summary>
    /// Author
    /// </summary>
    [Table("Author")]
    public class AuthorModel : ArticleBaseModel
    {
        [Key, MaxLength(64)]
        public string Id { get; set; }
        [Required]
        public string PermaName { get; set; }
        [Required]
        public string Name { get; set; }
        public string AlterName { get; set; }
        public string Description { get; set; }
        public string IconImageUrl { get; set; }

    }
    /// <summary>
    /// Author view.
    /// </summary>

    public class AuthorViewModel : AuthorModel
    {
        public int Counts { get; set; }
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
    }
}
