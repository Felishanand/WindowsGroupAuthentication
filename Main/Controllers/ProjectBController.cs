using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Main.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectBController : ControllerBase
    {
        [Authorize(Roles = "Group2")]
        // GET: api/<ProjectBController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "projectb value1", "projectb value2" };
        }

        [Authorize(Roles = "Group1")]
        [Route("Group1")]
        [HttpGet]
        public string GetGroup1()
        {
            return "Group1 values";
        }

        [Authorize(Roles = "Group2")]
        [Route("Group2")]
        [HttpGet]
        public string GetGroup2()
        {
            return "Group2 values";
        }

        [Authorize(Roles = "Group2,Group1")]
        [Route("both")]
        [HttpGet]
        public string GetBoth()
        {
            return "Group1 and Group2 values";
        }


        [Authorize(Roles = "Group1")]
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return $"projectb value {id}";
        }

    }
}
