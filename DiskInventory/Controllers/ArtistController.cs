//##########################################################################
//#   DATE          NAME                DESC
//#   11/12/2021    Luke Brandt         Initial Deployment
//#   11/19/2021    //                  Added methods for adding editing and deleting artist.  
//#   12/6/2021     //                  Add stored procedure calls
//#   12/8/2021     //                  Added delete validation
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
                    //context.Artists.Add(artist);
                    context.Database.ExecuteSqlRaw("execute sp_artist_insert @p0, @p1",
                        parameters: new[] { artist.ArtistName, artist.ArtistTypeId.ToString() });
                }
                else //if hidden view has a value edit the existing artist
                {
                    //context.Artists.Update(artist)
                    context.Database.ExecuteSqlRaw("execute sp_artist_update @p0, @p1, @p2",
                        parameters: new[] { artist.ArtistId.ToString(), artist.ArtistName, artist.ArtistTypeId.ToString() });
                }
                //context.SaveChanges();
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
            //context.Artists.Remove(artist);
            //context.SaveChanges();
            var hasBorrower = context.DiskHasBorrowers.Find(artist.ArtistId);
            if (hasBorrower == null)
            {
                context.Database.ExecuteSqlRaw("execute sp_artist_delete @p0",
                parameters: new[] { artist.ArtistId.ToString() });
                return RedirectToAction("Index", "Artist");
            }
            else
            {
                ModelState.AddModelError("", "Artist exists on checkout log. Unable to delete artist from database.");
                return View(artist);
            }
            
        }
       
    }
}
