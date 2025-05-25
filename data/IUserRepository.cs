using DotnetAPI.Models;

namespace DotnetAPI.Data
{
    public interface IUserRepository
    {
        public bool SaveChanges();

        public bool AddEntity<T>(T entityToAdd);

        public void RemoveEntity<T>(T entityToRemove);

        public IEnumerable<User> GetUsers();

        public User GetSingleUsers(int userId);

        public IEnumerable<UserSalary> GetUserSalaries();

        public UserSalary GetSingleUserSalary(int userId);

        public IEnumerable<UserJobInfo> GetUserJobInfo();
        public UserJobInfo GetSingleUserJobInfo(int userId);

    }
}