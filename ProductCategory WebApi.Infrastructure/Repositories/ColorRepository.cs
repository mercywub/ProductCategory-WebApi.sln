using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductCategory_WebApi.Domain.Interfaces;
using ProductCategory_WebApi.Domain.Models;
using ProductCategory_WebApi.Infrastructure.Datas;

namespace ProductCategory_WebApi.Infrastructure.Repositories
{
    public class ColorRepository : GenericRepository<Colors>, IColorRepository
    {
        public ColorRepository(DBContext context) : base(context) { }
    }
}
