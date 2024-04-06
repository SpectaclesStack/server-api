using SpectablesStack.Api.Interface;
using AutoMapper;
using stackup_vsc_setup.Model;
using stackup_vsc_setup.Data;

namespace Stackup.Api.Repository
{
    public class AnswersRepository : IAnswersRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;

        public AnswersRepository(DataContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public bool AnswerExists(int answerId)
        {
            return context.Answers.Any(a => a.AnswerId == answerId);
        }

        public bool CreateAnswer(Answers answers)
        {
            context.Add(answers);
            return Save();
        }

        public bool DeleteAnswer(Answers answers)
        {
            context.Remove(answers);
            return Save();
        }

        public ICollection<Answers> GetAnswers()
        {
            return context.Answers.OrderBy(a => a.AnswerId ).ToList();
        }

        public Answers GetAnswer(int answerId)
        {
            return context.Answers.Where(a => a.AnswerId == answerId).FirstOrDefault();
        }

        public bool Save()
        {
            var saved = context.SaveChanges();
            return saved > 0 ;
        }

        public bool UpdateAnswer(Answers answers)
        {
            context.Update(answers);
            return Save();
        }
    }
}
    