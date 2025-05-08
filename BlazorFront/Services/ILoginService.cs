namespace BlazorFront.Services
{
    public interface ILoginService
    {
        Task Login(string token);
        Task Logout();
    }
}