using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace приложение.ViewModels
{
    public class ClassViewModel
    {
        public int ID { get; set; }
        public int ClassNumber { get; set; }
        public string ClassLetter { get; set; }
        public string DisplayClass 
        { 
            get
            {
                return $"{ClassNumber}{ClassLetter}";
            }
        }
    }
}
