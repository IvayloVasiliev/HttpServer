namespace Panda.Services
{
    public interface IReceiptsService
    {
        void CreateFromPackage(decimal weight, string packageId, string recepientId);
    }
}
