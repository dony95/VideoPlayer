using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VideoPlayer.DAL.Repository;
using VideoPlayer.Models;
using VideoPlayer.Model;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VideoPlayer.Controllers
{
    public class FilmController : BaseController<Film>
    {
        public FilmRepository FilmRepository;
        public FilmController(FilmRepository repository) : base(repository) { this.FilmRepository = repository; }

        [HttpGet]
        [Route("film/Download/{id}")]
        [ActionName("DownloadFilm")]
        public override IActionResult Download(int? id = null)
        {
            if (id == null)
                return View("Index", FilmRepository.GetList(null));

            var video = FilmRepository.Find(id.Value);
            var fileContents = System.IO.File.ReadAllText(@"data/script.bat");

            if (video.SubtitleURL != null)
            {
                var subfileContents = System.IO.File.ReadAllText(@"data/titlovi_skripta.bat");
                subfileContents = subfileContents.Replace("#_URL", video.SubtitleURL.Replace("%", "%%"));
                subfileContents = subfileContents.Replace("#_FILENAME", video.Name + ".srt");
                fileContents = fileContents.Replace("#_TITLOVI", subfileContents);
            }
            else
                fileContents = fileContents.Replace("#_TITLOVI", "");

            fileContents = fileContents.Replace("#_LINK", video.VideoURL.Replace("%", "%%"));
            if (video.SubtitleURL != null) fileContents = fileContents.Replace("#_SUB", "-- sub-file=\"c:\\Documents and settings\\%username%\\Documents\\titlovi\\"
                + video.Name + ".srt\" --sout-transcode-senc=\"Eastern European(Windows-1250)\"");
            else
                fileContents = fileContents.Replace("#_SUB", "");

            return File(Encoding.ASCII.GetBytes(fileContents.Replace("192.168.1.8", "donyslav.ddns.net")), "application/bat", video.Name + ".bat");
        }
    }
}