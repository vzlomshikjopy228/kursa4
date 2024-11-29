using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace приложение.Services
{
    public interface INavigation
    {
        void NavigateToPage(Page page);
        void SetFrame(Frame frame);
    } 
}
