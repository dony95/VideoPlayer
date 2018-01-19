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
    public class FilmController : Controller
    {
        public readonly FilmRepository FilmRepository;

        public FilmController(FilmRepository repository)
        {
            FilmRepository = repository;
        }
        public IActionResult Index()
        {
            FillDropDownValues(null);
            return View(FilmRepository.GetList(null));
        }

        [HttpPost]
        public ActionResult IndexAjax(FilmFilterModel model)
        {
            return PartialView("_IndexTable", this.FilmRepository.GetList(model));
        }

        public ActionResult Create()
        {
            FillDropDownValues(null);
            return View();
        }


        [HttpPost]
        public ActionResult Create(Film model)
        {
            if (ModelState.IsValid)
            {
                this.FilmRepository.Add(model, autoSave: true);

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
            var model = FilmRepository.Find(id);
            FillDropDownValues(model.Categories);
            return View(model);
        }

        [HttpPost]
        [ActionName("Edit")]
        public async Task<ActionResult> EditPostAsync(int id)
        {
            var model = this.FilmRepository.Find(id);
            var didUpdateModelSucceed = await this.TryUpdateModelAsync(model);

            if (didUpdateModelSucceed && ModelState.IsValid)
            {
                this.FilmRepository.Update(model, autoSave: true);
                return RedirectToAction("Index");
            }

            this.FillDropDownValues(null);
            return View(model);
        }
        public ActionResult Details(int? id = null)
        {
            if (id == null)
                return View();
            var model = FilmRepository.Find(id.Value);
            return View(model);
        }

        [HttpGet]
        [Route("film/Download/{id}")]
        [ActionName("Download")]
        public ActionResult DownloadFilm(int? id = null)
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