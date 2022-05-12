using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ResearchGate.Models;

namespace ResearchGate.Controllers
{
    public class PapersController : Controller
    {
        private dbreserchgateEntities db = new dbreserchgateEntities();

        // GET: tbl_paper
        
        public ActionResult Index()
        {
            var tbl_paper = db.tbl_paper.Include(t => t.tbl_user);
            ViewBag.user = AdminController.currentUser;
            return View(tbl_paper.ToList());
        }

        // GET: tbl_paper/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_paper tbl_paper = db.tbl_paper.Find(id);
            if (tbl_paper == null)
            {
                return HttpNotFound();
            }
            return View(tbl_paper);
        }

        // GET: tbl_paper/Create
        public ActionResult Create()
        {
            ViewBag.p_fk_user = new SelectList(db.tbl_user, "u_id", "u_name");
            return View();
        }

        // POST: tbl_paper/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*tbl_paper uvm, HttpPostedFileBase imgfile*/  [Bind(Include = "p_id,p_name,p_file,p_fk_user")] tbl_paper tbl_paper)
        {
            if (ModelState.IsValid)
            {
                db.tbl_paper.Add(tbl_paper);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.p_fk_user = new SelectList(db.tbl_user, "u_id", "u_name", tbl_paper.p_fk_user);
            return View(tbl_paper);

           
        }



        public string uploadpaperfile(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".pdf") || extension.ToLower().Equals(".docx") )
                {
                    try
                    {

                        path = Path.Combine(Server.MapPath("~/Content/upload/papers"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "~/Content/upload/papers" + random + Path.GetFileName(file.FileName);

                        //    ViewBag.Message = "File uploaded successfully";
                    }
                    catch (Exception ex)
                    {
                        path = "-1";
                    }
                }
                else
                {
                    Response.Write("<script>alert('Only pdf ,docx formats are acceptable....'); </script>");
                }
            }

            else
            {
                Response.Write("<script>alert('Please select a file'); </script>");
                path = "-1";
            }



            return path;
        }



        //private String savePaperFile(HttpPostedFileBase paperFile, String currentpaperId = "")
        //{
        //    if (paperFile != null)
        //    {
        //        var fileExtenstion = Path.GetExtension(paperFile.FileName);
        //        var paperGuid = Guid.NewGuid().ToString();
        //        String paperId = paperGuid + fileExtenstion; //save new image file
        //        var filePath = Server.MapPath($"~/Uploads/papers/Users/{paperId}");
        //        paperFile.SaveAs(filePath); //delete old image file
        //        if (!String.IsNullOrEmpty(currentpaperId))
        //        {
        //            String oldImageFilePath = Server.MapPath($"~/Uploads/papers/Users/{currentpaperId}");
        //            System.IO.File.Delete(oldImageFilePath);
        //        }
        //        return paperId;
        //    }
        //    return currentpaperId;
        //}




        // GET: tbl_paper/Edit/5




        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_paper tbl_paper = db.tbl_paper.Find(id);
            if (tbl_paper == null)
            {
                return HttpNotFound();
            }
            ViewBag.p_fk_user = new SelectList(db.tbl_user, "u_id", "u_name", tbl_paper.p_fk_user);
            return View(tbl_paper);
        }

        // POST: tbl_paper/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "p_id,p_name,p_file,p_fk_user")] tbl_paper tbl_paper)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_paper).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.p_fk_user = new SelectList(db.tbl_user, "u_id", "u_name", tbl_paper.p_fk_user);
            return View(tbl_paper);
        }

        // GET: tbl_paper/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_paper tbl_paper = db.tbl_paper.Find(id);
            if (tbl_paper == null)
            {
                return HttpNotFound();
            }
            return View(tbl_paper);
        }

        // POST: tbl_paper/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_paper tbl_paper = db.tbl_paper.Find(id);
            db.tbl_paper.Remove(tbl_paper);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult SearchForPaper(String search)
        {
            List<tbl_paper> LIST = db.tbl_paper.ToList();
            return View(db.tbl_paper.Where(x => x.p_name.StartsWith(search) || x.p_file.StartsWith(search) || search == null).ToList());
            //return View();db.tbl_user.ToList()
        }

        public ActionResult searchPaper(String search)
        {
            List<tbl_paper> LIST = db.tbl_paper.ToList();
            return View(db.tbl_paper.Where(x => x.p_name.StartsWith(search) || x.p_file.StartsWith(search) || search == null).ToList());
            //return View();db.tbl_user.ToList()
        }


        // GET: tbl_paper/Details/5
        public ActionResult UserDetails(int? id , string name)
        {

            var tbl_paper = db.tbl_paper.Include(t => t.tbl_user);
            ViewBag.user = id;
            ViewBag.username = name;
            return View(tbl_paper.ToList());
        }


    }
}
