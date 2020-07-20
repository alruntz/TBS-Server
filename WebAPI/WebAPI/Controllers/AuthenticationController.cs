using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using WebAPI.Services;
using TBS.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationService m_service;

        public AuthenticationController(AuthenticationService service)
        {
            m_service = service;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get(string username, string password)
        {
            IActionResult response = Unauthorized();
            string tokenString = await m_service.GetToken(username, password);

            if (tokenString != null)
                response = Ok(new { token = tokenString } );

            return response;
        }
    }
}
