using LanguageCenter.Infrastructure.Data.Repository.Contracts;

namespace LanguageCenter.Infrastructure.Data.Repository.ApplicationRepository
{
    public class ApplicationRepository : Repository, IApplicationRepository
    {
        public ApplicationRepository(ApplicationDbContext context)
        {
            Context = context;
        }
    }
}
