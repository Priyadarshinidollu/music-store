
namespace music_store.src.user.interfaces;


public interface IUserRepository
{
    UserEntity? GetUserById(Guid UserId);
    UserEntity? GetUserByEmail(string email);
    UserEntity? GetUserByUsername(string Username);
    void AddUser(string Username, string Email, string Password);
}


