using AlquilaFacilPlatform.IAM.Application.Internal.CommandServices;
using AlquilaFacilPlatform.IAM.Application.Internal.OutboundServices;
using AlquilaFacilPlatform.IAM.Domain.Model.Aggregates;
using AlquilaFacilPlatform.IAM.Domain.Model.Commands;
using AlquilaFacilPlatform.IAM.Domain.Repositories;
using AlquilaFacilPlatform.Shared.Application.Internal.OutboundServices;
using AlquilaFacilPlatform.Shared.Domain.Repositories;
using Moq;

namespace AlquilaFacilPlatform.Tests.CoreIntegrationTests.WhiteBoxTests;

/// <summary>
/// White-Box Tests for UserCommandService
/// These tests validate internal logic paths, branches, and conditions.
/// Techniques: Statement Coverage, Branch Coverage, Condition Coverage, Path Coverage
/// </summary>
public class UserCommandServiceWhiteBoxTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<ITokenService> _mockTokenService;
    private readonly Mock<IHashingService> _mockHashingService;
    private readonly Mock<IProfilesExternalService> _mockProfilesExternalService;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly UserCommandService _service;

    public UserCommandServiceWhiteBoxTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockTokenService = new Mock<ITokenService>();
        _mockHashingService = new Mock<IHashingService>();
        _mockProfilesExternalService = new Mock<IProfilesExternalService>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _service = new UserCommandService(
            _mockUserRepository.Object,
            _mockTokenService.Object,
            _mockHashingService.Object,
            _mockProfilesExternalService.Object,
            _mockUnitOfWork.Object
        );
    }

    #region WB-01 to WB-08: SignIn Method - Branch Coverage

    /// <summary>
    /// WB-01: SignIn - Path where user is found and password is valid
    /// Covers: user != null AND VerifyPassword returns true AND email contains @
    /// </summary>
    [Fact]
    public async Task WB01_SignIn_UserFoundAndPasswordValid_ReturnsUserAndToken()
    {
        // Arrange
        var command = new SignInCommand("user@email.com", "ValidPassword123!");
        var existingUser = new User("testuser", "hashedPassword", "user@email.com");
        var expectedToken = "jwt-token-here";

        _mockUserRepository
            .Setup(r => r.FindByEmailAsync(command.Email))
            .ReturnsAsync(existingUser);
        _mockHashingService
            .Setup(h => h.VerifyPassword(command.Password, existingUser.PasswordHash))
            .Returns(true);
        _mockTokenService
            .Setup(t => t.GenerateToken(existingUser))
            .Returns(expectedToken);

        // Act
        var result = await _service.Handle(command);

        // Assert
        Assert.Equal(existingUser, result.user);
        Assert.Equal(expectedToken, result.token);
        _mockTokenService.Verify(t => t.GenerateToken(existingUser), Times.Once);
    }

    /// <summary>
    /// WB-02: SignIn - Path where user is null (not found)
    /// Covers: user == null branch
    /// </summary>
    [Fact]
    public async Task WB02_SignIn_UserNotFound_ThrowsException()
    {
        // Arrange
        var command = new SignInCommand("nonexistent@email.com", "Password123!");

        _mockUserRepository
            .Setup(r => r.FindByEmailAsync(command.Email))
            .ReturnsAsync((User?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.Handle(command));
        Assert.Equal("Invalid email or password", exception.Message);
    }

    /// <summary>
    /// WB-03: SignIn - Path where password verification fails
    /// Covers: VerifyPassword returns false branch
    /// </summary>
    [Fact]
    public async Task WB03_SignIn_PasswordVerificationFails_ThrowsException()
    {
        // Arrange
        var command = new SignInCommand("user@email.com", "WrongPassword123!");
        var existingUser = new User("testuser", "hashedPassword", "user@email.com");

        _mockUserRepository
            .Setup(r => r.FindByEmailAsync(command.Email))
            .ReturnsAsync(existingUser);
        _mockHashingService
            .Setup(h => h.VerifyPassword(command.Password, existingUser.PasswordHash))
            .Returns(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.Handle(command));
        Assert.Equal("Invalid email or password", exception.Message);
    }

    /// <summary>
    /// WB-04: SignIn - Path where email does not contain @
    /// Covers: !command.Email.Contains('@') branch
    /// </summary>
    [Fact]
    public async Task WB04_SignIn_EmailWithoutAtSymbol_ThrowsException()
    {
        // Arrange
        var command = new SignInCommand("useremail.com", "Password123!"); // No @ symbol
        var existingUser = new User("testuser", "hashedPassword", "user@email.com");

        _mockUserRepository
            .Setup(r => r.FindByEmailAsync(command.Email))
            .ReturnsAsync(existingUser);
        _mockHashingService
            .Setup(h => h.VerifyPassword(command.Password, existingUser.PasswordHash))
            .Returns(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.Handle(command));
        Assert.Equal("Invalid email or password", exception.Message);
    }

    #endregion

    #region WB-05 to WB-16: SignUp Method - Condition Coverage

    /// <summary>
    /// WB-05: SignUp - Valid path with all conditions met
    /// Covers: All password validations pass, email valid, phone valid, username not taken
    /// </summary>
    [Fact]
    public async Task WB05_SignUp_AllValidationsPass_ReturnsUser()
    {
        // Arrange
        var command = new SignUpCommand(
            "newuser", "ValidPass1!", "new@email.com",
            "Name", "FatherName", "MotherName",
            "1990-01-01", "123456789", "987654321"
        );

        _mockUserRepository
            .Setup(r => r.ExistsByUsername(command.Username))
            .ReturnsAsync(false);
        _mockHashingService
            .Setup(h => h.HashPassword(command.Password))
            .Returns("hashed-password");
        _mockUnitOfWork
            .Setup(u => u.CompleteAsync())
            .Returns(Task.CompletedTask);
        _mockProfilesExternalService
            .Setup(p => p.CreateProfile(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(1);

        // Act
        var result = await _service.Handle(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.Username, result.Username);
        Assert.Equal(command.Email, result.Email);
    }

    /// <summary>
    /// WB-06: SignUp - Password length < 8 characters
    /// Covers: command.Password.Length < 8 condition
    /// </summary>
    [Fact]
    public async Task WB06_SignUp_PasswordTooShort_ThrowsException()
    {
        // Arrange
        var command = new SignUpCommand(
            "newuser", "Pass1!", "new@email.com", // Only 6 chars
            "Name", "FatherName", "MotherName",
            "1990-01-01", "123456789", "987654321"
        );

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.Handle(command));
        Assert.Contains("at least 8 characters", exception.Message);
    }

    /// <summary>
    /// WB-07: SignUp - Password missing digit
    /// Covers: !command.Password.Any(char.IsDigit) condition
    /// </summary>
    [Fact]
    public async Task WB07_SignUp_PasswordMissingDigit_ThrowsException()
    {
        // Arrange
        var command = new SignUpCommand(
            "newuser", "Password!", "new@email.com", // No digit
            "Name", "FatherName", "MotherName",
            "1990-01-01", "123456789", "987654321"
        );

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.Handle(command));
        Assert.Contains("one digit", exception.Message);
    }

    /// <summary>
    /// WB-08: SignUp - Password missing uppercase
    /// Covers: !command.Password.Any(char.IsUpper) condition
    /// </summary>
    [Fact]
    public async Task WB08_SignUp_PasswordMissingUppercase_ThrowsException()
    {
        // Arrange
        var command = new SignUpCommand(
            "newuser", "password1!", "new@email.com", // No uppercase
            "Name", "FatherName", "MotherName",
            "1990-01-01", "123456789", "987654321"
        );

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.Handle(command));
        Assert.Contains("one uppercase letter", exception.Message);
    }

    /// <summary>
    /// WB-09: SignUp - Password missing lowercase
    /// Covers: !command.Password.Any(char.IsLower) condition
    /// </summary>
    [Fact]
    public async Task WB09_SignUp_PasswordMissingLowercase_ThrowsException()
    {
        // Arrange
        var command = new SignUpCommand(
            "newuser", "PASSWORD1!", "new@email.com", // No lowercase
            "Name", "FatherName", "MotherName",
            "1990-01-01", "123456789", "987654321"
        );

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.Handle(command));
        Assert.Contains("one lowercase letter", exception.Message);
    }

    /// <summary>
    /// WB-10: SignUp - Password missing special character
    /// Covers: !command.Password.Any(c => symbols.Contains(c)) condition
    /// </summary>
    [Fact]
    public async Task WB10_SignUp_PasswordMissingSpecialChar_ThrowsException()
    {
        // Arrange
        var command = new SignUpCommand(
            "newuser", "Password1", "new@email.com", // No special char
            "Name", "FatherName", "MotherName",
            "1990-01-01", "123456789", "987654321"
        );

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.Handle(command));
        Assert.Contains("one special character", exception.Message);
    }

    /// <summary>
    /// WB-11: SignUp - Email without @ symbol
    /// Covers: !command.Email.Contains('@') condition
    /// </summary>
    [Fact]
    public async Task WB11_SignUp_EmailWithoutAtSymbol_ThrowsException()
    {
        // Arrange
        var command = new SignUpCommand(
            "newuser", "ValidPass1!", "newemail.com", // No @
            "Name", "FatherName", "MotherName",
            "1990-01-01", "123456789", "987654321"
        );

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.Handle(command));
        Assert.Equal("Invalid email address", exception.Message);
    }

    /// <summary>
    /// WB-12: SignUp - Phone number too short (< 9 chars)
    /// Covers: command.Phone.Length < 9 condition
    /// </summary>
    [Fact]
    public async Task WB12_SignUp_PhoneTooShort_ThrowsException()
    {
        // Arrange
        var command = new SignUpCommand(
            "newuser", "ValidPass1!", "new@email.com",
            "Name", "FatherName", "MotherName",
            "1990-01-01", "12345678", "12345678" // Only 8 chars
        );

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.Handle(command));
        Assert.Equal("Phone number must to be valid", exception.Message);
    }

    /// <summary>
    /// WB-13: SignUp - Username already taken
    /// Covers: await userRepository.ExistsByUsername returns true
    /// </summary>
    [Fact]
    public async Task WB13_SignUp_UsernameAlreadyTaken_ThrowsException()
    {
        // Arrange
        var command = new SignUpCommand(
            "existinguser", "ValidPass1!", "new@email.com",
            "Name", "FatherName", "MotherName",
            "1990-01-01", "123456789", "987654321"
        );

        _mockUserRepository
            .Setup(r => r.ExistsByUsername(command.Username))
            .ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.Handle(command));
        Assert.Contains("already taken", exception.Message);
    }

    /// <summary>
    /// WB-14: SignUp - Exception during user creation (catch block)
    /// Covers: try-catch block when AddAsync or CompleteAsync fails
    /// </summary>
    [Fact]
    public async Task WB14_SignUp_DatabaseError_ThrowsWrappedException()
    {
        // Arrange
        var command = new SignUpCommand(
            "newuser", "ValidPass1!", "new@email.com",
            "Name", "FatherName", "MotherName",
            "1990-01-01", "123456789", "987654321"
        );

        _mockUserRepository
            .Setup(r => r.ExistsByUsername(command.Username))
            .ReturnsAsync(false);
        _mockHashingService
            .Setup(h => h.HashPassword(command.Password))
            .Returns("hashed-password");
        _mockUserRepository
            .Setup(r => r.AddAsync(It.IsAny<User>()))
            .ThrowsAsync(new Exception("Database connection failed"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.Handle(command));
        Assert.Contains("An error occurred while creating user", exception.Message);
    }

    #endregion

    #region WB-15 to WB-18: UpdateUsername Method - Path Coverage

    /// <summary>
    /// WB-15: UpdateUsername - Successful update path
    /// Covers: User found AND username not taken
    /// </summary>
    [Fact]
    public async Task WB15_UpdateUsername_ValidUpdate_ReturnsUpdatedUser()
    {
        // Arrange
        var command = new UpdateUsernameCommand(1, "newusername");
        var existingUser = new User("oldusername", "hashedPassword", "user@email.com");

        _mockUserRepository
            .Setup(r => r.FindByIdAsync(command.Id))
            .ReturnsAsync(existingUser);
        _mockUserRepository
            .Setup(r => r.ExistsByUsername(command.Username))
            .ReturnsAsync(false);
        _mockUnitOfWork
            .Setup(u => u.CompleteAsync())
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.Handle(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.Username, result.Username);
    }

    /// <summary>
    /// WB-16: UpdateUsername - User not found path
    /// Covers: userToUpdate == null branch
    /// </summary>
    [Fact]
    public async Task WB16_UpdateUsername_UserNotFound_ThrowsException()
    {
        // Arrange
        var command = new UpdateUsernameCommand(999, "newusername");

        _mockUserRepository
            .Setup(r => r.FindByIdAsync(command.Id))
            .ReturnsAsync((User?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.Handle(command));
        Assert.Equal("User not found", exception.Message);
    }

    /// <summary>
    /// WB-17: UpdateUsername - Username already exists path
    /// Covers: userExists == true branch
    /// </summary>
    [Fact]
    public async Task WB17_UpdateUsername_UsernameAlreadyExists_ThrowsException()
    {
        // Arrange
        var command = new UpdateUsernameCommand(1, "existingusername");
        var existingUser = new User("oldusername", "hashedPassword", "user@email.com");

        _mockUserRepository
            .Setup(r => r.FindByIdAsync(command.Id))
            .ReturnsAsync(existingUser);
        _mockUserRepository
            .Setup(r => r.ExistsByUsername(command.Username))
            .ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.Handle(command));
        Assert.Equal("This username already exists", exception.Message);
    }

    #endregion

    #region Statement Coverage - Verify All Code Paths Executed

    /// <summary>
    /// WB-18: Verify token generation is called after successful SignIn
    /// Covers: tokenService.GenerateToken statement execution
    /// </summary>
    [Fact]
    public async Task WB18_SignIn_Success_TokenServiceCalled()
    {
        // Arrange
        var command = new SignInCommand("user@email.com", "ValidPassword123!");
        var existingUser = new User("testuser", "hashedPassword", "user@email.com");

        _mockUserRepository
            .Setup(r => r.FindByEmailAsync(command.Email))
            .ReturnsAsync(existingUser);
        _mockHashingService
            .Setup(h => h.VerifyPassword(command.Password, existingUser.PasswordHash))
            .Returns(true);
        _mockTokenService
            .Setup(t => t.GenerateToken(existingUser))
            .Returns("token");

        // Act
        await _service.Handle(command);

        // Assert
        _mockTokenService.Verify(t => t.GenerateToken(It.IsAny<User>()), Times.Once);
    }

    /// <summary>
    /// WB-19: Verify HashPassword is called during SignUp
    /// Covers: hashingService.HashPassword statement execution
    /// </summary>
    [Fact]
    public async Task WB19_SignUp_Success_HashingServiceCalled()
    {
        // Arrange
        var command = new SignUpCommand(
            "newuser", "ValidPass1!", "new@email.com",
            "Name", "FatherName", "MotherName",
            "1990-01-01", "123456789", "987654321"
        );

        _mockUserRepository
            .Setup(r => r.ExistsByUsername(command.Username))
            .ReturnsAsync(false);
        _mockHashingService
            .Setup(h => h.HashPassword(command.Password))
            .Returns("hashed-password");
        _mockUnitOfWork
            .Setup(u => u.CompleteAsync())
            .Returns(Task.CompletedTask);
        _mockProfilesExternalService
            .Setup(p => p.CreateProfile(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(1);

        // Act
        await _service.Handle(command);

        // Assert
        _mockHashingService.Verify(h => h.HashPassword(command.Password), Times.Once);
    }

    /// <summary>
    /// WB-20: Verify ProfilesExternalService is called after user creation
    /// Covers: profilesExternalService.CreateProfile statement execution
    /// </summary>
    [Fact]
    public async Task WB20_SignUp_Success_ProfileServiceCalled()
    {
        // Arrange
        var command = new SignUpCommand(
            "newuser", "ValidPass1!", "new@email.com",
            "TestName", "FatherName", "MotherName",
            "1990-01-01", "123456789", "987654321"
        );

        _mockUserRepository
            .Setup(r => r.ExistsByUsername(command.Username))
            .ReturnsAsync(false);
        _mockHashingService
            .Setup(h => h.HashPassword(command.Password))
            .Returns("hashed-password");
        _mockUnitOfWork
            .Setup(u => u.CompleteAsync())
            .Returns(Task.CompletedTask);
        _mockProfilesExternalService
            .Setup(p => p.CreateProfile(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(1);

        // Act
        await _service.Handle(command);

        // Assert
        _mockProfilesExternalService.Verify(
            p => p.CreateProfile(
                command.Name, command.FatherName, command.MotherName,
                command.DateOfBirth, command.DocumentNumber, command.Phone, It.IsAny<int>()),
            Times.Once);
    }

    #endregion
}
