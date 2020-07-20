using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using WebAPI.Services;
using WebAPI.Security;
using TBS.Models;

namespace WebAPI.Controllers.Characters
{
    [Route("api/public/Characters")]
    [ApiController]
    public class Public : ControllerBase
    {
        private CharacterService m_service;

        public Public(CharacterService service)
        {
            m_service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Character resultObj = await m_service.Get(id);
            if (resultObj == null)
                return NotFound();

            return Ok(resultObj);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<Character> resultObj = await m_service.GetAll();
            if (resultObj == null)
                return NotFound();

            return Ok(resultObj);
        }
    }

    [Route("api/admin/Characters")]
    [ApiController]
    public class Admin : ControllerBase
    {
        private CharacterService m_service;

        public Admin(CharacterService service)
        {
            m_service = service;
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Put(string id, [FromBody] Character obj)
        {
            if (!(await m_service.Put(id, obj)))
                return NotFound();

            return Ok();
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Post([FromBody] Character obj)
        {
            Character resultObj = await m_service.Post(obj);
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
