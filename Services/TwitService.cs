using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MirumTest.Intrefaces;
using MirumTest.Models;

namespace MirumTest.Services
{
    public class TwitService : ITwitService
    {
        readonly API.API api = new();
        public IEnumerable<RecentTwitter> Twitters(string Search)
        {
            
            return api.SerchTenTweets(Search);
        }

        
    }
}
