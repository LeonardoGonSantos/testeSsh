using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SshNet;
using Renci.SshNet;

namespace testeSsh.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SshController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<SshController> _logger;

        public SshController(ILogger<SshController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public dynamic Get(string comand)
        {
            SshClient cSSH = new SshClient("localhost", 22, "leonardo.goncalves@pacerwm.com", "Password");

            cSSH.Connect();

            string answer = "";

            if (comand.Split(";").Count() > 0)
            {
                var array = comand.Split(";");

                foreach (var unitComand in array)
                {
                    SshCommand sc = cSSH.CreateCommand(unitComand);

                    sc.Execute();

                    answer += sc.Result;
                }

            }
            else
            {
                SshCommand sc = cSSH.CreateCommand(comand);

                sc.Execute();

                answer += sc.Result;
            }



            if (comand == "exit disconnect")
            {
                disconect(cSSH);
            }

            return answer;
        }

        public void disconect(SshClient cSSH)
        {
            cSSH.Disconnect();
            cSSH.Dispose();
        }
    }
}
