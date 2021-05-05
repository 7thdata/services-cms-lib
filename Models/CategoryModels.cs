using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wppSeventh.Services.Cms.Models
{
    class CategoryModels
    {
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
        public string ChannelName { get; set; }
        public string ChannelPermaName { get; set; }
        public string Culture { get; set; }
        public int Count { get; set; }
    }
}
