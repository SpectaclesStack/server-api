
using spectaclesStackServer.Model;
using spectaclesStackServer.Data;
using spectaclesStackServer.Dto;
using Microsoft.AspNetCore.Mvc;
using spectaclesStackServer.Repository;
using spectaclesStackServer.Interface;

namespace spectaclesStackServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : Controller
    {

        private readonly IUsersRepository userRepository;

        public UsersController(IUsersRepository usersRepository)
        {
            this.userRepository = usersRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Users>))]
        public IActionResult GetUser()
        {
            var users = userRepository.GetUsers();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(users);
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(Users))]
        [ProducesResponseType(400)]
        public IActionResult GetUsers(int userId)
        {
            if (!userRepository.UserExists(userId))
                return NotFound();

            var user = userRepository.GetUser(userId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUser([FromBody] Users createUser)
        {
            if (createUser == null)
                return BadRequest(ModelState);

            var user = userRepository.GetUsers()
                .Where(u => u.UserName == createUser.UserName)
                .FirstOrDefault();

            if (user != null)
            {
                ModelState.AddModelError("", "User already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //var userC = mapper.Map<Users>(createUser);

            if (!userRepository.CreateUser(createUser))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{userId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateUser(int userId, [FromBody] Users updatedUser)
        {
            if (updatedUser == null)
                return BadRequest(ModelState);

            if (userId != updatedUser.UserId)
                return BadRequest(ModelState);

            if (!userRepository.UserExists(userId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var userMap = updatedUser;

            if (!userRepository.UpdateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong updating user");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{userId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteUser(int userId)
        {
            if (!userRepository.UserExists(userId))
            {
                return NotFound();
            }

            var userToDelete = userRepository.GetUser(userId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!userRepository.DeleteUser(userToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting user");
            }

            return NoContent();
        }

    }
}


