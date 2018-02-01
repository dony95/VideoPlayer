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
    public class BaseController<TEntity> : Controller where TEntity : class
    {
        IRepositoryBase<TEntity> Repository;
        private readonly ILogger _logger;

        public BaseController(IRepositoryBase<TEntity> repository, ILogger<BaseController<TEntity>> logger)
        {
            Repository = repository;
            _logger = logger;
        }
        public IActionResult Index()
        {
            FillDropDownValues(null);
            _logger.LogInformation("INdex", null);
            return View(Repository.GetList(null));
        }

        [HttpPost]
        public IActionResult IndexAjax(FilmFilterModel model)
        {
            _logger.LogInformation("IndexAjaxsearch : ", model.Name, ", ", model.Category, ", ", model.Language, ", ", model.Year);
            return PartialView("_IndexTable", this.Repository.GetList(model));
        }

        public IActionResult Create()
        {
            FillDropDownValues(null);
            return View();
        }

        [HttpPost]
        public IActionResult Create(TEntity model)
        {
            Video video = model as Video;
            if (ModelState.IsValid)
            {
                this.Repository.Add(model, autoSave: true);
                _logger.LogInformation("Created : ", video.Name, ", ", video.ImdbURL);
                return RedirectToAction("Index");
            }
            else
            {
                _logger.LogError("Failed to Create", video.Name, ", ", video.ImdbURL);
                this.FillDropDownValues(null);
                return View(model);
            }
        }

        [Route("Edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var model = Repository.Find(id);
            FillDropDownValues(null);
            return View(model);
        }

        [HttpPost]
        [ActionName("Edit")]
        [Route("Edit/{id:int}")]
        public async Task<IActionResult> EditPost(int id)
        {
            var model = this.Repository.Find(id);
            var didUpdateModelSucceed = await this.TryUpdateModelAsync(model);

            if (didUpdateModelSucceed && ModelState.IsValid)
            {
                this.Repository.Update(model, autoSave: true);
                _logger.LogInformation("Edited : ", (model as Video).ID, ", ", (model as Video).Name);
                return RedirectToAction("Index");
            }

            this.FillDropDownValues(null);
            _logger.LogError("Failed to Edit", (model as Video).ID, ", ", (model as Video).Name);
            return View(model);
        }
        public IActionResult Details(int? id = null)
        {
            if (id == null)
            {
                _logger.LogError("Details, id == null", null);
                return View();
            }
                
            var model = Repository.Find(id.Value);
            _logger.LogInformation("Details : ", (model as Video).ID, ", ", (model as Video).Name);
            return View(model);
        }

        [HttpGet]
        [Route("Download/{id:int}")]
        [ActionName("Download")]
        public virtual IActionResult Download(int? id = null)
        {
            return null;
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