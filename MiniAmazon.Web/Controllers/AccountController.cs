using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using Facebook;
using MiniAmazon.Domain;
using MiniAmazon.Domain.Entities;
using MiniAmazon.Web.Infrastructure;
using MiniAmazon.Web.Models;
using System.Security.Cryptography;
using Models;

namespace MiniAmazon.Web.Controllers
{
    public class AccountController : BootstrapBaseController
    {
        private readonly IRepository _repository;
        private readonly IMappingEngine _mappingEngine;

        public AccountController(IRepository repository, IMappingEngine mappingEngine)
        {
            _repository = repository;
            _mappingEngine = mappingEngine;
        }

        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

        public ActionResult Facebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = "166603383506329",
                client_secret = "594078c31b94648e3e05e714fb578689",
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email" // Add other permissions as needed
            });

            return Redirect(loginUrl.AbsoluteUri);
        }

        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = "166603383506329",
                client_secret = "594078c31b94648e3e05e714fb578689",
                redirect_uri = RedirectUri.AbsoluteUri,
                //code = code
            });

            var accessToken = result.access_token;

            // Store the access token in the session
            Session["AccessToken"] = accessToken;

            // update the facebook client with the access token so 
            // we can make requests on behalf of the user
            fb.AccessToken = accessToken;

            // Get the user's information
            dynamic me = fb.Get("me?fields=first_name,last_name,age,genre,id,email,picture");
            string email = me.email;
            var account = _repository.First<Account>(u => u.Email == email);
            if (account != null)
            {
                if (account.Active)
                {
                    //inicio Session y guardo los datos del proveedor (facebook)
                    //Registrar Proveedor
                    var roles = new List<string>();
                    roles.Add(account.Role.Name);
                    FormsAuthentication.SetAuthCookie(account.Email, true);
                    SetAuthenticationCookie(account.Email, roles);
                }
                else
                {
                    Error("Your Account has been disabled, please contact with the web page administrator.");
                }
            }
            else
            {
                //Registrar usuario,proveedor, e iniciar sesion 
                //Registrar usuario con password automatica y activo
                //Enviar correo al usuario con su nueva password

                // Get the user's information
                string firstName = me.first_name;

                var user = new Account
                {
                    Name = firstName,
                    Email = email,
                    Password = "Prueba",
                    Active = true,
                    Followers = "",
                    Age = me.age,
                    Genre = me.genre,
                    Role = _repository.First<Role>(r => r.Name == "User")
                };
                _repository.Create(user);

                var roles = new List<string>();
                roles.Add(user.Role.Name);
                FormsAuthentication.SetAuthCookie(user.Email, true);
                SetAuthenticationCookie(user.Email, roles); 
                
                Success(" ¡Enhorabuena! Te has registrado correctamente en lexStore");
                Information("Se envio un link a tu correo para que puedas establecer una contraseña para tu cuenta.");
                
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("SignIn");
        }

        public ActionResult SignIn(string error = null)
        {
            if (!String.IsNullOrEmpty(error))
            {
                Error(error);
            }
            return View(new AccountSignInModel());
        }

        [HttpPost]
        public ActionResult SignIn(AccountSignInModel accountSignInModel)
        {
            var account =
                _repository.First<Account>(
                    x => x.Email == accountSignInModel.Email && x.Password == accountSignInModel.Password);

            if (account!=null)
            {
                var roles = new List<string>();
                roles.Add(account.Role.Name);
                FormsAuthentication.SetAuthCookie(accountSignInModel.Email, accountSignInModel.RememberMe);
                SetAuthenticationCookie(accountSignInModel.Email, roles);

                return RedirectToAction("Index");
            }
            Error("Wrong Email or Password. Please try again");
            //[Authorize]ViewBag.IncorrectPassword = 1;
            return View(accountSignInModel);
        }

        [Authorize]
        public ActionResult Index()
        {
            var model = _repository.First<Account>(x => x.Email == User.Identity.Name);
            
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            
            var cat = _repository.GetById<Account>(id);
            if(cat.Email.Equals(User.Identity.Name))
            {
                var model = Mapper.Map<Account, AccountInputModel>(cat);

                return View("Create", model);
            }
            Error("Error");
            return View("Index");
        }
        [HttpPost]
        public ActionResult Edit(AccountInputModel model, int id)
        {
            if (ModelState.IsValid)
            {
                var account = _repository.GetById<Account>(model.Id);
                var tempSale = account.Sales;
                account = _mappingEngine.Map<AccountInputModel, Account>(model);
                account.Sales = tempSale;
                _repository.Update(account);

                Success("The model was updated!");
                return RedirectToAction("index");
            }
            return View("Create", model);
        }

        public ActionResult ResetPassword()
        {
            return View("Create",new PasswordRecoveryInputModel());
        }

        [HttpPost]
        public ActionResult ResetPassword(PasswordRecoveryInputModel passwordRecoveryInputModel)
        {
            
            var acc = _repository.First<Account>(x => x.Email.Equals(passwordRecoveryInputModel.Email));
            if(acc!=null)
            {
                var passwordRecovery = new PasswordRecovery
                                           {
                                               Created = DateTime.Now,
                                               Used = false,
                                               Account = acc,
                                               Token = RandomGenerator.RandomString(10)
                                           };
                _repository.Create(passwordRecovery);
                new MailController().ResetPasswordEmail(passwordRecovery).Deliver();
                Success("A email has been sent to:" + passwordRecoveryInputModel.Email + " with instructions to reset the password");
            }
            else
            {
                Error("Email does not exist.");
                return View("Create",new PasswordRecoveryInputModel()); 
            }
            return RedirectToAction("Index", "Sale");
        }

        public ActionResult PasswordToken(string id)
        {
            var rep = _repository.First<PasswordRecovery>(x => x.Token.Equals(id) && x.Used == false);
            if(rep!=null)
            {
                
                return View("Create", new PasswordResetInputModel(rep.Account.Email));
            }
            Error("Wrong token or already used");
            return RedirectToAction("Index", "Sale");
        }

        [HttpPost]
        public ActionResult PasswordToken(PasswordResetInputModel model, String id)
        {
            var rep = _repository.First<PasswordRecovery>(x => x.Token.Equals(id) && x.Used == false);
            if (model.Password.Equals(model.PasswordTwo))
            {
                var account = _repository.First<Account>(x => x.Email.Equals(rep.Account.Email));
                account.Password = model.Password;
                _repository.Update(account);
                rep.Used = true;
                _repository.Update(rep);
            }
            else
            {
                Error("Passwords are not the same");
                return View("Create", new PasswordResetInputModel(model.emailObtainer()));
            }
            Information("Password succesfully changed!");
            return RedirectToAction("SignIn","Account");
        }


        public ActionResult ValidatedEmail(string email)
        {
            var acc = _repository.First<Account>(x => x.Email.Equals(email));
            if (acc == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EmailExist(string email)
        {
            var acc = _repository.First<Account>(x => x.Email.Equals(email));
            if (acc != null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

    }
}