//##########################################################################
//#   DATE          NAME                DESC
//#   11/12/2021    Luke Brandt         Initial Deployment
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
using Microsoft.EntityFrameworkCore;

namespace DiskInventory.Controllers
{
    public class ArtistController : Controller
    {
        private disk_inventorylbContext context { get; set; }
        public ArtistController(disk_inventorylbContext ctx)
        {
            context = ctx;
        }
        public IActionResult Index()
        {
            // List<Artist> artists = context.Artists.OrderBy(a => a.ArtistName).ToList();
            var artist = context.Artists.OrderBy(a => a.ArtistName).
                Include(at => at.ArtistType).ToList();
            return View(artist);
        }
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            ViewBag.ArtistTypes = context.ArtistTypes.OrderBy(t => t.ArtistTypeDescription).ToList();
            return View("Edit", new Artist());
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            ViewBag.ArtistTypes = context.ArtistTypes.OrderBy(t => t.ArtistTypeDescription).ToList();
            var artist = context.Artists.Find(id);
            return View(artist);
        }
        [HttpPost]
        public IActionResult Edit(Artist artist)
        {
            if (ModelState.IsValid)
            {
                if(artist.ArtistId == 0) // if hidden view is = to zero add artist
                {
                    context.Artists.Add(artist);
                }
                else //if hidden view has a value edit the existing artist
                {
                    context.Artists.Update(artist);
                }
                context.SaveChanges();
                return RedirectToAction("Index", "Artist");
            }
            else
            {
                ViewBag.Action = (artist.ArtistId == 0) ? "Add" : "Edit";
                ViewBag.ArtistTypes = context.ArtistTypes.OrderBy(t => t.ArtistTypeDescription).ToList();
                return View(artist);
            }
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var artist = context.Artists.Find(id);
            return View(artist);
        }
        [HttpPost]
        public IActionResult Delete(Artist artist)
        {
            context.Artists.Remove(artist);
            context.SaveChanges();
            return RedirectToAction("Index", "Artist");
        }
       
    }
}
