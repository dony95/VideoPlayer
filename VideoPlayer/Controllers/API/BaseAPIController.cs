using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoPlayer.DAL.Repository;
using VideoPlayer.Model;

namespace VideoPlayer.Controllers.API
{
    [Authorize]
    public class BaseAPIController<TEntity> : Controller where TEntity : EntityBase
    {
        public readonly IRepositoryBase<TEntity> Repository;

        public BaseAPIController(IRepositoryBase<TEntity> repository)
        {
            this.Repository = repository;
        }
        // GET: api/Cartoon
        [HttpGet, AllowAnonymous]
        public IEnumerable<TEntity> Get()
        {
            return Repository.GetList(null).Take(20);
        }

        //GET: api/Cartoon/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = Repository.Find(id);

            if (entity == null)
                return NotFound();

            return Ok(entity);
        }

        // POST: api/Cartoon
        [HttpPost]
        public IActionResult Post([FromBody]TEntity value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Repository.Add(value, autoSave: true);

            return CreatedAtAction("Get", new { id = value.ID }, value);
        }

        // PUT: api/Cartoon/5 
        //TODO: fixat put
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]TEntity value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = Repository.Find(id);

            if (entity == null)
                return NotFound();

            entity = value;
            entity.ID = id;

            Repository.Update(entity, autoSave: true);

            return CreatedAtAction("Get", new { id = entity.ID }, entity);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = Repository.Find(id);

            if (entity == null)
            {
                return NotFound();
            }

            Repository.Delete(id, autoSave: true);

            return Ok(entity);
        }
    }
}
