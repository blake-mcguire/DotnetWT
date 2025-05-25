namespace DotnetAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using DotnetAPI.DTOs;
using DotnetAPI.Data;
using DotnetAPI.Models;



[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    DataContextDapper _dapper;
    public UserController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }
    [HttpGet("TestConnection")]

    public DateTime TestConnection()
    {
        return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }
    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        string sql = @"
            SELECT [UserId],
                [FirstName],
                [LastName],
                [Email],
                [Gender],
                [Active] 
            FROM TutorialAppSchema.Users";
        IEnumerable<User> users = _dapper.LoadData<User>(sql);
        return users;

    }

        [HttpGet("GetSingleUser/{userId}")]
    public User GetSingleUsers(int userId)
    {
        string sql = @"
            SELECT [UserId],
                [FirstName],
                [LastName],
                [Email],
                [Gender],
                [Active] 
            FROM TutorialAppSchema.Users
            WHERE UserID = " + userId.ToString();
        
        User user = _dapper.LoadDataSingle<User>(sql);
        return user;

    }
    [HttpPut("EditUser/{userId}")]
    public IActionResult EditUser(User user)
    {
        string sql = $@"
            UPDATE TutorialAppSchema.Users
            SET
                [FirstName] = '" + user.FirstName + 
                "', [LastName] = '" + user.LastName + 
                "', [Email] = '" + user.Email + 
                "', [Gender] = '" + user.Gender + 
                "', [Active] = '" + user.Active +
            "' WHERE UserID = " + user.UserId;
        if(_dapper.Execute(sql))
        {
            return Ok();
        }
        else
        {
            return BadRequest();
        }
 
        
    }

    [HttpPost("AddUser")]
        public IActionResult AddUser(UserDTO user)
    {
        string sql = $@"INSERT INTO TutorialAppSchema.Users(
        [FirstName],
        [LastName],
        [Email],
        [Gender],
        [Active]
    )        
     VALUES
    (
        '{user.FirstName}',
        '{user.LastName}',
        '{user.Email}',
        '{user.Gender}',
        '{(user.Active ? 1 : 0)}'
    )";
        Console.WriteLine(sql);
        if (_dapper.Execute(sql))
        {
            return Ok();
        }
        else
        {
            throw new Exception("Failed to add user");
        }
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        string sql = $@"
            DELETE FROM TutorialAppSchema.Users
            WHERE UserID = {userId}";

        if (_dapper.Execute(sql))
        {
            return Ok();

        }
        else 
        {
            throw new Exception("Failed to delete user");
        }
    }

}


