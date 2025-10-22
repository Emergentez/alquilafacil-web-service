using AlquilaFacilPlatform.IAM.Domain.Model.Aggregates;

namespace AlquilaFacilPlatform.IAM.Application.Internal.OutboundServices;

public interface ITokenService
{
    string GenerateToken(User user);
}