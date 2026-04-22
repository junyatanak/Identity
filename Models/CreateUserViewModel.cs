using System.ComponentModel.DataAnnotations;

namespace Identity.Models;

    public class CreateUserViewModel
    {
    [Required]
    public required string Name { get; set; }
    [Required]
    [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
    public required string Email { get; set; }
    [Required]
    public required string Password { get; set; }
    public Country Country { get; set; }
    [Required]
    public int? Age { get; set; }
    [Required]
    public string Salary { get; set; }=string.Empty;
    
    }