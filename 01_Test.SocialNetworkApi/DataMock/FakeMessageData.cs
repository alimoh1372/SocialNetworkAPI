using SocialNetworkApi.Domain.MessageAgg;

namespace _01_Test.SocialNetworkApi.DataMock;

public static class FakeMessageData
{
    public static List<Message> Messages { get; set; } = new List<Message>
    {
        new Message(1,2,"Salam khoobi"),
        new Message(2,1,"Helloooooo")
    };
}