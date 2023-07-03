using Moq.EntityFrameworkCore.DbAsyncQueryProvider;
using SocialNetworkApi.Domain.UserAgg;

namespace _01_Test.SocialNetworkApi.DataMock;

public static class FakeUserData
{
    public static IQueryable<User> GetUsers()
    {
        return new List<User>
        {
            new User("ali","mohammadzade","ali@gmail.com",new DateTime(1993,03,7),"123456","I'm a person","/Images/DefaultProfile.jpg"),
            new User("reza","Mohammadzadeh","reza@gmail.com",new DateTime(1999,03,7),"123456","I'm a person","/Images/DefaultProfile.jpg")
        }.AsQueryable();
    }
}