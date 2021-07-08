using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using RegisterLoginSystem.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

namespace RegisterLoginSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly Entities _entities;
        private readonly ISession _session;
        private readonly IFeatureManager _featureManager;

        public HomeController(Entities entities, IHttpContextAccessor httpContextAccessor, IFeatureManager featureManager)
        {
            _entities = entities;
            _session = httpContextAccessor.HttpContext.Session;
            _featureManager = featureManager;
        }

        // GET: Home
        public ActionResult Index()
        {
            if (_session.GetInt32("UserId") != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        //GET: Register
        [FeatureGate("EnableRegister")]
        public ActionResult Register()
        {
            _session.Clear();
            return View();
        }

        //POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User _user)
        {
            if (ModelState.IsValid)
            {
                var check = _entities.Users.FirstOrDefault(s => s.Email == _user.Email);
                if (check == null)
                {
                    _user.Password = GetMD5(_user.Password);
                    _entities.Users.Add(_user);
                    _entities.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return View();
                }
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if (ModelState.IsValid && !(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)))
            {
                var f_password = GetMD5(password);
                var data = _entities.Users.Where(s => s.Email.Equals(email) && s.Password.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    //add session
                    _session.Clear();
                    _session.SetString("FullName", data.FirstOrDefault().FirstName + " " + data.FirstOrDefault().LastName);
                    _session.SetString("Email", data.FirstOrDefault().Email);
                    _session.SetInt32("UserId", data.FirstOrDefault().UserId);
                    _session.SetString("ShowSettings", data.FirstOrDefault().ShowSettings ? "true" : "false");
                  
                    return RedirectToAction("Index");
                }
                else
                {
                    _session.SetString("LoginError", _entities.Users.Any(s => s.Email.Equals(email))
                                        ? "Login failed, the password you tasted is not correct, try again or reset." 
                                        : "Login failed, you are not registered, please register first.");
                    ViewBag.error = "Email already exists";
                    return RedirectToAction("Login");
                }
            }
            return View();
        }


        //Logout
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();//remove session
            return RedirectToAction("Login");
        }

        public ActionResult Settings()
        {
            
            return View();
        }

        //create a string MD5
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

    }
}
