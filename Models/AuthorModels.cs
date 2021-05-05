using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wppSeventh.Services.Cms.Models
{
    class AuthorModels
    {
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
        public int DisplayOrder { get; set; }

    }
    /// <summary>
    /// Author view.
    /// </summary>

    public class AuthorViewModel : AuthorModel
    {
        public int Counts { get; set; }
    }
}
