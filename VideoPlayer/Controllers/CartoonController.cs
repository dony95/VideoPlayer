using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VideoPlayer.DAL.Repository;
using VideoPlayer.Model;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using VideoPlayer.Models;

namespace VideoPlayer.Controllers
{
    public class CartoonController : Controller
    {
        public readonly CartoonRepository CartoonRepository;

        public CartoonController(CartoonRepository repository)
        {
            CartoonRepository = repository;
        }
        
        public IActionResult Index()
        {
            FillDropDownValues(null);
            return View(CartoonRepository.GetList(null));
        }

        [HttpPost]
        public IActionResult IndexAjax(CartoonFilterModel model)
        {
            return PartialView("_IndexTable", this.CartoonRepository.GetList(model));
        }

        public IActionResult Create()
        {
            FillDropDownValues(null);
            return View();
        }

        [HttpPost]
        public IActionResult Create(Cartoon model)
        {
            if (ModelState.IsValid)
            {
                this.CartoonRepository.Add(model, autoSave: true);

                return RedirectToAction("Index");
            }
            else
            {
                this.FillDropDownValues(model.Categories);
                return View(model);
            }
        }

        public IActionResult Edit(int id)
        {
            var model = CartoonRepository.Find(id);
            FillDropDownValues(model.Categories);
            return View(model);
        }

        [HttpPost]
        [ActionName("Edit")]
        public async Task<IActionResult> EditPost(int id)
        {
            var model = this.CartoonRepository.Find(id);
            var didUpdateModelSucceed = await this.TryUpdateModelAsync(model);

            if (didUpdateModelSucceed && ModelState.IsValid)
            {
                this.CartoonRepository.Update(model, autoSave: true);
                return RedirectToAction("Index");
            }

            this.FillDropDownValues(null);
            return View(model);
        }
        public IActionResult Details(int? id = null)
        {
            if (id == null)
                return View();
            var model = CartoonRepository.Find(id.Value);
            return View(model);
        }

        [HttpGet]
        [Route("Download/{id}")]
        public IActionResult Download(int? id = null)
        {
            if (id == null)
                return View("Index", CartoonRepository.GetList(null));

            var video = CartoonRepository.Find(id.Value);
            var fileContents = System.IO.File.ReadAllText(@"Data/script.bat");

            if (video.SubtitleURL != null)
            {
                var subfileContents = System.IO.File.ReadAllText(@"Data/titlovi_skripta.bat");
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

            var selectItemsLanguage = new List<SelectListItem>();
            foreach (Language item in Enum.GetValues(typeof(Language)))
            {
                var listItem = new SelectListItem
                {
                    Text = item.ToString(),
                    Value = item.ToString(),
                    Selected = false
                };
                selectItemsLanguage.Add(listItem);
            }
            ViewBag.Languages = selectItemsLanguage;
        }
    }
}