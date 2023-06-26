using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace _00_Framework.Application;

public class AuthHelper :IAuthHelper 
{
    private readonly Jwt _jwtSetting;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public AuthHelper(IOptions<Jwt> jwtSetting, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _jwtSetting = jwtSetting.Value;
    }
    /// <summary>
    /// Create a token from model you give
    /// </summary>
    /// <param name="authViewModel">a model to make claims</param>
    /// <returns></returns>
    public Task<string> CreateToken(AuthViewModel authViewModel)
    {
        var tokenDescriptor = SecurityTokenDescriptor(authViewModel,_jwtSetting);

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken? token = tokenHandler.CreateToken(tokenDescriptor);

        return Task.FromResult<string>(tokenHandler.WriteToken(token));
    }

    private SecurityTokenDescriptor SecurityTokenDescriptor(AuthViewModel authViewModel, Jwt jwtSetting)
    {
        var eckeyTemp=Encoding.UTF8.GetBytes( jwtSetting.EncryptKey);
        byte[] eckey=new byte[256/8];
        Array.Copy(eckeyTemp,eckey,256/8);
        SymmetricSecurityKey symmetricSecurityKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._jwtSetting.SigningKey));
        SigningCredentials signingKey =
            new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);
        SymmetricSecurityKey encryptSymmetricKey = new SymmetricSecurityKey(eckey);
        EncryptingCredentials encryptKey =
            new EncryptingCredentials(encryptSymmetricKey,SecurityAlgorithms.Aes256KW, SecurityAlgorithms.Aes256CbcHmacSha512);

        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
        {
            Issuer = jwtSetting.Issuer,
            IssuedAt = DateTime.Now.AddSeconds(-10),
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("UserId", authViewModel.Id.ToString()),
                new Claim(ClaimTypes.Email, authViewModel.Username)
            }),
            Expires = DateTime.Now.AddMinutes(jwtSetting.ExpireTimeInMinute),
            NotBefore = DateTime.Now.AddSeconds(-20),
            SigningCredentials = signingKey,
            EncryptingCredentials = encryptKey
        };
        return tokenDescriptor;
    }


    public  Task<AuthViewModel> GetUserInfo()
    {
        AuthViewModel authViewModel;
        if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            return null;
        string userId = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "UserId")?.Value;
        string userName = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.Email)?.Value;
        authViewModel = new AuthViewModel(Convert.ToInt32( userId), userName);
        return Task.FromResult( authViewModel);
    }
  
}