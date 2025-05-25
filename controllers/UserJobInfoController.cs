namespace DotnetAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using DotnetAPI.DTOs;
using DotnetAPI.Data;
using DotnetAPI.Models;


[ApiController]
[Route("[controller]")]


public class UserJobInfoController : ControllerBase
{
    DataContextDapper _dapper;

    public UserJobInfoController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("GetUserJobInfo")]

    public IEnumerable<UserJobInfo> GetUserJobInfos()
    {
        string sql = @"
            SELECT [UserId],
                [JobTitle],
                [Department]
            FROM TutorialAppSchema.UserJobInfo";
        IEnumerable<UserJobInfo> userJobInfos = _dapper.LoadData<UserJobInfo>(sql);
        return userJobInfos;
    }

    [HttpGet("GetSingleUserJobInfo/{userId}")]
    public UserJobInfo GetSingleUserJobInfo(int userId)
    {
        string sql = @"
            SELECT [UserId],
                [JobTitle],
                [Department]
            FROM TutorialAppSchema.UserJobInfo
            WHERE UserID = " + userId.ToString();
        UserJobInfo userJobInfo = _dapper.LoadDataSingle<UserJobInfo>(sql);
        return userJobInfo;
    }

    [HttpPut("EditUserJobInfo/{userId}")]

    public IActionResult EditUserJobInfo(UserJobInfo userJobInfo)
    {
        string sql = $@"
            UPDate TutorialAppSchema.UserJobInfo
            SET
            [JobTitle] = '" + userJobInfo.JobTitle +
            "', [Department] = '" + userJobInfo.Department +
            "' WHERE UserId = " + userJobInfo.UserId.ToString();
        if (_dapper.Execute(sql))
        {
            return Ok();
        }
        else
        {
            return BadRequest("Error updating user job info");
        }
    }

    [HttpPost("AddUserJobInfo")]

    public IActionResult AddUserJobInfo(UserJobInfo userJobInfo)
    {
        string sql = $@"
            INSERT INTO TutorialAppSchema.UserJobInfo(
            [UserId],
            [JobTitle],
            [Department])
            VALUES(
                '{userJobInfo.UserId}',
                '{userJobInfo.JobTitle}',
                '{userJobInfo.Department}'
            )";
            if (_dapper.Execute(sql))
            {
                return Ok();
            }
            else 
            {
                throw new Exception("Error adding user job info");
            }

    }

    [HttpDelete("DeleteUserJobInfo/{userId}")]
    public IActionResult DeleteUserJobInfo(int userId)
    {
        string sql = $@"
            DELETE FROM TutorialAppSchema.UserJobInfo
            WHERE UserId = " + userId.ToString();

        if (_dapper.Execute(sql))
        {
            return Ok();
        }
        else
        {
            throw new Exception("Error Deleting user job info");
        }
    }

}