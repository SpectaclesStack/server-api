using spectaclesStackServer.Model;

namespace spectaclesStackServer.Interface
{
    public interface IAnswersRepository
    {
        ICollection<Answers> GetAnswers();
        Answers GetAnswer(int answerId);
        bool AnswerExists(int answerId);
        bool CreateAnswer(Answers answers);
        bool UpdateAnswer(Answers answers);
        bool DeleteAnswer(Answers answers);
        bool Save();
    }
}