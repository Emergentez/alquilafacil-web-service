using AlquilaFacilPlatform.Shared.Application.Internal.OutboundServices;
using AlquilaFacilPlatform.IAM.Domain.Model.Commands;
using AlquilaFacilPlatform.IAM.Domain.Repositories;
using AlquilaFacilPlatform.IAM.Domain.Services;
using AlquilaFacilPlatform.Shared.Domain.Repositories;

namespace AlquilaFacilPlatform.IAM.Application.Internal.CommandServices;

public class SeedAdminCommandService(IUserRepository repository, IUserCommandService commandService, IProfilesExternalService externalService, IUnitOfWork unitOfWork) : ISeedAdminCommandService
{
    public async Task Handle(SeedAdminCommand command)
    {
        const string username = "Admin";
        const string password = "Admin@123";
        const string email = "admin@gmail.com";
        const string name = "Admin";
        const string fatherName = "Alquila";
        const string motherName = "Facil";
        const string dateOfBirth = "01-01-2001";
        const string documentNumber = "000000000";
        const string phone = "000000000";
        const string bankAccountNumber = "24517896321054781236";
        const string interbankAccountNumber = "01824517896321054789";
        
        if (await repository.ExistsByUsername(username)) return;
        var signUpCommand = new SignUpCommand(username, password, email, name, fatherName, motherName, dateOfBirth,
            documentNumber, phone);
        var user = await commandService.Handle(signUpCommand);
        await externalService.UpdateProfile(name, fatherName, motherName, dateOfBirth, documentNumber, phone,
            bankAccountNumber, interbankAccountNumber, user!.Id);
        user.UpgradeToAdmin();
        repository.Update(user);
        await unitOfWork.CompleteAsync();
    }
}