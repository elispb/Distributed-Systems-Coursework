using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SecuroteckWebApplication.Controllers
{
    public class TalkBackController : ApiController
    {
        [ActionName("Hello")]
        public string Get()
        {
            string response = "Hello World";
            return response;

        }

        [ActionName("Sort")]
        public IHttpActionResult Get([FromUri]string[] integers)
        {
            int value;
            int i = 0;
            int[] parsedInts = new int[integers.Length];
            if (integers.Length == 1 && string.IsNullOrEmpty(integers[0]))
            {
                parsedInts = new int[0];
                return Ok(parsedInts);
            }
            foreach (string s in integers)
            {
                if (int.TryParse(s, out value))
                {
                    parsedInts[i] = value;
                }
                else
                { return BadRequest(); }
                i++;
            }
            Array.Sort(parsedInts);
            return Ok(parsedInts);
        }

    }
}
