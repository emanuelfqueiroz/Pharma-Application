namespace PharmaRep.Application.Common
{
    public interface IIdentifierService
    {
        int GetUserId();
        bool HasUserId();
    }

    public interface IPharmaUnitOfWork
    {
        void Commit();
        void Rollback();
        void BeginTransaction();
    }
}
