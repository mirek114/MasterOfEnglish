using MasterOfEnglish.Context;
using MasterOfEnglish.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MasterOfEnglish.Controllers
{
    public enum LanguageOfWordsInTest
    {
        Polish,
        English,
        Mixed
    }

    public class LerningController : Controller
    {
        private static LanguageOfWordsInTest selectedLanguage;
        private static List<int> categoryId = new List<int>();
        public ActionResult LernAllWords(int? id, bool clearCategory = false, bool changeStatus = false)
        {
            List<Word> words = new List<Word>();
            using (var db = new MasterOfEnglishContext())
            {
                if (id != null && changeStatus)
                {
                    var word = db.Words.Find(id);
                    word.Status = Models.WordStatus.znane;
                    db.Entry(word).State = EntityState.Modified;
                    db.SaveChanges();
                }

                if (clearCategory)
                {
                    categoryId.Clear();
                }

                if (categoryId.Count() != 0)
                {
                    words = db.Words.Where(w => w.UserId == UserController.ActualyLoggedUser.UserId
                                && w.Status == Models.WordStatus.nieznane && categoryId.Contains(w.CategoryId)).ToList();
                }
                else
                {
                    words = db.Words.Where(w => w.UserId == UserController.ActualyLoggedUser.UserId
                        && w.Status == Models.WordStatus.nieznane).ToList();
                }

                if (words.Count() == 0)
                {
                    return View();
                }
                else
                {
                    Random rnd = new Random();
                    int rand = rnd.Next(words.Count());

                    return View(words[rand]);
                }
            }
        }

        [HttpGet]
        public ActionResult LernWordsFromCategory()
        {
            using (var db = new MasterOfEnglishContext())
            {
                SelectCategorysViewModel categorysToSelect = new SelectCategorysViewModel();
                categorysToSelect.categorys = db.Categorys.Where(c => c.UserId == UserController.ActualyLoggedUser.UserId).ToList();

                return View(categorysToSelect);
            }
        }

        [HttpPost]
        public ActionResult LernWordsFromCategory(Models.SelectCategorysViewModel selectedCategorysFromView)
        {
            if (ModelState.IsValid)
            {
                FormsAuthentication.SetAuthCookie(UserController.ActualyLoggedUser.Email, false);

                categoryId = selectedCategorysFromView.selectedCategorys;

                return RedirectToAction("LernAllWords", "Lerning");
            }
            else
            {
                ModelState.AddModelError("", "Podane dane są nieprawidłowe!");
            }
            return View();
        }

        [HttpGet]
        public ActionResult SelectTestParameters()
        {
            using (var db = new MasterOfEnglishContext())
            {
                SelectTestParametersViewModel selectTestParametersViewModel = new SelectTestParametersViewModel();
                selectTestParametersViewModel.categorys = db.Categorys.Where(c => c.UserId == UserController.ActualyLoggedUser.UserId).ToList();

                return View(selectTestParametersViewModel);
            }
        }

        [HttpPost]
        public ActionResult SelectTestParameters(Models.SelectTestParametersViewModel modelFromView, string wordsRange)
        {
            if (ModelState.IsValid)
            {
                FormsAuthentication.SetAuthCookie(UserController.ActualyLoggedUser.Email, false);

                if (modelFromView.selectedCategorys != null && wordsRange == "wordsFromCategorys")
                    categoryId = modelFromView.selectedCategorys;
                else
                    categoryId.Clear();

                selectedLanguage = modelFromView.languageOfWordsInTest;

                return RedirectToAction("Test", "Lerning");
            }
            else
            {
                ModelState.AddModelError("", "Podane dane są nieprawidłowe!");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Test(int? id, bool changeStatus = false)
        {
            List<Word> words = new List<Word>();

            TestViewModel testViewModel = new TestViewModel();

            using (var db = new MasterOfEnglishContext())
            {
                if (id != null && changeStatus)
                {
                    var word = db.Words.Find(id);
                    word.Status = Models.WordStatus.nauczone;
                    db.Entry(word).State = EntityState.Modified;
                    db.SaveChanges();
                }
                if (categoryId.Count() != 0)
                {
                    words = db.Words.Where(w => w.UserId == UserController.ActualyLoggedUser.UserId
                                && w.Status != Models.WordStatus.nauczone && categoryId.Contains(w.CategoryId)).ToList();
                }
                else
                {
                    words = db.Words.Where(w => w.UserId == UserController.ActualyLoggedUser.UserId
                        && w.Status != Models.WordStatus.nauczone).ToList();
                }

                if (words.Count() == 0)
                {
                    return View();
                }
                else
                {
                    Random rnd = new Random();
                    int rand = rnd.Next(words.Count());

                    testViewModel.word = words[rand];
                    testViewModel.languageOfWordsInTest = selectedLanguage;

                    return View(testViewModel);
                }
            }
        }

        [HttpPost]
        public ActionResult Test(Models.TestViewModel testViewModel)
        {
            if (ModelState.IsValid)
            {
                FormsAuthentication.SetAuthCookie(UserController.ActualyLoggedUser.Email, false);

                
                using (var db = new MasterOfEnglishContext())
                {
                    var wordFromDb = db.Words.Find(testViewModel.word.WordId);
                    var temp = new TestViewModel();
                    temp.languageOfWordsInTest = testViewModel.languageOfWordsInTest;
                    temp.word = wordFromDb;
                    TempData["temp"] = temp; 
                    if (wordFromDb.EnglishWord == testViewModel.word.EnglishWord
                        && wordFromDb.PolishWord == testViewModel.word.PolishWord)
                    {
                        return RedirectToAction("CorrectAnswer", "Lerning");
                    }
                    else
                    {
                        return RedirectToAction("IncorrectAnswer", "Lerning");
                    }
                }

            }
            else
            {
                ModelState.AddModelError("", "Podane dane są nieprawidłowe!");
            }
            return View();
        }

        public ActionResult CorrectAnswer()
        {
            return View((TestViewModel)TempData["temp"]);
        }

        public ActionResult IncorrectAnswer()
        {
            return View((TestViewModel)TempData["temp"]);
        }

    }
}
