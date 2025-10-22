namespace AlquilaFacilPlatform.Shared.Domain.Repositories;

public interface IUnitOfWork
{
    Task CompleteAsync();
}