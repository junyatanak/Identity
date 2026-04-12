using System;
using Microsoft.AspNetCore.Identity;

namespace Identity.Models;

public class RoleEdit
{
    public IdentityRole Role { get; set; } = new IdentityRole();
    public IEnumerable<AppUser> Members { get; set; } = new List<AppUser>();
    public IEnumerable<AppUser> NonMembers { get; set; } = new List<AppUser>();

}
