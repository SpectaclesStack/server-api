using Microsoft.AspNetCore.Mvc;
using spectaclesStackServer.Dto;
using spectaclesStackServer.Interface;
using spectaclesStackServer.Model;
using SpectacularOauth;

namespace spectaclesStackServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AnswersController : Controller
    {
        private readonly IAnswersRepository answerRepository;
        private readonly IUsersRepository userRepository;

        public AnswersController(IAnswersRepository answerRepository, IUsersRepository userRepository)
        {
            this.answerRepository = answerRepository;
            this.userRepository = userRepository;
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

    private string GetAccessToken()
    {
        IDictionary<string, string> headers = GetRequestHeaders();
        if (!headers.ContainsKey("Authorization"))
            return null;

        return headers["Authorization"].ToString()["Bearer ".Length..].Trim();
    }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Answers>))]
        public IActionResult GetAnswers()
        {
            var answers = answerRepository.GetAnswers();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(answers);
        }

        [HttpGet("{AnswerId}")]
        [ProducesResponseType(200, Type = typeof(Answers))]
        [ProducesResponseType(400)]
        public IActionResult GetAnswers(int AnswerId)
        {
            if (!answerRepository.AnswerExists(AnswerId))
                return NotFound();

            var answer = answerRepository.GetAnswer(AnswerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(answer);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async  Task<IActionResult> CreateAnswer([FromBody] Answers createAnswer)
        {
    string accessToken = GetAccessToken();
    
        string username = await OauthHelper.getUsername(accessToken);

        // Ensure the user exists
        if (!userRepository.UserExists(createAnswer.userid))
            return Unauthorized();

            if (createAnswer == null)
                return BadRequest(ModelState);

            var answer = answerRepository.GetAnswers()
                .Where(a => a.AnswerId == createAnswer.AnswerId)
                .FirstOrDefault();

            if (answer != null)
            {
                ModelState.AddModelError("", "Answer already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var answerC = createAnswer;

            if (!answerRepository.CreateAnswer(answerC))
            {
                ModelState.AddModelError("", "Something went wrong while saving the answer");
                return StatusCode(500, ModelState);
            }

            var newAnswer = answerRepository.GetAnswer(createAnswer.AnswerId);
            
            return Ok(newAnswer);
        }

        [HttpPut("{AnswerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateAnswer(int answerId, [FromBody] Answers updatedAnswer)
        {
        string accessToken = GetAccessToken();
    
        string username = await OauthHelper.getUsername(accessToken);

        // Ensure the user exists
        if (!userRepository.UserExists(updatedAnswer.userid))
            return Unauthorized();

            if (updatedAnswer == null)
                return BadRequest(ModelState);

            if (answerId != updatedAnswer.AnswerId)
                return BadRequest(ModelState);

            if (!answerRepository.AnswerExists(answerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var answerMap = updatedAnswer;

            if (!answerRepository.UpdateAnswer(answerMap))
            {
                ModelState.AddModelError("", "Something went wrong updating while updating the answer");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{AnswerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteAnswer(int answerId)
        {
            if (!answerRepository.AnswerExists(answerId))
            {
                return NotFound();
            }

            var answerToDelete = answerRepository.GetAnswer(answerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!answerRepository.DeleteAnswer(answerToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting the answer");
            }

            return NoContent();
        }

    }
}

