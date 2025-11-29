using AlquilaFacilPlatform.IAM.Domain.Model.Aggregates;
using AlquilaFacilPlatform.IAM.Domain.Model.Commands;
using AlquilaFacilPlatform.IAM.Domain.Services;
using AlquilaFacilPlatform.IAM.Interfaces.REST;
using AlquilaFacilPlatform.IAM.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AlquilaFacilPlatform.Tests.CoreIntegrationTests.BlackBoxTests;

/// <summary>
/// Black-Box Tests for Authentication Module (US01, US02)
/// These tests validate functionality without knowledge of internal implementation.
/// Techniques: Equivalence Partitioning, Boundary Value Analysis
/// </summary>
public class AuthenticationBlackBoxTests
{
    #region BB-01 to BB-08: Registration Tests (Sign Up)

    /// <summary>
    /// BB-01: Successful registration with valid data
    /// Input: Valid email, valid password (8+ chars, uppercase, lowercase, digit, symbol)
    /// Expected: User created successfully with JWT token
    /// </summary>
    [Fact]
    public async Task BB01_SignUp_WithValidData_ReturnsSuccess()
    {
        // Arrange
        var mockUserCommandService = new Mock<IUserCommandService>();
        var validResource = new SignUpResource(
            "testuser",
            "Test123!@#",
            "test@email.com",
            "TestName",
            "FatherName",
            "MotherName",
            "1995-05-15",
            "987654321",
            "123456789"
        );

        var expectedUser = new User("testuser", "hashed-password", "test@email.com");
        mockUserCommandService
            .Setup(s => s.Handle(It.IsAny<SignUpCommand>()))
            .ReturnsAsync(expectedUser);

        var controller = new AuthenticationController(mockUserCommandService.Object);

        // Act
        var result = await controller.SignUp(validResource);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    /// <summary>
    /// BB-02: Registration with invalid email (missing @)
    /// Input: email without @ symbol
    /// Expected: Error 400 - Invalid email
    /// </summary>
    [Fact]
    public async Task BB02_SignUp_WithEmailMissingAtSymbol_ThrowsException()
    {
        // Arrange
        var mockUserCommandService = new Mock<IUserCommandService>();
        var invalidEmailResource = new SignUpResource(
            "testuser",
            "Test123!@#",
            "testemail.com", // Missing @
            "TestName",
            "FatherName",
            "MotherName",
            "1995-05-15",
            "987654321",
            "123456789"
        );

        mockUserCommandService
            .Setup(s => s.Handle(It.IsAny<SignUpCommand>()))
            .ThrowsAsync(new Exception("Invalid email address"));

        var controller = new AuthenticationController(mockUserCommandService.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            controller.SignUp(invalidEmailResource));
        Assert.Equal("Invalid email address", exception.Message);
    }

    /// <summary>
    /// BB-03: Registration with invalid email (missing domain)
    /// Input: email without domain after @
    /// Expected: Error 400 - Invalid email
    /// </summary>
    [Fact]
    public async Task BB03_SignUp_WithEmailMissingDomain_ThrowsException()
    {
        // Arrange
        var mockUserCommandService = new Mock<IUserCommandService>();
        var invalidEmailResource = new SignUpResource(
            "testuser",
            "Test123!@#",
            "test@", // Missing domain
            "TestName",
            "FatherName",
            "MotherName",
            "1995-05-15",
            "987654321",
            "123456789"
        );

        mockUserCommandService
            .Setup(s => s.Handle(It.IsAny<SignUpCommand>()))
            .ThrowsAsync(new Exception("Invalid email address"));

        var controller = new AuthenticationController(mockUserCommandService.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            controller.SignUp(invalidEmailResource));
        Assert.Equal("Invalid email address", exception.Message);
    }

    /// <summary>
    /// BB-04: Registration with password too short (< 8 characters)
    /// Input: Password with only 6 characters
    /// Expected: Error 400 - Password must be at least 8 characters
    /// </summary>
    [Fact]
    public async Task BB04_SignUp_WithPasswordTooShort_ThrowsException()
    {
        // Arrange
        var mockUserCommandService = new Mock<IUserCommandService>();
        var shortPasswordResource = new SignUpResource(
            "testuser",
            "Test1!", // Only 6 characters
            "test@email.com",
            "TestName",
            "FatherName",
            "MotherName",
            "1995-05-15",
            "987654321",
            "123456789"
        );

        mockUserCommandService
            .Setup(s => s.Handle(It.IsAny<SignUpCommand>()))
            .ThrowsAsync(new Exception("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit and one special character"));

        var controller = new AuthenticationController(mockUserCommandService.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            controller.SignUp(shortPasswordResource));
        Assert.Contains("Password must be at least 8 characters", exception.Message);
    }

    /// <summary>
    /// BB-05: Registration with password missing uppercase letter
    /// Input: Password without uppercase
    /// Expected: Error 400 - Password must contain uppercase
    /// </summary>
    [Fact]
    public async Task BB05_SignUp_WithPasswordMissingUppercase_ThrowsException()
    {
        // Arrange
        var mockUserCommandService = new Mock<IUserCommandService>();
        var noUppercaseResource = new SignUpResource(
            "testuser",
            "test123!@#", // No uppercase
            "test@email.com",
            "TestName",
            "FatherName",
            "MotherName",
            "1995-05-15",
            "987654321",
            "123456789"
        );

        mockUserCommandService
            .Setup(s => s.Handle(It.IsAny<SignUpCommand>()))
            .ThrowsAsync(new Exception("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit and one special character"));

        var controller = new AuthenticationController(mockUserCommandService.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            controller.SignUp(noUppercaseResource));
        Assert.Contains("uppercase letter", exception.Message);
    }

    /// <summary>
    /// BB-06: Registration with password missing digit
    /// Input: Password without numbers
    /// Expected: Error 400 - Password must contain digit
    /// </summary>
    [Fact]
    public async Task BB06_SignUp_WithPasswordMissingDigit_ThrowsException()
    {
        // Arrange
        var mockUserCommandService = new Mock<IUserCommandService>();
        var noDigitResource = new SignUpResource(
            "testuser",
            "TestTest!@#", // No digit
            "test@email.com",
            "TestName",
            "FatherName",
            "MotherName",
            "1995-05-15",
            "987654321",
            "123456789"
        );

        mockUserCommandService
            .Setup(s => s.Handle(It.IsAny<SignUpCommand>()))
            .ThrowsAsync(new Exception("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit and one special character"));

        var controller = new AuthenticationController(mockUserCommandService.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            controller.SignUp(noDigitResource));
        Assert.Contains("digit", exception.Message);
    }

    /// <summary>
    /// BB-07: Registration with password missing special character
    /// Input: Password without symbols
    /// Expected: Error 400 - Password must contain special character
    /// </summary>
    [Fact]
    public async Task BB07_SignUp_WithPasswordMissingSpecialChar_ThrowsException()
    {
        // Arrange
        var mockUserCommandService = new Mock<IUserCommandService>();
        var noSpecialCharResource = new SignUpResource(
            "testuser",
            "Test12345", // No special character
            "test@email.com",
            "TestName",
            "FatherName",
            "MotherName",
            "1995-05-15",
            "987654321",
            "123456789"
        );

        mockUserCommandService
            .Setup(s => s.Handle(It.IsAny<SignUpCommand>()))
            .ThrowsAsync(new Exception("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit and one special character"));

        var controller = new AuthenticationController(mockUserCommandService.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            controller.SignUp(noSpecialCharResource));
        Assert.Contains("special character", exception.Message);
    }

    /// <summary>
    /// BB-08: Registration with duplicate email
    /// Input: Email that already exists in system
    /// Expected: Error 409 - User already exists
    /// </summary>
    [Fact]
    public async Task BB08_SignUp_WithDuplicateEmail_ThrowsConflictException()
    {
        // Arrange
        var mockUserCommandService = new Mock<IUserCommandService>();
        var duplicateEmailResource = new SignUpResource(
            "newuser",
            "Test123!@#",
            "existing@email.com", // Email already exists
            "TestName",
            "FatherName",
            "MotherName",
            "1995-05-15",
            "987654321",
            "123456789"
        );

        mockUserCommandService
            .Setup(s => s.Handle(It.IsAny<SignUpCommand>()))
            .ThrowsAsync(new Exception("Username newuser is already taken"));

        var controller = new AuthenticationController(mockUserCommandService.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            controller.SignUp(duplicateEmailResource));
        Assert.Contains("already taken", exception.Message);
    }

    #endregion

    #region BB-09 to BB-12: Login Tests (Sign In)

    /// <summary>
    /// BB-09: Successful login with valid credentials
    /// Input: Correct email and password
    /// Expected: Valid JWT token returned
    /// </summary>
    [Fact]
    public async Task BB09_SignIn_WithValidCredentials_ReturnsJwtToken()
    {
        // Arrange
        var mockUserCommandService = new Mock<IUserCommandService>();
        var validCredentials = new SignInResource("user@email.com", "Test123!@#");

        var expectedUser = new User("testuser", "hashed-password", "user@email.com");
        var expectedToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.mock-token";

        mockUserCommandService
            .Setup(s => s.Handle(It.Is<SignInCommand>(cmd =>
                cmd.Email == validCredentials.Email &&
                cmd.Password == validCredentials.Password)))
            .ReturnsAsync((expectedUser, expectedToken));

        var controller = new AuthenticationController(mockUserCommandService.Object);

        // Act
        var result = await controller.SignIn(validCredentials);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var authResource = Assert.IsType<AuthenticatedUserResource>(okResult.Value);
        Assert.NotNull(authResource.Token);
        Assert.NotEmpty(authResource.Token);
    }

    /// <summary>
    /// BB-10: Login with non-existent email
    /// Input: Email that doesn't exist in system
    /// Expected: Error 401 - Invalid credentials
    /// </summary>
    [Fact]
    public async Task BB10_SignIn_WithNonExistentEmail_ThrowsUnauthorized()
    {
        // Arrange
        var mockUserCommandService = new Mock<IUserCommandService>();
        var nonExistentEmailCredentials = new SignInResource(
            "nonexistent@email.com",
            "Test123!@#"
        );

        mockUserCommandService
            .Setup(s => s.Handle(It.IsAny<SignInCommand>()))
            .ThrowsAsync(new Exception("Invalid email or password"));

        var controller = new AuthenticationController(mockUserCommandService.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            controller.SignIn(nonExistentEmailCredentials));
        Assert.Equal("Invalid email or password", exception.Message);
    }

    /// <summary>
    /// BB-11: Login with incorrect password
    /// Input: Correct email but wrong password
    /// Expected: Error 401 - Invalid credentials
    /// </summary>
    [Fact]
    public async Task BB11_SignIn_WithIncorrectPassword_ThrowsUnauthorized()
    {
        // Arrange
        var mockUserCommandService = new Mock<IUserCommandService>();
        var wrongPasswordCredentials = new SignInResource(
            "user@email.com",
            "WrongPassword123!" // Incorrect password
        );

        mockUserCommandService
            .Setup(s => s.Handle(It.IsAny<SignInCommand>()))
            .ThrowsAsync(new Exception("Invalid email or password"));

        var controller = new AuthenticationController(mockUserCommandService.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            controller.SignIn(wrongPasswordCredentials));
        Assert.Equal("Invalid email or password", exception.Message);
    }

    /// <summary>
    /// BB-12: Login with empty fields
    /// Input: Empty email and password
    /// Expected: Error 400 - Required fields
    /// </summary>
    [Fact]
    public async Task BB12_SignIn_WithEmptyFields_ThrowsBadRequest()
    {
        // Arrange
        var mockUserCommandService = new Mock<IUserCommandService>();
        var emptyCredentials = new SignInResource("", "");

        mockUserCommandService
            .Setup(s => s.Handle(It.IsAny<SignInCommand>()))
            .ThrowsAsync(new Exception("Email and password are required"));

        var controller = new AuthenticationController(mockUserCommandService.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            controller.SignIn(emptyCredentials));
        Assert.Contains("required", exception.Message);
    }

    #endregion

    #region Boundary Value Analysis Tests

    /// <summary>
    /// BVA-01: Password at minimum length boundary (exactly 8 characters)
    /// </summary>
    [Fact]
    public async Task BVA01_SignUp_WithPasswordExactly8Chars_ShouldSucceed()
    {
        // Arrange
        var mockUserCommandService = new Mock<IUserCommandService>();
        var minLengthPasswordResource = new SignUpResource(
            "testuser",
            "Test12!@", // Exactly 8 characters
            "test@email.com",
            "TestName",
            "FatherName",
            "MotherName",
            "1995-05-15",
            "987654321",
            "123456789"
        );

        var expectedUser = new User("testuser", "hashed-password", "test@email.com");
        mockUserCommandService
            .Setup(s => s.Handle(It.IsAny<SignUpCommand>()))
            .ReturnsAsync(expectedUser);

        var controller = new AuthenticationController(mockUserCommandService.Object);

        // Act
        var result = await controller.SignUp(minLengthPasswordResource);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    /// <summary>
    /// BVA-02: Password below minimum length boundary (7 characters)
    /// </summary>
    [Fact]
    public async Task BVA02_SignUp_WithPassword7Chars_ShouldFail()
    {
        // Arrange
        var mockUserCommandService = new Mock<IUserCommandService>();
        var belowMinPasswordResource = new SignUpResource(
            "testuser",
            "Test1!@", // Only 7 characters
            "test@email.com",
            "TestName",
            "FatherName",
            "MotherName",
            "1995-05-15",
            "987654321",
            "123456789"
        );

        mockUserCommandService
            .Setup(s => s.Handle(It.IsAny<SignUpCommand>()))
            .ThrowsAsync(new Exception("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit and one special character"));

        var controller = new AuthenticationController(mockUserCommandService.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            controller.SignUp(belowMinPasswordResource));
        Assert.Contains("at least 8 characters", exception.Message);
    }

    #endregion
}
