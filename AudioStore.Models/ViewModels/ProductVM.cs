using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioStore.Models.ViewModels
{
    public class ProductVM
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public string ManufacturerName { get; set; }
        public double Price { get; set; }
    }
}
