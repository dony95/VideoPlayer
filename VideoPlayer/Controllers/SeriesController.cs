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

namespace VideoPlayer.Controllers
{
    public class SeriesController : BaseController<Series>
    {
        public SeriesRepository SeriesRepository;
        public SeriesController(SeriesRepository SeriesRepository) : base(SeriesRepository)
        {
            this.SeriesRepository = SeriesRepository;
        }

        //[HttpGet]
        //[Route("film/Download/{id}")]
        //[ActionName("Download")]
        //public ActionResult DownloadEpisode(int? id = null)
        //{
        //    if (id == null)
        //        return View("Index", SeriesRepository.GetList(null));

        //    Episode video = SeriesRepository.FindEpisode(id.Value);
        //    var fileContents = System.IO.File.ReadAllText(@"~/App_Data/script.bat");

        //    if (video.SubtitleURL != null)
        //    {
        //        var subfileContents = System.IO.File.ReadAllText(@"~/App_Data/titlovi_skripta.bat");
        //        subfileContents = subfileContents.Replace("#_URL", video.SubtitleURL.Replace("%", "%%"));
        //        subfileContents = subfileContents.Replace("#_FILENAME", video.Name + ".srt");
        //        fileContents = fileContents.Replace("#_TITLOVI", subfileContents);
        //    }
        //    else
        //        fileContents = fileContents.Replace("#_TITLOVI", "");

        //    fileContents = fileContents.Replace("#_LINK", video.VideoURL.Replace("%", "%%"));
        //    if (video.SubtitleURL != null) fileContents = fileContents.Replace("#_SUB", "-- sub-file=\"c:\\Documents and settings\\%username%\\Documents\\titlovi\\"
        //        + video.Name + ".srt\" --sout-transcode-senc=\"Eastern European(Windows-1250)\"");
        //    else
        //        fileContents = fileContents.Replace("#_SUB", "");

        //    return File(Encoding.ASCII.GetBytes(fileContents.Replace("192.168.1.8", "donyslav.ddns.net")), "text/plain", video.Name + ".bat");
        //}
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