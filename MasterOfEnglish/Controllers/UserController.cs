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
    public class UserController : Controller
    {
        public static User ActualyLoggedUser;
        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(Models.User user)
        {
            if (ModelState.IsValid)
            {
                if (CheckLoginDetails(user.Email, user.Password))
                {
                    FormsAuthentication.SetAuthCookie(user.Email, false);
                    using (var db = new MasterOfEnglishContext())
                    {

                        ActualyLoggedUser = db.Users.FirstOrDefault(u => u.Email == user.Email);
                    }
                    return RedirectToAction("ShowAllWords", "Word");
                }
                else
                {
                    ModelState.AddModelError("", "Dane logowania są niepoprawne!");
                }
            }
            return View(user);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Register(Models.User user)
        {
            if (ModelState.IsValid)
            {
                if (user.ConfirmPassword == user.Password)
                {
                    FormsAuthentication.SetAuthCookie(user.Email, false);

                    using (var db = new MasterOfEnglishContext())
                    {
                        if (db.Users.FirstOrDefault(u => u.Email == user.Email) != null)
                        {
                            ModelState.AddModelError("", "Taki użytkownik już istnieje w systemie!");
                            return View(user);
                        }

                        var crypto = new SimpleCrypto.PBKDF2();
                        var encryptedPassword = crypto.Compute(user.ConfirmPassword);

                        var newUser = db.Users.Create();

                        newUser.Email = user.Email;
                        newUser.Password = encryptedPassword;
                        newUser.PasswordSalt = crypto.Salt;

                        db.Users.Add(newUser);

                        var newCategory = db.Categorys.Create();

                        newCategory.Name = "brak";
                        newCategory.UserId = user.UserId;

                        db.Categorys.Add(newCategory);
                        db.SaveChanges();
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Podane hasła muszą być identyczne!");
                }
            }
            return View(user);
        }

        private bool CheckLoginDetails(string email, string password)
        {
            bool areCorrect = false;
            var crypto = new SimpleCrypto.PBKDF2();

            using (var db = new MasterOfEnglishContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Email == email);

                if (user != null)
                {
                    if (user.Password == crypto.Compute(password, user.PasswordSalt))
                    {
                        areCorrect = true;
                    }
                }
            }
            return areCorrect;
        }
    }
}
