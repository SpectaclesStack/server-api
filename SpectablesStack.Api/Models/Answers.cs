using System.ComponentModel.DataAnnotations;

namespace stackup_vsc_setup.Model
{
    public class Answers
    {

    public int AnswerId{get;set;} 
    public int QuestionId{get;set;} 
    public int UserId{get;set;} 
    public DateTime CreateAt{get;set;} 

   }
}


