namespace music_store.src.user;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using music_store.src.user.Dtos;
using music_store.src.user.interfaces;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
  private readonly IUserService _userService;
  private readonly IHttpContextAccessor _httpContextAccessor;

  private readonly IConfiguration _configuration;
  public UserController(IUserService userService, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
  {
    _configuration = configuration;
    _httpContextAccessor = httpContextAccessor;
    _userService = userService ?? throw new ArgumentNullException(nameof(userService));
  }

  [HttpGet("{id}")]
  public IActionResult GetUser(Guid id)
  {
    var user = _userService.GetUserById(id);

    if (user == null)
    {
      return NotFound("User not Found");
    }

    return Ok(user);
  }


  [HttpPost("login")]
  public IActionResult Login([FromBody] UserLoginDto loginRequest)
  {
    var user = _userService.ValidateUserCredentials(loginRequest.Email, loginRequest.Password);

    if (user == null)
    {
      return Unauthorized(new { Message = "Invalid Username or Password" });
    }

    return Ok(CreateToken(user));

  }

  private string CreateToken(UserEntity user)
  {
    List<Claim> claims =
    [
                new(ClaimTypes.Name, user.Username),
                new("userId", user.UserId.ToString()),
                new(ClaimTypes.Role, "Admin"),
                new(ClaimTypes.Role, "User"),
            ];

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
    _configuration.GetSection("JWT:Token").Value!));
    Console.WriteLine(key);
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

    var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );

    var jwt = new JwtSecurityTokenHandler().WriteToken(token);

    return jwt;
  }
  [HttpPost("register")]
  public IActionResult Register([FromBody] UserRegisterDto registerRequest)
  {
    // Implement user registration logic
    // You might want to hash the Password and save the user to the database
    var user = _userService.GetUserByEmail(registerRequest.Email);
    if (user != null)
    {
      return Ok("User Already Exists");
    }
    _userService.RegisterUser(registerRequest.Username, registerRequest.Email, registerRequest.Password);

    return Ok("User Created");
  }

  [HttpPost("testtoken"), Authorize(Roles = "Admin,User")]
  public IActionResult Testtoken([FromBody] string Name)
  {
    var result = Guid.Empty;
    if (_httpContextAccessor.HttpContext is not null)
    {
      var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
      Console.WriteLine("Received Token: " + token);

      var userIdString = _httpContextAccessor.HttpContext.User.FindFirstValue("userId");

      if (!string.IsNullOrEmpty(userIdString) && Guid.TryParse(userIdString, out Guid userId))
      {
        result = userId;
      }
      else
      {
        return BadRequest("Invalid or missing userId in the token.");
      }
    }
    Console.WriteLine($"User not found for UserId: {_userService.GetUserById(result)}");

    return Ok(result);
  }



}

