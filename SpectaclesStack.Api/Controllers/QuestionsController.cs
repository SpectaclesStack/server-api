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

        public QuestionsController(IQuestionsRepository questionsRepository)
        {
            this.questionsRepository = questionsRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Questions>))]
        public IActionResult GetQuestions()
    {
        try
        {
            // Retrieve questions from the repository
            var questions = questionsRepository.GetQuestions();

            // Check if questions is null or empty
            if (questions == null || !questions.Any())
            {
                // Return a 404 Not Found response with an appropriate message
                return NotFound("No questions found.");
            }

            // Return a 200 OK response with the questions
            return Ok(questions);
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"An error occurred: {ex.Message}");

            // Handle unexpected exceptions
            ModelState.AddModelError("", "An unexpected error occurred while processing the request");
            return StatusCode(500, ModelState);
        }
    }


        [HttpGet("{questionId}")]
        [ProducesResponseType(200, Type = typeof(Questions))]
        [ProducesResponseType(400)]
        public IActionResult GetQuestion(int questionId)
    {
        try
        {
            if (!questionsRepository.QuestionExists(questionId))
                return NotFound();

            var question = questionsRepository.GetQuestion(questionId);

            if (question == null)
                return NotFound(); // Handle if the question is not found

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(question);
        }
        catch (Exception ex)
        {
            // Log the exception for debugging purposes
            Console.WriteLine($"An error occurred: {ex.Message}");
            
            // Return a generic error message
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }


        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
       public async Task<IActionResult> CreateQuestion([FromBody] Questions createQuestion)
    {
        try
        {
                string accessToken = OauthHelper.GetAccessToken(HttpContext);

                bool authorized = await OauthHelper.Autheenticate(accessToken);

                if (!authorized)
                {
                    return Unauthorized();
                }

                if (createQuestion == null)
            {
                return BadRequest("Question data is null.");
            }

            createQuestion.CreateAt = DateTime.UtcNow;

            var existingQuestion = questionsRepository.GetQuestions()
                .FirstOrDefault(q => q.QuestionId == createQuestion.QuestionId);

            if (existingQuestion != null)
            {
                ModelState.AddModelError("", "Question already exists.");
                return UnprocessableEntity(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!questionsRepository.CreateQuestion(createQuestion))
            {
                ModelState.AddModelError("", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }

            var newQuestion = questionsRepository.GetQuestion(createQuestion.QuestionId);
            return Ok(newQuestion);
        }
        catch (Exception ex)
        {
            // Log the exception for debugging purposes
            Console.WriteLine($"An error occurred: {ex.Message}");

            // Return a generic error message
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
        [HttpPut("{questionId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
       public async Task<IActionResult> UpdateQuestion(int questionId, [FromBody] Questions updatedQuestion)
{
    try
    {
                string accessToken = OauthHelper.GetAccessToken(HttpContext);

                bool authorized = await OauthHelper.Autheenticate(accessToken);

                if (!authorized)
                {
                    return Unauthorized();
                }

                if (updatedQuestion == null)
        {
            return BadRequest("Updated question data is null.");
        }

        if (questionId != updatedQuestion.QuestionId)
        {
            ModelState.AddModelError("", "Question ID in the URL does not match the ID in the request body.");
            return BadRequest(ModelState);
        }

        if (!questionsRepository.QuestionExists(questionId))
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        updatedQuestion.CreateAt = DateTime.UtcNow;

        if (!questionsRepository.UpdateQuestion(updatedQuestion))
        {
            ModelState.AddModelError("", "Something went wrong updating the question.");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }
    catch (Exception ex)
    {
        // Log the exception for debugging purposes
        Console.WriteLine($"An error occurred: {ex.Message}");

        // Return a generic error message
        return StatusCode(500, "An error occurred while processing your request.");
    }
}

    }
}