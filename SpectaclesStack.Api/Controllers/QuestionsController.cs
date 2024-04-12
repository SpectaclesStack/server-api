using Microsoft.AspNetCore.Mvc;
using spectaclesStackServer.Dto;
using spectaclesStackServer.Interface;
using spectaclesStackServer.Model;
using SpectacularOauth;

namespace spectaclesStackServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : Controller
    {
        private readonly IQuestionsRepository questionsRepository;

        private readonly IUsersRepository userRepository;

        public QuestionsController(IQuestionsRepository questionsRepository, IUsersRepository userRepository)
        {
            this.questionsRepository = questionsRepository;
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
        [ProducesResponseType(200, Type = typeof(IEnumerable<Questions>))]
        public IActionResult GetQuestions()
        {
            var questions = questionsRepository.GetQuestions();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(questions);
        }

        [HttpGet("{questionId}")]
        [ProducesResponseType(200, Type = typeof(Questions))]
        [ProducesResponseType(400)]
        public IActionResult GetQuestion(int questionId)
        {
            if (!questionsRepository.QuestionExists(questionId))
                return NotFound();

            var question = questionsRepository.GetQuestion(questionId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(question);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateQuestion([FromBody] Questions createQuestion)
        { 

        string accessToken = GetAccessToken();
    
        string username = await OauthHelper.getUsername(accessToken);

        // Ensure the user exists
        if (!userRepository.UserExists(createQuestion.userid))
            return Unauthorized();

            createQuestion.CreateAt = DateTime.UtcNow;
            
            if (createQuestion == null)
            {
                return BadRequest(ModelState);
            }

            var question = questionsRepository.GetQuestions()
                .Where(q => q.QuestionId == createQuestion.QuestionId)
                .FirstOrDefault();
            

            if (question != null)
            {
                ModelState.AddModelError("", "Question already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //var questionC = createQuestion;

            if (!questionsRepository.CreateQuestion(createQuestion))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            var newQuestion = questionsRepository.GetQuestion(createQuestion.QuestionId);

            return Ok(newQuestion);
        }

        [HttpPut("{questionId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateQuestion(int questionId, [FromBody] Questions updatedQuestion)
        {
        string accessToken = GetAccessToken();
    
        string username = await OauthHelper.getUsername(accessToken);

        // Ensure the user exists
        if (!userRepository.UserExists(updatedQuestion.userid))
            return Unauthorized();

            if (updatedQuestion == null)
                return BadRequest(ModelState);

            if (questionId != updatedQuestion.QuestionId)
                return BadRequest(ModelState);

            if (!questionsRepository.QuestionExists(questionId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();
            
            if (!questionsRepository.UpdateQuestion(updatedQuestion))
            {
                ModelState.AddModelError("", "Something went wrong updating question");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{questionId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteQuestion(int questionId)
        {
            if (!questionsRepository.QuestionExists(questionId))
            {
                return NotFound();
            }

            var questionToDelete = questionsRepository.GetQuestion(questionId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!questionsRepository.DeleteQuestion(questionToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting question");
            }

            return NoContent();
        }

    }
}