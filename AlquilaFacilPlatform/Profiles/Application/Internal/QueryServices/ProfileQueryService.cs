using AlquilaFacilPlatform.Shared.Application.Internal.OutboundServices;
using AlquilaFacilPlatform.Profiles.Domain.Model.Aggregates;
using AlquilaFacilPlatform.Profiles.Domain.Model.Queries;
using AlquilaFacilPlatform.Profiles.Domain.Repositories;
using AlquilaFacilPlatform.Profiles.Domain.Services;

namespace AlquilaFacilPlatform.Profiles.Application.Internal.QueryServices;

public class ProfileQueryService(IProfileRepository profileRepository, ISubscriptionExternalService subscriptionExternalService) : IProfileQueryService
{

    public async Task<Profile?> Handle(GetProfileByUserIdQuery query)
    {
        return await profileRepository.FindByUserIdAsync(query.UserId);
    }

    public async Task<string> Handle(GetSubscriptionStatusByUserIdQuery query)
    {
        return await subscriptionExternalService.GetSubscriptionStatusByUserId(query.Id);
    }
    public async Task<List<string>> Handle(GetProfileBankAccountsByUserIdQuery query)
    {
        var bankAccounts = new List<string>();
        var profile = await profileRepository.FindByUserIdAsync(query.UserId);
        if (profile.Id != 0) 
        {
            bankAccounts.Add(profile.BankAccountNumber);
            bankAccounts.Add(profile.InterbankAccountNumber);
        }
        return bankAccounts;
    }
}