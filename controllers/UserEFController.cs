namespace DotnetAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using DotnetAPI.DTOs;
using DotnetAPI.Data;
using DotnetAPI.Models;

using AutoMapper;

[ApiController]
[Route("[controller]")]
public class UserEFController : ControllerBase
{
    DataContextEF _entityFramework;

    IUserRepository _userRepository;
    IMapper _mapper;
    public UserEFController(IConfiguration config, IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _entityFramework = new DataContextEF(config);
        _mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UserDTO, User>();
        }
        ));
    }
    


    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {

        IEnumerable<User> users = _userRepository.GetUsers();
        return users;

    }



    [HttpGet("GetSingleUser/{userId}")]
    public User GetSingleUsers(int userId)
    {
        return _userRepository.GetSingleUsers(userId);
    }

    [HttpPut("EditUser/{userId}")]
    public IActionResult EditUser(int userId, [FromBody] User user)
    {

        User? userDb = _userRepository.GetSingleUsers(user.UserId);
        if (userDb != null)
        {
           userDb.Active = user.Active;
           userDb.Email = user.Email;
           userDb.FirstName = user.FirstName;
           userDb.LastName = user.LastName;
           userDb.Gender = user.Gender;

            if (_userRepository.SaveChanges())
            {
                return Ok();
            }

            throw new Exception("Failed to update user");
        }

        else
        {
            throw new Exception("User not found");
        }
       
       
 
        
    }

    [HttpPost("AddUser")]
        public IActionResult AddUser(UserDTO user)
    {
        User userDb = _mapper.Map<User>(user);

           _userRepository.AddEntity<User>(userDb);
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }

            throw new Exception("Failed to create user");
    
       
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        User? userDb = _entityFramework.Users
            .Where(u => u.UserId == userId)
            .FirstOrDefault<User>();
        if (userDb != null)
        {
            _userRepository.RemoveEntity(userDb);
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            else
            {
                throw new Exception("Failed to delete user");
            }
        }
        else
        {
            throw new Exception("User not found");
        }
    
    }

}


