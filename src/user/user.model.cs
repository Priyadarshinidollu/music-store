namespace music_store.src.user;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public class UserEntity
{

  [Key]
  public Guid UserId { get; set; }
  public string? Email { get; set; }
  public string? Username { get; set; }
  public string PasswordHash { get; set; }

  public UserEntity()
  {
    UserId = Guid.NewGuid(); // Set a new GUID when the entity is created
  }
  public void SetPassword(string Password)
  {
    var PasswordHasher = new PasswordHasher<UserEntity>();
    PasswordHash = PasswordHasher.HashPassword(this, Password);
  }

  public bool VerifyPassword(string Password)
  {
    var PasswordHasher = new PasswordHasher<UserEntity>();
    return PasswordHasher.VerifyHashedPassword(this, PasswordHash, Password) == PasswordVerificationResult.Success;
  }
}

