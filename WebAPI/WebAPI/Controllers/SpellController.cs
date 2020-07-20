using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using WebAPI.Services;
using WebAPI.Security;
using TBS.Models;

namespace WebAPI.Controllers.Spells
{
    [Route("api/public/Spells")]
    [ApiController]
    public class Public : ControllerBase
    {
        private SpellService m_service;

        public Public(SpellService service)
        {
            m_service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Spell resultObj = await m_service.Get(id);
            if (resultObj == null)
                return NotFound();

            return Ok(resultObj);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<Spell> resultObj = await m_service.GetAll();
            if (resultObj == null)
                return NotFound();

            return Ok(resultObj);
        }
    }

    [Route("api/admin/Spells")]
    [ApiController]
    public class Admin : ControllerBase
    {
        private SpellService m_service;

        public Admin(SpellService service)
        {
            m_service = service;
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Put(string id, [FromBody] Spell obj)
        {
            if (!(await m_service.Put(id, obj)))
                return NotFound();

            return Ok();
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Post([FromBody] Spell obj)
        {
            Spell resultObj = await m_service.Post(obj);
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
