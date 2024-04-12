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

        public AnswersController(IAnswersRepository answerRepository)
        {
            this.answerRepository = answerRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Answers>))]
       public IActionResult GetAnswers()
    {
        try
        {
            var answers = answerRepository.GetAnswers();

            if (answers == null || !answers.Any())
            {
                return NotFound("No answers found.");
            }

            return Ok(answers);
        }
        catch (Exception ex)
        {
            
            Console.WriteLine($"An error occurred: {ex.Message}");

            ModelState.AddModelError("", "An unexpected error occurred while processing the request");
            return StatusCode(500, ModelState);
        }
    }


        [HttpGet("{AnswerId}")]
        [ProducesResponseType(200, Type = typeof(Answers))]
        [ProducesResponseType(400)]
        public IActionResult GetAnswers(int AnswerId)
    {
        try
        {
            if (!answerRepository.AnswerExists(AnswerId))
            {
                
                return NotFound("Answer not found.");
            }
            
            var answer = answerRepository.GetAnswer(AnswerId);

            if (answer == null)
            {
                return NotFound("Answer not found.");
            }

            return Ok(answer);
        }
        catch (Exception ex)
        {
    
            Console.WriteLine($"An error occurred: {ex.Message}");

            ModelState.AddModelError("", "An unexpected error occurred while processing the request");
            return StatusCode(500, ModelState);
        }
    }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
       public async Task<IActionResult> CreateAnswer([FromBody] Answers createAnswer)
    {
       try
       {
            string accessToken = OauthHelper.GetAccessToken(HttpContext);

            bool authorized = await OauthHelper.Autheenticate(accessToken);

            if (!authorized)
            {
                return Unauthorized();
            }
                
            if (createAnswer == null)
                return BadRequest(ModelState);

            createAnswer.CreateAt = DateTime.UtcNow;

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

            if (!answerRepository.CreateAnswer(createAnswer))
            {
                ModelState.AddModelError("", "Something went wrong while saving the answer");
                return StatusCode(500, ModelState);
            }

            var newAnswer = answerRepository.GetAnswer(createAnswer.AnswerId);

            return Ok(newAnswer);
       }
       catch (Exception ex)
      {
        
        Console.WriteLine($"An error occurred: {ex.Message}");

        ModelState.AddModelError("", "An unexpected error occurred while processing the request");
        return StatusCode(500, ModelState);
      }
    }

    [HttpPut("{AnswerId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateAnswer(int answerId, [FromBody] Answers updatedAnswer)
   {
        try
        {
            string accessToken = OauthHelper.GetAccessToken(HttpContext);

            bool authorized = await OauthHelper.Autheenticate(accessToken);

            if (!authorized)
            {
                return Unauthorized();
            }

            updatedAnswer.CreateAt = DateTime.UtcNow;
        
            if (updatedAnswer == null)
                return BadRequest(ModelState);

            if (answerId != updatedAnswer.AnswerId)
            {
                ModelState.AddModelError("", "The AnswerId in the URL does not match the one in the request body");
                return BadRequest(ModelState);
            }

            if (!answerRepository.AnswerExists(answerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!answerRepository.UpdateAnswer(updatedAnswer))
            {
            
                ModelState.AddModelError("", "Something went wrong while updating the answer");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
        
        catch (Exception ex)
        {
            
            Console.WriteLine($"An error occurred: {ex.Message}");

            ModelState.AddModelError("", "An unexpected error occurred while processing the request");
            return StatusCode(500, ModelState);
        }
    }

    }
}

