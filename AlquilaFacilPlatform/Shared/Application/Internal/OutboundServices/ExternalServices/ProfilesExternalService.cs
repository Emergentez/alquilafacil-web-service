using AlquilaFacilPlatform.Profiles.Interfaces.ACL;

namespace AlquilaFacilPlatform.Shared.Application.Internal.OutboundServices;

public class ProfilesExternalService(IProfilesContextFacade profilesContextFacade) : IProfilesExternalService
{
    public async Task<int> CreateProfile(
        string name,
        string fatherName,
        string motherName,
        string dateOfBirth,
        string documentNumber,
        string phone,
        int userId
    )
    {
        return await profilesContextFacade.CreateProfile(
            name,
            fatherName,
            motherName,
            dateOfBirth,
            documentNumber,
            phone,
            userId
        );
    }
    
    public async Task<int> UpdateProfile(
        string name,
        string fatherName,
        string motherName,
        string dateOfBirth,
        string documentNumber,
        string phone,
        string bankAccountNumber,
        string interbankAccountNumber,
        int userId
    )
    {
        return await profilesContextFacade.UpdateProfile(
            name,
            fatherName,
            motherName,
            dateOfBirth,
            documentNumber,
            phone,
            bankAccountNumber,
            interbankAccountNumber,
            userId
        );
    }
}