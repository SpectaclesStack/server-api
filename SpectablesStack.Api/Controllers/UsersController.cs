
using stackup_vsc_setup.Model;
using stackup_vsc_setup.Data;
using SpectablesStack.Api.Dto;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using SpectablesStack.Api.Interface;
using SpectablesStack.Api.Repository;

namespace SpectablesStack.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : Controller
    {

        private readonly IUsersRepository userRepository;
        private readonly IMapper mapper;

        public UsersController(IUsersRepository usersRepository, IMapper mapper)
        {
            this.userRepository = usersRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Users>))]
        public IActionResult GetUser()
        {
            var users = mapper.Map<List<UsersDto>>(userRepository.GetUsers());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(users);
        }

        [HttpGet("{UserId}")]
        [ProducesResponseType(200, Type = typeof(Users))]
        [ProducesResponseType(400)]
        public IActionResult GetUsers(int userId)
        {
            if (!userRepository.UserExists(userId))
                return NotFound();

            var user = mapper.Map<UsersDto>(userRepository.GetUser(userId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUser([FromBody] UsersDto createUser)
        {
            if (createUser == null)
                return BadRequest(ModelState);

            var user = userRepository.GetUsers()
                .Where(u => u.UserId == createUser.UserId)
                .FirstOrDefault();

            if(user != null)
            {
                ModelState.AddModelError("", "User already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userC = mapper.Map<Users>(createUser);

            if(!userRepository.CreateUser(userC))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{UserId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateUser(int userId, [FromBody]UsersDto updatedUser)
        {
            if (updatedUser == null)
                return BadRequest(ModelState);

            if (userId != updatedUser.UserId)
                return BadRequest(ModelState);

            if (!userRepository.UserExists(userId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var userMap = mapper.Map<Users>(updatedUser);

            if(!userRepository.UpdateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong updating user");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{UserId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteUser(int userId)
        {
            if(!userRepository.UserExists(userId))
            {
                return NotFound();
            }

            var userToDelete = userRepository.GetUser(userId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!userRepository.DeleteUser(userToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting user");
            }

            return NoContent();
        }

    }
}
        
    