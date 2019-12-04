using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace OTPSMS.API.Controllers
{
    [ApiController]
    [Route("api/otp/sms")]
    public class OTPController : ControllerBase
    {
        public OTPController()
        {
            
        }

        [HttpPost("send")]
        public async Task Send(string phone)
        {
            
        }

        [HttpGet("verify")]
        public async Task Verify()
        {
            
        }

        private string GenerateOTP()
        {
            var generator = new Random();
            String code = generator.Next(0, 999999).ToString("D6");
            return code;
        }
    }
}
