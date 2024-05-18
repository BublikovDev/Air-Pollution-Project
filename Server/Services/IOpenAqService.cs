namespace Server.Services
{
    public interface IOpenAqService
    {
        public Task GetLocations(int country);
    }
}
