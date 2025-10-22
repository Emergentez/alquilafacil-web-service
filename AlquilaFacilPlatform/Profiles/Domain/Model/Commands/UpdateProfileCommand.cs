namespace AlquilaFacilPlatform.Profiles.Domain.Model.Commands;

public record UpdateProfileCommand(
    string Name, 
    string? FatherName, 
    string? MotherName, 
    string DateOfBirth, 
    string DocumentNumber,
    string Phone,
    string BankAccountNumber,
    string InterbankAccountNumber,
    int UserId
    );