using SpectablesStack.Api.Interface ;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using stackup_vsc_setup.Model;
using stackup_vsc_setup.Data;

namespace SpectablesStack.Api.Repository
{
    public class QuestionsRepository : IQuestionsRepository

    {
        private readonly DataContext context;
        private readonly IMapper mapper;

        public QuestionsRepository(DataContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public bool QuestionExists(int questionId)
        {
            return context.Questions.Any(q => q.QuestionId == questionId);
        }

        public bool CreateQuestion(Questions questions)
        {
            context.Add(questions);
            return Save();
        }

        public bool DeleteQuestion(Questions questions)
        {
            context.Remove(questions);
            return Save();
        }

        public ICollection<Questions> GetQuestions()
        {
            return context.Questions.OrderBy(q => q.QuestionId ).ToList();
        }

        public Questions GetQuestion(int questionId)
        {
            return context.Questions.Where(q => q.QuestionId == questionId).FirstOrDefault();
        }

        public bool Save()
        {
            var saved = context.SaveChanges();
            return saved > 0;
        }

        public bool UpdateQuestion(Questions questions)
        {
            context.Update(questions);
            return Save();
        }
    }
}