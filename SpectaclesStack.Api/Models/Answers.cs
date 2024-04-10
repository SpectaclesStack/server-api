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

        [ForeignKey("questionid")]
        public required int questionid { get;set;}

        [ForeignKey("userid")]
        public required int userid { get;set;}

        [Column("createdat")]
        public required DateTime CreateAt { get; set; }

        [Column ("answer")]
        public required string answer{get;set;}
    }
}


