using Microsoft.AspNetCore.Mvc;
using spectaclesStackServer.Dto;
using spectaclesStackServer.Interface;
using spectaclesStackServer.Model;

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

            // Check if the answers collection is null or empty
            if (answers == null || !answers.Any())
            {
                // Return a 404 Not Found response if no answers are found
                return NotFound("No answers found.");
            }

            // Return a 200 OK response with the answers
            return Ok(answers);
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


        [HttpGet("{AnswerId}")]
        [ProducesResponseType(200, Type = typeof(Answers))]
        [ProducesResponseType(400)]
        public IActionResult GetAnswers(int AnswerId)
    {
        try
        {
            // Check if the answer with the provided ID exists
            if (!answerRepository.AnswerExists(AnswerId))
            {
                // Return a 404 Not Found response if the answer does not exist
                return NotFound("Answer not found.");
            }

            // Retrieve the answer by its ID
            var answer = answerRepository.GetAnswer(AnswerId);

            // Check if the retrieved answer is null
            if (answer == null)
            {
                // Return a 404 Not Found response if the answer is null
                return NotFound("Answer not found.");
            }

            // Return a 200 OK response with the retrieved answer
            return Ok(answer);
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

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
       public IActionResult CreateAnswer([FromBody] Answers createAnswer)
    {
       try
       {
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
    public IActionResult UpdateAnswer(int answerId, [FromBody] Answers updatedAnswer)
   {
        try
        {  
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

