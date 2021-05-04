using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private IHomeAdaptor _homeAdaptor;

        public HomeController(IHomeAdaptor homeAdaptor)
        {
            _homeAdaptor = homeAdaptor;
        }

        [HttpGet]
        [Route("Group1")]
        public async Task<IActionResult> GetGroup1Async()
        {
            try
            {
                var result = _homeAdaptor.GetGroup1Values();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("Group2")]
        public async Task<IActionResult> GetGroup2Async()
        {
            try
            {
                var result = _homeAdaptor.GetGroup2Values();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("Both")]
        public async Task<IActionResult> GetBothAsync()
        {
            try
            {
                var result = _homeAdaptor.GetBothValues();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
