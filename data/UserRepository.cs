using DotnetAPI.Models;

namespace DotnetAPI.Data
{
    public class UserRepository : IUserRepository
    {
        DataContextEF _entityFramework;
        public UserRepository(IConfiguration config)
        {
            _entityFramework = new DataContextEF(config);
        }


        public bool SaveChanges()
        {
            return _entityFramework.SaveChanges() > 0;
        }


        public bool AddEntity<T>(T entityToAdd)
        {
            if (entityToAdd != null)
            {
                _entityFramework.Add(entityToAdd);
                return true;

            }
            return false;
        }


        public void RemoveEntity<T>(T entityToRemove)
        {
            if (entityToRemove != null)
            {
                _entityFramework.Remove(entityToRemove);

            }

        }

        public IEnumerable<User> GetUsers()
        {

            IEnumerable<User> users = _entityFramework.Users.ToList<User>();
            return users;

        }

        public User GetSingleUsers(int userId)
        {


            User? user = _entityFramework.Users
                .Where(u => u.UserId == userId)
                .FirstOrDefault<User>();
            if (user != null)
            {
                return user;
            }
            else
            {
                throw new Exception("User not found");
            }


        }

        public IEnumerable<UserSalary> GetUserSalaries()
        {
            IEnumerable<UserSalary> userSalaries = _entityFramework.UserSalary.ToList<UserSalary>();
            return userSalaries;

        }


        public UserSalary GetSingleUserSalary(int userId)
        {
            UserSalary? userSalary = _entityFramework.UserSalary
                .Where(u => u.UserId == userId)
                .FirstOrDefault<UserSalary>();
            if (userSalary != null)
            {
                return userSalary;
            }
            else
            {
                throw new Exception("User not found");
            }
        }

        public IEnumerable<UserJobInfo> GetUserJobInfo()
        {
            IEnumerable<UserJobInfo> userJobInfo = _entityFramework.UserJobInfo.ToList<UserJobInfo>();
            return userJobInfo;
        }

        public UserJobInfo GetSingleUserJobInfo(int userId)
        {
            UserJobInfo? singleUserJobInfo = _entityFramework.UserJobInfo
                .Where(u => u.UserId == userId)
                .FirstOrDefault<UserJobInfo>();
            if (singleUserJobInfo != null)
            {
                return singleUserJobInfo;
            }
            else
            {
                throw new Exception("User not found");
            }
        }

        
    }
}