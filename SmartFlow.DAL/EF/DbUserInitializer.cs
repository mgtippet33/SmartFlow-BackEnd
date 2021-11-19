using Microsoft.AspNetCore.Identity;
using SmartFlow.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.DAL.EF
{
    public class DbUserInitializer
    {
        public static async Task RoleInitializeAsync(
            RoleManager<IdentityRole<int>> roleManager)
        {
            if (await roleManager.FindByNameAsync("Administrator") == null)
            {
                await roleManager.CreateAsync(new IdentityRole<int>("Administrator"));
            }
            if (await roleManager.FindByNameAsync("Visitor") == null)
            {
                await roleManager.CreateAsync(new IdentityRole<int>("Visitor"));
            }
            if (await roleManager.FindByNameAsync("BusinessPertner") == null)
            {
                await roleManager.CreateAsync(new IdentityRole<int>("BusinessPertner"));
            }
        }
    }
}
