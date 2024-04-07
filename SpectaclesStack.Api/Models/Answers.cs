using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.Internal.Mapping;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace spectaclesStackServer.Model
{
    [Table("answers")]
    public class Answers {

        [Key]
        [Column("answerid")]
        public int AnswerId {get;set;}

        //[Column("questionid")]
        //public int QuestionId { get; set; }
        //[Column("questionid")]
        [ForeignKey("questionid")]
        public required Questions Questions { get;set;}

        [ForeignKey("userid")]
        public required Users Users { get;set;}

        [Column("createdat")]
        public required DateTime CreateAt { get; set; }

    }
}


