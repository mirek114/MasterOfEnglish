using MasterOfEnglish.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MasterOfEnglish.Controllers
{
    public class CategoryController : Controller
    {
        [HttpGet]
        public ActionResult ShowAllCategorys(string searchCategory)
        {
            using(var db = new MasterOfEnglishContext())
            {
                var categorys = db.Categorys.Where(c=>c.UserId == UserController.ActualyLoggedUser.UserId && c.Name !="brak" 
                    &&(c.Name == searchCategory || searchCategory ==null)).ToList();
                return View(categorys);
            }
        }


        [HttpGet]
        public ActionResult CreateCategory()
        {
            return View();
        }


        [HttpPost]
        public ActionResult CreateCategory(Models.Category category)
        {
            if (ModelState.IsValid)
            {
                FormsAuthentication.SetAuthCookie(UserController.ActualyLoggedUser.Email, false);
                using (var db = new MasterOfEnglishContext())
                {
                    var newCategory = db.Categorys.Create();
                    newCategory.Name = category.Name;
                    newCategory.UserId = UserController.ActualyLoggedUser.UserId;

                    db.Categorys.Add(newCategory);
                    db.SaveChanges();

                }
                return RedirectToAction("CreateCategory", "Category");
            }
            else
            {
                ModelState.AddModelError("", "Podane dane są nieprawidłowe!");
            }


            return View(category);

        }

        [HttpGet]
        public ActionResult ShowAllWordsFromCategory(int id)
        {
            using (var db = new MasterOfEnglishContext())
            {
                var words = db.Words.Where(w => w.UserId == UserController.ActualyLoggedUser.UserId && w.CategoryId == id).ToList();
                return View(words);
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {

            using (var db = new MasterOfEnglishContext())
            {
                return View(db.Categorys.Find(id));
            }
        }


        [HttpPost]
        public ActionResult Edit(Models.Category category)
        {

            if (ModelState.IsValid)
            {
                FormsAuthentication.SetAuthCookie(UserController.ActualyLoggedUser.Email, false);
                using (var db = new MasterOfEnglishContext())
                {
                    db.Entry(category).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ShowAllCategorys", "Category");
                }
            }
            else
            {
                ModelState.AddModelError("", "Podane dane są nieprawidłowe!");
            }
            return View(category);

        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            using (var db = new MasterOfEnglishContext())
            {
                var category = db.Categorys.Find(id);
                if (category == null)
                    HttpNotFound();
                return View(category);
            }

        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var db = new MasterOfEnglishContext())
                    {
                        var categoryToDetele = db.Categorys.Find(id);
                        var wordsFromCategory = db.Words.Where(c => c.CategoryId == id);

                        var noneCategoryId = db.Categorys.FirstOrDefault(c=>c.Name=="brak").CategoryId;

                        foreach (var kvp in wordsFromCategory)
                        {
                            kvp.CategoryId = noneCategoryId;
                            db.Entry(kvp).State = EntityState.Modified;
                        }

                        db.Categorys.Remove(categoryToDetele);
                        db.SaveChanges();
                        return RedirectToAction("ShowAllCategorys", "Category");
                    }
                }
                return View();
            }
            catch
            {
                return View();
            }

        }
    }
}
