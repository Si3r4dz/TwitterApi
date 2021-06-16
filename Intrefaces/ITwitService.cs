using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MirumTest.Models;

namespace MirumTest.Intrefaces
{
   public interface ITwitService
    {
        IEnumerable<RecentTwitter> Twitters(string Search);
        

    }
}
