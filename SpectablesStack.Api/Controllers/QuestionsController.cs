using stackup_vsc_setup.Model;
using stackup_vsc_setup.Data;
using SpectablesStack.Api.Dto;
using SpectablesStack.Api.Interface;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace Stackup.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : Controller
    {
        private readonly IQuestionsRepository questionsRepository;
        private readonly IMapper mapper;

        public QuestionsController(IQuestionsRepository questionsRepository, IMapper mapper)
        {
            this.questionsRepository = questionsRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Questions>))]
        public IActionResult GetUser()
        {
            var questions = mapper.Map<List<QuestionsDto>>(questionsRepository.GetQuestions());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(questions);
        }

        [HttpGet("{QuestionId}")]
        [ProducesResponseType(200, Type = typeof(Questions))]
        [ProducesResponseType(400)]
        public IActionResult GetUsers(int QuestionId)
        {
            if (!questionsRepository.QuestionExists(QuestionId))
                return NotFound();

            var question = mapper.Map<QuestionsDto>(questionsRepository.GetQuestion(QuestionId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(question);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateQuestion([FromBody] QuestionsDto createQuestion)
        {
            if (createQuestion == null)
                return BadRequest(ModelState);

            var question = questionsRepository.GetQuestions()
                .Where(q => q.QuestionId == createQuestion.QuestionId)
                .FirstOrDefault();

            if(question != null)
            {
                ModelState.AddModelError("", "Question already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var questionC = mapper.Map<Questions>(createQuestion);

            if(!questionsRepository.CreateQuestion(questionC))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{QuestionId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateQuestion(int questionId, [FromBody]QuestionsDto updatedQuestion)
        {
            if (updatedQuestion == null)
                return BadRequest(ModelState);

            if (questionId != updatedQuestion.QuestionId)
                return BadRequest(ModelState);

            if (!questionsRepository.QuestionExists(questionId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var questionMap = mapper.Map<Questions>(questionId);

            if(!questionsRepository.UpdateQuestion(questionMap))
            {
                ModelState.AddModelError("", "Something went wrong updating question");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{QuestionId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteQuestion(int questionId)
        {
            if(!questionsRepository.QuestionExists(questionId))
            {
                return NotFound();
            }

            var questionToDelete = questionsRepository.GetQuestion(questionId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!questionsRepository.DeleteQuestion(questionToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting question");
            }

            return NoContent();
        }

    }
}