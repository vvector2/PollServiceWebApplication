using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicatationPollService.Models.ViewModels {
    public class TableModelView<T> {
        public int NumberOfFilteredElm { get; set; }
        public IEnumerable<T> Elements { get; set; }
    }
}