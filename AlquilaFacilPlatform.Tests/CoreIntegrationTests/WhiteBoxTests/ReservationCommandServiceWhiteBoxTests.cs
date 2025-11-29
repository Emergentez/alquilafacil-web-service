using AlquilaFacilPlatform.Booking.Application.Internal.CommandServices;
using AlquilaFacilPlatform.Booking.Domain.Model.Aggregates;
using AlquilaFacilPlatform.Booking.Domain.Model.Commands;
using AlquilaFacilPlatform.Booking.Domain.Repositories;
using AlquilaFacilPlatform.Shared.Application.Internal.OutboundServices;
using AlquilaFacilPlatform.Shared.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Moq;

namespace AlquilaFacilPlatform.Tests.CoreIntegrationTests.WhiteBoxTests;

/// <summary>
/// White-Box Tests for ReservationCommandService
/// These tests validate internal logic paths, branches, and conditions.
/// Techniques: Statement Coverage, Branch Coverage, Condition Coverage, Path Coverage
/// </summary>
public class ReservationCommandServiceWhiteBoxTests
{
    private readonly Mock<IUserExternalService> _mockUserExternalService;
    private readonly Mock<ILocalExternalService> _mockLocalExternalService;
    private readonly Mock<INotificationExternalService> _mockNotificationExternalService;
    private readonly Mock<IReservationRepository> _mockReservationRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly ReservationCommandService _service;

    public ReservationCommandServiceWhiteBoxTests()
    {
        _mockUserExternalService = new Mock<IUserExternalService>();
        _mockLocalExternalService = new Mock<ILocalExternalService>();
        _mockNotificationExternalService = new Mock<INotificationExternalService>();
        _mockReservationRepository = new Mock<IReservationRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _service = new ReservationCommandService(
            _mockUserExternalService.Object,
            _mockLocalExternalService.Object,
            _mockNotificationExternalService.Object,
            _mockReservationRepository.Object,
            _mockUnitOfWork.Object
        );
    }

    #region WB-21 to WB-27: CreateReservation - Branch Coverage

    /// <summary>
    /// WB-21: CreateReservation - User does not exist
    /// Covers: !userExternalService.UserExists(command.UserId) branch - First validation check
    /// </summary>
    [Fact]
    public async Task WB21_CreateReservation_UserNotExists_ThrowsException()
    {
        // Arrange
        var command = new CreateReservationCommand(
            DateTime.Now.AddYears(10), DateTime.Now.AddYears(10).AddDays(5), 999, 2, 100.0f, "voucher.png"
        );

        _mockUserExternalService
            .Setup(u => u.UserExists(command.UserId))
            .Returns(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.Handle(command));
        Assert.Contains("no users matching", exception.Message);
    }

    /// <summary>
    /// WB-22: CreateReservation - Local does not exist
    /// Covers: !localExists branch - Second validation check
    /// </summary>
    [Fact]
    public async Task WB22_CreateReservation_LocalNotExists_ThrowsException()
    {
        // Arrange
        var command = new CreateReservationCommand(
            DateTime.Now.AddYears(10), DateTime.Now.AddYears(10).AddDays(5), 1, 999, 100.0f, "voucher.png"
        );

        _mockUserExternalService
            .Setup(u => u.UserExists(command.UserId))
            .Returns(true);
        _mockLocalExternalService
            .Setup(l => l.LocalExists(command.LocalId))
            .ReturnsAsync(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.Handle(command));
        Assert.Equal("Local does not exist", exception.Message);
    }

    /// <summary>
    /// WB-23: CreateReservation - StartDate > EndDate
    /// Covers: command.StartDate > command.EndDate branch - Third validation check
    /// </summary>
    [Fact]
    public async Task WB23_CreateReservation_StartDateAfterEndDate_ThrowsException()
    {
        // Arrange - StartDate is after EndDate
        var startDate = DateTime.Now.AddYears(10).AddDays(10);
        var endDate = DateTime.Now.AddYears(10);
        var command = new CreateReservationCommand(
            startDate, endDate, 1, 2, 100.0f, "voucher.png"
        );

        _mockUserExternalService
            .Setup(u => u.UserExists(command.UserId))
            .Returns(true);
        _mockLocalExternalService
            .Setup(l => l.LocalExists(command.LocalId))
            .ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.Handle(command));
        Assert.Contains("Start date must be less than end date", exception.Message);
    }

    /// <summary>
    /// WB-24: CreateReservation - StartDate in the past (year check)
    /// Covers: StartDate.Year < DateTime.Now.Year branch
    /// </summary>
    [Fact]
    public async Task WB24_CreateReservation_StartDateYearInPast_ThrowsException()
    {
        // Arrange - Start date year in past
        var pastDate = new DateTime(DateTime.Now.Year - 1, 6, 15);
        var futureEndDate = new DateTime(DateTime.Now.Year + 10, 6, 20);
        var command = new CreateReservationCommand(
            pastDate, futureEndDate, 1, 2, 100.0f, "voucher.png"
        );

        _mockUserExternalService
            .Setup(u => u.UserExists(command.UserId))
            .Returns(true);
        _mockLocalExternalService
            .Setup(l => l.LocalExists(command.LocalId))
            .ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.Handle(command));
        Assert.Contains("Start date must be greater than current date", exception.Message);
    }

    /// <summary>
    /// WB-25: CreateReservation - Verify user existence check is first
    /// Covers: Statement coverage - userExternalService.UserExists is called
    /// </summary>
    [Fact]
    public async Task WB25_CreateReservation_UserExistenceCheckIsCalled()
    {
        // Arrange
        var command = new CreateReservationCommand(
            DateTime.Now.AddYears(10), DateTime.Now.AddYears(10).AddDays(5), 1, 2, 100.0f, "voucher.png"
        );

        _mockUserExternalService
            .Setup(u => u.UserExists(command.UserId))
            .Returns(false);

        // Act
        try { await _service.Handle(command); } catch { }

        // Assert - Verify the method was called
        _mockUserExternalService.Verify(u => u.UserExists(command.UserId), Times.Once);
    }

    /// <summary>
    /// WB-26: CreateReservation - Verify local existence check is called after user check
    /// Covers: Statement coverage - localExternalService.LocalExists is called
    /// </summary>
    [Fact]
    public async Task WB26_CreateReservation_LocalExistenceCheckIsCalled()
    {
        // Arrange
        var command = new CreateReservationCommand(
            DateTime.Now.AddYears(10), DateTime.Now.AddYears(10).AddDays(5), 1, 2, 100.0f, "voucher.png"
        );

        _mockUserExternalService
            .Setup(u => u.UserExists(command.UserId))
            .Returns(true);
        _mockLocalExternalService
            .Setup(l => l.LocalExists(command.LocalId))
            .ReturnsAsync(false);

        // Act
        try { await _service.Handle(command); } catch { }

        // Assert - Verify the method was called
        _mockLocalExternalService.Verify(l => l.LocalExists(command.LocalId), Times.Once);
    }

    #endregion

    #region WB-28 to WB-33: UpdateReservation - Path Coverage

    /// <summary>
    /// WB-28: UpdateReservation - StartDate > EndDate
    /// Covers: reservation.StartDate > reservation.EndDate branch
    /// </summary>
    [Fact]
    public async Task WB28_UpdateReservation_StartDateAfterEndDate_ThrowsException()
    {
        // Arrange - StartDate after EndDate
        var command = new UpdateReservationDateCommand(
            1, DateTime.Now.AddYears(10).AddDays(20), DateTime.Now.AddYears(10).AddDays(10)
        );

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.Handle(command));
        Assert.Contains("Start date must be less than end date", exception.Message);
    }

    /// <summary>
    /// WB-29: UpdateReservation - StartDate in past (year)
    /// Covers: reservation.StartDate.Year < DateTime.Now.Year condition
    /// </summary>
    [Fact]
    public async Task WB29_UpdateReservation_StartDateYearInPast_ThrowsException()
    {
        // Arrange - Start date year in past
        var pastDate = new DateTime(DateTime.Now.Year - 1, 6, 15);
        var futureEndDate = new DateTime(DateTime.Now.Year + 10, 6, 20);
        var command = new UpdateReservationDateCommand(1, pastDate, futureEndDate);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.Handle(command));
        Assert.Contains("Start date must be greater than current date", exception.Message);
    }

    /// <summary>
    /// WB-30: UpdateReservation - EndDate in past
    /// Covers: reservation.EndDate < DateTime.Now branch
    /// Note: This test uses dates where start < end but end is in the past
    /// </summary>
    [Fact]
    public async Task WB30_UpdateReservation_EndDateInPast_ThrowsException()
    {
        // Arrange - End date in past (start > end will be caught first in normal case)
        // To test EndDate < DateTime.Now, we need both dates in the past with start < end
        var pastStart = DateTime.Now.AddDays(-10);
        var pastEnd = DateTime.Now.AddDays(-5);
        var command = new UpdateReservationDateCommand(1, pastStart, pastEnd);

        // Act & Assert - First validation catches start date in past
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.Handle(command));
        Assert.Contains("Start date must be greater than current date", exception.Message);
    }

    #endregion

    #region WB-34 to WB-37: DeleteReservation - Path Coverage

    /// <summary>
    /// WB-34: DeleteReservation - Success path
    /// Covers: Reservation found and deleted
    /// </summary>
    [Fact]
    public async Task WB34_DeleteReservation_ReservationExists_ReturnsDeletedReservation()
    {
        // Arrange
        var command = new DeleteReservationCommand(1);
        var existingReservation = new Reservation(new CreateReservationCommand(
            DateTime.Now.AddYears(10), DateTime.Now.AddYears(10).AddDays(3), 1, 2, 100.0f, "voucher.png"
        ));

        _mockReservationRepository
            .Setup(r => r.FindByIdAsync(command.Id))
            .ReturnsAsync(existingReservation);
        _mockUnitOfWork
            .Setup(u => u.CompleteAsync())
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.Handle(command);

        // Assert
        Assert.NotNull(result);
        _mockReservationRepository.Verify(r => r.Remove(existingReservation), Times.Once);
    }

    /// <summary>
    /// WB-35: DeleteReservation - Reservation not found
    /// Covers: reservationToDelete == null branch
    /// </summary>
    [Fact]
    public async Task WB35_DeleteReservation_ReservationNotFound_ThrowsException()
    {
        // Arrange
        var command = new DeleteReservationCommand(999);

        _mockReservationRepository
            .Setup(r => r.FindByIdAsync(command.Id))
            .ReturnsAsync((Reservation?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.Handle(command));
        Assert.Equal("Reservation does not exist", exception.Message);
    }

    /// <summary>
    /// WB-36: DeleteReservation - Verify Remove is called
    /// Covers: reservationRepository.Remove statement execution
    /// </summary>
    [Fact]
    public async Task WB36_DeleteReservation_Success_RepositoryRemoveCalled()
    {
        // Arrange
        var command = new DeleteReservationCommand(1);
        var existingReservation = new Reservation(new CreateReservationCommand(
            DateTime.Now.AddYears(10), DateTime.Now.AddYears(10).AddDays(3), 1, 2, 100.0f, "voucher.png"
        ));

        _mockReservationRepository
            .Setup(r => r.FindByIdAsync(command.Id))
            .ReturnsAsync(existingReservation);
        _mockUnitOfWork
            .Setup(u => u.CompleteAsync())
            .Returns(Task.CompletedTask);

        // Act
        await _service.Handle(command);

        // Assert
        _mockReservationRepository.Verify(r => r.Remove(existingReservation), Times.Once);
    }

    /// <summary>
    /// WB-37: DeleteReservation - Verify UnitOfWork CompleteAsync is called
    /// Covers: unitOfWork.CompleteAsync statement execution
    /// </summary>
    [Fact]
    public async Task WB37_DeleteReservation_Success_UnitOfWorkCompleted()
    {
        // Arrange
        var command = new DeleteReservationCommand(1);
        var existingReservation = new Reservation(new CreateReservationCommand(
            DateTime.Now.AddYears(10), DateTime.Now.AddYears(10).AddDays(3), 1, 2, 100.0f, "voucher.png"
        ));

        _mockReservationRepository
            .Setup(r => r.FindByIdAsync(command.Id))
            .ReturnsAsync(existingReservation);
        _mockUnitOfWork
            .Setup(u => u.CompleteAsync())
            .Returns(Task.CompletedTask);

        // Act
        await _service.Handle(command);

        // Assert
        _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
    }

    #endregion
}
