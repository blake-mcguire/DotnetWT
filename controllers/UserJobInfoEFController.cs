namespace DotnetAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using DotnetAPI.DTOs;
using DotnetAPI.Data;
using DotnetAPI.Models;
using AutoMapper;

[ApiController]
[Route("[controller]")]
public class UserJobInfoEFController : ControllerBase
{
    
    IUserRepository _userRepository;
    IMapper _mapper;
    public UserJobInfoEFController(IConfiguration config, IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UserJobInfoDTO, UserJobInfo>();
        }
        ));
    }

    [HttpGet("GetUserJobInfo")]
    public IEnumerable<UserJobInfo> GetUserJobInfo()
    {
        
        return _userRepository.GetUserJobInfo();
    }

    [HttpGet("GetSingleUserJobInfo/{userId}")]

    public UserJobInfo GetSingleUserJobInfo(int userId)
    {
        UserJobInfo? singleUserJobInfo = _userRepository.GetSingleUserJobInfo(userId);
        if (singleUserJobInfo != null)
        {
            return singleUserJobInfo;
        }
        else
        {
            throw new Exception("User not found");
        }
    }

    [HttpPut("EditUserJobInfo/{userId}")]
    public IActionResult EditUserJobInfo(int userId, [FromBody] UserJobInfo userJobInfo)
    {
        UserJobInfo? jobInfoDb = _userRepository.GetSingleUserJobInfo(userJobInfo.UserId);
        if (jobInfoDb != null)
        {
            
            jobInfoDb.JobTitle = userJobInfo.JobTitle;
            jobInfoDb.Department = userJobInfo.Department;

            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            else
            {
                throw new Exception("Error updating user job info");
            }
            
        }
        else
        {
            throw new Exception("User not found");
        }
    }

    [HttpPost("AddUserJobInfo")]
    public IActionResult AddUserJobInfo(UserJobInfo userJobInfo)
    {
        UserJobInfo userJobInfoDb = _mapper.Map<UserJobInfo>(userJobInfo);
        
            userJobInfoDb.UserId = userJobInfo.UserId;
            userJobInfoDb.JobTitle = userJobInfo.JobTitle;
            userJobInfoDb.Department = userJobInfo.Department;

            _userRepository.AddEntity<UserJobInfo>(userJobInfoDb);
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            else
            {
                throw new Exception("Failed to add user job info");
            }


    }

    [HttpDelete("DeleteUserJobInfo/{userId}")]

    public IActionResult DeleteUserJobInfo(int userId)
    {
        UserJobInfo? jobInfoDb = _userRepository.GetSingleUserJobInfo(userId);
            if (jobInfoDb != null)
            {
            _userRepository.RemoveEntity<UserJobInfo>(jobInfoDb);
                if (_userRepository.SaveChanges())
                {
                    return Ok();
                }
                else
                {
                    throw new Exception("Failed to delete user job info");
                }
            }
            else
            {
                throw new Exception("User not found");
            }
    }
}