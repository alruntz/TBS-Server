using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using WebAPI.Services;
using WebAPI.Security;
using TBS.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace WebAPI.Controllers.Users
{
    [Route("api/public/users")]
    [ApiController]
    public class Public : ControllerBase
    {
        private readonly UserService m_service;

        public Public(UserService service)
        {
            m_service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            if (user == null)
                return NotFound();

            User userCreate = await m_service.CreateUser(user);

            if (userCreate == null)
                return NotFound();

            return Ok(userCreate);
        }
    }

    [Route("api/private/users")]
    [ApiController]
    public class Private : ControllerBase
    {
        private readonly UserService m_service;

        public Private(UserService service)
        {
            m_service = service;
        }

        [HttpGet]
        [Authorize(Policy = "Private")]
        public async Task<IActionResult> Get()
        {
            User user = await m_service.GetUser(HttpContext.User);
            if (user == null)
                return Unauthorized();

            user.Password = null;
            return Ok(user);
        }

        [HttpPut]
        [Authorize(Policy = "Private")]
        public async Task<IActionResult> Put([FromBody] JObject user)
        {
            BsonDocument bson = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(user.ToString());
            if (bson == null)
                return NotFound();

            User myUser = await Authentication.GetUser(HttpContext.User);

            if (! (await m_service.PutUser(bson, myUser, myUser)))
                return NotFound();

            return Ok();
        }
    }

    [Route("api/private/users")]
    [ApiController]
    public class Admin : ControllerBase
    {
        private readonly UserService m_service;

        public Admin(UserService service)
        {
            m_service = service;
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Private")]
        public async Task<IActionResult> Put(string id, [FromBody] JObject user)
        {
            BsonDocument bson = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(user.ToString());
            if (bson == null)
                return NotFound();

            User myUser = await Authentication.GetUser(HttpContext.User);
            User targetUser = await m_service.GetUser(id);

            if (targetUser == null)
                return NotFound();

            if (!(await m_service.PutUser(bson, targetUser, myUser)))
                return NotFound();

            return Ok();
        }
    }
}
