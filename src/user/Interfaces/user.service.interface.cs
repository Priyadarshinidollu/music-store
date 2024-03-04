namespace music_store.src.user.interfaces;
using music_store.src.user;
public interface IUserService
{
    UserEntity GetUserById(Guid UserId);

    UserEntity GetUserByEmail(string Email);
    UserEntity? ValidateUserCredentials(string Email, string Password);
    void RegisterUser(string Username, string Email, string Password);
}

