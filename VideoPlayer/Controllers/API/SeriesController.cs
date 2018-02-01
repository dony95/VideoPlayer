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
    public class SeriesController : BaseAPIController<Series>
    {
        public SeriesController(SeriesRepository repository) : base(repository) { }
    }

    [Produces("application/json")]
    [ApiVersion("1.1")]
    [Route("api/v{version:apiVersion}/Series")]
    public class SeriesV1_1Controller : BaseAPIController<Series>
    {
        public SeriesV1_1Controller(SeriesRepository repository) : base(repository) { }
    }
}