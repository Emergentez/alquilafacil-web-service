namespace AlquilaFacilPlatform.Profiles.Interfaces.REST.Resources;

public record UpdateProfileResource(
    string Name,
    string? FatherName,
    string? MotherName,
    string Phone,
    string DocumentNumber, 
    string DateOfBirth,
    string BankAccountNumber,
    string InterbankAccountNumber
    );