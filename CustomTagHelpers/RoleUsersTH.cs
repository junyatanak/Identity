using System;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Identity.CustomTagHelpers;

[HtmlTargetElement("td",Attributes ="i-role")]
public class RoleUsersTH:TagHelper
{
    private UserManager<AppUser> userManager;
    private RoleManager<IdentityRole> roleManager;

    public RoleUsersTH(UserManager<AppUser> usermgr, RoleManager<IdentityRole> rolemgr)
    {
        userManager = usermgr;
        roleManager = rolemgr;
    }
    [HtmlAttributeName("i-role")]
    public string Role { get; set; } = string.Empty;

    public override async Task ProcessAsync(TagHelperContext context,TagHelperOutput output)
    {
        List<string> names = new List<string>();
        IdentityRole? role = await roleManager.FindByIdAsync(Role);
        if(role != null)
        {
            var users = userManager.Users.ToList();
            foreach(var user in users)
            {
                if(user != null && await userManager.IsInRoleAsync(user,role.Name!))
                names.Add(user.UserName!);
            }
        }
        output.Content.SetContent(names.Count == 0 ? "No Users" : string.Join(",", names));
    }


}
