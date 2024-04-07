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

        //[Column("userid")]
        //public int UserId { get; set; }

        //[Column("userid")]
        [ForeignKey("UserId")]
        public required Users Users { get;set;}

        [Column("title")]
        public required string Title {get;set;}

        [Column("body")]
        public required string Body {get;set;}

        [Column("createdat")]
        public required DateTime CreateAt {get;set;} 

    }

}


