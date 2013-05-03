using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Mvc;
using AutoMapper;
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

        public ActionResult SignIn()
        {
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
                
                return RedirectToAction("Index");
            }
            Error("Wrong <strong>Email</strong> or <strong>Password</strong>. Please try again");
            ViewBag.IncorrectPassword = 1;
            return View(accountSignInModel);
        }


        public ActionResult Index()
        {
            IEnumerable<Account> model = _repository.Query<Account>(x => x == x );
            return View(new GeneralModel(model, _repository));
        }

        public ActionResult Add()
        {
            IEnumerable<Account> jmz = _repository.Query<Account>(x => x == x);
            return View(jmz);
        }

        public ActionResult Create()
        {
            return View(new AccountInputModel());
        }

        [HttpPost]
        public ActionResult Create(AccountInputModel accountInputModel)
        {
            var account = Mapper.Map<AccountInputModel, Account>(accountInputModel);
            

            _repository.Create(account);
            
            return RedirectToAction("Index","Sale");
        }

        public ActionResult Delete(int id)
        {
            var model = _repository.First<Account>(x => x.Id == id);
            model.Active = false;
            _repository.Update(model);
            Information("The Account " + model.Name + " was disabled. Remember you cannot delete elements.");

            return RedirectToAction("index");
        }
        public ActionResult Edit(int id)
        {
            var cat = _repository.GetById<Account>(id);
            var model = Mapper.Map<Account, AccountInputModel>(cat);

            return View("Create", model);
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

        public ActionResult Details(int id)
        {
            var model = _repository.GetById<Account>(id);
            return View(model);
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
                AesCryptoServiceProvider myAes = new AesCryptoServiceProvider();
                Byte[] encodedBytes = EncryptStringToBytes_Aes(acc.Email, myAes.Key, myAes.IV);
                

                PasswordRecovery passwordRecovery = new PasswordRecovery();
                passwordRecovery.Created = DateTime.Now;
                passwordRecovery.HashKey = BitConverter.ToString(myAes.Key);
                passwordRecovery.Iv = BitConverter.ToString(myAes.IV);
                passwordRecovery.HashToken = BitConverter.ToString(encodedBytes);
                passwordRecovery.Used = false;
                _repository.Create(passwordRecovery);
                Success("A email has been sent to:" + passwordRecoveryInputModel.Email + " with instructions to reset the password");
            }
            else
            {
                Error("Email does not exist.");
                return View("Create",new PasswordRecoveryInputModel()); 
            }
            return RedirectToAction("Index", "Sale");
        }

        public ActionResult PasswordToken(string token = "CD-2E-C6-13-04-E0-CF-DE-51-C8-BB-D4-48-28-67-90")
        {
            PasswordRecovery rep = _repository.First<PasswordRecovery>(x => x.HashToken.Equals(token) && x.Used == false);
            if(rep!=null)
            {
                AesCryptoServiceProvider myAes = new AesCryptoServiceProvider();
                //Byte[] encodedBytes = ASCIIEncoding.Default.GetBytes(token);
                //Byte[] hashKey = ASCIIEncoding.Default.GetBytes(rep.HashKey);
                //Byte[] Iv = ASCIIEncoding.Default.GetBytes(rep.Iv);
                //string decoded = DecryptStringFromBytes_Aes(encodedBytes, hashKey, Iv);
                return View("Create", new PasswordResetInputModel("gadi@me.com"));
            }
            Error("Wrong token or already used");
            return RedirectToAction("Index", "Sale");
        }

        [HttpPost]
        public ActionResult PasswordToken(PasswordResetInputModel model, String token)
        {
            if (model.Password.Equals(model.PasswordTwo))
            {
                var account = _repository.First<Account>(x => x.Email.Equals(model.emailObtainer()));
                account.Password = model.Password;
                _repository.Update(account);
            }
            else
            {
                Error("Passwords are not the same");
                return View("Create", model: new PasswordResetInputModel(model.emailObtainer()));
            }
            Error("Wrong token or already used");
            return RedirectToAction("Index", "Sale");
        }

        

        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
            byte[] encrypted;
            // Create an AesCryptoServiceProvider object 
            // with the specified key and IV. 
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption. 
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream. 
            return encrypted;

        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext = null;

            // Create an AesCryptoServiceProvider object 
            // with the specified key and IV. 
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption. 
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream 
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
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