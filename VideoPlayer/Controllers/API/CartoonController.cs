using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoPlayer.DAL.Repository;
using VideoPlayer.Model;

namespace VideoPlayer.Controllers.API
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CartoonController : BaseAPIController<Cartoon>
    {
        //public readonly CartoonRepository CartoonRepository;

        public CartoonController(CartoonRepository repository) : base(repository) { }
        // GET: api/Cartoon
        //[HttpGet]
        //public IEnumerable<Cartoon> Get()
        //{
        //    return CartoonRepository.GetList(null).Take(20);
        //}

        //// GET: api/Cartoon/5
        //[HttpGet("{id}", Name = "Get")]
        //public IActionResult Get(int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    var cartoon = CartoonRepository.Find(id);

        //    if (cartoon == null)
        //        return NotFound();

        //    return Ok(cartoon);
        //}

        //// POST: api/Cartoon
        //[HttpPost]
        //public IActionResult Post([FromBody]Cartoon value)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    CartoonRepository.Add(value, autoSave:true);

        //    return CreatedAtAction("Get", new { id = value.ID }, value);
        //}

        //// PUT: api/Cartoon/5 
        ////TODO: fixat put
        //[HttpPut("{id}")]
        //public IActionResult Put(int id, [FromBody]Cartoon value)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var cartoon = CartoonRepository.Find(id);

        //    if (cartoon == null)
        //        return NotFound();

        //    cartoon = value;
        //    cartoon.ID = id;

        //    CartoonRepository.Update(cartoon, autoSave: true);

        //    return CreatedAtAction("Get", new { id = cartoon.ID }, cartoon);
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public IActionResult Delete(int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var cartoon = CartoonRepository.Find(id);

        //    if (cartoon == null)
        //    {
        //        return NotFound();
        //    }

        //    CartoonRepository.Delete(id, autoSave:true);

        //    return Ok(cartoon);
        //}
    }

    [Produces("application/json")]
    [ApiVersion("1.1")]
    [Route("api/v{version:apiVersion}/cartoon")]
    public class CartoonV1_1Controller : BaseAPIController<Cartoon>
    {
        public CartoonV1_1Controller(CartoonRepository repository) : base(repository) { }
    }
}