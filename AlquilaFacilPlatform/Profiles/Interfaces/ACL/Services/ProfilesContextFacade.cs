using AlquilaFacilPlatform.Profiles.Domain.Model.Commands;
using AlquilaFacilPlatform.Profiles.Domain.Services;

namespace AlquilaFacilPlatform.Profiles.Interfaces.ACL.Services;

public class ProfilesContextFacade(IProfileCommandService profileCommandService) : IProfilesContextFacade
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
        var createProfileCommand = new CreateProfileCommand(name, fatherName, motherName, dateOfBirth, documentNumber, phone, userId);
        var profile = await profileCommandService.Handle(createProfileCommand);
        return profile?.Id ?? 0;
    }
    
    public async Task<int> UpdateProfile(
        
        string name, 
        string fatherName, 
        string motherName, 
        string dateOfBirth, 
        string documentNumber,
        string phone,
        string BankAccountNumber,
        string InterbankAccountNumber,
        int userId 
    )
    {
        var updateProfileCommand = new UpdateProfileCommand(name, fatherName, motherName, dateOfBirth, documentNumber, phone, BankAccountNumber, InterbankAccountNumber, userId);
        var profile = await profileCommandService.Handle(updateProfileCommand);
        return profile?.Id ?? 0;
    }
}