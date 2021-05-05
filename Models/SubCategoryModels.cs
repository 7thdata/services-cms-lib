using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wppSeventh.Services.Cms.Models
{
    class SubCategoryModels
    {
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
        public string Culture { get; set; }
        public int Count { get; set; }
    }
}
