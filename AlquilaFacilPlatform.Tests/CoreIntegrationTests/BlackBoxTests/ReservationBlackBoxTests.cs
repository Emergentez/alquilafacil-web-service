using AlquilaFacilPlatform.Booking.Domain.Model.Aggregates;
using AlquilaFacilPlatform.Booking.Domain.Model.Commands;
using AlquilaFacilPlatform.Booking.Domain.Model.Queries;
using AlquilaFacilPlatform.Booking.Domain.Services;
using AlquilaFacilPlatform.Booking.Interfaces.REST;
using AlquilaFacilPlatform.Booking.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AlquilaFacilPlatform.Tests.CoreIntegrationTests.BlackBoxTests;

/// <summary>
/// Black-Box Tests for Reservation Module (US03, US04)
/// These tests validate functionality without knowledge of internal implementation.
/// Techniques: Equivalence Partitioning, Boundary Value Analysis
/// </summary>
public class ReservationBlackBoxTests
{
    #region BB-25 to BB-28: Create Reservation Tests

    /// <summary>
    /// BB-25: Successful reservation creation with valid data
    /// Input: Valid dates, valid user, valid local, valid price
    /// Expected: Reservation created successfully with 201 status
    /// </summary>
    [Fact]
    public async Task BB25_CreateReservation_WithValidData_ReturnsCreated()
    {
        // Arrange
        var mockCommandService = new Mock<IReservationCommandService>();
        var mockQueryService = new Mock<IReservationQueryService>();

        var validResource = new CreateReservationResource(
            DateTime.UtcNow.AddDays(5),
            DateTime.UtcNow.AddDays(7),
            1,
            2,
            150.50f,
            "https://example.com/voucher.png"
        );

        var expectedReservation = new Reservation(new CreateReservationCommand(
            validResource.StartDate,
            validResource.EndDate,
            validResource.UserId,
            validResource.LocalId,
            validResource.price,
            validResource.voucherImageUrl
        ));

        mockCommandService
            .Setup(s => s.Handle(It.IsAny<CreateReservationCommand>()))
            .ReturnsAsync(expectedReservation);

        var controller = new ReservationController(mockCommandService.Object, mockQueryService.Object);

        // Act
        var result = await controller.CreateReservationAsync(validResource);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(201, objectResult.StatusCode);
    }

    /// <summary>
    /// BB-26: Reservation creation with end date before start date
    /// Input: EndDate earlier than StartDate
    /// Expected: Error - Invalid date range
    /// </summary>
    [Fact]
    public async Task BB26_CreateReservation_WithEndDateBeforeStartDate_ThrowsException()
    {
        // Arrange
        var mockCommandService = new Mock<IReservationCommandService>();
        var mockQueryService = new Mock<IReservationQueryService>();

        var invalidDateResource = new CreateReservationResource(
            DateTime.UtcNow.AddDays(10), // Start date
            DateTime.UtcNow.AddDays(5),  // End date before start
            1,
            2,
            150.50f,
            "https://example.com/voucher.png"
        );

        mockCommandService
            .Setup(s => s.Handle(It.IsAny<CreateReservationCommand>()))
            .ThrowsAsync(new Exception("End date must be after start date"));

        var controller = new ReservationController(mockCommandService.Object, mockQueryService.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            controller.CreateReservationAsync(invalidDateResource));
        Assert.Contains("End date must be after start date", exception.Message);
    }

    /// <summary>
    /// BB-27: Reservation creation with start date in the past
    /// Input: StartDate before current date
    /// Expected: Error - Start date cannot be in the past
    /// </summary>
    [Fact]
    public async Task BB27_CreateReservation_WithStartDateInPast_ThrowsException()
    {
        // Arrange
        var mockCommandService = new Mock<IReservationCommandService>();
        var mockQueryService = new Mock<IReservationQueryService>();

        var pastDateResource = new CreateReservationResource(
            DateTime.UtcNow.AddDays(-5), // Past start date
            DateTime.UtcNow.AddDays(2),
            1,
            2,
            150.50f,
            "https://example.com/voucher.png"
        );

        mockCommandService
            .Setup(s => s.Handle(It.IsAny<CreateReservationCommand>()))
            .ThrowsAsync(new Exception("Start date cannot be in the past"));

        var controller = new ReservationController(mockCommandService.Object, mockQueryService.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            controller.CreateReservationAsync(pastDateResource));
        Assert.Contains("Start date cannot be in the past", exception.Message);
    }

    /// <summary>
    /// BB-28: Reservation creation with negative price
    /// Input: Price less than zero
    /// Expected: Error - Price must be positive
    /// </summary>
    [Fact]
    public async Task BB28_CreateReservation_WithNegativePrice_ThrowsException()
    {
        // Arrange
        var mockCommandService = new Mock<IReservationCommandService>();
        var mockQueryService = new Mock<IReservationQueryService>();

        var negativePriceResource = new CreateReservationResource(
            DateTime.UtcNow.AddDays(5),
            DateTime.UtcNow.AddDays(7),
            1,
            2,
            -50.00f, // Negative price
            "https://example.com/voucher.png"
        );

        mockCommandService
            .Setup(s => s.Handle(It.IsAny<CreateReservationCommand>()))
            .ThrowsAsync(new Exception("Price must be a positive value"));

        var controller = new ReservationController(mockCommandService.Object, mockQueryService.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            controller.CreateReservationAsync(negativePriceResource));
        Assert.Contains("Price must be a positive value", exception.Message);
    }

    #endregion

    #region BB-29 to BB-30: Update Reservation Tests

    /// <summary>
    /// BB-29: Successful reservation update with valid dates
    /// Input: Valid new start and end dates
    /// Expected: Reservation updated successfully
    /// </summary>
    [Fact]
    public async Task BB29_UpdateReservation_WithValidDates_ReturnsOk()
    {
        // Arrange
        var mockCommandService = new Mock<IReservationCommandService>();
        var mockQueryService = new Mock<IReservationQueryService>();

        var updateResource = new UpdateReservationResource(
            DateTime.UtcNow.AddDays(10),
            DateTime.UtcNow.AddDays(15),
            1,
            2
        );

        var updatedReservation = new Reservation(new CreateReservationCommand(
            updateResource.StartDate,
            updateResource.EndDate,
            updateResource.UserId,
            updateResource.LocalId,
            100.0f,
            "https://example.com/voucher.png"
        ));

        mockCommandService
            .Setup(s => s.Handle(It.IsAny<UpdateReservationDateCommand>()))
            .ReturnsAsync(updatedReservation);

        var controller = new ReservationController(mockCommandService.Object, mockQueryService.Object);

        // Act
        var result = await controller.UpdateReservationAsync(1, updateResource);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    /// <summary>
    /// BB-30: Update reservation with non-existent ID
    /// Input: Reservation ID that doesn't exist
    /// Expected: Error - Reservation not found
    /// </summary>
    [Fact]
    public async Task BB30_UpdateReservation_WithNonExistentId_ThrowsException()
    {
        // Arrange
        var mockCommandService = new Mock<IReservationCommandService>();
        var mockQueryService = new Mock<IReservationQueryService>();

        var updateResource = new UpdateReservationResource(
            DateTime.UtcNow.AddDays(10),
            DateTime.UtcNow.AddDays(15),
            1,
            2
        );

        mockCommandService
            .Setup(s => s.Handle(It.IsAny<UpdateReservationDateCommand>()))
            .ThrowsAsync(new Exception("Reservation not found"));

        var controller = new ReservationController(mockCommandService.Object, mockQueryService.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            controller.UpdateReservationAsync(9999, updateResource));
        Assert.Contains("Reservation not found", exception.Message);
    }

    #endregion

    #region BB-31 to BB-32: Delete Reservation Tests

    /// <summary>
    /// BB-31: Successful reservation deletion
    /// Input: Valid existing reservation ID
    /// Expected: Reservation deleted successfully
    /// </summary>
    [Fact]
    public async Task BB31_DeleteReservation_WithValidId_ReturnsOk()
    {
        // Arrange
        var mockCommandService = new Mock<IReservationCommandService>();
        var mockQueryService = new Mock<IReservationQueryService>();

        var deletedReservation = new Reservation();

        mockCommandService
            .Setup(s => s.Handle(It.IsAny<DeleteReservationCommand>()))
            .ReturnsAsync(deletedReservation);

        var controller = new ReservationController(mockCommandService.Object, mockQueryService.Object);

        // Act
        var result = await controller.DeleteReservationAsync(1);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(200, objectResult.StatusCode);
        Assert.Equal("Reservation deleted", objectResult.Value);
    }

    /// <summary>
    /// BB-32: Delete non-existent reservation
    /// Input: Reservation ID that doesn't exist
    /// Expected: Error - Reservation not found
    /// </summary>
    [Fact]
    public async Task BB32_DeleteReservation_WithNonExistentId_ThrowsException()
    {
        // Arrange
        var mockCommandService = new Mock<IReservationCommandService>();
        var mockQueryService = new Mock<IReservationQueryService>();

        mockCommandService
            .Setup(s => s.Handle(It.IsAny<DeleteReservationCommand>()))
            .ThrowsAsync(new Exception("Reservation not found"));

        var controller = new ReservationController(mockCommandService.Object, mockQueryService.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            controller.DeleteReservationAsync(9999));
        Assert.Contains("Reservation not found", exception.Message);
    }

    #endregion

    #region BB-33 to BB-36: Query Reservation Tests

    /// <summary>
    /// BB-33: Get reservations by valid user ID
    /// Input: Existing user ID with reservations
    /// Expected: List of user's reservations
    /// </summary>
    [Fact]
    public async Task BB33_GetReservationsByUserId_WithValidUserId_ReturnsReservations()
    {
        // Arrange
        var mockCommandService = new Mock<IReservationCommandService>();
        var mockQueryService = new Mock<IReservationQueryService>();

        var reservations = new List<Reservation>
        {
            new Reservation(new CreateReservationCommand(
                DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(3), 1, 2, 100.0f, "voucher1.png")),
            new Reservation(new CreateReservationCommand(
                DateTime.UtcNow.AddDays(5), DateTime.UtcNow.AddDays(7), 1, 3, 150.0f, "voucher2.png"))
        };

        mockQueryService
            .Setup(s => s.GetReservationsByUserIdAsync(It.Is<GetReservationsByUserId>(q => q.UserId == 1)))
            .ReturnsAsync(reservations);

        var controller = new ReservationController(mockCommandService.Object, mockQueryService.Object);

        // Act
        var result = await controller.GetReservationsByUserIdAsync(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        var returnedReservations = Assert.IsAssignableFrom<IEnumerable<ReservationResource>>(okResult.Value);
        Assert.Equal(2, returnedReservations.Count());
    }

    /// <summary>
    /// BB-34: Get reservations by user ID with no reservations
    /// Input: User ID with no reservations
    /// Expected: Empty list
    /// </summary>
    [Fact]
    public async Task BB34_GetReservationsByUserId_WithNoReservations_ReturnsEmptyList()
    {
        // Arrange
        var mockCommandService = new Mock<IReservationCommandService>();
        var mockQueryService = new Mock<IReservationQueryService>();

        mockQueryService
            .Setup(s => s.GetReservationsByUserIdAsync(It.Is<GetReservationsByUserId>(q => q.UserId == 999)))
            .ReturnsAsync(new List<Reservation>());

        var controller = new ReservationController(mockCommandService.Object, mockQueryService.Object);

        // Act
        var result = await controller.GetReservationsByUserIdAsync(999);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        var returnedReservations = Assert.IsAssignableFrom<IEnumerable<ReservationResource>>(okResult.Value);
        Assert.Empty(returnedReservations);
    }

    /// <summary>
    /// BB-35: Get reservations by start date
    /// Input: Valid start date with existing reservations
    /// Expected: List of reservations matching start date
    /// </summary>
    [Fact]
    public async Task BB35_GetReservationsByStartDate_WithValidDate_ReturnsReservations()
    {
        // Arrange
        var mockCommandService = new Mock<IReservationCommandService>();
        var mockQueryService = new Mock<IReservationQueryService>();

        var targetDate = DateTime.UtcNow.AddDays(5).Date;
        var reservations = new List<Reservation>
        {
            new Reservation(new CreateReservationCommand(
                targetDate, targetDate.AddDays(3), 1, 2, 100.0f, "voucher1.png"))
        };

        mockQueryService
            .Setup(s => s.GetReservationByStartDateAsync(It.IsAny<GetReservationByStartDate>()))
            .ReturnsAsync(reservations);

        var controller = new ReservationController(mockCommandService.Object, mockQueryService.Object);

        // Act
        var result = await controller.GetReservationByStartDateAsync(targetDate);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    /// <summary>
    /// BB-36: Get reservation user details by owner ID
    /// Input: Valid owner ID
    /// Expected: Reservation details for the owner
    /// </summary>
    [Fact]
    public async Task BB36_GetReservationUserDetails_WithValidOwnerId_ReturnsDetails()
    {
        // Arrange
        var mockCommandService = new Mock<IReservationCommandService>();
        var mockQueryService = new Mock<IReservationQueryService>();

        var localReservations = new List<LocalReservationResource>
        {
            new LocalReservationResource(1, DateTime.UtcNow, DateTime.UtcNow.AddDays(2), 2, 10, true, "voucher1.png"),
            new LocalReservationResource(2, DateTime.UtcNow.AddDays(3), DateTime.UtcNow.AddDays(5), 2, 11, false, "voucher2.png")
        };

        mockQueryService
            .Setup(s => s.GetReservationsByOwnerIdAsync(It.Is<GetReservationsByOwnerIdQuery>(q => q.OwnerId == 2)))
            .ReturnsAsync(localReservations);

        var controller = new ReservationController(mockCommandService.Object, mockQueryService.Object);

        // Act
        var result = await controller.GetReservationUserDetailsAsync(2);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        var details = Assert.IsType<ReservationDetailsResource>(okResult.Value);
        Assert.Equal(2, details.Reservations.Count());
    }

    #endregion

    #region Boundary Value Analysis Tests

    /// <summary>
    /// BVA-03: Reservation with same start and end date (minimum duration)
    /// Input: StartDate equals EndDate
    /// Expected: Success or validation error depending on business rules
    /// </summary>
    [Fact]
    public async Task BVA03_CreateReservation_WithSameDayReservation_Success()
    {
        // Arrange
        var mockCommandService = new Mock<IReservationCommandService>();
        var mockQueryService = new Mock<IReservationQueryService>();

        var sameDay = DateTime.UtcNow.AddDays(5);
        var sameDayResource = new CreateReservationResource(
            sameDay,
            sameDay, // Same day
            1,
            2,
            50.0f,
            "https://example.com/voucher.png"
        );

        var expectedReservation = new Reservation(new CreateReservationCommand(
            sameDayResource.StartDate,
            sameDayResource.EndDate,
            sameDayResource.UserId,
            sameDayResource.LocalId,
            sameDayResource.price,
            sameDayResource.voucherImageUrl
        ));

        mockCommandService
            .Setup(s => s.Handle(It.IsAny<CreateReservationCommand>()))
            .ReturnsAsync(expectedReservation);

        var controller = new ReservationController(mockCommandService.Object, mockQueryService.Object);

        // Act
        var result = await controller.CreateReservationAsync(sameDayResource);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(201, objectResult.StatusCode);
    }

    /// <summary>
    /// BVA-04: Reservation with zero price (boundary)
    /// Input: Price equals zero
    /// Expected: Success (free reservation) or validation error
    /// </summary>
    [Fact]
    public async Task BVA04_CreateReservation_WithZeroPrice_Success()
    {
        // Arrange
        var mockCommandService = new Mock<IReservationCommandService>();
        var mockQueryService = new Mock<IReservationQueryService>();

        var zeroPriceResource = new CreateReservationResource(
            DateTime.UtcNow.AddDays(5),
            DateTime.UtcNow.AddDays(7),
            1,
            2,
            0.0f, // Zero price (free)
            "https://example.com/voucher.png"
        );

        var expectedReservation = new Reservation(new CreateReservationCommand(
            zeroPriceResource.StartDate,
            zeroPriceResource.EndDate,
            zeroPriceResource.UserId,
            zeroPriceResource.LocalId,
            zeroPriceResource.price,
            zeroPriceResource.voucherImageUrl
        ));

        mockCommandService
            .Setup(s => s.Handle(It.IsAny<CreateReservationCommand>()))
            .ReturnsAsync(expectedReservation);

        var controller = new ReservationController(mockCommandService.Object, mockQueryService.Object);

        // Act
        var result = await controller.CreateReservationAsync(zeroPriceResource);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(201, objectResult.StatusCode);
    }

    /// <summary>
    /// BVA-05: Reservation starting today (minimum future date)
    /// Input: StartDate equals today
    /// Expected: Success
    /// </summary>
    [Fact]
    public async Task BVA05_CreateReservation_StartingToday_Success()
    {
        // Arrange
        var mockCommandService = new Mock<IReservationCommandService>();
        var mockQueryService = new Mock<IReservationQueryService>();

        var todayResource = new CreateReservationResource(
            DateTime.UtcNow.Date, // Today
            DateTime.UtcNow.Date.AddDays(2),
            1,
            2,
            100.0f,
            "https://example.com/voucher.png"
        );

        var expectedReservation = new Reservation(new CreateReservationCommand(
            todayResource.StartDate,
            todayResource.EndDate,
            todayResource.UserId,
            todayResource.LocalId,
            todayResource.price,
            todayResource.voucherImageUrl
        ));

        mockCommandService
            .Setup(s => s.Handle(It.IsAny<CreateReservationCommand>()))
            .ReturnsAsync(expectedReservation);

        var controller = new ReservationController(mockCommandService.Object, mockQueryService.Object);

        // Act
        var result = await controller.CreateReservationAsync(todayResource);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(201, objectResult.StatusCode);
    }

    /// <summary>
    /// BVA-06: Very long reservation period (365 days)
    /// Input: Reservation for entire year
    /// Expected: Success
    /// </summary>
    [Fact]
    public async Task BVA06_CreateReservation_WithYearLongDuration_Success()
    {
        // Arrange
        var mockCommandService = new Mock<IReservationCommandService>();
        var mockQueryService = new Mock<IReservationQueryService>();

        var longReservationResource = new CreateReservationResource(
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddDays(366), // 365 days duration
            1,
            2,
            36500.0f,
            "https://example.com/voucher.png"
        );

        var expectedReservation = new Reservation(new CreateReservationCommand(
            longReservationResource.StartDate,
            longReservationResource.EndDate,
            longReservationResource.UserId,
            longReservationResource.LocalId,
            longReservationResource.price,
            longReservationResource.voucherImageUrl
        ));

        mockCommandService
            .Setup(s => s.Handle(It.IsAny<CreateReservationCommand>()))
            .ReturnsAsync(expectedReservation);

        var controller = new ReservationController(mockCommandService.Object, mockQueryService.Object);

        // Act
        var result = await controller.CreateReservationAsync(longReservationResource);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(201, objectResult.StatusCode);
    }

    #endregion

    #region Equivalence Partitioning Tests

    /// <summary>
    /// EP-01: Valid user ID partition (positive integer)
    /// </summary>
    [Fact]
    public async Task EP01_GetReservations_WithPositiveUserId_ReturnsOk()
    {
        // Arrange
        var mockCommandService = new Mock<IReservationCommandService>();
        var mockQueryService = new Mock<IReservationQueryService>();

        mockQueryService
            .Setup(s => s.GetReservationsByUserIdAsync(It.IsAny<GetReservationsByUserId>()))
            .ReturnsAsync(new List<Reservation>());

        var controller = new ReservationController(mockCommandService.Object, mockQueryService.Object);

        // Act
        var result = await controller.GetReservationsByUserIdAsync(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    /// <summary>
    /// EP-02: Valid local ID partition (positive integer)
    /// </summary>
    [Fact]
    public async Task EP02_CreateReservation_WithValidLocalId_ReturnsCreated()
    {
        // Arrange
        var mockCommandService = new Mock<IReservationCommandService>();
        var mockQueryService = new Mock<IReservationQueryService>();

        var validResource = new CreateReservationResource(
            DateTime.UtcNow.AddDays(5),
            DateTime.UtcNow.AddDays(7),
            1,
            100, // Valid local ID
            150.50f,
            "https://example.com/voucher.png"
        );

        var expectedReservation = new Reservation(new CreateReservationCommand(
            validResource.StartDate,
            validResource.EndDate,
            validResource.UserId,
            validResource.LocalId,
            validResource.price,
            validResource.voucherImageUrl
        ));

        mockCommandService
            .Setup(s => s.Handle(It.IsAny<CreateReservationCommand>()))
            .ReturnsAsync(expectedReservation);

        var controller = new ReservationController(mockCommandService.Object, mockQueryService.Object);

        // Act
        var result = await controller.CreateReservationAsync(validResource);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(201, objectResult.StatusCode);
    }

    #endregion
}
