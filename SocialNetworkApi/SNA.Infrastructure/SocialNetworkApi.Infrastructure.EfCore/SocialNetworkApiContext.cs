using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SocialNetworkApi.Domain.MessageAgg;
using SocialNetworkApi.Domain.UserAgg;
using SocialNetworkApi.Domain.UserRelationAgg;
using SocialNetworkApi.Infrastructure.EfCore.Mapping;

namespace SocialNetworkApi.Infrastructure.EfCore
{
    public class SocialNetworkApiContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserRelation> UserRelations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public SocialNetworkApiContext(DbContextOptions<SocialNetworkApiContext> options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Get the assembly of mapping
            Assembly assembly = typeof(UserMapping).Assembly;

            //apply all mapping to context 
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}