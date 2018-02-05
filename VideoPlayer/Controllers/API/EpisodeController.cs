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
    public class EpisodeController : BaseAPIController<Episode>
    {
        public SeasonRepository SeasonRepository;
        public EpisodeController(EpisodeRepository repository, SeasonRepository seasonRepository): base(repository)
        {
            this.SeasonRepository = seasonRepository;
        }

        // POST: api/Cartoon
        [HttpPost]
        public override IActionResult Post([FromBody]Episode value)
        {
            var season = SeasonRepository.Find(value.SeasonId);

            if (!ModelState.IsValid || season == null)
            {
                return BadRequest(ModelState);
            }

            Repository.Add(value, autoSave: true);

            return CreatedAtAction("Get", new { id = value.ID }, value);
        }

        // PUT: api/Cartoon/5 
        //TODO: fixat put
        [HttpPut("{id}")]
        public override IActionResult Put(int id, [FromBody]Episode value)
        {
            var season = SeasonRepository.Find(value.SeasonId);

            if (!ModelState.IsValid || season == null)
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
    public class EpisodeV1_1Controller : BaseAPIController<Episode>
    {
        public SeasonRepository SeasonRepository;
        public EpisodeV1_1Controller(EpisodeRepository repository, SeasonRepository seasonRepository) : base(repository)
        {
            this.SeasonRepository = seasonRepository;
        }

        [HttpPost]
        public override IActionResult Post([FromBody]Episode value)
        {
            var season = SeasonRepository.Find(value.SeasonId);

            if (!ModelState.IsValid || season == null)
            {
                return BadRequest(ModelState);
            }

            Repository.Add(value, autoSave: true);

            return CreatedAtAction("Get", new { id = value.ID }, value);
        }

        // PUT: api/Cartoon/5 
        //TODO: fixat put
        [HttpPut("{id}")]
        public override IActionResult Put(int id, [FromBody]Episode value)
        {
            var season = SeasonRepository.Find(value.SeasonId);

            if (!ModelState.IsValid || season == null)
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