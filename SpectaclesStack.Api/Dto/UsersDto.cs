namespace spectaclesStackServer.Dto
{
    public class UsersDto
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public DateTime CreateAt { get; set; }
    }
}