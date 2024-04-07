using spectaclesStackServer.Model;

namespace spectaclesStackServer.Interface
{
    public interface IQuestionsRepository
    {
        ICollection<Questions> GetQuestions();
        Questions GetQuestion(int questionId);
        bool QuestionExists(int questionId);
        bool CreateQuestion(Questions questions);
        bool UpdateQuestion(Questions questions);
        bool DeleteQuestion(Questions questions);
        bool Save();
    }
}