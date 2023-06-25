namespace _00_Framework.Application;

public interface IAuthHelper
{
  Task<string> CreateToken(AuthViewModel authViewModel);
  Task<AuthViewModel> GetUserInfo();
}