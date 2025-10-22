using AlquilaFacilPlatform.IAM.Interfaces.ACL;

namespace AlquilaFacilPlatform.Shared.Application.Internal.OutboundServices.ExternalServices;

public class UserExternalService(IIamContextFacade iamContextFacade) : IUserExternalService
{
    public bool UserExists(int userId)
    {
        return iamContextFacade.UsersExists(userId);
    }
}