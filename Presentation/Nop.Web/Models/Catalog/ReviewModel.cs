using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nop.Web.Models.Catalog
{
    public class ReviewModel
    {
        public string ProductName { get; set; }

        public string ProductSeName { get; set; }

        public string CustomerName { get; set; }

        public string Title { get; set; }

        public string ReviewText { get; set; }

        public DateTime Date { get; set; }
    }
}