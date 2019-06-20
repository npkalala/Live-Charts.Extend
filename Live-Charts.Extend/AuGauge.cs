using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.Wpf.Points;
using LiveCharts.Wpf;
namespace Live_Charts.Extend
{
    /// <summary>
    /// The gauge chart is useful to display progress or completion.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.UserControl" />
    public class AuGauge : UserControl
    {
        SolidColorBrush _titleColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#818181"));
        /// <summary>
        /// Initializes a new instance of the <see cref="Gauge"/> class.
        /// </summary>
        public AuGauge()
        {
            //var fontFamily = new FontFamily("Microsoft YaHei UI");
            //(FontFamily)this.FindResource("SourceHanSansTC");
            //this.FontFamily = fontFamily;

            Canvas = new Canvas();
            Content = Canvas;

            PieBack = new PieSlice();
            Pie = new PieSlice();
            MeasureTextBlock = new TextBlock();
            LeftLabel = new TextBlock();
            RightLabel = new TextBlock();
            //August

            MeasureTextBlockUnit = new TextBlock();
            MeasureTextBlockUnit.Foreground = _titleColor;
            Title = new TextBlock();
            //Title.FontFamily = fontFamily;
            Title.Foreground = _titleColor;

            Canvas.Children.Add(PieBack);
            Canvas.Children.Add(Pie);
            Canvas.Children.Add(MeasureTextBlock);
            //August
            Canvas.Children.Add(MeasureTextBlockUnit);
            Canvas.Children.Add(Title);

            #region Grid
            //Grid Left
            GridLeft = new Grid();
            GridleftRowUp = new RowDefinition
            {
                Height = new GridLength(50, GridUnitType.Star)
            };
            GridleftRowDown = new RowDefinition
            {
                Height = new GridLength(50, GridUnitType.Star)
            };
            GridLeft.RowDefinitions.Add(GridleftRowUp);
            GridLeft.RowDefinitions.Add(GridleftRowDown);
            //Grid Right
            GridRight = new Grid();
            GridRightRowUp = new RowDefinition
            {
                Height = new GridLength(50, GridUnitType.Star)
            };
            GridRightRowDown = new RowDefinition
            {
                Height = new GridLength(150, GridUnitType.Star)
            };
            GridRight.RowDefinitions.Add(GridRightRowUp);
            GridRight.RowDefinitions.Add(GridRightRowDown);

            //Label Max/Min
            Max = new TextBlock
            {
                Foreground = _titleColor,
                Text = " Max ",
                //FontFamily = new FontFamily("Microsoft YaHei UI")//(FontFamily)this.FindResource("SourceHanSansTC")
            };
            Min = new TextBlock
            {
                Foreground = _titleColor,
                Text = " Min "
            };
            GridLeft.Children.Add(Min);
            GridRight.Children.Add(Max);

            //Grid Barder
            GridLeftBoarder = new Border
            {
                BorderThickness = new Thickness(0, 1, 0, 0),
                BorderBrush = _titleColor
            };
            GridLeft.Children.Add(GridLeftBoarder);
            GridRightBoarder = new Border
            {
                BorderThickness = new Thickness(0, 1, 0, 0),
                BorderBrush = _titleColor
            };
            GridRight.Children.Add(GridRightBoarder);

            //Label Left/Right
            GridLeft.Children.Add(LeftLabel);
            LeftLabel.Foreground = _titleColor;
            LeftLabel.HorizontalAlignment = HorizontalAlignment.Center;

            GridRight.Children.Add(RightLabel);
            RightLabel.Foreground = _titleColor;
            RightLabel.HorizontalAlignment = HorizontalAlignment.Center;

            GridLeftBoarder.SetValue(Grid.RowProperty, 1);
            GridRightBoarder.SetValue(Grid.RowProperty, 1);

            Min.SetValue(Grid.RowProperty, 1);
            Max.SetValue(Grid.RowProperty, 1);
            LeftLabel.SetValue(Grid.RowProperty, 0);
            RightLabel.SetValue(Grid.RowProperty, 0);

            Canvas.Children.Add(GridLeft);
            Canvas.Children.Add(GridRight);
            Panel.SetZIndex(GridLeft, 1);
            Panel.SetZIndex(GridRight, 1);
            #endregion
            //RightLabel.TextDecorations = System.Windows.TextDecorations.Underline;
            //LeftLabel.TextDecorations = System.Windows.TextDecorations.Underline;

            //Canvas.Children.Add(RightLabel);
            //Canvas.Children.Add(LeftLabel);

            Panel.SetZIndex(MeasureTextBlock, 1);
            Panel.SetZIndex(RightLabel, 1);
            Panel.SetZIndex(LeftLabel, 1);

            Panel.SetZIndex(PieBack, 0);
            Panel.SetZIndex(Pie, 1);

            Canvas.SetBinding(WidthProperty,
                new Binding { Path = new PropertyPath(WidthProperty), Source = this });
            Canvas.SetBinding(HeightProperty,
                new Binding { Path = new PropertyPath(HeightProperty), Source = this });

            PieBack.SetBinding(Shape.FillProperty,
                new Binding { Path = new PropertyPath(GaugeBackgroundProperty), Source = this });
            PieBack.SetBinding(Shape.StrokeThicknessProperty,
                new Binding { Path = new PropertyPath(StrokeThicknessProperty), Source = this });
            PieBack.SetBinding(Shape.StrokeProperty,
                new Binding { Path = new PropertyPath(StrokeProperty), Source = this });
            PieBack.SetBinding(RenderTransformProperty,
                new Binding { Path = new PropertyPath(GaugeRenderTransformProperty), Source = this });

            Pie.SetBinding(Shape.StrokeThicknessProperty,
                new Binding { Path = new PropertyPath(StrokeThicknessProperty), Source = this });
            Pie.SetBinding(RenderTransformProperty,
                new Binding { Path = new PropertyPath(GaugeRenderTransformProperty), Source = this });
            Pie.Stroke = Brushes.Transparent;

            SetCurrentValue(GaugeBackgroundProperty, new SolidColorBrush(Color.FromRgb(21, 101, 191)) { Opacity = .1 });
            SetCurrentValue(StrokeThicknessProperty, 0d);
            SetCurrentValue(StrokeProperty, new SolidColorBrush(Color.FromRgb(222, 222, 222)));

            SetCurrentValue(FromColorProperty, Color.FromRgb(100, 180, 245));
            SetCurrentValue(ToColorProperty, Color.FromRgb(21, 101, 191));

            SetCurrentValue(MinHeightProperty, 20d);
            SetCurrentValue(MinWidthProperty, 20d);

            SetCurrentValue(AnimationsSpeedProperty, TimeSpan.FromMilliseconds(800));

            //MeasureTextBlock.FontWeight = FontWeights.Bold;

            IsNew = true;

            SizeChanged += (sender, args) =>
            {
                IsChartInitialized = true;
                Update();
            };
        }

        #region Properties

        private Canvas Canvas { get; set; }
        private PieSlice PieBack { get; set; }
        private PieSlice Pie { get; set; }
        private TextBlock MeasureTextBlock { get; set; }
        private TextBlock LeftLabel { get; set; }
        private TextBlock RightLabel { get; set; }
        private bool IsNew { get; set; }
        private bool IsChartInitialized { get; set; }

        private TextBlock MeasureTextBlockUnit { get; set; }
        private TextBlock Title { get; set; }

        #region Grid for Left/Right
        private Grid GridLeft { get; set; }
        private RowDefinition GridleftRowUp { get; set; }
        private RowDefinition GridleftRowDown { get; set; }

        private Border GridLeftBoarder { get; set; }
        private TextBlock Min { get; set; }

        private Grid GridRight { get; set; }
        private RowDefinition GridRightRowUp { get; set; }
        private RowDefinition GridRightRowDown { get; set; }

        private Border GridRightBoarder { get; set; }
        private TextBlock Max { get; set; }

        private Image Icon { get; set; }
        #endregion

        /// <summary>
        /// The gauge active fill property
        /// </summary>
        public static readonly DependencyProperty GaugeActiveFillProperty = DependencyProperty.Register(
            "GaugeActiveFill", typeof(Brush), typeof(AuGauge), new PropertyMetadata(default(Brush)));
        /// <summary>
        /// Gets or sets the gauge active fill, if this property is set, From/to color properties interpolation will be ignored
        /// </summary>
        /// <value>
        /// The gauge active fill.
        /// </value>
        public Brush GaugeActiveFill
        {
            get { return (Brush)GetValue(GaugeActiveFillProperty); }
            set { SetValue(GaugeActiveFillProperty, value); }
        }

        /// <summary>
        /// The labels visibility property
        /// </summary>
        public static readonly DependencyProperty LabelsVisibilityProperty = DependencyProperty.Register(
            "LabelsVisibility", typeof(Visibility), typeof(AuGauge), new PropertyMetadata(default(Visibility)));
        /// <summary>
        /// Gets or sets the labels visibility.
        /// </summary>
        /// <value>
        /// The labels visibility.
        /// </value>
        public Visibility LabelsVisibility
        {
            get { return (Visibility)GetValue(LabelsVisibilityProperty); }
            set { SetValue(LabelsVisibilityProperty, value); }
        }

        /// <summary>
        /// The gauge render transform property
        /// </summary>
        public static readonly DependencyProperty GaugeRenderTransformProperty = DependencyProperty.Register(
            "GaugeRenderTransform", typeof(Transform), typeof(AuGauge), new PropertyMetadata(default(Transform)));
        /// <summary>
        /// Gets or sets the gauge render transform.
        /// </summary>
        /// <value>
        /// The gauge render transform.
        /// </value>
        public Transform GaugeRenderTransform
        {
            get { return (Transform)GetValue(GaugeRenderTransformProperty); }
            set { SetValue(GaugeRenderTransformProperty, value); }
        }

        /// <summary>
        /// The uses360 mode property
        /// </summary>
        public static readonly DependencyProperty Uses360ModeProperty = DependencyProperty.Register(
            "Uses360Mode", typeof(bool), typeof(AuGauge), new PropertyMetadata(default(bool), UpdateCallback));
        /// <summary>
        /// Gets or sets whether the gauge uses 360 mode, 360 mode will plot a full circle instead of a semi circle
        /// </summary>
        public bool Uses360Mode
        {
            get { return (bool)GetValue(Uses360ModeProperty); }
            set { SetValue(Uses360ModeProperty, value); }
        }

        /// <summary>
        /// From property
        /// </summary>
        public static readonly DependencyProperty FromProperty = DependencyProperty.Register(
            "From", typeof(double), typeof(AuGauge), new PropertyMetadata(0d, UpdateCallback));
        /// <summary>
        /// Gets or sets the value where the gauge starts
        /// </summary>
        public double From
        {
            get { return (double)GetValue(FromProperty); }
            set { SetValue(FromProperty, value); }
        }

        /// <summary>
        /// To property
        /// </summary>
        public static readonly DependencyProperty ToProperty = DependencyProperty.Register(
            "To", typeof(double), typeof(AuGauge), new PropertyMetadata(1d, UpdateCallback));
        /// <summary>
        /// Gets or sets the value where the gauge ends
        /// </summary>
        public double To
        {
            get { return (double)GetValue(ToProperty); }
            set { SetValue(ToProperty, value); }
        }

        /// <summary>
        /// The value property
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(double), typeof(AuGauge), new PropertyMetadata(default(double), UpdateCallback));
        /// <summary>
        /// Gets or sets the current value of the gauge
        /// </summary>
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// The inner radius property
        /// </summary>
        public static readonly DependencyProperty InnerRadiusProperty = DependencyProperty.Register(
            "InnerRadius", typeof(double?), typeof(AuGauge), new PropertyMetadata(null, UpdateCallback));
        /// <summary>
        /// Gets o sets inner radius
        /// </summary>
        public double? InnerRadius
        {
            get { return (double?)GetValue(InnerRadiusProperty); }
            set { SetValue(InnerRadiusProperty, value); }
        }

        /// <summary>
        /// The stroke property
        /// </summary>
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
            "Stroke", typeof(Brush), typeof(AuGauge), new PropertyMetadata(default(Brush)));
        /// <summary>
        /// Gets or sets stroke, the stroke is the brush used to draw the gauge border.
        /// </summary>
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }


        /// <summary>
        /// The stroke thickness property
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
            "StrokeThickness", typeof(double), typeof(AuGauge), new PropertyMetadata(default(double)));
        /// <summary>
        /// Gets or sets stroke brush thickness
        /// </summary>
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        /// <summary>
        /// To color property
        /// </summary>
        public static readonly DependencyProperty ToColorProperty = DependencyProperty.Register(
            "ToColor", typeof(Color), typeof(AuGauge), new PropertyMetadata(default(Color), UpdateCallback));
        /// <summary>
        /// Gets or sets the color when the current value equals to min value, any value between min and max will use an interpolated color.
        /// </summary>
        public Color ToColor
        {
            get { return (Color)GetValue(ToColorProperty); }
            set { SetValue(ToColorProperty, value); }
        }

        /// <summary>
        /// From color property
        /// </summary>
        public static readonly DependencyProperty FromColorProperty = DependencyProperty.Register(
            "FromColor", typeof(Color), typeof(AuGauge), new PropertyMetadata(default(Color), UpdateCallback));
        /// <summary>
        /// Gets or sets the color when the current value equals to max value, any value between min and max will use an interpolated color.
        /// </summary>
        public Color FromColor
        {
            get { return (Color)GetValue(FromColorProperty); }
            set { SetValue(FromColorProperty, value); }
        }

        /// <summary>
        /// The gauge background property
        /// </summary>
        public static readonly DependencyProperty GaugeBackgroundProperty = DependencyProperty.Register(
            "GaugeBackground", typeof(Brush), typeof(AuGauge), new PropertyMetadata(default(Brush)));
        /// <summary>
        /// Gets or sets the gauge background
        /// </summary>
        public Brush GaugeBackground
        {
            get { return (Brush)GetValue(GaugeBackgroundProperty); }
            set { SetValue(GaugeBackgroundProperty, value); }
        }

        /// <summary>
        /// The animations speed property
        /// </summary>
        public static readonly DependencyProperty AnimationsSpeedProperty = DependencyProperty.Register(
            "AnimationsSpeed", typeof(TimeSpan), typeof(AuGauge), new PropertyMetadata(default(TimeSpan)));
        /// <summary>
        /// G3ts or sets the gauge animations speed
        /// </summary>
        public TimeSpan AnimationsSpeed
        {
            get { return (TimeSpan)GetValue(AnimationsSpeedProperty); }
            set { SetValue(AnimationsSpeedProperty, value); }
        }

        /// <summary>
        /// The disablea animations property
        /// </summary>
        public static readonly DependencyProperty DisableAnimationsProperty = DependencyProperty.Register(
            "DisableAnimations", typeof(bool), typeof(AuGauge), new PropertyMetadata(default(bool)));
        /// <summary>
        /// Gets or sets whether the chart is animated
        /// </summary>
        public bool DisableAnimations
        {
            get { return (bool)GetValue(DisableAnimationsProperty); }
            set { SetValue(DisableAnimationsProperty, value); }
        }

        /// <summary>
        /// The label formatter property
        /// </summary>
        public static readonly DependencyProperty LabelFormatterProperty = DependencyProperty.Register(
            "LabelFormatter", typeof(Func<double, string>), typeof(AuGauge), new PropertyMetadata(default(Func<double, string>)));
        /// <summary>
        /// Gets or sets the label formatter, a label formatter takes a double value, and return a string, e.g. val => val.ToString("C");
        /// </summary>
        public Func<double, string> LabelFormatter
        {
            get { return (Func<double, string>)GetValue(LabelFormatterProperty); }
            set { SetValue(LabelFormatterProperty, value); }
        }

        /// <summary>
        /// The high font size property
        /// </summary>
        public static readonly DependencyProperty HighFontSizeProperty = DependencyProperty.Register(
            "HighFontSize", typeof(double?), typeof(AuGauge), new PropertyMetadata(null));
        /// <summary>
        /// Gets o sets the label size, if this value is null then it will be automatically calculated, default is null.
        /// </summary>
        public double? HighFontSize
        {
            get { return (double?)GetValue(HighFontSizeProperty); }
            set { SetValue(HighFontSizeProperty, value); }
        }

        /// <summary>
        /// The gauge background property
        /// </summary>
        public static readonly DependencyProperty FontColorProperty = DependencyProperty.Register(
            "FontColor", typeof(Brush), typeof(AuGauge), new PropertyMetadata(default(Brush)));
        /// <summary>
        /// Gets or sets the gauge background
        /// </summary>
        public Brush FontColor
        {
            get { return (Brush)GetValue(FontColorProperty); }
            set { SetValue(FontColorProperty, value); }
        }

        /// <summary>
        /// The gauge Title
        /// </summary>
        public static readonly DependencyProperty GaugeTitleProperty = DependencyProperty.Register(
            "GaugeTitle", typeof(string), typeof(AuGauge), new PropertyMetadata(default(string)));
        /// <summary>
        /// Gets or sets the gauge background
        /// </summary>
        public string GaugeTitle
        {
            get { return (string)GetValue(GaugeTitleProperty); }
            set { SetValue(GaugeTitleProperty, value); }
        }

        /// <summary>
        /// The gauge unit
        /// </summary>
        public static readonly DependencyProperty GaugeUnitProperty = DependencyProperty.Register(
            "GaugeUnit", typeof(string), typeof(AuGauge), new PropertyMetadata(default(string)));
        /// <summary>
        /// Gets or sets the gauge background
        /// </summary>
        public string GaugeUnit
        {
            get { return (string)GetValue(GaugeUnitProperty); }
            set { SetValue(GaugeUnitProperty, value); }
        }

        /// <summary>
        /// The gauge unit
        /// </summary>
        public static readonly DependencyProperty IconSourceProperty = DependencyProperty.Register(
            "IconSource", typeof(ImageSource), typeof(AuGauge), new PropertyMetadata(default(ImageSource)));
        /// <summary>
        /// Gets or sets the gauge background
        /// </summary>
        public ImageSource IconSource
        {
            get { return (ImageSource)GetValue(IconSourceProperty); }
            set { SetValue(IconSourceProperty, value); }
        }
        #endregion

        bool _isCallBack = false;
        private static void UpdateCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var gauge = (AuGauge)dependencyObject;

            gauge.Update();
        }

        private void Update()
        {
            if (!IsChartInitialized)
                return;

            Func<double, string> defFormatter = x => x.ToString(CultureInfo.InvariantCulture);

            var completed = (Value - From) / (To - From);

            var t = 0d;

            if (double.IsNaN(completed) || double.IsInfinity(completed))
            {
                completed = 0;
            }

            completed = completed > 1 ? 1 : (completed < 0 ? 0 : completed);
            var angle = Uses360Mode ? 360 : 180;

            if (!_isCallBack && Uses360Mode)
            {
                Grid.SetRow(Min, 0);
                Grid.SetRow(Max, 0);
                Grid.SetRow(LeftLabel, 1);
                Grid.SetRow(RightLabel, 1);
                //LeftLabel.UpdateLayout();
                //RightLabel.UpdateLayout();
                GridLeft.Children.Remove(GridLeftBoarder);
                GridLeftBoarder = new Border
                {
                    BorderThickness = new Thickness(1, 0, 0, 0),
                    BorderBrush = _titleColor
                };
                GridLeft.Children.Add(GridLeftBoarder);
                Grid.SetRow(GridLeftBoarder, 1);
                GridRight.Children.Remove(GridRightBoarder);
                Panel.SetZIndex(GridLeftBoarder, 1);
                Canvas.SetLeft(GridLeftBoarder, ActualWidth / 2);
                _isCallBack = true;
            }

            if (!Uses360Mode)
            {
                LeftLabel.Text = (LabelFormatter ?? defFormatter)(From);
                RightLabel.Text = (LabelFormatter ?? defFormatter)(To);

                LeftLabel.UpdateLayout();
                RightLabel.UpdateLayout();

                //LeftLabel.Visibility = LabelsVisibility;
                GridLeft.Visibility = LabelsVisibility;
                //RightLabel.Visibility = LabelsVisibility;
                GridRight.Visibility = LabelsVisibility;
                t = LeftLabel.ActualHeight;

                if (Icon != null)
                    Icon.Visibility = Visibility.Hidden;
            }
            else
            {
                LeftLabel.Text = (LabelFormatter ?? defFormatter)(From);
                RightLabel.Text = (LabelFormatter ?? defFormatter)(To);

                LeftLabel.UpdateLayout();
                RightLabel.UpdateLayout();

                GridLeft.Visibility = LabelsVisibility;
                GridRight.Visibility = LabelsVisibility;

                //LeftLabel.Visibility = Visibility.Hidden;
                //RightLabel.Visibility = Visibility.Hidden;
                //
                t = GridLeft.ActualHeight;

                if (Icon != null)
                    Icon.Visibility = Visibility.Visible;
            }

            double r, top;

            if (Uses360Mode)
            {
                //r = ActualWidth > ActualHeight ? ActualHeight : ActualWidth;
                //r = r / 2 - t; //r / 2 - 2 * t;
                r = ActualWidth > ActualHeight ? ActualHeight / 2 - t : ActualWidth / 2;
                top = ActualHeight / 2;
            }
            else
            {
                r = ActualWidth;

                if (ActualWidth > ActualHeight * 2)
                {
                    r = ActualHeight * 2;
                }
                else
                {
                    t = 0;
                }

                //r = r / 2 - 2 * t;
                r = r / 2;
                top = ActualHeight / 2 + r / 2;
            }

            if (r < 0)
                r = 1;

            PieBack.Radius = r;
            PieBack.InnerRadius = InnerRadius ?? r * .8;
            PieBack.RotationAngle = 270;
            PieBack.WedgeAngle = angle;

            Pie.Radius = PieBack.Radius * 0.96;
            Pie.InnerRadius = PieBack.InnerRadius * 1.06;
            Pie.RotationAngle = PieBack.RotationAngle;

            Canvas.SetLeft(PieBack, ActualWidth / 2);
            Canvas.SetTop(PieBack, top);
            Canvas.SetLeft(Pie, ActualWidth / 2);
            Canvas.SetTop(Pie, top);

            //Canvas.SetTop(LeftLabel, top);
            //Canvas.SetTop(RightLabel, top);
            //Canvas.SetRight(LeftLabel, ActualWidth / 2 + ( r + PieBack.InnerRadius ) / 2 - LeftLabel.ActualWidth / 2);
            //Canvas.SetRight(RightLabel, ActualWidth / 2 - ( r + PieBack.InnerRadius ) / 2 - RightLabel.ActualWidth / 2);

            MeasureTextBlock.FontSize = HighFontSize ?? Pie.InnerRadius * .4;
            if (Uses360Mode)
                MeasureTextBlock.FontSize = Pie.InnerRadius * .6;
            MeasureTextBlock.Text = (LabelFormatter ?? defFormatter)(Value);
            if (FontColor != null)
            {
                MeasureTextBlock.Foreground = FontColor;
            }

            MeasureTextBlock.UpdateLayout();

            double ratio = Uses360Mode ? .3 : .4;
            MeasureTextBlockUnit.Text = (Uses360Mode ? "" : " ") + GaugeUnit;
            MeasureTextBlockUnit.FontSize = MeasureTextBlock.FontSize * ratio;
            MeasureTextBlockUnit.UpdateLayout();

            Title.Text = GaugeTitle;
            Title.FontSize = MeasureTextBlock.FontSize * ratio;
            Title.UpdateLayout();

            //LeftLabel.FontSize = MeasureTextBlock.FontSize * ratio;
            //RightLabel.FontSize = MeasureTextBlock.FontSize * ratio;
            //LeftLabel.UpdateLayout();
            //RightLabel.UpdateLayout();
            if (Uses360Mode)
            {
                double top360 = top - r - GridLeft.ActualHeight; //top - Gridleft.ActualHeight + ( r + PieBack.InnerRadius ) / 2;
                Canvas.SetTop(GridLeft, top360);
                Canvas.SetTop(GridRight, top360);
                Canvas.SetLeft(GridRight, (ActualWidth / 2) - GridRight.ActualWidth);
                Canvas.SetLeft(GridLeft, ActualWidth / 2);
                if (IconSource != null)
                {
                    if (Icon == null)
                    {
                        Icon = new Image
                        {
                            Source = IconSource
                        };
                        Icon.UpdateLayout();
                        Canvas.Children.Add(Icon);
                    }
                    Icon.Width = Icon.Height = Pie.InnerRadius * .5;
                    Canvas.SetTop(Icon, top - PieBack.InnerRadius * 0.9);
                    Canvas.SetLeft(Icon, (ActualWidth - Icon.Width) / 2);
                    Icon.UpdateLayout();
                }
            }
            else
            {
                LeftLabel.FontSize = Min.FontSize = MeasureTextBlock.FontSize * ratio * 0.6;
                Min.UpdateLayout();
                LeftLabel.UpdateLayout();
                Canvas.SetTop(GridLeft, top - GridLeft.ActualHeight);
                Canvas.SetLeft(GridLeft, ActualWidth / 2 - PieBack.InnerRadius);
                RightLabel.FontSize = Max.FontSize = MeasureTextBlock.FontSize * ratio * 0.6;
                Max.UpdateLayout();
                RightLabel.UpdateLayout();
                Canvas.SetTop(GridRight, top - GridRight.ActualHeight);
                Canvas.SetLeft(GridRight, ActualWidth / 2 + PieBack.InnerRadius - GridRight.ActualWidth);
            }
            GridLeft.UpdateLayout();
            GridRight.UpdateLayout();
            //August Lee
            if (Uses360Mode)
            {
                Canvas.SetTop(MeasureTextBlock, top - MeasureTextBlock.ActualHeight * .5);
                Canvas.SetLeft(MeasureTextBlock, ActualWidth / 2 - MeasureTextBlock.ActualWidth / 2);

                Canvas.SetTop(MeasureTextBlockUnit, top + (MeasureTextBlock.ActualHeight) * .5);
                Canvas.SetLeft(MeasureTextBlockUnit, ActualWidth / 2 - MeasureTextBlockUnit.ActualWidth / 2);

                Canvas.SetTop(Title, top + Title.ActualHeight + MeasureTextBlock.ActualHeight / 2);
                Canvas.SetLeft(Title, ActualWidth / 2 - Title.ActualWidth / 2);
            }
            else
            {
                Canvas.SetTop(MeasureTextBlock, top - (MeasureTextBlock.ActualHeight * 1.5));
                Canvas.SetLeft(MeasureTextBlock, ActualWidth / 2 - MeasureTextBlock.ActualWidth / 2 - MeasureTextBlockUnit.ActualWidth / 2);

                Canvas.SetTop(MeasureTextBlockUnit, top - (MeasureTextBlock.ActualHeight * 0.5 + MeasureTextBlockUnit.ActualHeight * 1.2));
                Canvas.SetLeft(MeasureTextBlockUnit, ActualWidth / 2 + MeasureTextBlock.ActualWidth / 2 - MeasureTextBlockUnit.ActualWidth / 2);

                Canvas.SetTop(Title, top - Title.ActualHeight);
                Canvas.SetLeft(Title, ActualWidth / 2 - Title.ActualWidth / 2);
            }

            var interpolatedColor = new Color
            {
                R = LinearInterpolation(FromColor.R, ToColor.R),
                G = LinearInterpolation(FromColor.G, ToColor.G),
                B = LinearInterpolation(FromColor.B, ToColor.B),
                A = LinearInterpolation(FromColor.A, ToColor.A)
            };

            if (IsNew)
            {
                Pie.Fill = new SolidColorBrush(FromColor);
                Pie.WedgeAngle = 0;
            }

            if (DisableAnimations)
            {
                Pie.WedgeAngle = completed * angle;
            }
            else
            {
                Pie.BeginAnimation(PieSlice.WedgeAngleProperty, new DoubleAnimation(completed * angle, AnimationsSpeed));
            }

            if (GaugeActiveFill == null)
            {
                ((SolidColorBrush)Pie.Fill).BeginAnimation(SolidColorBrush.ColorProperty,
                    new ColorAnimation(interpolatedColor, AnimationsSpeed));
            }
            else
            {
                Pie.Fill = GaugeActiveFill;
            }

            IsNew = false;
        }

        private byte LinearInterpolation(double from, double to)
        {
            var p1 = new Point(From, from);
            var p2 = new Point(To, to);

            var deltaX = p2.X - p1.X;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            var m = (p2.Y - p1.Y) / (deltaX == 0 ? double.MinValue : deltaX);

            var v = Value > To ? To : (Value < From ? From : Value);

            return (byte)(m * (v - p1.X) + p1.Y);
        }
    }
}
