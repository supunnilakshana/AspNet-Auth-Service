using BS_Web_Api.Models;

namespace BS_Web_Api.Services.UserService
{
    public interface IUserService
    {
        User? GetById(Guid userId);
        User? GetByUserName(String userName);
        User Create(User user);
        User Update(Guid userId, User user);
        void Delete(Guid userId);

    }
}
