using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("user")]
    public class User
    {
        [Key]
        public int user_id { get; set; }

        [Required]
        [MaxLength(500)]
        public string user_name { get; set; }

        [Required]
        [MaxLength(500)]
        public string password { get; set; }

        [Required]
        [MaxLength(500)]
        public string salt { get; set; }

        [Required]
        public int login_fail_count { get; set; }

        [Required]
        public bool is_lock { get; set; }
    }
}