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


}
