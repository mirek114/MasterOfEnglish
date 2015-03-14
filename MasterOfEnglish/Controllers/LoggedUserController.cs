using MasterOfEnglish.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MasterOfEnglish.Controllers
{
    public class LoggedUserController : Controller
    {
        [HttpGet]
        public ActionResult EditPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditPassword(Models.EditPasswordViewModel editedPassword)
        {
            if (ModelState.IsValid)
            {
                if (editedPassword.NewPassword == editedPassword.ReapeatNewPassword)
                {
                    FormsAuthentication.SetAuthCookie(UserController.ActualyLoggedUser.Email, false);

                    using (var db = new MasterOfEnglishContext())
                    {
                        var user = db.Users.Find(UserController.ActualyLoggedUser.UserId);

                        var crypto = new SimpleCrypto.PBKDF2();

                        if (user.Password == crypto.Compute(editedPassword.ActualPassword, user.PasswordSalt))
                        {
                            user.Password = crypto.Compute(editedPassword.NewPassword);
                            user.PasswordSalt = crypto.Salt;
                            db.Entry(user).State = EntityState.Modified;
                            db.SaveChanges();

                        }
                        else
                        {
                            ModelState.AddModelError("", "Podano nieprawidłowe aktualne hasło!");
                            return View();
                        }
                    }

                    return RedirectToAction("ShowAllWords", "Word");
                }
                else
                {
                    ModelState.AddModelError("", "Podane hasła muszą być identyczne!");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult EditEmail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditEmail(Models.EditEmailViewModel editedEmailData)
        {
            if (ModelState.IsValid)
            {
                FormsAuthentication.SetAuthCookie(UserController.ActualyLoggedUser.Email, false);

                if (editedEmailData.NewEmail == editedEmailData.ConfirmNewEmail)
                {
                    using (var db = new MasterOfEnglishContext())
                    {
                        if (db.Users.FirstOrDefault(u => u.Email == editedEmailData.NewEmail) == null)
                        {
                            var user = db.Users.Find(UserController.ActualyLoggedUser.UserId);
                            user.Email = editedEmailData.NewEmail;
                            db.Entry(user).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            ModelState.AddModelError("", "Uzytkownik o podanym adresie email jest już zarejestrowany na stronie!");
                            return View(editedEmailData);
                        }

                    }

                    return RedirectToAction("ShowAllWords", "Word");
                }
                else
                {
                    ModelState.AddModelError("", "Podane adresy email muszą być identyczne!");
                }
            }
            return View(editedEmailData);

        }

        [HttpGet]
        public ActionResult RemoveUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RemoveUser(Models.RemoveUserViewModel enteredPassword)
        {
            if (ModelState.IsValid)
            {
                FormsAuthentication.SetAuthCookie(UserController.ActualyLoggedUser.Email, false);

                using (var db = new MasterOfEnglishContext())
                {
                    var user = db.Users.Find(UserController.ActualyLoggedUser.UserId);

                    var crypto = new SimpleCrypto.PBKDF2();

                    if (user.Password == crypto.Compute(enteredPassword.Password, user.PasswordSalt))
                    {

                        db.Users.Remove(user);
                        db.SaveChanges();

                    }
                    else
                    {
                        ModelState.AddModelError("", "Podano nieprawidłowe hasło!");
                        return View();
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Podaj hasło!");
            }

            return View();

        }
    }
}
