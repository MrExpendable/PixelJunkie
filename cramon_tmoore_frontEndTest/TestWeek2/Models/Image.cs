using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWeek2.Models
{
    public class Image
    {
        public string Title { get; set; }
        public string Owner { get; set; }
        public int Likes { get; set; }
        public uint Price { get; set; }
        public List<String> Tags { get; set; }

        public Image()
        {
            Title = "No";
            Owner = "You";
            Likes = 12;
            Price = 100;
            Tags = new List<String>
            {
                "No",
                "Please",
                "Work"
            };
        }
    }
}