using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Principal;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Main.Controllers
{   
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectAController : ControllerBase
    {
        // GET: api/ProjectA
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return User.Claims.Select(s => s.Type).ToList();
        }

        [Authorize(Roles ="Group1")]
        [Route("User")]
        [HttpGet]
        public string GetUser()
        {
            return User.Identity.Name;
        }

        [Authorize(Roles = "Group1")]
        [Route("Groups")]
        [HttpGet]
        public List<string> GetAllgroups()
        {
            List<string> groups = new List<string>();

            PrincipalContext ctx = GetPrincipalContext();

            PrincipalSearchResult<Principal> result = GetAllGroups(ctx);

            foreach (Principal p in result)
            {
                using (GroupPrincipal gp = (GroupPrincipal)p)
                {
                    groups.Add(gp.Name);
                }
            }

            return groups;
        }
                
        [Route("Roles")]       
        [HttpGet]
        public IEnumerable<string> GetRoles()
        {   
            var roles = User.Claims.Select(v => v.Value).ToList();
            return roles;
        }

        [Route("IsGroup")]
        [HttpGet]
        public string IsInGroup(string groupName)
        {
            return $"{User.Identity.Name} is {(Security.IsInGroup(User, groupName) ? ($"in the {groupName} ") :($"Not in the {groupName}"))}";
        }

        [Route("Users")]
        [HttpGet]
        public List<string> GetUsers()
        {
            List<string> users = new List<string>();

            PrincipalContext ctx = GetPrincipalContext();

            PrincipalSearchResult<Principal> result = GetAllUsers(ctx);

            foreach (Principal p in result)
            {
                using (UserPrincipal up = (UserPrincipal)p)
                {
                    users.Add(up.Name);
                }
            }

            return users;
        }
       

        
        private static PrincipalSearchResult<Principal> GetAllGroups(PrincipalContext ctx)
        {
            GroupPrincipal group = new GroupPrincipal(ctx);
            group.Name = "*";

            PrincipalSearcher ps = new PrincipalSearcher();
            ps.QueryFilter = group;

            PrincipalSearchResult<Principal> result = ps.FindAll();
            return result;
        }

        private static PrincipalSearchResult<Principal> GetAllUsers(PrincipalContext ctx)
        {
            UserPrincipal user = new UserPrincipal(ctx);
            user.Name = "*";

            PrincipalSearcher ps = new PrincipalSearcher();
            ps.QueryFilter = user;

            PrincipalSearchResult<Principal> result = ps.FindAll();
            return result;
        }

        private static PrincipalContext GetPrincipalContext()
        {
            return new PrincipalContext(ContextType.Machine, Environment.MachineName);
        }
    }
}