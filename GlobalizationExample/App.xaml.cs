using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;

namespace WpfPlayground
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            //CultureInfo culture = new CultureInfo("mr-IN");
            //Thread.CurrentThread.CurrentCulture = culture;
            //Thread.CurrentThread.CurrentUICulture = culture;

            //FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.Name)));

            //var flowDirection = CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft
            //    ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

            //FrameworkElement.FlowDirectionProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(flowDirection));
            base.OnStartup(e);
        }

        internal CultureInfo RestartWithCulture { get; set; }
    }
}
