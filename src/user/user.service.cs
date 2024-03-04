namespace music_store.src.user;

using Microsoft.AspNetCore.Identity;
using music_store.src.user.interfaces;
public class UserService : IUserService
{
  private readonly IUserRepository _userRepository;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public UserService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
  {
    _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

  }

  public UserEntity? GetUserByEmail(string Email)
  {
    return _userRepository.GetUserByEmail(Email);
  }

  public UserEntity? GetUserById(Guid UserId)
  {
    return _userRepository.GetUserById(UserId);
  }

  public UserEntity? ValidateUserCredentials(string Email, string Password)
  {
    var user = _userRepository.GetUserByEmail(Email);
    if (VerifyPassword(Password, user.PasswordHash))
    {
      return user;
    }
    // Check if the user exists and the provided Password is correct
    return null;
  }

  public void RegisterUser(string Username, string Email, string Password)
  {

    _userRepository.AddUser(Username, Email, Password);
  }

  private static bool VerifyPassword(string Password, string PasswordHash)
  {
    var PasswordHasher = new PasswordHasher<UserEntity>();
    return PasswordHasher.VerifyHashedPassword(null, PasswordHash, Password) == PasswordVerificationResult.Success;
  }

}