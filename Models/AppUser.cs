using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Identity.Models;

public class AppUser:IdentityUser
{
    public Country Country { get; set; }
    [Required]
    public int? Age { get; set; }
    [Required]
    public string Salary { get; set; } = string.Empty;


}
