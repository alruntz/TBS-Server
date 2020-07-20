using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TBS.Models;
using WebAPI.Services;
using WebAPI.Security;

namespace WebAPI.Controllers.Items
{
    [Route("api/public/items")]
    [ApiController]
    public class Public : ControllerBase
    {
        private ItemService m_service;

        public Public(ItemService service)
        {
            m_service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Item resultObj = await m_service.Get(id);
            if (resultObj == null)
                return NotFound();

            return Ok(resultObj);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<Item> resultObj = await m_service.GetAll();
            if (resultObj == null)
                return NotFound();

            return Ok(resultObj);
        }
    }


    [Route("api/admin/items")]
    [ApiController]
    public class Admin : ControllerBase
    {
        private ItemService m_service;

        public Admin(ItemService service)
        {
            m_service = service;
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Put(string id, [FromBody] Item obj)
        {
            if (!(await m_service.Put(id, obj)))
                return NotFound();

            return Ok();
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Post([FromBody] Item obj)
        {
            Item resultObj = await m_service.Post(obj);
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
