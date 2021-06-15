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

namespace MirumTest.Controllers
{
    public class TwitterController : Controller
    {

        public IActionResult Index()
        {

            return View();
        }



        //Displaying 10 Tweets in "SearchResult" Page 

        public IActionResult Result(string Search)
        {
            ViewData["Search"] = Search;
            API.API api = new API.API();
            var model = new List<RecentTwitter>();
            var mirum = new List<MirumTweets>();
            model = api.SerchTenTweets(Search);
            mirum = api.SearchMirumTweets();
            dynamic mymodel = new ExpandoObject();
            mymodel.model = model;
            mymodel.mirum = mirum;

            if (model != null)
            {


                return View(model);


            }
            else
            {
                return RedirectToAction("Error");

            }
        }

        public IActionResult Error(ErrorViewModel mod)
        {
            return View();
        }




    }
}
