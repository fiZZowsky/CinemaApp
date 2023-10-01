using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Application.CinemaApp
{
    public class ForgotPasswordModelDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
