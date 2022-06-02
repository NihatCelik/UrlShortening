using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;

namespace DataAccess.Concrete.EntityFramework
{
    public class UrlRepository : EfEntityRepositoryBase<Url, ProjectDbContext>, IUrlRepository
    {
        public UrlRepository(ProjectDbContext context) : base(context)
        {
        }
    }
}
