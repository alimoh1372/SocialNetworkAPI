using _00_Framework.Application;

namespace SocialNetworkApi.Application.Contracts.MessageContracts;

public interface IMessageApplication
{
    /// <summary>
    /// Send A message From UserA To UserB
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    OperationResult Send(SendMessage command);

    /// <summary>
    /// To edit message by user A(Sender)
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    OperationResult Edit(EditMessage command);

    /// <summary>
    /// Like the message by UserB(receiver)
    /// </summary>
    /// <param name="id">id of message</param>
    /// <returns></returns>
    OperationResult Like(long id);

    /// <summary>
    /// Unlike the liked message by UserB(receiver)
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    OperationResult Unlike(long id);
    /// <summary>
    /// Show that the user B is read the message
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    OperationResult AsRead(long id);

    /// <summary>
    /// To get All message between two user
    /// </summary>
    /// <param name="idUserA"></param>
    /// <param name="idUserB"></param>
    /// <returns>
    /// List of <see cref="MessageViewModel"/> if there isn't any Message return <see langword="null"/>
    /// </returns>
    Task<List<MessageViewModel>> LoadChatHistory(long idUserA, long idUserB);

    /// <summary>
    /// Get the Latest Message between two user
    /// </summary>
    /// <param name="fromUserId"></param>
    /// <param name="toUserId"></param>
    /// <returns> A <see cref="MessageViewModel"/></returns>
    Task<MessageViewModel> GetLatestMessage(long fromUserId, long toUserId);

    /// <summary>
    /// Get The edit model=<see cref="EditMessage"/> 
    /// </summary>
    /// <param name="id"></param>
    /// <returns><see langword="null"/> if there isn't any message with <paramref name="id"/></returns>
    Task<EditMessage> GetEditMessageBy(long id);

    Task<MessageViewModel> GetMessageViewModelBy(long id);
}
