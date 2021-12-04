//##########################################################################
//#   DATE          NAME                DESC
//#   12/3/2021    Luke Brandt         Initial Deployment
//#   
//#
//#
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
    public class DiskHasBorrowerController : Controller
    {
        private disk_inventorylbContext context { get; set; }
        public DiskHasBorrowerController(disk_inventorylbContext ctx)
        {
            context = ctx;
        }
        public IActionResult Index()
        {
            var diskhasborrowers = context.DiskHasBorrowers.
                Include(d => d.Disk).OrderBy(d => d.Disk.DiskName).ThenBy(b => b.Borrower.BorrowerLname).
                Include(b => b.Borrower).ToList();
            return View(diskhasborrowers);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            ViewBag.DiskBorrowers = context.Borrowers.OrderBy(b => b.BorrowerLname).ToList();
            ViewBag.Disks = context.Disks.OrderBy(d => d.DiskName).ToList();
            DiskHasBorrower newcheckout = new DiskHasBorrower();
            newcheckout.BorrowedDate = DateTime.Today;
           
            return View("Edit", newcheckout);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            ViewBag.DiskBorrowers = context.Borrowers.OrderBy(b => b.BorrowerLname).ToList();
            ViewBag.Disks = context.Disks.OrderBy(d => d.DiskName).ToList();
            DiskHasBorrower newcheckout = new DiskHasBorrower();
            newcheckout.BorrowedDate = DateTime.Today;

            var checkout = context.DiskHasBorrowers.Find(id);
            return View(checkout);
        }
        [HttpPost]
        public IActionResult Edit(DiskHasBorrower diskhasborrower)
        {
            if (ModelState.IsValid)
            {
                if (diskhasborrower.DiskHasBorrowerId == 0)
                {
                    context.DiskHasBorrowers.Add(diskhasborrower);
                }
                else
                {
                    context.DiskHasBorrowers.Update(diskhasborrower);
                }
                context.SaveChanges();
                return RedirectToAction("Index", "DiskHasBorrower");
            }
            else
            {
                ViewBag.Action = (diskhasborrower.DiskHasBorrowerId == 0) ? "Add" : "Edit";
                ViewBag.Action = "Add";
                ViewBag.DiskBorrowers = context.Borrowers.OrderBy(b => b.BorrowerLname).ToList();
                ViewBag.Disks = context.Disks.OrderBy(d => d.DiskName).ToList();
                DiskHasBorrower newcheckout = new DiskHasBorrower();
                newcheckout.BorrowedDate = DateTime.Today;
                return View(diskhasborrower);
            }
        }
    }
}
