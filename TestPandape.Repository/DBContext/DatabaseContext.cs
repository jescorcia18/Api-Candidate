
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestPandape.Repository.DataModel;

namespace TestPandape.Repository.DBContext
{
    public class DatabaseContext : DbContext
    {
       

        public DbSet<CandidateDataModel> Candidates { get; set; }
        public DbSet<CandidateExperiencesDataModel> Experiences { get; set; }

        //Db SqlServer
        //private readonly IConfiguration _config;

        //databaseSQL
        //public DatabaseContext(IConfiguration config)
        //{
        //    _config = config;
        //}
        ////public DatabaseContext(DbContextOptions<DatabaseContext> options, IConfiguration config) : base(options)
        ////{
        ////    _config = config;
        ////}

        //////public DatabaseContext() : base() { }
        ////public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        ////{ }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "ApplicantDB");
            //optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultConnection"));
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CandidateDataModel>().HasAlternateKey(c => c.Email);
            base.OnModelCreating(modelBuilder);
        }
    }
}
