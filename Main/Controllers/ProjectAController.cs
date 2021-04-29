using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Main.Controllers
{
    [Authorize]
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

        [Authorize]
        [Route("User")]
        [HttpGet]
        public string GetUser()
        {
            return User.Identity.Name;
        }

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

        // GET: api/ProjectA/Role
        [Route("Roles")]
        [HttpGet]
        public IEnumerable<string> GetRoles()
        {
            var roles = User.Claims.Select(v => v.Value).ToList();
            return roles;
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