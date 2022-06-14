using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMovies.Models
{

        public class UserInfo : IdentityUser
    {
        [Key]
        public int Id { get; set; } 
        [ForeignKey("IdentityUser")]
        public IdentityUser user { get; set; }
        public int tip { get; set; }    
    }
}
