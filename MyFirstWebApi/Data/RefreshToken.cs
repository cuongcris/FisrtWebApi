using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyFirstWebApi.Data
{
    [Table("RefreshToken")]
    public class RefreshToken
    {
        [Key]
        public Guid Id { get; set; }
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public string Token { get; set; }
        public string JwtId { get; set; }
        public bool isUsed { get; set; }
        public bool isRevoke { get; set; }
        public DateTime ExpireAt { get; set; }
        public DateTime IssueAt { get; set; } //create date
    }
}
