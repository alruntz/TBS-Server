using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using WebAPI.Services;
using WebAPI.Security;
using TBS.Models;

namespace WebAPI.Controllers.Maps
{
    [Route("api/public/Maps")]
    [ApiController]
    public class Public : ControllerBase
    {
        private MapService m_service;

        public Public(MapService service)
        {
            m_service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Map resultObj = await m_service.Get(id);
            if (resultObj == null)
                return NotFound();

            return Ok(resultObj);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<Map> resultObj = await m_service.GetAll();
            if (resultObj == null)
                return NotFound();

            return Ok(resultObj);
        }
    }

    [Route("api/admin/Maps")]
    [ApiController]
    public class Admin : ControllerBase
    {
        private MapService m_service;

        public Admin(MapService service)
        {
            m_service = service;
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Put(string id, [FromBody] Map obj)
        {
            if (!(await m_service.Put(id, obj)))
                return NotFound();

            return Ok();
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Post([FromBody] Map obj)
        {
            Map resultObj = await m_service.Post(obj);
            if (resultObj == null)
                return NotFound();

            return Ok(resultObj);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!(await m_service.Delete(id)))
                return NotFound();

            return Ok();
        }
    }
}
