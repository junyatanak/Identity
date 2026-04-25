using System;
using System.ComponentModel.DataAnnotations;

namespace Identity.Models;

public class UpdateUserViewModel
{
    [Required]
    public string Id { get; set; } = string.Empty; 
    [Required]
    [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
    public string Email { get; set; } = string.Empty;
    public string? Password { get; set; }
    [Required]
    public Country? Country { get; set; }
    [Required]
    public int? Age { get; set; }
    [Required]
    public string Salary { get; set; }=string.Empty;



}
