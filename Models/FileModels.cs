using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wppSeventh.Services.Cms.Models
{
    class FileModels
    {
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
}
