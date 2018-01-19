using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VideoPlayer.DAL.Repository;
using VideoPlayer.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using VideoPlayer.Models;

namespace VideoPlayer.Controllers
{
    public class SeriesController : Controller
    {
        public SeriesRepository SeriesRepository;

        public SeriesController(SeriesRepository SeriesRepository)
        {
            this.SeriesRepository = SeriesRepository;
        }

        public IActionResult Index()
        {
            FillDropDownValues(null);
            return View(SeriesRepository.GetList(null));
        }

        [HttpPost]
        public ActionResult IndexAjax(FilmFilterModel model)
        {
            return PartialView("_IndexTable", this.SeriesRepository.GetList(model));
        }

        public ActionResult Create()
        {
            FillDropDownValues(null);
            return View();
        }

        [HttpPost]
        public ActionResult Create(Series model)
        {
            if (ModelState.IsValid)
            {
                this.SeriesRepository.Add(model, autoSave: true);

                return RedirectToAction("Index");
            }
            else
            {
                this.FillDropDownValues(model.Categories);
                return View(model);
            }
        }

        public ActionResult Edit(int id)
        {
            var model = SeriesRepository.Find(id);
            FillDropDownValues(model.Categories);
            return View(model);
        }

        [HttpPost]
        [ActionName("Edit")]
        public async Task<ActionResult> EditPostAsync(int id)
        {
            var model = this.SeriesRepository.Find(id);
            var didUpdateModelSucceed = await this.TryUpdateModelAsync(model);

            if (didUpdateModelSucceed && ModelState.IsValid)
            {
                this.SeriesRepository.Update(model, autoSave: true);
                return RedirectToAction("Index");
            }

            this.FillDropDownValues(null);
            return View(model);
        }
        public ActionResult Details(int? id = null)
        {
            if (id == null)
                return View();
            var model = SeriesRepository.Find(id.Value);
            return View(model);
        }

        /*[HttpGet]
        [Route("film/Download/{id}")]
        [ActionName("Download")]
        public ActionResult DownloadEpisode(int? id = null)
        {
            if (id == null)
                return View("Index", SeriesRepository.GetList(null));

            var video = SeriesRepository.Find(id.Value);
            var fileContents = System.IO.File.ReadAllText(Server.MapPath(@"~/App_Data/script.bat"));

            if (video.SubtitleURL != null)
            {
                var subfileContents = System.IO.File.ReadAllText(Server.MapPath(@"~/App_Data/titlovi_skripta.bat"));
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
        }*/
        public ActionResult CreateSeason(int seriesID)
        {
            FillDropDownValues(null);
            return View("Season/Create");
        }

        [HttpPost]
        public ActionResult CreateSeason(Season model, int seriesID)
        {
            if (ModelState.IsValid)
            {
                this.SeriesRepository.AddSeason(model, seriesID, autoSave: true);
                return RedirectToAction("Index");
            }
            else
            {
                this.FillDropDownValues(SeriesRepository.Find(seriesID).Categories);
                return View(model);
            }
        }

        public ActionResult EditSeason(int seasonid, int seriesid)
        {
            var series = SeriesRepository.Find(seriesid);
            FillDropDownValues(series.Categories);
            foreach (Season s in series.Seasons)
                if (s.SeasonNumber == seasonid)
                    return View(s);
            return View();
        }

        /*[HttpPost]
        [ActionName("Edit")]
        public ActionResult EditPostSeason(int seasonid, int seriesid)
        {
            var series = this.SeriesRepository.Find(seriesid);
            Season season = null;
            foreach (Season s in series.Seasons)
                if (s.SeasonNumber == seasonid)
                    season = s;
            var didUpdateModelSucceed = this.TryUpdateModel(season);

            if (didUpdateModelSucceed && ModelState.IsValid)
            {
                this.SeriesRepository.Update(model, autoSave: true);
                return RedirectToAction("Index");
            }

            this.FillDropDownValues(null);
            return View(model);
        }*/
        public void FillDropDownValues(List<Category> listCategories)
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