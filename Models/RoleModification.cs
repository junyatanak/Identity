using System;
using System.ComponentModel.DataAnnotations;

namespace Identity.Models;

public class RoleModification
{
    [Required]
    public string RoleName { get; set; } = string.Empty;

    public string RoleId { get; set; } = string.Empty;
    public string[]? AddIds { get; set; }
    public string[]? DeleteIds { get; set; }

}
