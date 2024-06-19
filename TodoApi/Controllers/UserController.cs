using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using TodoApi.DTO;
using TodoApi.Models;
using TodoApi.Util;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDB _context;

        public UserController(AppDB context)
        {
            _context = context;
        }

        [HttpPost("CreateUser", Name = "CreateUser")]
        public async Task<ActionResult<CreateUserResponseDTO>> CreateUser(UserRequestDTO user)
        {
            CreateUserResponseDTO createUserResponse = new CreateUserResponseDTO();
            var salt = Util.GlobalFunction.GenerateSalt();
            var oldUser = _context.User.Where(u => u.user_name == user.UserName).FirstOrDefault();
            if (oldUser != null)
            {
                createUserResponse = new CreateUserResponseDTO
                {
                    status = "fail",
                    message = "User already exists"
                };
            }
            else
            {
                var newUser = new User
                {
                    user_name = user.UserName,
                    password = Util.GlobalFunction.ComputeHash(salt, user.Password),
                    salt = salt,
                    login_fail_count = 0,
                    is_lock = false
                };
                await _context.User.AddAsync(newUser);
                await _context.SaveChangesAsync();
                createUserResponse = new CreateUserResponseDTO
                {
                    status = "success",
                    message = "User created successfully"
                };
            }

            return Ok(createUserResponse);
        }

        [HttpPost("Login", Name = "Login")]
        public async Task<ActionResult<LoginResponseDTO>> Login(LoginRequestDTO user)
        {
            LoginResponseDTO loginResponseDTO = new LoginResponseDTO();
            var oldUser = _context.User.Where(u => u.user_name == user.UserName).FirstOrDefault();
            if (oldUser == null)
            {
                loginResponseDTO = new LoginResponseDTO
                {
                    status = "fail",
                    message = "Username or Password is incorrect"
                };
            }
            else
            {
                var hash = Util.GlobalFunction.ComputeHash(oldUser.salt, user.Password);
                if (hash == oldUser.password)
                {
                    Claim[] claims = GlobalFunction.CreateClaim(oldUser.user_id, new DateTimeOffset(DateTime.UtcNow).ToUniversalTime().ToUnixTimeSeconds().ToString());
                    var token = GlobalFunction.CreateJWTToken(claims);
                    loginResponseDTO = new LoginResponseDTO
                    {
                        status = "success",
                        message = "Login success",
                        accessToken = token
                    };
                }
                else
                {
                    loginResponseDTO = new LoginResponseDTO
                    {
                        status = "fail",
                        message = "Username or Password is incorrect"
                    };
                }
            }

            return Ok(loginResponseDTO);
        }

        [Authorize]
        [HttpGet("GetUser", Name = "GetUser")]
        public async Task<ActionResult<List<GetUserResponseDTO>>> GetUser()
        {
            List<GetUserResponseDTO> userList = await (from user in _context.User
                                                       select new GetUserResponseDTO
                                                       {
                                                           UserID = GlobalFunction.AES_Encrypt_ECB_128(user.user_id.ToString(), "sampleKey123"),
                                                           UserName = user.user_name,
                                                           IsLock = user.is_lock.ToString(),
                                                           FailCount = user.login_fail_count.ToString()
                                                       }).ToListAsync();
            return Ok(userList);
        }

        [Authorize]
        [HttpPut("UnlockUser", Name = "UnlockUser")]
        public async Task<ActionResult<CreateUserResponseDTO>> UnlockUser(UnlockUserRequestDTO id)
        {
            CreateUserResponseDTO createUserResponse = new CreateUserResponseDTO();
            int userID = int.Parse(GlobalFunction.AES_Decrypt_ECB_128(id.UserID, "sampleKey123"));
            var oldUser = _context.User.Where(u => u.user_id == userID).FirstOrDefault();
            if (oldUser == null)
            {
                createUserResponse = new CreateUserResponseDTO
                {
                    status = "fail",
                    message = "User not found"
                };
            }
            else
            {
                oldUser.is_lock = false;
                oldUser.login_fail_count = 0;
                await _context.SaveChangesAsync();
                createUserResponse = new CreateUserResponseDTO
                {
                    status = "success",
                    message = "User unlocked successfully"
                };
            }

            return Ok(createUserResponse);
        }

        [Authorize]
        [HttpGet("TestClaim", Name = "TestClaim")]
        public ActionResult<int> TestClaim()
        {
            int userID = int.Parse(HttpContext.User?.FindFirst(x => x.Type == "sid")?.Value ?? "0");
            return userID;
        }

    }
}