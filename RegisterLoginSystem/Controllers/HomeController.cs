using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using RegisterLoginSystem.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;
using RegisterLoginSystem.Dal;
using RegisterLoginSystem.Repository;
using RegisterLoginSystem.Service;

namespace RegisterLoginSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly ISession _session;
        private readonly IFeatureManager _featureManager;

        public HomeController(IHttpContextAccessor httpContextAccessor, IFeatureManager featureManager, IUserService userService)
        {
            _userService = userService;
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
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                var check = _userService.GetUserByEmail(user.Email);
                if (check == null)
                {
                    user.Password = GetMD5(user.Password);
                    _userService.AddUser(user);
           
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
                var data = _userService.GetUsersByEmailAndPassword(email, f_password);
                if (data.Count() > 0)
                {
                    var user = data.FirstOrDefault();
                    //add session
                    _session.Clear();
                    _session.SetString("FullName", user.FirstName + " " + user.LastName);
                    _session.SetString("Email", user.Email);
                    _session.SetInt32("UserId", user.UserId);
                    _session.SetString("ShowSettings", user.ShowSettings ? "true" : "false");
                  
                    return RedirectToAction("Index", user);
                }
                else
                {
                    _session.SetString("LoginError", _userService.GetAllUsers().Any(s => s.Email.Equals(email))
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

        [HttpGet]
        public ActionResult Edit()
        {
            var user = _userService.GetAllUsers().FirstOrDefault(u => u.UserId == _session.GetInt32("UserId"));
            user.Password = string.Empty;
            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (!string.IsNullOrEmpty(user.Password) && !string.IsNullOrEmpty(user.ConfirmPassword))
            {
                user.Password = GetMD5(user.Password);
                _userService.UpdateUser(user);               
                return RedirectToAction("Index");
            }
            return View(user);
        }

    }
}
