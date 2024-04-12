
using spectaclesStackServer.Model;
using spectaclesStackServer.Data;
using spectaclesStackServer.Dto;
using Microsoft.AspNetCore.Mvc;
using spectaclesStackServer.Repository;
using spectaclesStackServer.Interface;
using SpectacularOauth;

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
        try
        {
            if (!userRepository.UserExists(userId))
            {
                return NotFound();
            }

            var user = userRepository.GetUser(userId);

            if (user == null)
            {
                return NotFound(); 
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(user);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");

            return StatusCode(500, "An error occurred while processing your request.");
        }
      }


        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
       public async Task<IActionResult> CreateUser([FromBody] Users createUser)
    {
        try
        {
                string accessToken = OauthHelper.GetAccessToken(HttpContext);

                bool authorized = await OauthHelper.Autheenticate(accessToken);

                if (!authorized)
                {
                    return Unauthorized();
                }

                if (createUser == null)
            {
                return BadRequest("User data is null.");
            }

            createUser.DateCreated = DateTime.UtcNow;

            var existingUser = userRepository.GetUsers()
                .FirstOrDefault(u => u.UserName == createUser.UserName);

            if (existingUser != null)
            {
                return Ok(existingUser); 
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!userRepository.CreateUser(createUser))
            {
                ModelState.AddModelError("", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }

            var newUser = userRepository.GetUser(createUser.UserId);
            return Ok(newUser);
        }
        catch (Exception ex)
        {
            
            Console.WriteLine($"An error occurred: {ex.Message}");

            return StatusCode(500, "An error occurred while processing your request.");
        }
    }


        [HttpPut("{userId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] Users updatedUser)
    {
        try
        {
                string accessToken = OauthHelper.GetAccessToken(HttpContext);

                bool authorized = await OauthHelper.Autheenticate(accessToken);

                if (!authorized)
                {
                    return Unauthorized();
                }

                if (updatedUser == null)
            {
                return BadRequest("Updated user data is null.");
            }

            if (userId != updatedUser.UserId)
            {
                ModelState.AddModelError("", "User ID in the URL does not match the ID in the request body.");
                return BadRequest(ModelState);
            }

            if (!userRepository.UserExists(userId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            updatedUser.DateCreated = DateTime.UtcNow;

            if (!userRepository.UpdateUser(updatedUser))
            {
                ModelState.AddModelError("", "Something went wrong updating the user.");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
        catch (Exception ex)
        {
          
            Console.WriteLine($"An error occurred: {ex.Message}");

            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    }
}


