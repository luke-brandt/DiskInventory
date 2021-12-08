//##########################################################################
//#   DATE          NAME                DESC
//#   11/16/2021    Luke Brandt         Initial Deployment
//#   12/3/2021        //               Added add update and delete methods
//#   12/6/2021        //               add stored procedure calls
//#   12/8/2021        //               add validation for borrower deletion
//#
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiskInventory.Models;
using Microsoft.EntityFrameworkCore;

namespace DiskInventory.Controllers
{
    public class BorrowerController : Controller
    {
        private disk_inventorylbContext context { get; set; }
        public BorrowerController(disk_inventorylbContext ctx)
        {
            context = ctx;
        }
        public IActionResult Index()
        {
            List<Borrower> borrowers = context.Borrowers.OrderBy(b => b.BorrowerLname).ThenBy(b => b.BorrowerFname).ToList();
            return View(borrowers);
        }
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            return View("Edit", new Borrower());
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            var borrower = context.Borrowers.Find(id);
            return View(borrower);
        }
        [HttpPost]
        public IActionResult Edit(Borrower borrower)
        {
            if (ModelState.IsValid)
            {
                if(borrower.BorrowerId == 0)
                {
                    //context.Borrowers.Add(borrower);
                    context.Database.ExecuteSqlRaw("execute sp_borrower_insert @p0, @p1, @p2",
                        parameters: new[] { borrower.BorrowerFname, borrower.BorrowerLname, borrower.BorrowerPhone.ToString() });
                }
                else
                {
                    //context.Borrowers.Update(borrower);
                    context.Database.ExecuteSqlRaw("execute sp_borrower_update @p0, @p1, @p2, @p3",
                        parameters: new[] {borrower.BorrowerFname, borrower.BorrowerLname, borrower.BorrowerPhone.ToString(), borrower.BorrowerId.ToString() });
                }
                //context.SaveChanges();
                return RedirectToAction("Index", "Borrower");

            }
            else
            {
                ViewBag.Action = (borrower.BorrowerId == 0) ? "Add" : "Edit";
                return View(borrower);
            }
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var borrower = context.Borrowers.Find(id);
            return View(borrower);
        }
        [HttpPost]
        public IActionResult Delete(Borrower borrower, DiskHasBorrower diskHasBorrower)
        {
            //context.Borrowers.Remove(borrower);
            //context.SaveChanges();
            var hasBorrower = context.DiskHasBorrowers.Find(borrower.BorrowerId);
            if (hasBorrower == null)
            {
                context.Database.ExecuteSqlRaw("execute sp_borrower_delete @p0",
                                parameters: new[] { borrower.BorrowerId.ToString() });
                return RedirectToAction("Index", "Borrower");
            }
            else
            {
                ModelState.AddModelError("", "Borrower exists on checkout log. Unable to delete borrower from database.");
                return View(borrower);
            }
            
        }
    }
}
