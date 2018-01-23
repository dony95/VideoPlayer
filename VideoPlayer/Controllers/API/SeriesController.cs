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
    [Route("api/Series")]
    public class SeriesController : BaseAPIController<Series>
    {
        public SeriesController(SeriesRepository repository) : base(repository) { }
    }
}