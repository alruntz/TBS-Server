using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using WebAPI.Services;
using TBS.Models;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;

namespace WebAPI.Controllers.Players
{
    #region Public EndPoints
    [Route("api/public/players")]
    [ApiController]
    public class Public : ControllerBase
    {
        private readonly PlayerService _playerService;

        public Public(PlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {     
            List<Player> player = await _playerService.GetPlayers();
            if (player == null)
                return NotFound();

            return Ok(player);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Player player = await _playerService.GetPlayer(id);

            if (player == null)
            {
                return NotFound();
            }

            return Ok(player);
        }
    }
    #endregion


    #region Private EndPoints
    [Route("api/private/players")]
    [ApiController]
    public class Private : ControllerBase
    {
        private readonly PlayerService m_service;

        public Private(PlayerService playerService)
        {
            m_service = playerService;
        }

        [HttpGet]
        [Authorize(Policy = "Private")]
        public async Task<IActionResult> Get()
        {
            Player player = await m_service.GetPlayer(HttpContext.User);
            if (player == null)
                return NotFound();

            return Ok(player);
        }

        [HttpPut]
        [Authorize(Policy = "Private")]
        public async Task<IActionResult> Put([FromBody] JObject player)
        {
            BsonDocument bson = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(player.ToString());
            if (!(await m_service.PutPlayer((await m_service.GetPlayer(HttpContext.User)).Id, bson)))
                return NotFound();

            return Ok();
        }

        [Route("team")]
        [HttpPut]
        [Authorize(Policy = "Private")]
        public async Task<IActionResult> Put([FromBody]Team team)
        {
            if (!(await m_service.PutTeam((await m_service.GetPlayer(HttpContext.User)).Id, team)))
                return NotFound();

            return Ok();
        }
    }

    #endregion


    #region Admin EndPoints
    [Route("api/admin/players")]
    [ApiController]
    public class Admin : ControllerBase
    {
        private readonly PlayerService m_service;

        public Admin(PlayerService playerService)
        {
            m_service = playerService;
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Put(string id, [FromBody] JObject player)
        {
            BsonDocument bson = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(player.ToString());
            if (!(await m_service.PutPlayer(id, bson)))
                return NotFound();

            return Ok();
        }

        [HttpPut("{id}/team")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Put(string id, [FromBody]Team team)
        {
            if (!(await m_service.PutTeam(id, team)))
                return NotFound();

            return Ok();
        }

        [HttpPut("{id}/items")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Put(string id, [FromBody]List<string> items)
        {
            Console.WriteLine("Put items : start");
            if (items == null) { Console.WriteLine("Put items : items == null"); return NotFound(); }
            if (!(await m_service.PutItems(id, items))) { Console.WriteLine("Put items : Put item service false"); return NotFound(); }

            return Ok();
        }
    }
    #endregion
}
