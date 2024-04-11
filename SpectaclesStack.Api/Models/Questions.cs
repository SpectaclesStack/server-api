using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace spectaclesStackServer.Model
{
    [Table("questions")]
    public class Questions 
    {

        [Key]
        [Column("questionid")]
        public int QuestionId{get;set;}

        [ForeignKey("userid")]
        public int userid { get; set; }
    
        [Column("title")]
        public required string Title {get;set;}

        [Column("body")]
        public required string Body {get;set;}

        [Column("createdat")]
        public required DateTime CreateAt {get;set;} 
    }
}


