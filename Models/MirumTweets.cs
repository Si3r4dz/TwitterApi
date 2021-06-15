using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MirumTest.Models
{
    public class MirumTweets
    {
        [StringLength(140)]
        public string Text { get; set; }
        public DateTime Created_at { get; set; }

    }
}
