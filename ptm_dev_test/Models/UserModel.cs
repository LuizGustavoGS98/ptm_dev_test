using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ptm_dev_test.Models
{
    public class UserModel
    {
        [Key]
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
