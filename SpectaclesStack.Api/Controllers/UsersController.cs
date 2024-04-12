
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

        private IDictionary<string, string> GetRequestHeaders()
        {
            IDictionary<string, string> headers = new Dictionary<string, string>();

            foreach (var (key, value) in Request.Headers)
            {
                headers[key] = value.ToString();
            }

            return headers;
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
public async Task<IActionResult> CreateUser([FromBody] Users createUser)
{
    // Check for Authorization header and retrieve access token
    string accessToken = GetAccessToken();
    
    // Retrieve username from GitHub using access token
    string username = await OauthHelper.getUsername(accessToken);

    // If the createUser object is null, return BadRequest
    if (createUser == null)
        return BadRequest("User data is missing");

    // Ensure that the DateCreated field is set
    if (createUser.DateCreated == null)
        createUser.DateCreated = DateTime.UtcNow;

    // Check if the username already exists
    if (userRepository.GetUsers().Any(u => u.UserName == username))
        return Conflict("User already exists");

    // Set the username to the retrieved username
    createUser.UserName = username;

    // Perform model validation
    if (!TryValidateModel(createUser))
        return BadRequest(ModelState);

    // Attempt to create the user
    if (!userRepository.CreateUser(createUser))
        return StatusCode(500, "Something went wrong while saving the user");

    // Retrieve the newly created user
    var newUser = userRepository.GetUsers().FirstOrDefault(u => u.UserName == username);

    // Return the newly created user
    return Ok(newUser);
}


[HttpPut("{userId}")]
[ProducesResponseType(400)]
[ProducesResponseType(204)]
[ProducesResponseType(404)]
public async Task<IActionResult> UpdateUser(int userId, [FromBody] Users updatedUser)
{
    string accessToken = GetAccessToken();
    
    string username = await OauthHelper.getUsername(accessToken);

    // Ensure the user exists
    if (!userRepository.UserExists(userId))
        return NotFound();

    // Retrieve the user to be updated
    var existingUser = userRepository.GetUser(userId);

    // Ensure the user making the request is the owner of the resource
    if (existingUser.UserName != username)
        return Unauthorized();

    // Update only the allowed fields
    existingUser.UserName = updatedUser.UserName;

    // Perform validation
    if (!TryValidateModel(existingUser))
        return BadRequest(ModelState);

    // Update the user
    if (!userRepository.UpdateUser(existingUser))
        return StatusCode(500, "Something went wrong updating user");

    return NoContent();
}

private string GetAccessToken()
{
    IDictionary<string, string> headers = GetRequestHeaders();
    if (!headers.ContainsKey("Authorization"))
        return null;

    return headers["Authorization"].ToString()["Bearer ".Length..].Trim();
}


        [HttpDelete("{UserId}")]
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


