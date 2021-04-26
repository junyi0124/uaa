using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;

using UAA.AuthApi.Data;
using UAA.ExtensionsHttp;
using UAA.Model;
using UAA.Entity;
using UAA.AuthApi.Helper;

namespace UAA.AuthApi.Services
{
  public interface IAccountService
  {
    Task<(AuthenticateResponse, AppException)> Authenticate(AuthenticateRequest model, string ipAddress);
    Task<AuthenticateResponse> RefreshToken(string refreshToken, string ipAddress);

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




    public async Task<(AuthenticateResponse,AppException)> Authenticate(AuthenticateRequest model, string ipAddress)
    {
      var account = await _context.Users.FirstOrDefaultAsync(x => x.UserName == model.UserName);

      if (account == null 
        || account.Status.CheckStatus(1) 
        || !HmacHelper.Verify(model.Password, account.PasswordHash, account.PasswordSalt))
        return (null, new AppException("username or password incurrect"));

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

    public Task<AuthenticateResponse> RefreshToken(string token, string ipAddress)
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

    private RefreshToken generateRefreshToken(string ipAddress)
    {
      return new RefreshToken
      {
        Token = randomTokenString(),
        Expires = DateTime.UtcNow.AddDays(7),
        Created = DateTime.UtcNow,
        CreatedByIp = ipAddress
      };
    }

    private (RefreshToken, UserAccount) getRefreshToken(string token)
    {
      var account = _context.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
      if (account == null) throw new AppException("Invalid token");
      var refreshToken = account.RefreshTokens.Single(x => x.Token == token);
      if (!refreshToken.IsActive) throw new AppException("Invalid token");
      return (refreshToken, account);
    }

    private string generateJwtToken(UserAccount account)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new[] { new Claim("id", account.Id.ToString()) }),
        Expires = DateTime.UtcNow.AddMinutes(15),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
          SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      return tokenHandler.WriteToken(token);
    }
  }
}


