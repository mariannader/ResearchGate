using ResearchGate.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ResearchGate.Controllers
{
    public class AdminController : Controller
    {
        dbreserchgateEntities db = new dbreserchgateEntities();
        public static string currentUser = "";

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }


        
        public ActionResult SignUp()
        {

            return View();
        }

        [HttpPost]
        public ActionResult SignUp(tbl_user uvm, HttpPostedFileBase imgfile)
        {
            string path = uploadimgfile(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Image could not be uploaded....";
            }
            else
            {
                tbl_user u = new tbl_user();
                u.u_name = uvm.u_name;
                u.u_email = uvm.u_email;
                u.u_password = uvm.u_password;
                u.u_image = path;
                u.u_contact = uvm.u_contact;
                u.u_position = uvm.u_position;
                db.tbl_user.Add(u);
                db.SaveChanges();
                return RedirectToAction("login");


            }

            return View();
        } //method......................... end.....................





        public ActionResult login()
        {
            return View();
        }


        [HttpPost]
        
        public ActionResult login(tbl_user avm)
        {
            tbl_user ad = db.tbl_user.Where(x => x.u_email == avm.u_email && x.u_password == avm.u_password).SingleOrDefault();
            currentUser = ad.u_name;
            if (ad != null)
            {
                Session["u_id"] = ad.u_id.ToString();
                return RedirectToAction("Index", "Papers");
            }
            else
            {
                ViewBag.error = "Invalid username or password";
            }
            return View();
        }


        //public ActionResult profile(string currentUser)
        //{
        //    var tbl_paper = db.tbl_paper.Include(t => t.tbl_paper);
        //    ViewBag.user = currentUser;
        //    return View(tbl_paper.ToList());

        //}




        public ActionResult SearchForUser(String search)
        {
            List<tbl_user> LIST = db.tbl_user.ToList();
            return View(db.tbl_user.Where(x => x.u_name.StartsWith(search) || x.u_email.StartsWith(search) || search == null).ToList());
            //return View();db.tbl_user.ToList()
        }
        public ActionResult searchUser(String search)
        {
            List<tbl_user> LIST = db.tbl_user.ToList();
            return View(db.tbl_user.Where(x => x.u_name.StartsWith(search) || x.u_email.StartsWith(search) || search == null).ToList());
            //return View();db.tbl_user.ToList()
        }

        public ActionResult EditProfile()
        {
            

            return View();
        }







        






        public string uploadimgfile(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {
                    try
                    {

                        path = Path.Combine(Server.MapPath("~/Content/upload"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "~/Content/upload/" + random + Path.GetFileName(file.FileName);

                        //    ViewBag.Message = "File uploaded successfully";
                    }
                    catch (Exception ex)
                    {
                        path = "-1";
                    }
                }
                else
                {
                    Response.Write("<script>alert('Only jpg ,jpeg or png formats are acceptable....'); </script>");
                }
            }

            else
            {
                Response.Write("<script>alert('Please select a file'); </script>");
                path = "-1";
            }



            return path;
        }








    }
}