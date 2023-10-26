using TestPandape.Business.IServices;
using TestPandape.Business.Services;
using TestPandape.Entity.UriServices;
using TestPandape.Lib.IUtilities;
using TestPandape.Lib.Utilities;
using TestPandape.Repository.IRepository;
using TestPandape.Repository.Repository;

namespace TestPandape.API
{
    public static class IoC
    {
        public static IServiceCollection AddDependency(this IServiceCollection services)
        {
            services.AddScoped<ICandidateRepository, CandidateRepository>();
            services.AddScoped<IUtils, Utils>();
            services.AddScoped<ICandidateBL, CandidateBL>();
            services.AddScoped <IExperienceBL,ExperienceBL>();
            services.AddScoped <IExperienceRepository,ExperienceRepository>();
            services.AddCors();
            return services;
        }
    }
}
