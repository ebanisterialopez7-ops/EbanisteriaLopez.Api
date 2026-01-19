using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EbanisteriaLopez.Api.Models
{
    [Table("AspNetUsers")]
    public class Usuario
    {
        internal string UserName;

        [Key]
        public string Id { get; set; } = string.Empty;

        [StringLength(256)]
        public string? Email { get; set; }

        public string? PasswordHash { get; set; }
    }
}