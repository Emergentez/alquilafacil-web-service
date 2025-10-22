using AlquilaFacilPlatform.Profiles.Domain.Model.Aggregates;
using AlquilaFacilPlatform.Profiles.Domain.Model.Queries;

namespace AlquilaFacilPlatform.Profiles.Domain.Services;

public interface IProfileQueryService
{
    Task<Profile?> Handle(GetProfileByUserIdQuery query);
    Task<string> Handle(GetSubscriptionStatusByUserIdQuery query);
    Task<List<string>> Handle(GetProfileBankAccountsByUserIdQuery query);
}