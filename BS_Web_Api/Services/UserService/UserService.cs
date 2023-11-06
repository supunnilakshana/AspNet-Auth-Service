using BS_Web_Api.Data;
using BS_Web_Api.Models;

namespace BS_Web_Api.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _dbcontext;

        public UserService()
        {
                _dbcontext = new AppDbContext();
        }
        public User Create(User user)
        {
            try
            {
                _dbcontext.Users.Add(user);     
                _dbcontext.SaveChanges();
                return user;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Delete(Guid userId)
        {
            try
            {
                User? user = _dbcontext.Users.FirstOrDefault(e => e.Equals(userId));
                if (user == null)
                   throw new  Exception("User Not Found");
                _dbcontext.Users.Remove(user);
                _dbcontext.SaveChanges();
                
            }
            catch (Exception)
            {

                throw;
            }
        }

        public User? GetById(String userId)
        {

            try
            {
             User? user=   _dbcontext.Users.FirstOrDefault(e=>e.Id.ToString().Equals(userId));
                if (user==null)
                    return null;
                return user;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public User? GetByUserName(string userName)
        {
            try
            {
                User? user = _dbcontext.Users.FirstOrDefault(e => e.UserName.Equals(userName));
                if (user == null)
                    return null;
                return user;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public User Update(Guid userId, User user)
        {
            try
            {
                User? pwuser = _dbcontext.Users.FirstOrDefault(e => e.Id.ToString().Equals(userId));
                if (pwuser == null)
                    throw new Exception("User Not Found");
                _dbcontext.Users.Update(user);
                _dbcontext.SaveChanges();
                return user;

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
