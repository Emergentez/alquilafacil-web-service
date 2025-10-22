using AlquilaFacilPlatform.Locals.Domain.Model.Aggregates;
using AlquilaFacilPlatform.Locals.Domain.Model.Commands;
using AlquilaFacilPlatform.Locals.Domain.Repositories;
using AlquilaFacilPlatform.Locals.Domain.Services;
using AlquilaFacilPlatform.Shared.Application.Internal.OutboundServices;
using AlquilaFacilPlatform.Shared.Domain.Repositories;

namespace AlquilaFacilPlatform.Locals.Application.Internal.CommandServices;

public class CommentCommandService(ICommentRepository commentRepository, ILocalRepository localRepository, IUserExternalService userExternalService, IUnitOfWork unitOfWork) : ICommentCommandService
{
    public async Task<Comment?> Handle(CreateCommentCommand command)
    {
        var local = await localRepository.FindByIdAsync(command.LocalId);

        if (local == null)
        {
            throw new Exception("There is no locals matching the id specified");
        }
        
        if (!userExternalService.UserExists(command.UserId))
        {
            throw new Exception("There are no users matching the id specified");
        }
        
        if (command.Rating is > 5 or < 0)
        {
            throw new Exception("Rating needs to be a number between 0 and 5");
        }

        var comment = new Comment(command);
        await commentRepository.AddAsync(comment);
        await unitOfWork.CompleteAsync();
        return comment;
    }
}