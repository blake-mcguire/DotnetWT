namespace DotnetAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using DotnetAPI.DTOs;
using DotnetAPI.Data;
using DotnetAPI.Models;
using AutoMapper;

[ApiController]
[Route("[controller]")]

public class UserSalaryEFController : ControllerBase
{
    IUserRepository _userRepository;
    
    IMapper _mapper;
    public UserSalaryEFController(IConfiguration config, IUserRepository userRepository)
    {
        _userRepository = userRepository;
       
        _mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UserSalaryDTO, UserSalary>();
        }
        ));
   
    }

    [HttpGet("GetUserSalaries")]

    public IEnumerable<UserSalary> GetUserSalaries()
    {
        return _userRepository.GetUserSalaries();

    }


    [HttpGet("GetsIngleUserSalary/{userId}")]

    public UserSalary GetSingleUserSalary(int userId)
    {
        UserSalary? userSalary = _userRepository.GetSingleUserSalary(userId);
        
           
        if (userSalary != null)
        {
            return userSalary;
        }
        else
        {
            throw new Exception("User not found");
        }
    }


    [HttpPut("EditUserSalary/{userId}")]

    public IActionResult EditUserSalary(int userId, [FromBody] UserSalary userSalary)
    {
        UserSalary? salary = _userRepository.GetSingleUserSalary(userId);


            if (salary != null)
        {
            salary.Salary = userSalary.Salary;
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            else
            {
                throw new Exception("Failed to update user salary");
            }

        }
        else
        {
            throw new Exception("User not found");
        }
    }


    [HttpPost("AddUserSalary")]
    public IActionResult AddUserSalary(UserSalary userSalaryDTO)
    {
        UserSalary userSalaryDB = _mapper.Map<UserSalary>(userSalaryDTO);

        _userRepository.AddEntity(userSalaryDB);
        if (_userRepository.SaveChanges())
        {
            return Ok();
        }
        else
        {
            throw new Exception("Failed to add user salary");
        }
    }

    [HttpDelete("DeleteUserSalary/{userId}")]

    public IActionResult DeleteUserSalary(int userId)
    {
        UserSalary? userSalary = _userRepository.GetSingleUserSalary(userId);
        if (userSalary != null)
        {
            _userRepository.RemoveEntity<UserSalary>(userSalary);
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            else
            {
                throw new Exception("Failed to delete user salary");
            }
        }
        else
        {
            throw new Exception("User not found");
        }
    }

};