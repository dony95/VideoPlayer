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
    public class FilmController : BaseAPIController<Film>
    {
        public FilmController(FilmRepository repository) : base(repository) { }
    }

    [Produces("application/json")]
    [ApiVersion("1.1")]
    [Route("api/v{version:apiVersion}/film")]
    public class FilmV1_1Controller : BaseAPIController<Film>
    {
        public FilmV1_1Controller(FilmRepository repository) : base(repository) { }
    }
}