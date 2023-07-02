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
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRelation> UserRelations { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public SocialNetworkApiContext(DbContextOptions<SocialNetworkApiContext> options):base(options)
        {
            
        }

        public SocialNetworkApiContext() : base()
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