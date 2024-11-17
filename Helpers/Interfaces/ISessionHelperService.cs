namespace clone_oblt.Helpers.HelperInterfaces
{
    public interface ISessionHelperService
    {
        (string SessionId, string DeviceId) GetSessionInfo();
    }
}
