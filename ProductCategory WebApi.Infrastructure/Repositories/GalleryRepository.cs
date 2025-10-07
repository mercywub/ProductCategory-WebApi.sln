using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductCategory_WebApi.Domain.Interfaces;
using ProductCategory_WebApi.Domain.Models;
using ProductCategory_WebApi.Infrastructure.Datas;

namespace ProductCategory_WebApi.Infrastructure.Repositories
{
    public class GalleryRepository : GenericRepository<Gallary>, IGalleryRepository
    {
        public GalleryRepository(DBContext context) : base(context) { }
    }
}
