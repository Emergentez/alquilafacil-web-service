namespace AlquilaFacilPlatform.Profiles.Interfaces.ACL;

public interface IProfilesContextFacade
{
    Task<int> CreateProfile(
        string name,
        string? fatherName,
        string? motherName,
        string dateOfBirth,
        string documentNumber,
        string phone,
        int userId
        );

    Task<int> UpdateProfile(
        string name,
        string fatherName,
        string motherName,
        string dateOfBirth,
        string documentNumber,
        string phone,
        string BankAccountNumber,
        string InterbankAccountNumber,
        int userId
    );
}