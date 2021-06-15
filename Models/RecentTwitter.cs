using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MirumTest.Models
{
    public class RecentTwitter
    {
        public int Id { get; set; }


        public string Text { get; set; }

        public string Username { get; set; }

        public DateTime Created_at { get; set; }

        [StringLength(25, MinimumLength=1), DataType(DataType.Text),Required]
        public string search { get; set; }


    }
}
