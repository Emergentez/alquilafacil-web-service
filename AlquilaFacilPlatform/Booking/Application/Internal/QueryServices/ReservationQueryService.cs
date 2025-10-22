using AlquilaFacilPlatform.Booking.Domain.Model.Aggregates;
using AlquilaFacilPlatform.Booking.Domain.Model.Queries;
using AlquilaFacilPlatform.Booking.Domain.Repositories;
using AlquilaFacilPlatform.Booking.Domain.Services;
using AlquilaFacilPlatform.Booking.Interfaces.REST.Resources;
using AlquilaFacilPlatform.Shared.Application.Internal.OutboundServices;
using AlquilaFacilPlatform.Subscriptions.Domain.Model.ValueObjects;

namespace AlquilaFacilPlatform.Booking.Application.Internal.QueryServices;

public class ReservationQueryService(IReservationRepository reservationRepository, ILocalExternalService localExternalService, ISubscriptionExternalService subscriptionExternalService) : IReservationQueryService
{
    public async Task<IEnumerable<Reservation>> GetReservationsByUserIdAsync(GetReservationsByUserId query)
    {
        return await reservationRepository.GetReservationsByUserIdAsync(query.UserId);
    }

    public async Task<IEnumerable<Reservation>> GetReservationByStartDateAsync(GetReservationByStartDate query)
    {
        return await reservationRepository.GetReservationByStartDateAsync(query.StartDate);
    }

    public async Task<IEnumerable<Reservation>> GetReservationByEndDateAsync(GetReservationByEndDate query)
    {
        return await reservationRepository.GetReservationByEndDateAsync(query.EndDate);
    }

    public async Task<IEnumerable<LocalReservationResource>> GetReservationsByOwnerIdAsync(GetReservationsByOwnerIdQuery query)
    {
        var localReservations = new List<LocalReservationResource>();
        var ownerLocals = await localExternalService.GetLocalsByUserId(query.OwnerId);
        // Extracting local IDs from the locals
        var localIdsList = ownerLocals.Select(local => local.Id).ToList();
        var reservations = await reservationRepository.GetReservationsByLocalIdsListAsync(localIdsList);
        // Extracting user IDs from the reservations
        var userIdsList = reservations.Select(reservation => reservation.UserId).ToList();
        // Getting subscriptions by user IDs
        var subscriptions = await subscriptionExternalService.GetSubscriptionByUserIdsList(userIdsList);
        // Creating a dictionary of subscriptions by user ID
        var subscriptionDict = subscriptions
            .GroupBy(s => s.UserId)
            .ToDictionary(g => g.Key, g => g.First());
        foreach (var reservation in reservations)
        {
            subscriptionDict.TryGetValue(reservation.UserId, out var subscription);
            var localReservationResource = new LocalReservationResource(
                reservation.Id,
                reservation.StartDate,
                reservation.EndDate,
                reservation.UserId,
                reservation.LocalId,
                subscription?.PlanId == (int)ESubscriptionStatus.Active,
                reservation.VoucherImageUrl
            );
            localReservations.Add(localReservationResource);
        }

        return localReservations;
    }
}