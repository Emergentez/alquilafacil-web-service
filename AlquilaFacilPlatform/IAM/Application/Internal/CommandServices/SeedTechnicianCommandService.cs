using AlquilaFacilPlatform.IAM.Domain.Model.Commands;
using AlquilaFacilPlatform.IAM.Domain.Repositories;
using AlquilaFacilPlatform.IAM.Domain.Services;
using AlquilaFacilPlatform.Shared.Application.Internal.OutboundServices;
using AlquilaFacilPlatform.Shared.Domain.Repositories;

namespace AlquilaFacilPlatform.IAM.Application.Internal.CommandServices;

public class SeedTechnicianCommandService(IUserRepository repository, IUserCommandService commandService, IProfilesExternalService externalService, IUnitOfWork unitOfWork) : ISeedTechnicianCommandService
{
    public async Task Handle(SeedTechnicianCommand command)
    {
        const string username = "Technician";
        const string password = "Technician@123";
        const string email = "technician@gmail.com";
        const string name = "Technician";
        const string fatherName = "Alquila";
        const string motherName = "Facil";
        const string dateOfBirth = "01-01-2001";
        const string documentNumber = "000000001";
        const string phone = "000000001";
        const string bankAccountNumber = "24517896321054781237";
        const string interbankAccountNumber = "01824517896321054790";
        
        if (await repository.ExistsByUsername(username)) return;
        var signUpCommand = new SignUpCommand(username, password, email, name, fatherName, motherName, dateOfBirth,
            documentNumber, phone);
        var user = await commandService.Handle(signUpCommand);
        await externalService.UpdateProfile(name, fatherName, motherName, dateOfBirth, documentNumber, phone,
            bankAccountNumber, interbankAccountNumber, user!.Id);
        user.UpgradeToTechnician();
        repository.Update(user);
        await unitOfWork.CompleteAsync();
    }
}