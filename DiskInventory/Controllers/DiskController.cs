//##########################################################################
//#   DATE          NAME                DESC
//#   11/16/2021    Luke Brandt         Initial Deployment
//#   11/19/2021    //                  Added methods for adding editing and deleting artist.
//#   12/8/2021     //                  added use of stored procedures
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
    public class DiskController : Controller
    {
        private disk_inventorylbContext context { get; set; }
        public DiskController(disk_inventorylbContext ctx)
        {
            context = ctx;
        }
        public IActionResult Index()
        {
            //List<Disk> disks = context.Disks.OrderBy(d => d.DiskName).ToList();
            var disks = context.Disks.OrderBy(d => d.DiskName).
                Include(g => g.Genre).
                Include(s => s.Status).
                Include(d => d.DiskType).ToList();
            return View(disks);
        }
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            ViewBag.DiskTypes = context.DiskTypes.OrderBy(t => t.DiskTypeDescription).ToList();
            ViewBag.DiskStatuses = context.Statuses.OrderBy(s => s.StatusDescription).ToList();
            ViewBag.DiskGenres = context.Genres.OrderBy(s => s.GenreDescription).ToList();
            Disk newdisk = new Disk();
            newdisk.ReleaseDate = DateTime.Today;
            return View("Edit", newdisk);
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
                    //context.Disks.Add(disk);
                    context.Database.ExecuteSqlRaw("sp_disk_insert @p0, @p1, @p2, @p3, @p4",
                        parameters: new[] {disk.DiskName, disk.GenreId.ToString(), disk.DiskTypeId.ToString(), disk.StatusId.ToString(), disk.ReleaseDate.ToString() });
                }
                else
                {
                    //context.Disks.Update(disk);
                    context.Database.ExecuteSqlRaw("sp_disk_update @p0, @p1, @p2, @p3, @p4, @p5",
                        parameters: new[] { disk.DiskId.ToString(), disk.DiskName, disk.GenreId.ToString(), disk.DiskTypeId.ToString(), disk.StatusId.ToString(), disk.ReleaseDate.ToString() });
                }
                //context.SaveChanges();
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
            //context.Disks.Remove(disk);
            //context.SaveChanges();
            var hasBorrower = context.DiskHasBorrowers.Find(disk.DiskId);
            if (hasBorrower == null)
            {
                context.Database.ExecuteSqlRaw("execute sp_disk_delete @p0",
                                parameters: new[] { disk.DiskId.ToString() });
                return RedirectToAction("Index", "Disk");
            }
            else
            {
                ModelState.AddModelError("", "Disk exists on checkout log. Unable to delete disk from database.");
                return View(disk);
            }
            
        }
    }
}
