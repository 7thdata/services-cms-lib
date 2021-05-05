using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wppSeventh.Services.Cms.Models
{
    class ChannelModels
    {
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
    /// Channel view.
    /// </summary>
    public class ArticleChannelViewModel : ArticleChannelModel
    {
        public int Count { get; set; }
        public string Culture { get; set; }
    }

}
