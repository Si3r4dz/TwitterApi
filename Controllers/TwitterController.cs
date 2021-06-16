using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using MirumTest.Models;
using MirumTest.API;
using X.PagedList;
using X.PagedList.Mvc;
using MirumTest.Intrefaces;

namespace MirumTest.Controllers
{
    public class TwitterController : Controller
    {

        readonly ITwitService _twitService;
        public TwitterController(ITwitService twitService)
        {
            _twitService = twitService;
        }

        public IActionResult Index()
        {

            return View();
        }

       public static IEnumerable<RecentTwitter> model;

        //Displaying 10 Tweets in "SearchResult" Page 

        public IActionResult Result(int? page)
        {

            
                int pageSize = 5;
                int pageNumber = (page ?? 1);

                var mod = model.ToPagedList(pageNumber, pageSize);

                return View(mod);
           

            
        }

        public IActionResult Error(ErrorViewModel mod)
        {
            return View();
        }

        
        public IActionResult GetTwits(String Search)
        {
            model = _twitService.Twitters(Search);
            return RedirectToAction("Result");
        }


    }
}
