using AlquilaFacilPlatform.IAM.Domain.Model.Aggregates;
using AlquilaFacilPlatform.IAM.Domain.Model.Commands;

namespace AlquilaFacilPlatform.IAM.Domain.Services;

public interface IUserCommandService
{
    Task<(User user, string token)> Handle(SignInCommand command);
    Task<User?> Handle(SignUpCommand command);
    Task<User?> Handle(UpdateUsernameCommand command);
    
}