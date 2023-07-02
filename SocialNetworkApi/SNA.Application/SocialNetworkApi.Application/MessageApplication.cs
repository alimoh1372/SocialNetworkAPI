using _00_Framework.Application;
using Microsoft.EntityFrameworkCore;
using SocialNetworkApi.Application.Contracts.MessageContracts;
using SocialNetworkApi.Domain.MessageAgg;
using SocialNetworkApi.Infrastructure.EfCore;

namespace SocialNetworkApi.Application;

/// <summary>
/// Do the messaging operations
/// </summary>
public class MessageApplication : IMessageApplication
{
    private readonly SocialNetworkApiContext _context;


    public MessageApplication(SocialNetworkApiContext context)
    {
        _context = context;
    }

    public OperationResult Send(SendMessage command)
    {
        OperationResult result = new OperationResult();

        //check the message isn't from a user to himself
        if (command.FkToUserId == command.FkFromUserId)
            return result.Failed(ApplicationMessage.CantSelfRequest);

        //check to deny send message to non-friend users
        if (!_context.UserRelations.Any(x =>
                x.FkUserAId == command.FkFromUserId && x.FkUserBId == command.FkToUserId ||
                x.FkUserBId == command.FkFromUserId && x.FkUserAId == command.FkToUserId
                                                    && x.Approve == true))
            return result.Failed(ApplicationMessage.CantSendToNonFriendUsers);

        Message message = new Message(command.FkFromUserId, command.FkToUserId, command.MessageContent);

        //Add to database
        _context.Messages.Add(message);


        _context.SaveChanges();


        return result.Succedded();

    }

    public OperationResult Edit(EditMessage command)
    {
        var operationResult = new OperationResult();
        var message = _context.Messages.FirstOrDefault(x => x.Id == command.Id);

        if (message == null)
            return operationResult.Failed(ApplicationMessage.NotFound);
        if (message.FkFromUserId != command.FkFromUserId)
            return operationResult.Failed(ValidatingMessage.ForbiddenToAccess);
        if (message.CreationDate.AddMinutes(+3) < DateTime.Now)
            return operationResult.Failed(ApplicationMessage.EditTimeOver);
        
        message.Edit(command.MessageContent);
        _context.SaveChanges();
        return operationResult.Succedded();
    }

    public OperationResult Like(long id)
    {
        throw new System.NotImplementedException();
    }

    public OperationResult Unlike(long id)
    {
        throw new System.NotImplementedException();
    }

    public OperationResult AsRead(long id)
    {
        throw new System.NotImplementedException();
    }

    public async Task<List<MessageViewModel>> LoadChatHistory(LoadChat request)
    {

        return await _context.Messages
            .Include(x => x.FromUser)
            .Include(x => x.ToUser)
            .Select(x => new MessageViewModel
            {
                Id = x.Id,
                CreationDate = x.CreationDate,
                FkFromUserId = x.FkFromUserId,
                SenderFullName = x.FromUser.Name + " " + x.FromUser.LastName,
                FromUserProfilePicture = x.FromUser.ProfilePicture,
                FkToUserId = x.FkToUserId,
                ReceiverFullName = x.ToUser.Name + " " + x.ToUser.LastName,
                ToUserProfilePicture = x.ToUser.ProfilePicture,
                MessageContent = x.MessageContent
            })
            .Where(x => (x.FkFromUserId == request.IdUserACurrentUser && x.FkToUserId == request.IdUserB)
                                   || (x.FkFromUserId == request.IdUserB && x.FkToUserId == request.IdUserACurrentUser))
            .ToListAsync();

    }

    public async Task<MessageViewModel?> GetLatestMessage(long fromUserId, long toUserId)
    {
        return await _context.Messages
            .Include(x => x.FromUser)
            .Include(x => x.ToUser)
            .Select(x => new MessageViewModel
            {
                Id = x.Id,
                CreationDate = x.CreationDate,
                FkFromUserId = x.FkFromUserId,
                SenderFullName = x.FromUser.Name + " " + x.FromUser.LastName,
                FromUserProfilePicture = x.FromUser.ProfilePicture,
                FkToUserId = x.FkToUserId,
                ReceiverFullName = x.ToUser.Name + " " + x.ToUser.LastName,
                ToUserProfilePicture = x.ToUser.ProfilePicture,
                MessageContent = x.MessageContent
            }).
            OrderBy(x => x.Id).
            LastOrDefaultAsync(x => x.FkFromUserId == fromUserId && x.FkToUserId == toUserId);
    }

    public async Task<EditMessage?> GetEditMessageBy(long id)
    {
        return await _context.Messages.Select(x => new EditMessage
        {
            Id = x.Id,
            FkFromUserId = x.FkFromUserId,
            FkToUserId = x.FkToUserId,
            MessageContent = x.MessageContent
        })
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<MessageViewModel?> GetMessageViewModelBy(long id)
    {
        return await _context.Messages
            .Include(x => x.FromUser)
            .Include(x => x.ToUser)
            .Select(x => new MessageViewModel
            {
                Id = x.Id,
                CreationDate = x.CreationDate,
                FkFromUserId = x.FkFromUserId,
                SenderFullName = x.FromUser.Name + " " + x.FromUser.LastName,
                FromUserProfilePicture = x.FromUser.ProfilePicture,
                FkToUserId = x.FkToUserId,
                ReceiverFullName = x.ToUser.Name + " " + x.ToUser.LastName,
                ToUserProfilePicture = x.ToUser.ProfilePicture,
                MessageContent = x.MessageContent
            })
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
