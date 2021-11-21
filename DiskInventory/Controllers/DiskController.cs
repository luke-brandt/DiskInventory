//##########################################################################
//#   DATE          NAME                DESC
//#   11/16/2021    Luke Brandt         Initial Deployment
//#   11/19/2021    //                  Added methods for adding editing and deleting artist.
//#
//#
//#
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiskInventory.Models;

namespace DiskInventory.Controllers
{
    public class DiskController : Controller
    {
        private disk_inventorylbContext context { get; set; }
        public DiskController(disk_inventorylbContext ctx)
        {
            context = ctx;
        }
        public IActionResult Index()
        {
            List<Disk> disks = context.Disks.OrderBy(d => d.DiskName).ToList();
            return View(disks);
        }
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            ViewBag.DiskTypes = context.DiskTypes.OrderBy(t => t.DiskTypeDescription).ToList();
            ViewBag.DiskStatuses = context.Statuses.OrderBy(s => s.StatusDescription).ToList();
            ViewBag.DiskGenres = context.Genres.OrderBy(s => s.GenreDescription).ToList();
            return View("Edit", new Disk());
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            ViewBag.DiskTypes = context.DiskTypes.OrderBy(t => t.DiskTypeDescription).ToList();
            ViewBag.DiskStatuses = context.Statuses.OrderBy(s => s.StatusDescription).ToList();
            ViewBag.DiskGenres = context.Genres.OrderBy(s => s.GenreDescription).ToList();
            var disk = context.Disks.Find(id);
            return View(disk);
        }
        [HttpPost]
        public IActionResult Edit(Disk disk)
        {
            if (ModelState.IsValid)
            {
                if(disk.DiskId == 0)
                {
                    context.Disks.Add(disk);
                }
                else
                {
                    context.Disks.Update(disk);
                }
                context.SaveChanges();
                return RedirectToAction("Index", "Disk");
            }
            else
            {
                ViewBag.Action = (disk.DiskId == 0) ? "Add" : "Edit";
                ViewBag.DiskTypes = context.DiskTypes.OrderBy(t => t.DiskTypeDescription).ToList();
                ViewBag.DiskStatuses = context.Statuses.OrderBy(s => s.StatusDescription).ToList();
                ViewBag.DiskGenres = context.Genres.OrderBy(s => s.GenreDescription).ToList();
                return View(disk);
            }
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var disk = context.Disks.Find(id);
            return View(disk);
        }
        [HttpPost]
        public IActionResult Delete(Disk disk)
        {
            context.Disks.Remove(disk);
            context.SaveChanges();
            return RedirectToAction("Index", "Disk");
        }
    }
}
