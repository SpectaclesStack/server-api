namespace SpectablesStack.Api.Dto
{
    public class QuestionsDto
    {
        public int QuestionId{get;set;} 
        public int UserId{get;set;} 
        public string? Title{get;set;} 
        public string? Body{get;set;} 
        public DateTime CreateAt{get;set;} 
    }
}