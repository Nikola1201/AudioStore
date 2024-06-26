using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioStore.Models.ViewModels
{
    public class CategoryVM
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string SuperCategoryName { get; set; }
    }
}
