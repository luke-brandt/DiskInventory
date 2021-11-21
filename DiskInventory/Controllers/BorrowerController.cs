//##########################################################################
//#   DATE          NAME                DESC
//#   11/16/2021    Luke Brandt         Initial Deployment
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
    }
}
