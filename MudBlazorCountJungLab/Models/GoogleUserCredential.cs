using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MudBlazorCountJungLab.Models
{
    public class GoogleResponse
    {
        public string ClientId { get; set; } = "";
        public string SelectedBy { get; set; } = "";
        public string Credential { get; set; } = "";
    }

    public class GoogleUserCredential
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string EMail { get; set; } = "";
        public string Picture { get; set; } = "";
        public string GivenName { get; set; } = "";
        public string FamilyName { get; set; } = "😃";
        public string Locale { get; set; } = "";

        public ClaimsPrincipal ToClaimsPrincipal() => new(new ClaimsIdentity(new Claim[]
        {
            new (ClaimTypes.Name, Username), new (ClaimTypes.Hash, Password), new (ClaimTypes.Email,EMail), new (ClaimTypes.UserData, Picture),
            new (ClaimTypes.GivenName,GivenName), new (ClaimTypes.Surname,FamilyName), new (ClaimTypes.Locality, Locale)
        }, "GoogleCredential"));

        public static GoogleUserCredential FromClaimsPrincipal(ClaimsPrincipal principal) => new()
        {
            Username = principal.FindFirst(ClaimTypes.Name)?.Value ?? "",
            Password = principal.FindFirst(ClaimTypes.Hash)?.Value ?? "",
            EMail = principal.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty,
            Picture = principal.FindFirst(ClaimTypes.UserData)?.Value ?? string.Empty,
            GivenName = principal.FindFirst(ClaimTypes.GivenName)?.Value ?? string.Empty,
            FamilyName = principal.FindFirst(ClaimTypes.Surname)?.Value ?? string.Empty,
            Locale = principal.FindFirst(ClaimTypes.Locality)?.Value ?? string.Empty
        };

        public static GoogleUserCredential? GetUserInfoFromGoogleJWT(string jwt)
        {
            GoogleUserCredential? userInfo = null;
            JwtSecurityTokenHandler tokenHandler = new();

            if (tokenHandler.CanReadToken(jwt))
            {
                JwtSecurityToken jwtSecurityToken = tokenHandler.ReadJwtToken(jwt);
                userInfo = new GoogleUserCredential
                {
                    Username = jwtSecurityToken.Claims.First(c => c.Type == "name").Value,
                    //Password = ""
                    EMail = jwtSecurityToken.Claims.First(c => c.Type == "email").Value,
                    Picture = jwtSecurityToken.Claims.First(c => c.Type == "picture").Value,
                    GivenName = jwtSecurityToken.Claims.First(c => c.Type == "given_name").Value,
                    FamilyName = jwtSecurityToken.Claims.First(c => c.Type == "family_name").Value,
                    Locale = jwtSecurityToken.Claims.First(c => c.Type == "locale").Value
                };
            }

            return userInfo;
        }
        public string CreateJWTbyCredential1()
        {

            ClaimsIdentity identity = new ClaimsIdentity(new List<Claim>
            {
                 new (ClaimTypes.Name, Username), new (ClaimTypes.Hash, Password), new (ClaimTypes.Email,EMail), new (ClaimTypes.UserData, Picture),
            new (ClaimTypes.GivenName,GivenName), new (ClaimTypes.Surname,FamilyName), new (ClaimTypes.Locality, Locale)
            }, "GoogleCredential");

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = identity,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("This is my secret key")), SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest)
            };
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler()
            {
                TokenLifetimeInMinutes = 60 * 24
            };
            SecurityToken plainToken = handler.CreateToken(tokenDescriptor);
            string encodedToken = handler.WriteToken(plainToken);
            return encodedToken;
        }
        public string CreateJWTbyCredential2()
        {
            List<Claim> claims = new List<Claim>
            {
                 new (ClaimTypes.Name, Username), new (ClaimTypes.Hash, Password), new (ClaimTypes.Email,EMail), new (ClaimTypes.UserData, Picture),
                new (ClaimTypes.GivenName,GivenName), new (ClaimTypes.Surname,FamilyName), new (ClaimTypes.Locality, Locale)
            };
            //the secret key must have 16 characters in it
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("This is my secret key"));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                                   claims: claims,
                                   expires: DateTime.UtcNow.AddDays(1),
                                   signingCredentials: cred);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
