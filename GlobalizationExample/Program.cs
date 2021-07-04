using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfPlayground
{
    class Program
    {
        [STAThread]
        static int Main(string[] args)
        {
            CultureInfo[] supportedCultures = new CultureInfo[] { new CultureInfo("en-US"), new CultureInfo("mr-IN"), new CultureInfo("ar-SA") };
            App app = new App();
            MainWindow window = null;
            CultureInfo selectedCulture = null;
            
            
            if (selectedCulture != null)
            {
                Thread.CurrentThread.CurrentCulture = selectedCulture;
                Thread.CurrentThread.CurrentUICulture = selectedCulture;
            }
                
            window = new MainWindow(selectedCulture, supportedCultures); 
            app.Run(window);
            

            return 0;
        }
    }
}
