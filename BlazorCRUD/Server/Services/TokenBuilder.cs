using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlazorCRUD.Server.Services
{
	public interface ITokenBuilder
	{
		public string BuildToken(Claim[] subject, DateTime expires);
	}

	public class TokenBuilder : ITokenBuilder
	{
		private readonly byte[] _key;

		public TokenBuilder(IConfiguration config)
		{
			_key = Encoding.ASCII.GetBytes(config["TokenKey"]);
		}

		public string BuildToken(Claim[] subject, DateTime expires)
		{
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new(subject),
				Expires = expires,
				SigningCredentials = new SigningCredentials(
					new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature
				)
			};

			var handler = new JwtSecurityTokenHandler();
			return handler.WriteToken(handler.CreateToken(tokenDescriptor));
		}
	}
}
