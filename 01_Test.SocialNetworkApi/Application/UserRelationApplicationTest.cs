using Moq;
using SocialNetworkApi.Infrastructure.EfCore;

namespace _01_Test.SocialNetworkApi.Application;

public class UserRelationApplicationTest
{
    private readonly Mock<SocialNetworkApiContext> _context;

    public UserRelationApplicationTest(Mock<SocialNetworkApiContext> context)
    {
        _context = context;
    }

    [Fact]
    public void Create_WithValidModel_ReturnSucceededResult()
    {

    }
}