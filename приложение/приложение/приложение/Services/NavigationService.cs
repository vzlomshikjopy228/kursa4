using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace приложение.Services
{
    public class NavigationService : INavigation
    {
        private Frame _mainFrame;
        private readonly IServiceProvider _serviceProvider;
        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public void SetFrame(Frame mainFrame)
        {
            _mainFrame = mainFrame ?? throw new ArgumentNullException(nameof(mainFrame));
        }

        public void NavigateToPage(Page page)
        {
            _mainFrame?.Navigate(page);
        }
    }
}
