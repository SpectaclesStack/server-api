using stackup_vsc_setup.Model;

namespace SpectablesStack.Api.Interface 
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