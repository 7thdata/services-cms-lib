using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wppSeventh.Services.Cms.Data;
using wppSeventh.Services.Cms.Services.Interfaces;

namespace wppSeventh.Services.Cms.Services
{
    public class CmsFileServices 
    {
        private readonly CmsDbContext _db;
        public CmsFileServices(CmsDbContext db)
        {
            _db = db;
        }
    }
}
