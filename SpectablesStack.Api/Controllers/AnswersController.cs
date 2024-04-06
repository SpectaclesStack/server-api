using stackup_vsc_setup.Model;
using stackup_vsc_setup.Data;
using SpectablesStack.Api.Dto;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using SpectablesStack.Api.Interface;
using SpectablesStack.Api.Repository;
using SpectablesStack.Api.Interface;


namespace SpectablesStack.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AnswersController : Controller
    {
        private readonly IAnswersRepository answerRepository;
        private readonly IMapper mapper;

        public AnswersController(IAnswersRepository answerRepository, IMapper mapper)
        {
            this.answerRepository = answerRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Answers>))]
        public IActionResult GetAnswers()
        {
            var answers = mapper.Map<List<AnswersDto>>(answerRepository.GetAnswers());

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

            var answer = mapper.Map<AnswersDto>(answerRepository.GetAnswer(AnswerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(answer);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateAnswer([FromBody] AnswersDto createAnswer)
        {
            if (createAnswer == null)
                return BadRequest(ModelState);

            var answer = answerRepository.GetAnswers()
                .Where(a => a.AnswerId == createAnswer.AnswerId)
                .FirstOrDefault();

            if(answer != null)
            {
                ModelState.AddModelError("", "Answer already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var answerC = mapper.Map<Answers>(createAnswer);

            if(!answerRepository.CreateAnswer(answerC))
            {
                ModelState.AddModelError("", "Something went wrong while saving the answer");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{AnswerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateAnswer(int answerId, [FromBody]AnswersDto updatedAnswer)
        {
            if (updatedAnswer == null)
                return BadRequest(ModelState);

            if (answerId != updatedAnswer.AnswerId)
                return BadRequest(ModelState);

            if (!answerRepository.AnswerExists(answerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var answerMap = mapper.Map<Answers>(updatedAnswer);

            if(!answerRepository.UpdateAnswer(answerMap))
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
            if(!answerRepository.AnswerExists(answerId))
            {
                return NotFound();
            }

            var answerToDelete = answerRepository.GetAnswer(answerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!answerRepository.DeleteAnswer(answerToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting the answer");
            }

            return NoContent();
        }

    }
}
        
    