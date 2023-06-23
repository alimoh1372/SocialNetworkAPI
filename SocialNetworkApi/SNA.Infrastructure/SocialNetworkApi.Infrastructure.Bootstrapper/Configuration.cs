using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SocialNetworkApi.Application;
using SocialNetworkApi.Application.Contracts.MessageContracts;
using SocialNetworkApi.Application.Contracts.UserContracts;
using SocialNetworkApi.Application.Contracts.UserRelationContracts;
using SocialNetworkApi.Infrastructure.EfCore;

namespace SocialNetworkApi.Infrastructure.Bootstrapper;

public class Configuration
{
    /// <summary>
    /// wire up the Social network needed injection
    /// </summary>
    /// <param name="services"></param>
    /// <param name="connectionString">to use the real database</param>
    public static void Configure(IServiceCollection services, string connectionString)
    {

        services.AddScoped<IUserApplication, UserApplication>();

        services.AddScoped<IUserRelationApplication, UserRelationApplication>();


        services.AddScoped<IMessageApplication, MessageApplication>();



        services.AddDbContext<SocialNetworkApiContext>(x => x.UseSqlServer(connectionString));
        //services.AddDbContext<SocialNetworkApiContext>(x =>
        //    x.UseInMemoryDatabase("SocialNetworkApiDb"));
    }
}