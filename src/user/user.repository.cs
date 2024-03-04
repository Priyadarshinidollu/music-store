namespace music_store.src.user;
using music_store.src.user.interfaces;
using music_store.src.data;
public class UserRepository : IUserRepository
{
  private readonly DbConnectionContext _db;
  public UserRepository(DbConnectionContext db)
  {
    _db = db;
  }

  public void AddUser(string Username, string Email, string Password)
  {

    var newUser = new UserEntity
    {
      Username = Username,
      Email = Email,
      PasswordHash = Password
    };

    // Set the password using the SetPassword method
    newUser.SetPassword(Password);
    // Add the user to the database
    _db.User.Add(newUser);
    _db.SaveChanges();

  }

  public UserEntity? GetUserById(Guid UserId)
  {
    return _db.User.Find(UserId);
  }

  public UserEntity? GetUserByEmail(string email)
  {
    return _db.User.FirstOrDefault(u => u.Email == email); // Assuming Email is the property in UserEntity
  }


  public UserEntity? GetUserByUsername(string Username)
  {
    return _db.User.FirstOrDefault(u => u.Username == Username); // Assuming Email is the property in UserEntity

  }
}