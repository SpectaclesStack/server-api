using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NpgsqlTypes;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.Internal.Mapping;
namespace spectaclesStackServer.Model
{
    [Table("users")]
    public class Users {

        [Key]
        [Column("userid")]
        public int UserId{get;set;}

        [Column("username")]
        public required string UserName {get;set;}

        [Column("datecreated")]
        public required DateTime? CreateAt { get; set; }

    }

}