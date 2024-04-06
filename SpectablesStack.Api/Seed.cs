using stackup_vsc_setup.Model;
using stackup_vsc_setup.Data;

namespace stackup_vsc_setup
{
    public class Seed
    {
        private readonly DataContext dataContext;
        DateTime currentDateTime = DateTime.Now;

        public Seed(DataContext context)
        {
            this.dataContext = context;
        }

        public void SeedDataContext()
        {
        
            if (!dataContext.Users.Any())
            {
                var users = new List<Users>()
                {
                    new Users()
                    {
                            UserId = 1,
                            UserName = "Duncan211",
                            Email = "Duncan@123.com",
                            CreateAt = currentDateTime,
                    }
                };
                users.ForEach(s => dataContext.Users.Add(s));  
                dataContext.SaveChanges();  
                       
            var questions = new List<Questions>()
            {
                    new Questions()
                    {
                        QuestionId = 1,
                        Title = "Asking questions",
                        Body = "Here's a question",
                        CreateAt = currentDateTime    
                    }
                };
                questions.ForEach(s => dataContext.Questions.Add(s));  
                dataContext.SaveChanges(); 

                var answers = new List<Answers>()
                {
                    new Answers()
                    {
                        AnswerId = 1,
                        QuestionId = 1, 
                        UserId = 1, 
                        CreateAt = currentDateTime 
                    }
                };
                answers.ForEach(s => dataContext.Answers.Add(s));  
                dataContext.SaveChanges();   
            }
        }    
    }          
}
