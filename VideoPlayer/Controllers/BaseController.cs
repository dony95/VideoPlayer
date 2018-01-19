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
    public class BaseController<TEntity> : Controller where TEntity : class
    {
        IRepositoryBase<TEntity> Repository;

        public BaseController(IRepositoryBase<TEntity> repository)
        {
            Repository = repository;
        }
        public IActionResult Index()
        {
            FillDropDownValues(null);
            return View(Repository.GetList(null));
        }

        [HttpPost]
        public IActionResult IndexAjax(FilmFilterModel model)
        {
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
            if (ModelState.IsValid)
            {
                this.Repository.Add(model, autoSave: true);

                return RedirectToAction("Index");
            }
            else
            {
                this.FillDropDownValues(null);
                return View(model);
            }
        }

        public IActionResult Edit(int id)
        {
            var model = Repository.Find(id);
            FillDropDownValues(null);
            return View(model);
        }

        [HttpPost]
        [ActionName("Edit")]
        public async Task<IActionResult> EditPost(int id)
        {
            var model = this.Repository.Find(id);
            var didUpdateModelSucceed = await this.TryUpdateModelAsync(model);

            if (didUpdateModelSucceed && ModelState.IsValid)
            {
                this.Repository.Update(model, autoSave: true);
                return RedirectToAction("Index");
            }

            this.FillDropDownValues(null);
            return View(model);
        }
        public IActionResult Details(int? id = null)
        {
            if (id == null)
                return View();
            var model = Repository.Find(id.Value);
            return View(model);
        }

        [HttpGet]
        [Route("Download/{id}")]
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