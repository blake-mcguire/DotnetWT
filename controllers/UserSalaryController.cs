namespace DotnetAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using DotnetAPI.DTOs;
using DotnetAPI.Data;
using DotnetAPI.Models;


[ApiController]
[Route("[controller]")]
public class UserSalaryController : ControllerBase
{
    DataContextDapper _dapper;
    public UserSalaryController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("UserSalary")]
    public IEnumerable<UserSalary> GetUserSalaries()
    {
        string sql = @"
            SELECT [UserId],
                   [Salary]
            FROM TutorialAppSchema.UserSalary";
        IEnumerable<UserSalary> userSalaries = _dapper.LoadData<UserSalary>(sql);
        return userSalaries;
    }

    [HttpGet("GetSingleUserSalary/{userId}")]
    public UserSalary GetSingleUserSalary(int userId)
    {
        string sql = @"
            SELECT [UserId],
                [Salary]
            FROM TutorialAppSchema.UserSalary
            WHERE UserID = " + userId.ToString();
        UserSalary userSalary = _dapper.LoadDataSingle<UserSalary>(sql);
        return userSalary;
    }

    [HttpPut("EditUserSalary/{userId}")]
    public IActionResult EditUserSalary(UserSalary userSalary)
    {
        string sql = $@"
            UPDate TutorialAppSchema.UserSalary
            SET
            [Salary] = '" + userSalary.Salary +
            "' WHERE UserId = " + userSalary.UserId.ToString();
        if (_dapper.Execute(sql))
        {
            return Ok();
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpPost("AddUserSalary")]
    public IActionResult AddUserSalary(UserSalary userSalary)
    {
        string sql = $@"
            INSERT INTO TutorialAppSchema.UserSalary
            ([UserId],
            [Salary])
            VALUES
            ('" + userSalary.UserId +
            "', '" + userSalary.Salary +
            "')";
        if (_dapper.Execute(sql))
        {
            return Ok();
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpDelete("DeleteUserSalary/{userId}")]
    public IActionResult DeleteUserSalary(int userId)
    {
        string sql = $@"
            DELETE FROM TutorialAppSchema.UserSalary
            WHERE UserId = " + userId.ToString();
        if (_dapper.Execute(sql))
        {
            return Ok();
        }
        else
        {
            return BadRequest();
        }
    }

}