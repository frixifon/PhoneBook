using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MvcTest.Models;
using MvcTest.Util;
using static MvcTest.Models.Models;

namespace MvcTest.Controllers
{
    public class HomeController : Controller
    {
        BookContext db = new BookContext();
        UserContext udb = new UserContext();
        PhoneContext pdb = new PhoneContext();
        static string camefrom;
        
        public ActionResult Index()
        {
            
            //// получаем из бд все объекты Book
            //IEnumerable<Book> books = db.Books;
            //// передаем все объекты в динамическое свойство Books в ViewBag
            //ViewBag.Books = books;
            //ViewData["Head"] = "Мой Магазин";
            //int hour = DateTime.Now.Hour;
            //ViewBag.Greeting = hour < 12 ? "Доброе утро!" : "Добрый вечер!";
            //IEnumerable<Phone> phones = pdb.Phones;
            //ViewBag.Phones = phones;
            return View();

        }
        [Authorize(Roles = "admin, user, guest")]
        public ActionResult Phone(string searchString)
        {
            IEnumerable<Phone> phones = pdb.Phones;
            ViewBag.Phones = phones;
            
            return View();
        }
        [Authorize(Roles = "admin, user")]
        [HttpGet]
        public ActionResult Group()
        {
            IEnumerable<Group> groups = pdb.Groups;
            ViewBag.Groups = groups;
            IEnumerable<Phone> phones = pdb.Phones;
            ViewBag.Phones = phones;
            ViewBag.CUser = udb.Users.Where(x => x.Email == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id;
            var CUser = udb.Users.Where(x => x.Email == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id;
            var GroupData = this.pdb.Groups.Where(g => g.UId == CUser);
            var PhoneData = pdb.Phones.Where(p => p.UId == CUser);
            ViewBag.Groupdata = GroupData;
            ViewBag.Phonedata = PhoneData;
            return View();
            
        }

        [Authorize(Roles = "admin, user")]
        [HttpPost]
        public ActionResult Group(FormCollection form)
        {
            IEnumerable<Group> groups = pdb.Groups;
            ViewBag.Groups = groups;
            IEnumerable<Phone> phones = pdb.Phones;
            ViewBag.Phones = phones;
            ViewBag.CUser = udb.Users.Where(x => x.Email == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id;
            var CUser = udb.Users.Where(x => x.Email == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id;
            var GroupData = this.pdb.Groups.Where(g => g.UId == CUser);
            ViewBag.Groupdata = GroupData;
            string searchName = Convert.ToString(form["searchString"]);
            var PhoneData = pdb.Phones.Where(p =>p.UId == CUser && p.Name.Contains(searchName));
            if(searchName == "" || searchName == null)
            {
                ViewBag.Test = "Не работает";
            }
            else
            {
                ViewBag.Test = "Заработало!";
                ViewBag.test = searchName;
            }
            return View();

        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult AddGroup(Group objgroup)
        {
            if(ModelState.IsValid)
            {
                this.pdb.Groups.Add(objgroup);
                this.pdb.SaveChanges();
                if(ViewBag.Id > 0)
                {
                    ViewBag.Success = "Группа успешно добавлена";

                }
                ModelState.Clear();
            }
            return View();
        }
        [Authorize(Roles = "admin, user")]
        [HttpGet]
        public ActionResult AddGroup()
        {
            IEnumerable<Group> group = pdb.Groups;
            ViewBag.Groups = group;
            return View();
        }

        [Authorize(Roles = "admin, user")]
        public ActionResult AddPhone(Phone objphone)
        {

            if (ModelState.IsValid)
            {
                this.pdb.Phones.Add(objphone);
                this.pdb.SaveChanges();
                if (ViewBag.Id > 0)
                {
                    ViewBag.Success = "Добавлено";

                }
                ModelState.Clear();
            }
            return View();
        }
        /*public string Index()
        {
            string result = "Вы не авторизованы";
            if (User.Identity.IsAuthenticated)
            {
                result = "Ваш логин: " + User.Identity.Name;
            }
            return result;
        }*/

        // GET: Home 
        [Authorize(Roles = "admin")]
        public ActionResult UploadFiles()
        {
            return View();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult UploadFiles(HttpPostedFileBase[] files)
        {
            //Ensure model state is valid  
            if (ModelState.IsValid)
            {   //iterating through multiple file collection   
                foreach (HttpPostedFileBase file in files)
                {
                    //Checking file is available to save.  
                    if (file != null)
                    {
                        var InputFileName = Path.GetFileName(file.FileName);
                        if (InputFileName.Contains(".jpg") || InputFileName.Contains(".png") || InputFileName.Contains(".gif"))
                        {
                            var ServerSavePath2 = Path.Combine(Server.MapPath("/Images/") + InputFileName);
                            file.SaveAs(ServerSavePath2);
                            //assigning file uploaded status to ViewBag for showing message to user.  
                            ViewBag.UploadStatus = files.Count().ToString() + " files uploaded successfully.";
                            return View();
                        }
                        var ServerSavePath = Path.Combine(Server.MapPath("/Files/") + InputFileName);
                        //Save file to server folder  
                        file.SaveAs(ServerSavePath);
                        //assigning file uploaded status to ViewBag for showing message to user.  
                        ViewBag.UploadStatus = files.Count().ToString() + " files uploaded successfully.";
                    }

                }
            }
            return View();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult AddBook(Book objbook)
        {

            if (ModelState.IsValid)
            {
                this.db.Books.Add(objbook);
                this.db.SaveChanges();
                if (ViewBag.Id > 0)
                {
                    ViewBag.Success = "Добавлено";

                }
                ModelState.Clear();
            }
            return View();
        }
        [Authorize(Roles="admin")]
        [HttpGet]
        public ActionResult AddBook()
        {
            IEnumerable<Book> books = db.Books;
            ViewBag.Books = books;
            return View();
        }

        [HttpGet]
        [Authorize(Roles="admin")]
        public ActionResult Admin()
        {
            ViewBag.Books = this.db.Books.ToList();
            ViewBag.Phones = this.pdb.Phones.ToList();
            ViewBag.Users = this.udb.Users.ToList();
            ViewBag.Roles = this.udb.Roles.ToList();
            ViewBag.Groups = this.pdb.Groups.ToList();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Admin(FormCollection formCollection)
        {
            try { string[] ids = formCollection["PhoneID"].Split(new char[] { ',' }); foreach (string phoneid in ids)
                {
                    var phone = this.pdb.Phones.Find(int.Parse(phoneid));
                    this.pdb.Phones.Remove(phone);
                    this.pdb.SaveChanges();
                }
            } catch(Exception) { }
            try { string[] ids1 = formCollection["UserID"].Split(new char[] { ',' }); foreach (string userid in ids1)
                {
                    var user = this.udb.Users.Find(int.Parse(userid));
                    this.udb.Users.Remove(user);
                    this.udb.SaveChanges();
                }
            } catch (Exception) { }
            try { string [] ids2 = formCollection["RoleID"].Split(new char[] { ',' }); foreach (string roleid in ids2)
                {
                    var role = this.udb.Roles.Find(int.Parse(roleid));
                    this.udb.Roles.Remove(role);
                    this.udb.SaveChanges();
                }
            } catch (Exception) { }
            try
            {
                string[] ids = formCollection["GroupID"].Split(new char[] { ',' }); foreach (string groupid in ids)
                {
                    var group = this.pdb.Groups.Find(int.Parse(groupid));
                    this.pdb.Groups.Remove(group);
                    this.pdb.SaveChanges();
                }
            }
            catch (Exception) { }
            return RedirectToAction("Admin");
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult GetHtml()
        {
            return new HtmlResult("<h2>Привет мир!</h2>");
        }
        public ActionResult GetVoid(int id)
        {
            if (id > 3)
            {
                return RedirectToAction("Contact");
            }
            return View("About");
        }
        public FileResult GetFile(int id)
        {
            ViewBag.BookId = id;

            // Путь к файлу
            string file_path = Server.MapPath("~/Files/book"+id+".pdf");
            // Тип файла - content-type
            string file_type = "application/pdf";
            // Имя файла - необязательно
            string file_name = "book"+id+".pdf";
            return File(file_path, file_type, file_name);
        }
        public FileResult GetBytes()
        {
            string path = Server.MapPath("~/Files/PDFIcon.pdf");
            byte[] mas = System.IO.File.ReadAllBytes(path);
            string file_type = "application/pdf";
            string file_name = "PDFIcon.pdf";
            return File(mas, file_type, file_name);
        }

        public FileResult GetStream()
        {
            string path = Server.MapPath("~/Files/PDFIcon.pdf");
            // Объект Stream
            FileStream fs = new FileStream(path, FileMode.Open);
            string file_type = "application/pdf";
            string file_name = "PDFIcon.pdf";
            return File(fs, file_type, file_name);
        }

        public ActionResult GetImage(int id, string name)
        {
            ViewBag.BookId = id;
            ViewBag.Name = name;
            string stringid = Convert.ToString(id);
            string path = "../../Images/book"+stringid+".jpg";
            return new ImageResult(path, name);
        }

        public ActionResult Contact()
        {
            return View();
        }



        [Authorize(Roles = "admin, user")]
        public ActionResult EditPhone(int id)
        {
            var Phonedata = pdb.Phones.Where(b => b.Id == id).FirstOrDefault();
            if (Phonedata != null)
            {
                TempData["StudentID"] = id;
                TempData.Keep();
                return View(Phonedata);
            }
            return View();
        }
        [Authorize(Roles = "admin, user")]
        [HttpPost]
        public ActionResult EditPhone(Phone objbook)
        {
            int StudentId = (int)TempData["StudentId"];
            var StudentData = this.pdb.Phones.Where(b => b.Id == StudentId).FirstOrDefault();
            if (StudentData != null)
            {
                StudentData.Name = objbook.Name;
                StudentData.Surname = objbook.Surname;
                StudentData.Number = objbook.Number;
                StudentData.Email = objbook.Email;
                StudentData.GId = objbook.GId;
                StudentData.UId = objbook.UId;
                pdb.Entry(StudentData).State = EntityState.Modified;
                this.pdb.SaveChanges();
            }
            return RedirectToAction("Phone");
        }

        [Authorize(Roles = "admin, user")]
        public ActionResult EditGroup(int id)
        {
            var Groupdata = pdb.Groups.Where(g => g.GId == id).FirstOrDefault();
            if (Groupdata != null)
            {
                TempData["StudentID"] = id;
                TempData.Keep();
                return View(Groupdata);
            }
            return View();
        }
        [Authorize(Roles = "admin, user")]
        [HttpPost]
        public ActionResult EditGroup(Group objbook)
        {
            int StudentId = (int)TempData["StudentId"];
            var StudentData = this.pdb.Groups.Where(g => g.GId == StudentId).FirstOrDefault();
            if (StudentData != null)
            {
                StudentData.Name = objbook.Name;
                pdb.Entry(StudentData).State = EntityState.Modified;
                this.pdb.SaveChanges();
            }
            return RedirectToAction("Phone");
        }


        [Authorize(Roles = "admin")]
        public ActionResult EditBook(int id)
        {
            var Bookdata = db.Books.Where(b => b.Id == id).FirstOrDefault();
            if (Bookdata != null)
            {
                TempData["StudentID"] = id;
                TempData.Keep();
                return View(Bookdata);
            }
            return View();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult EditBook(Book objbook)
        {

            int StudentId = (int)TempData["StudentId"];
            var StudentData = this.db.Books.Where(b => b.Id == StudentId).FirstOrDefault();
            if (StudentData != null)
            {
                StudentData.Name = objbook.Name;
                StudentData.Author = objbook.Author;
                StudentData.Price = objbook.Price;
                StudentData.Image = objbook.Image;
                StudentData.File = objbook.File;
                db.Entry(StudentData).State = EntityState.Modified;
                this.db.SaveChanges();
            }
            return RedirectToAction("Admin");
        }



        [Authorize(Roles = "admin")]
        public ActionResult EditUser(int id)
        {
            var Userdata = udb.Users.Where(u => u.Id == id).FirstOrDefault();
            if (Userdata != null)
            {
                TempData["StudentID"] = id;
                TempData.Keep();
                return View(Userdata);
            }
            return View();
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult EditUser(User objbook)
        {

            int StudentId = (int)TempData["StudentId"];
            var StudentData = this.udb.Users.Where(u => u.Id == StudentId).FirstOrDefault();
            if (StudentData != null)
            {
                StudentData.Email = objbook.Email;
                StudentData.Password = objbook.Password;
                StudentData.Age = objbook.Age;
                StudentData.RoleId = objbook.RoleId;
                udb.Entry(StudentData).State = EntityState.Modified;
                this.udb.SaveChanges();
            }
            return RedirectToAction("Admin");
        }

        [Authorize(Roles = "admin")]
        public ActionResult EditRole(int id)
        {
            var Userdata = udb.Roles.Where(r => r.Id == id).FirstOrDefault();
            if (Userdata != null)
            {
                TempData["StudentID"] = id;
                TempData.Keep();
                return View(Userdata);
            }
            return View();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult EditRole(Role objbook)
        {

            int StudentId = (int)TempData["StudentId"];
            var StudentData = this.udb.Roles.Where(r => r.Id == StudentId).FirstOrDefault();
            if (StudentData != null)
            {
                StudentData.Name = objbook.Name;
                udb.Entry(StudentData).State = EntityState.Modified;
                this.udb.SaveChanges();
            }
            return RedirectToAction("Admin");
        }

        [Authorize]
        [HttpGet]
        public ActionResult Buy(int id)
        {
            ViewBag.User = User.Identity.Name;
            ViewBag.BookId = id;
            return View();
            
        }
        
        [Authorize]
        [HttpPost]
        public ActionResult Buy(Purchase purchase)
        {
            
            // добавляем информацию о покупке в базу данных
            
            // сохраняем в бд все изменения
            if (ModelState.IsValid)
            {
                purchase.Date = DateTime.Now;
                db.Purchases.Add(purchase);
                db.SaveChanges();
                return RedirectToAction("Thanks", purchase);
            }
            else
            {
                ViewBag.BookId = purchase.BookId;
                return View();
            }
        }

        [Authorize]
        public ActionResult Thanks(Purchase purchase)
        {
            
            foreach (Book b in db.Books)
                if (b.Id == purchase.BookId) ViewBag.BookName = b.Name;
            foreach (Book b in db.Books)
                if (b.Id == purchase.BookId) ViewBag.File = b.File;
            ViewBag.Klient = purchase.Klient;
            ViewBag.Email = purchase.Email;

            //кодирование
            string passwordOriginal = "Esthete2711";
            byte[] bytesOriginal = Encoding.UTF8.GetBytes(passwordOriginal);
            string passwordBase64 = Convert.ToBase64String(bytesOriginal);
            //декодирование
            byte[] bytesDecode = Convert.FromBase64String(passwordBase64);
            string passwordDecode = Encoding.UTF8.GetString(bytesDecode);

            return View();
        }

        [HttpPost]
        public ViewResult RsvpForm(Purchase purchase)
        {
            if(ModelState.IsValid)
            {
                return View("Thanks", purchase);
            }
            else
            {
                return View();
            }
        }

       
    }
}