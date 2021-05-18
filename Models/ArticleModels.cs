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
        [Key, MaxLength(64)]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        
        public string Text { get; set; }
        public string MarkdownText { get; set; }
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
    /// Article view.
    /// </summary>
    public class ArticleViewModel : ArticleModel
    {
        public string ChannelName { get; set; }
        public string ChannelPermaName { get; set; }
        public string CategoryName { get; set; }
        public string CategoryPermaName { get; set; }
        public string SubCategoryName { get; set; }
        public string SubCategoryPermaName { get; set; }
        public string AuthorName { get; set; }
        public string AuthorPermaName { get; set; }
        public string AuthorImageUrl { get; set; }
        public string AuthorIntroduction { get; set; }
    }

}
