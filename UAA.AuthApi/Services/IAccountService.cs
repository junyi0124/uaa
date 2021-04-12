using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UAA.AuthApi.Data;
using UAA.ExtensionsHttp;
using UAA.Model;
using UAA.Model.Auth;

namespace UAA.AuthApi.Services
{
  public interface IAccountService
  {
    AuthenticateResponse Authenticate(AuthenticateRequest model, string v);
    AuthenticateResponse RefreshToken(string refreshToken, string v);
  }

  public class AccountService : IAccountService
  {
    private readonly UaaDbContext _context;
    private readonly IMapper _mapper;
    private readonly AppSettings _appSettings;

    public AccountService(
        UaaDbContext context,
        IMapper mapper,
        IOptions<AppSettings> appSettings)
    {
      _context = context;
      _mapper = mapper;
      _appSettings = appSettings.Value;
    }


    public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress)
    {
      var account = _context.Users.SingleOrDefault(x => x.UserName == model.UserName);

      if (account == null || account.Status.CheckStatus(1) || !BC.Verify(model.Password, account.PasswordHash))
        return (null, "");

      // authentication successful so generate jwt and refresh tokens
      var jwtToken = generateJwtToken(account);
      var refreshToken = generateRefreshToken(ipAddress);
      account.RefreshTokens.Add(refreshToken);

      // remove old refresh tokens from account
      removeOldRefreshTokens(account);

      // save changes to db
      _context.Update(account);
      _context.SaveChanges();

      var response = _mapper.Map<AuthenticateResponse>(account);
      response.JwtToken = jwtToken;
      response.RefreshToken = refreshToken.Token;
      return response;
    }

    public AuthenticateResponse RefreshToken(string token, string ipAddress)
    {
      var (refreshToken, account) = getRefreshToken(token);

      // replace old refresh token with a new one and save
      var newRefreshToken = generateRefreshToken(ipAddress);
      refreshToken.Revoked = DateTime.UtcNow;
      refreshToken.RevokedByIp = ipAddress;
      refreshToken.ReplacedByToken = newRefreshToken.Token;
      account.RefreshTokens.Add(newRefreshToken);

      removeOldRefreshTokens(account);

      _context.Update(account);
      _context.SaveChanges();

      // generate new jwt
      var jwtToken = generateJwtToken(account);

      var response = _mapper.Map<AuthenticateResponse>(account);
      response.JwtToken = jwtToken;
      response.RefreshToken = newRefreshToken.Token;
      return response;
    }

    private string generateJwtToken(Account account)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new[] { new Claim("id", account.Id.ToString()) }),
        Expires = DateTime.UtcNow.AddMinutes(15),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      return tokenHandler.WriteToken(token);
    }
  }
}


