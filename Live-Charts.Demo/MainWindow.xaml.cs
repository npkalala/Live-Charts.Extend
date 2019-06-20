using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Live_Charts.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public BusGaugeModel ViewModel { get; private set; } = new BusGaugeModel();
        #region
        public static readonly DependencyProperty IconStabilityProperty = DependencyProperty.Register("IconStability", typeof(ImageSource), typeof(MainWindow), null);
        public static readonly DependencyProperty IconAcceleratorProperty = DependencyProperty.Register("IconAccelerator", typeof(ImageSource), typeof(MainWindow), null);
        public static readonly DependencyProperty IconSignalProperty = DependencyProperty.Register("IconSignal", typeof(ImageSource), typeof(MainWindow), null);
        public static readonly DependencyProperty IconStationProperty = DependencyProperty.Register("IconStation", typeof(ImageSource), typeof(MainWindow), null);
        public static readonly DependencyProperty IconGaugeLightProperty = DependencyProperty.Register("IconGaugeLight", typeof(ImageSource), typeof(MainWindow), null);

        public ImageSource IconStability
        {
            get => (ImageSource)GetValue(IconStabilityProperty);
            set => SetValue(IconStabilityProperty, value);
        }

        public ImageSource IconAccelerator
        {
            get => (ImageSource)GetValue(IconAcceleratorProperty);
            set => SetValue(IconAcceleratorProperty, value);
        }

        public ImageSource IconSignal
        {
            get => (ImageSource)GetValue(IconSignalProperty);
            set => SetValue(IconSignalProperty, value);
        }

        public ImageSource IconStation
        {
            get => (ImageSource)GetValue(IconStationProperty);
            set => SetValue(IconStationProperty, value);
        }

        public ImageSource IconGaugeLight
        {
            get => (ImageSource)GetValue(IconGaugeLightProperty);
            set => SetValue(IconGaugeLightProperty, value);
        }
        private System.Timers.Timer _timersTimer;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            IconStability = ImageService.GetSVGBitmap(ImageLink.Stability);
            IconAccelerator = ImageService.GetSVGBitmap(ImageLink.Accelerator);
            IconSignal = ImageService.GetSVGBitmap(ImageLink.Signal);
            IconStation = ImageService.GetSVGBitmap(ImageLink.Station);
            IconGaugeLight = ImageService.GetSVGBitmap(ImageLink.GaugeLight);
            this.DataContext = ViewModel;
            _timersTimer = new System.Timers.Timer
            {
                Interval = 3000
            };
            _timersTimer.Elapsed += _timersTimer_Elapsed;
            _timersTimer.Start();
            QueryData();
        }

        private void _timersTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) => QueryData();

        public void QueryData()
        {
            Random crandom = new Random();
            ViewModel.V1 = crandom.Next(0, 40).ToString();
            ViewModel.V2 = crandom.Next(0, 2500).ToString();
            ViewModel.V3 = Math.Round(crandom.NextDouble() * 1, 1).ToString();
            ViewModel.V4 = crandom.Next(0, 50).ToString();
            ViewModel.V5 = crandom.Next(0, 50).ToString();
            ViewModel.V6 = crandom.Next(0, 150).ToString();
        }

        private void Clear()
        {
            ViewModel.V1 = ViewModel.V2 = ViewModel.V3 = ViewModel.V4 = ViewModel.V5 = ViewModel.V6 = "0";
        }

        public class BusGaugeModel : PropertyObserver
        {
            public string V1 { get => GetValue<string>(); set => SetValue(value); }
            public string V2 { get => GetValue<string>(); set => SetValue(value); }
            public string V3 { get => GetValue<string>(); set => SetValue(value); }
            public string V4 { get => GetValue<string>(); set => SetValue(value); }
            public string V5 { get => GetValue<string>(); set => SetValue(value); }
            public string V6 { get => GetValue<string>(); set => SetValue(value); }
        }
    }

    internal class FromColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double from = double.Parse(((string)parameter).Split(',')[0]);
            double to = double.Parse(((string)parameter).Split(',')[1]);
            return CalculatePercent(from, to, (double)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private Color CalculatePercent(double from, double to, double value)
        {
            double percent = value / (to - from);
            if (percent <= 0.33)
            {
                return (Color)ColorConverter.ConvertFromString("#0BBBBA");
            }
            else if (percent > 0.33 && percent <= 0.66)
            {
                return (Color)ColorConverter.ConvertFromString("#FFE200");
            }
            else
            {
                return (Color)ColorConverter.ConvertFromString("#F50606");
            }
        }
    }

    internal class ToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double from = double.Parse(((string)parameter).Split(',')[0]);
            double to = double.Parse(((string)parameter).Split(',')[1]);
            return CalculatePercent(from, to, (double)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private Color CalculatePercent(double from, double to, double value)
        {
            double percent = value / (to - from);
            if (percent <= 0.33)
            {
                return (Color)ColorConverter.ConvertFromString("#1384D7");
            }
            else if (percent > 0.33 && percent <= 0.66)
            {
                return (Color)ColorConverter.ConvertFromString("#FF7F00");
            }
            else
            {
                return (Color)ColorConverter.ConvertFromString("#FD3636");
            }
        }
    }

    internal class TextColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double from = double.Parse(((string)parameter).Split(',')[0]);
            double to = double.Parse(((string)parameter).Split(',')[1]);
            return CalculatePercent(from, to, (double)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private Brush CalculatePercent(double from, double to, double value)
        {
            double percent = value / (to - from);
            if (percent <= 0.33)
            {
                return (Brush)(new BrushConverter()).ConvertFromString("#3A8AF4");
            }
            else if (percent > 0.33 && percent <= 0.66)
            {
                return (Brush)(new BrushConverter()).ConvertFromString("#FFBA00");
            }
            else
            {
                return (Brush)(new BrushConverter()).ConvertFromString("#F50606");
            }
        }
    }
}
