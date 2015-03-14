using MasterOfEnglish.Context;
using MasterOfEnglish.Models;
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
    public class WordController : Controller
    {

        public ActionResult ShowAllWords(string searchTerm)
        {
            using (var db = new MasterOfEnglishContext())
            {
                var serchedWordsAndCategoryList = new WordsAndCategorysViewModel();

                serchedWordsAndCategoryList.words = db.Words.Where(w => w.UserId == UserController.ActualyLoggedUser.UserId
                    && (w.PolishWord == searchTerm || w.EnglishWord == searchTerm || w.Status.ToString() == searchTerm || searchTerm == null)).ToList();

                serchedWordsAndCategoryList.categorys = db.Categorys.Where(c => c.UserId == UserController.ActualyLoggedUser.UserId).ToList();
                return View(serchedWordsAndCategoryList);
            }

        }


        public ActionResult SetAllWordsAsUnkown()
        {
            using (var db = new MasterOfEnglishContext())
            {
                var words = db.Words.Where(w => w.UserId == UserController.ActualyLoggedUser.UserId && w.Status != WordStatus.nieznane).ToList();
                words.ForEach(w => w.Status = WordStatus.nieznane);
                db.SaveChanges();
                return RedirectToAction("ShowAllWords", "Word");
            }
        }

        [HttpGet]
        public ActionResult SetWordsFromCategorysAsUnkown()
        {
            using (var db = new MasterOfEnglishContext())
            {
                SelectCategorysViewModel categorysToSelect = new SelectCategorysViewModel();
                categorysToSelect.categorys = db.Categorys.Where(c => c.UserId == UserController.ActualyLoggedUser.UserId).ToList();

                return View(categorysToSelect);
            }
        }

        [HttpPost]
        public ActionResult SetWordsFromCategorysAsUnkown(Models.SelectCategorysViewModel selectCategorys)
        {
            using (var db = new MasterOfEnglishContext())
            {
                var words = db.Words.Where(w => w.UserId == UserController.ActualyLoggedUser.UserId 
                    && selectCategorys.selectedCategorys.Contains(w.CategoryId) 
                    && w.Status !=WordStatus.nieznane).ToList();

                words.ForEach(w => w.Status = WordStatus.nieznane);
                db.SaveChanges();
                return RedirectToAction("ShowAllWords", "Word");
            }
        }

        [HttpGet]
        public ActionResult CreateWord()
        {
            using (var db = new MasterOfEnglishContext())
            {
                var wordAndCategorys = new CategorysAndWordViewModel();
                wordAndCategorys.Categorys = db.Categorys.Where(c => c.UserId == UserController.ActualyLoggedUser.UserId).ToList();

                return View(wordAndCategorys);
            }
        }


        [HttpPost]
        public ActionResult CreateWord(Models.Word word, string ResultProvince)
        {
            if (ModelState.IsValid)
            {
                FormsAuthentication.SetAuthCookie(UserController.ActualyLoggedUser.Email, false);
                using (var db = new MasterOfEnglishContext())
                {
                    var newWord = db.Words.Create();
                    newWord.PolishWord = word.PolishWord;
                    newWord.EnglishWord = word.EnglishWord;
                    newWord.Status = WordStatus.nieznane;
                    newWord.CategoryId = word.CategoryId;
                    newWord.UserId = UserController.ActualyLoggedUser.UserId;

                    db.Words.Add(newWord);
                    db.SaveChanges();

                }
                return RedirectToAction("CreateWord", "Word");
            }
            else
            {
                ModelState.AddModelError("", "Podane dane są nieprawidłowe!");
            }
            return View(word);
        }



        [HttpGet]
        public ActionResult Edit(int id)
        {

            using (var db = new MasterOfEnglishContext())
            {
                var wordAndCategorys = new CategorysAndWordViewModel();
                wordAndCategorys.Categorys = db.Categorys.Where(c => c.UserId == UserController.ActualyLoggedUser.UserId).ToList();
                wordAndCategorys.Word = db.Words.Find(id);
                return View(wordAndCategorys);
            }
        }


        [HttpPost]
        public ActionResult Edit(Models.Word word)
        {

            if (ModelState.IsValid)
            {
                FormsAuthentication.SetAuthCookie(UserController.ActualyLoggedUser.Email, false);
                using (var db = new MasterOfEnglishContext())
                {
                    db.Entry(word).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ShowAllWords", "Word");
                }
            }
            else
            {
                ModelState.AddModelError("", "Podane dane są nieprawidłowe!");
            }
            return View(word);

        }


        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); 
            using (var db = new MasterOfEnglishContext())
            {
                var word = db.Words.Find(id);
                if (word == null)
                    HttpNotFound();
                return View(word);
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
                        var wordToDetele = db.Words.Find(id);
                        db.Words.Remove(wordToDetele);
                        db.SaveChanges();
                        return RedirectToAction("ShowAllWords", "Word");
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
