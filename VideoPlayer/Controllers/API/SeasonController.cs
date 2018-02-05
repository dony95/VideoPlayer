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
    public class SeasonController : BaseAPIController<Season>
    {
        public SeriesRepository SeriesRepository;
        public SeasonController(SeasonRepository repository, SeriesRepository SeriesRepository) : base(repository)
        {
            this.SeriesRepository = SeriesRepository;
        }

        // POST: api/Cartoon
        [HttpPost]
        public override IActionResult Post([FromBody]Season value)
        {
            var series = SeriesRepository.Find(value.SeriesId);

            if (!ModelState.IsValid || series == null)
            {
                return BadRequest(ModelState);
            }

            Repository.Add(value, autoSave: true);

            return CreatedAtAction("Get", new { id = value.ID }, value);
        }

        // PUT: api/Cartoon/5 
        //TODO: fixat put
        [HttpPut("{id}")]
        public override IActionResult Put(int id, [FromBody]Season value)
        {
            var series = SeriesRepository.Find(value.SeriesId);

            if (!ModelState.IsValid || series == null)
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
    }


    [Produces("application/json")]
    [ApiVersion("1.1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SeasonV1_1Controller : BaseAPIController<Season>
    {
        public SeriesRepository SeriesRepository;
        public SeasonV1_1Controller(SeasonRepository repository, SeriesRepository SeriesRepository) : base(repository)
        {
            this.SeriesRepository = SeriesRepository;
        }

        // POST: api/Cartoon
        [HttpPost]
        public override IActionResult Post([FromBody]Season value)
        {
            var series = SeriesRepository.Find(value.SeriesId);

            if (!ModelState.IsValid || series == null)
            {
                return BadRequest(ModelState);
            }

            Repository.Add(value, autoSave: true);

            return CreatedAtAction("Get", new { id = value.ID }, value);
        }

        // PUT: api/Cartoon/5 
        //TODO: fixat put
        [HttpPut("{id}")]
        public override IActionResult Put(int id, [FromBody]Season value)
        {
            var series = SeriesRepository.Find(value.SeriesId);

            if (!ModelState.IsValid || series == null)
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
    }
}