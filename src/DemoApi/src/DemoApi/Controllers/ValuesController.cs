using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoApi.Model.Contract;
using Microsoft.AspNetCore.Mvc;

namespace DemoApi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly ISampleService _sampleService;

        public ValuesController(ISampleService sampleService)
        {
            _sampleService = sampleService;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var sampleResult = _sampleService.GetSomeValue();
            return new string[] { "val22333ue8", sampleResult };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
