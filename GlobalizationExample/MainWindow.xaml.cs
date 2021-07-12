using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GlobalizationExample.Properties;

namespace GlobalizationExample
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum Rating
    {
        [LocalizedDescription(nameof(Resources.Rating_ExcellentBook), typeof(Resources))]
        Excellent = 0,

        [LocalizedDescription(nameof(Resources.Rating_VeryGoodBook), typeof(Resources))]
        VeryGood = 1,

        [LocalizedDescription(nameof(Resources.Rating_GoodBook), typeof(Resources))]
        Good = 2,

        [LocalizedDescription(nameof(Resources.Rating_AverageBook), typeof(Resources))]
        Average = 3,

        [LocalizedDescription(nameof(Resources.Rating_BelowAverageBook), typeof(Resources))]
        BelowAverage =4,

        [LocalizedDescription(nameof(Resources.Rating_PoorBook), typeof(Resources))]
        Poor = 5,

        [LocalizedDescription(nameof(Resources.Rating_UselessBook), typeof(Resources))]
        Useless = 6
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        ObservableCollection<Rating> _ratings;
        ObservableCollection<CultureInfo> _cultures;
        CultureInfo _currentCulture;

        public event PropertyChangedEventHandler PropertyChanged;

        private class SwitchCultureCommand : ICommand
        {
            public event EventHandler CanExecuteChanged;
            MainWindow _window;
            public SwitchCultureCommand(MainWindow window)
            {
                _window = window;
            }

            public bool CanExecute(object parameter)
            {
                return _window?.SelectedCulture != null;
            }

            public void Execute(object parameter)
            {
                CultureInfo culture = _window.SelectedCulture;
                Thread uiThread = Thread.CurrentThread;
                uiThread.CurrentCulture = culture;
                uiThread.CurrentUICulture = culture;
                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;

                MainWindow window = new MainWindow(_window.SelectedCulture, _window.Cultures);
                window.FlowDirection = culture.TextInfo.IsRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
                window.Language = XmlLanguage.GetLanguage(culture.Name);
                App.Current.MainWindow = window;
                
                window.Language = XmlLanguage.GetLanguage(culture.IetfLanguageTag);
                window.Show();
                _window.Close();
                //_window.RestartWithCulture = _window.SelectedCulture;
                //
            }
        }

        private class AboutMessageDisplayCommand : ICommand
        {
            public event EventHandler CanExecuteChanged;

            MainWindow _window;
            public AboutMessageDisplayCommand(MainWindow window)
            {
                _window = window;
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                MessageBoxOptions options = CultureInfo.CurrentCulture.TextInfo.IsRightToLeft ? MessageBoxOptions.RtlReading : MessageBoxOptions.None;
                MessageBox.Show(_window, Properties.Resources.MainWindow_Ribbon_HelpTab_About_Message, _window.Title
                    , MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, options);
            }
        }

        public MainWindow() : this(null, null)
        {

        }

        internal MainWindow(CultureInfo selectedCulture, IEnumerable<CultureInfo> supportedCultures)
        {
            InitializeComponent();
            DataContext = this;
            _ratings = new ObservableCollection<Rating>((Rating[])Enum.GetValues(typeof(Rating)));
            _cultures = new ObservableCollection<CultureInfo>(supportedCultures ?? new CultureInfo[] { new CultureInfo("en-US"), new CultureInfo("mr-IN"), new CultureInfo("ar-IQ") });
            var selectedCultureDetails = _cultures.Where((c, index) => string.Equals(c.Name, selectedCulture?.Name, StringComparison.OrdinalIgnoreCase))
                .Select((c2, index2) => new { Index = index2, Culture = c2 }).FirstOrDefault();

            SelectedCulture = selectedCulture ?? (_cultures.Count > 0 ? _cultures[0] : null);
            ChangeCultureCommand = new SwitchCultureCommand(this);
            AboutCommand = new MainWindow.AboutMessageDisplayCommand(this);
        }

        public DateTime PurchaseDate { get { return DateTime.Now; } }

        public double Price { get { return 1234.57d; } }

        public string BookName { get { return "C# and .NET Framework for dummies"; } }

        public ObservableCollection<Rating> Ratings => _ratings;

        public ObservableCollection<CultureInfo> Cultures => _cultures;

        public CultureInfo SelectedCulture
        {
            get { return _currentCulture; }
            set
            {
                if(object.ReferenceEquals(_currentCulture, value)) { return; }

                _currentCulture = value;
                OnPropertyChanged();
            }
        }

        public ICommand ChangeCultureCommand { get; }

        public ICommand AboutCommand { get; }

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler ev = PropertyChanged;
            if(ev != null)
            {
                ev(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        internal CultureInfo RestartWithCulture { get; private set; }
    }
}
