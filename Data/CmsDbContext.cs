using Microsoft.EntityFrameworkCore;
using wppSeventh.Services.Cms.Models;

namespace wppSeventh.Services.Cms.Data
{
    public class CmsDbContext : DbContext
    {
        public CmsDbContext(DbContextOptions<CmsDbContext> options)
           : base(options)
        {
        }

        /// <summary>
        /// CMS data
        /// </summary>
        public DbSet<ArticleModel> Articles { get; set; }
        public DbSet<ArticleChannelModel> Channels { get; set; }
        public DbSet<ArticleCategoryModel> Categories { get; set; }
        public DbSet<ArticleSubCategoryModel> SubCategories { get; set; }
        public DbSet<ImageModel> Images { get; set; }
        public DbSet<AuthorModel> Author { get; set; }
    }
}
