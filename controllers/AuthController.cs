using System.Data;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Text;
using DotnetAPI.Data;
using DotnetAPI.DTOs;
using DotnetPI.DTOs;
using Microsoft.AspNetCore.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using DotnetApi.Helpers;


namespace DotnetAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        

        private readonly AuthHelper _authHelper;
        public AuthController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
            _authHelper = new AuthHelper(config);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register(UserForRegistrationDTO userForRegistration)
        {
            if (userForRegistration.Password == userForRegistration.PasswordConfirm)
            {
                string sql = "SELECT Email FROM TutorialAppSchema.Auth WHERE Email = '" + userForRegistration.Email + "'";

                IEnumerable<string> existingUser = _dapper.LoadData<string>(sql);

                if (existingUser.Count() == 0)
                {
                    byte[] passwordSalt = new byte[128 / 8];
                    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                    {
                        rng.GetNonZeroBytes(passwordSalt);
                    }



                    byte[] passwordHash = _authHelper.GetPasswordHash(userForRegistration.Password, passwordSalt);

                    string sqlAddAuth = @"INSERT INTO TutorialAppSchema.Auth(
                    [Email], 
                    [PasswordHash], 
                    [PasswordSalt]) 
                    VALUES('" + userForRegistration.Email +
                    "', @PasswordHash, @PasswordSalt)";

                    List<SqlParameter> sqlParameters = new List<SqlParameter>();

                    SqlParameter passwordSaltParameter = new SqlParameter("@PasswordSalt", SqlDbType.VarBinary);

                    passwordSaltParameter.Value = passwordSalt;

                    SqlParameter passwordHashParameter = new SqlParameter("@PasswordHash", SqlDbType.VarBinary);

                    passwordHashParameter.Value = passwordHash;


                    sqlParameters.Add(passwordSaltParameter);
                    sqlParameters.Add(passwordHashParameter);



                    if (_dapper.ExecuteSqlWithParameters(sqlAddAuth, sqlParameters))
                    {
                        string sqlAddUser = $@"INSERT INTO TutorialAppSchema.Users(
                            [FirstName],
                            [LastName],
                            [Email],
                            [Gender],
                            [Active]
                        )        
                        VALUES
                        (
                            '{userForRegistration.FirstName}',
                            '{userForRegistration.LastName}',
                            '{userForRegistration.Email}',
                            '{userForRegistration.Gender}',
                            1
                        )";

                        if (_dapper.Execute(sqlAddUser))
                        {
                            return Ok();
                        }

                        throw new Exception("Failed to add user!");
                    }
                    throw new Exception("Failed To Add User");
                }
                throw new Exception("User with this email already exists");
            }
            throw new Exception("Passwords do not match");
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(UserForLoginDTO userForLogin)
        {
            string sqlForHashAndSalt = @"SELECT [PasswordHash], [PasswordSalt] 
            FROM TutorialAppSchema.Auth WHERE Email = '" + userForLogin.Email + "'";

            UserForLoginConfirmationDTO userForConfirmation = _dapper.LoadDataSingle<UserForLoginConfirmationDTO>(sqlForHashAndSalt);

            byte[] passwordHash = _authHelper.GetPasswordHash(userForLogin.Password, userForConfirmation.PasswordSalt);
            for (int index = 0; index < passwordHash.Length; index++)
            {
                if (passwordHash[index] != userForConfirmation.PasswordHash[index])
                {
                    return StatusCode(401, "Incorrect Password!");
                }
            }

            string userIdSql = "SELECT UserId FROM TutorialAppSchema.Users WHERE Email = '" + userForLogin.Email + "'";

            int userId = _dapper.LoadDataSingle<int>(userIdSql);

            return Ok(new Dictionary<string, string>
            {
                { "token", _authHelper.CreateToken(userId) },
            });

        }

        [HttpGet("RefreshToken")]
        public IActionResult RefreshToken()
        {
            string userId = User.FindFirst("userId")?.Value + ""; //This takes from Controller base and finds the first claim with the specified type of userId

            string userIdSql = "SELECT UserId FROM TutorialAppSchema.Users WHERE UserId = " + userId;

            int userIdFromDb = _dapper.LoadDataSingle<int>(userIdSql);

            return Ok(new Dictionary<string, string>
            {
                {"token", _authHelper.CreateToken(userIdFromDb)}
            });

        }

        // private byte[] GetPasswordHash(string password, byte[] passwordSalt)
        // {
        //     string passwordSaltPlusString = _config.GetSection("AppSettings:PasswordKey").Value + Convert.ToBase64String(passwordSalt);

        //     return KeyDerivation.Pbkdf2(
        //         password: password,
        //         salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
        //         prf: KeyDerivationPrf.HMACSHA256,
        //         iterationCount: 10000000,
        //         numBytesRequested: 256 / 8
        //     );

        // }

        // //Token Creation Method
        // private string CreateToken(int userId)
        // {
        //     Claim[] claims = new Claim[] {
        //         new Claim("userId", userId.ToString())
        //     };

        //     string? tokenKeyString = _config.GetSection("AppSettings:TokenKey").Value;

        //     SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(
        //         Encoding.UTF8.GetBytes(
        //             tokenKeyString != null ? tokenKeyString : ""
        //         )
        //     );

        //     SigningCredentials credentials = new SigningCredentials(
        //         tokenKey, SecurityAlgorithms.HmacSha512Signature
        //     );

        //     SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
        //     {
        //         Subject = new ClaimsIdentity(claims),
        //         SigningCredentials = credentials,
        //         Expires = DateTime.Now.AddDays(1),

        //     };

        //     JwtSecurityTokenHandler tokenhandler = new JwtSecurityTokenHandler();

        //     SecurityToken token = tokenhandler.CreateToken(descriptor);

        //     return tokenhandler.WriteToken(token);

        // }

    }
}