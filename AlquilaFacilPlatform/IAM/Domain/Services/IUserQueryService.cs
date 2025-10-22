using AlquilaFacilPlatform.IAM.Domain.Model.Aggregates;
using AlquilaFacilPlatform.IAM.Domain.Model.Queries;

namespace AlquilaFacilPlatform.IAM.Domain.Services;

public interface IUserQueryService
{
    Task<User?> Handle(GetUserByIdQuery query);
    Task<IEnumerable<User>> Handle(GetAllUsersQuery query);
    Task<User?> Handle(GetUserByEmailQuery query);
    
    Task<string?> Handle(GetUsernameByIdQuery query);
    bool Handle(UserExistsQuery query);
}