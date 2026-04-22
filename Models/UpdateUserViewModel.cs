using System;
using System.ComponentModel.DataAnnotations;

namespace Identity.Models;

public class UpdateUserViewModel
{
    [Required]
    public string Id { get; set; } = string.Empty; 


}
