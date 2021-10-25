using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Interfaces;

namespace WebStore.ServicesHosting.Controllers
{
    [Route(WebAPIAddresses.Values)]
    [ApiController]
    public class ValuesApiController : ControllerBase
    {
        private readonly ILogger<ValuesApiController> _logger;

        private readonly Dictionary<int, string> _values = Enumerable.Range(1, 10)
            .Select(i => (Id: i, Value: $"Value-{i}"))
            .ToDictionary(v => v.Id, v => v.Value);

        public ValuesApiController(ILogger<ValuesApiController> logger) 
        {
            _logger = logger;
        }

        //     public async Task<IEnumerable<string>> Get() { return await _values.Values; }

        [HttpGet]
        public IActionResult Get() => Ok(_values.Values);

        [HttpGet("{id}")]
        public IActionResult GetById(int id) 
        {
            if (!_values.ContainsKey(id))
            {
                return NotFound();
            }
            return Ok(_values[id]);
        }

        [HttpGet("count")]
        public IActionResult Count() 
        {
            return Ok(_values.Count);
        }

        [HttpPost]
        [HttpPost("add")]
        public IActionResult Add(string Value)
        {
            var id = _values.Count == 0 ? 1 : _values.Keys.Max() + 1;
            _values[id] = Value;

            return CreatedAtAction(nameof(GetById), new { Id = id });
        }

        [HttpPut("{id}")]
        public IActionResult Replace(int id, [FromBody] string Value)
        {
            if (!_values.ContainsKey(id))
            {
                return NotFound();
            }
            _values[id] = Value;

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!_values.ContainsKey(id))
            {
                return NotFound();
            }

            _values.Remove(id);
            return Ok();
        }
    }
}
