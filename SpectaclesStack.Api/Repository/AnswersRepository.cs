using spectaclesStackServer.Data;
using spectaclesStackServer.Interface;
using spectaclesStackServer.Model;

namespace spectaclesStackServer.Repository
{
    public class AnswersRepository : IAnswersRepository
    {
        private readonly DataContext context;

        public AnswersRepository(DataContext context)
        {
            this.context = context;
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
            return context.Answers.OrderBy(a => a.AnswerId).ToList();
        }

        public Answers GetAnswer(int answerId)
        {
            return context.Answers.Where(a => a.AnswerId == answerId).FirstOrDefault()!;
        }

        public bool Save()
        {
            var saved = context.SaveChanges();
            return saved > 0;
        }

        public bool UpdateAnswer(Answers answers)
        {
            context.Update(answers);
            return Save();
        }
    }
}
