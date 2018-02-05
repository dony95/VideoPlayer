using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VideoPlayer.DAL.Repository;
using VideoPlayer.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using VideoPlayer.Models;
using System.Text;
using Microsoft.Extensions.Logging;

namespace VideoPlayer.Controllers
{
    public class SeriesController : BaseController<Series>
    {
        public SeriesRepository SeriesRepository;
        public SeasonRepository SeasonRepository;
        public EpisodeRepository EpisodeRepository;
        public SeriesController(SeriesRepository SeriesRepository, SeasonRepository SeasonRepository, EpisodeRepository EpisodeRepository) : base(SeriesRepository)
        {
            this.SeriesRepository = SeriesRepository;
            this.EpisodeRepository = EpisodeRepository;
            this.SeasonRepository = SeasonRepository;
        }

        [HttpGet]
        [Route("series/Download/{id}")]
        [ActionName("Download")]
        public ActionResult DownloadEpisode(int? id = null)
        {
            if (id == null)
                return View("Index", SeriesRepository.GetList(null));

            Episode video = SeriesRepository.FindEpisode(id.Value);
            var fileContents = System.IO.File.ReadAllText(@"~/App_Data/script.bat");

            if (video.SubtitleURL != null)
            {
                var subfileContents = System.IO.File.ReadAllText(@"~/App_Data/titlovi_skripta.bat");
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

            return File(Encoding.ASCII.GetBytes(fileContents.Replace("192.168.1.8", "donyslav.ddns.net")), "text/plain", video.Name + ".bat");
        }
        public ActionResult CreateSeason(int seriesID)
        {
            FillDropDownValues(null);
            var model = new Season() { SeriesId = seriesID };
            return View("Season/Create", model);
        }

        [HttpPost]
        public ActionResult CreateSeason(Season model, int seriesID)
        {
            if (ModelState.IsValid)
            {
                model.SeriesId = seriesID;
                this.SeasonRepository.Add(model, autoSave: true);
                return RedirectToAction("Index");
            }
            else
            {
                this.FillDropDownValues(SeriesRepository.Find(seriesID).Categories);
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult EditSeason(int seasonid)
        {
            var model = this.SeasonRepository.Find(seasonid);
            this.FillDropDownValues(null);
            return View(model);
        }

        [HttpPost]
        [ActionName("EditSeason")]
        public async Task<ActionResult> EditPostSeasonAsync(int seasonid, int seriesID)
        {
            var model = this.SeasonRepository.Find(seasonid);
            var didUpdateModelSucceed = await this.TryUpdateModelAsync(model);

            if (didUpdateModelSucceed && ModelState.IsValid)
            {
                this.SeasonRepository.Update(model, autoSave: true);
                return RedirectToAction("Details/"+seriesID);
            }

            this.FillDropDownValues(null);
            return View(model);
        }

        [HttpGet]
        public IActionResult CreateEpisode(int seriesID, int seasonID)
        {
            ViewBag.seriesID = seriesID;
            ViewBag.seasonID = seasonID;
            var model = new Episode() { SeasonId = seasonID };
            return View("Episode/CreateEpisode");
        }

        [HttpPost]
        [ActionName("CreateEpisode")]
        public IActionResult CreateEpisodePost(Episode model, int seriesID, int seasonID)
        {
            if (ModelState.IsValid)
            {
                this.EpisodeRepository.Add(model, autoSave: true);
                return RedirectToAction("Details/"+ seriesID);
            }
            else
            {
                this.FillDropDownValues(null);
                return View("Episode/CreateEpisode?seriesID="+ seriesID + "&seasonID=" + seasonID, model);
            }
        }

        public new void FillDropDownValues(List<Category> listCategories)
        {
            var selectItemsYear = new List<SelectListItem>();
            for (var i = DateTime.Now.Year; i >= 1900; i--)
            {
                var listItem = new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString(),
                    Selected = false
                };
                selectItemsYear.Add(listItem);
            }
            ViewBag.Years = selectItemsYear;

            var selectItems = new List<SelectListItem>();
            foreach (Category item in Enum.GetValues(typeof(Category)))
            {
                var listItem = new SelectListItem
                {
                    Text = item.ToString(),
                    Value = item.ToString(),
                    Selected = false
                };
                if (listCategories != null && listCategories.Contains(item))
                    listItem.Selected = true;
                selectItems.Add(listItem);
            }
            ViewBag.Categories = selectItems;
        }
    }
}