using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.Reflection.PortableExecutable;

namespace TrafficLights.Views
{
    public partial class TrafficLightsCanvas : UserControl
    {
        /// <summary>
        /// ���� �������
        /// </summary>
        private readonly IBrush CaseColor = Brushes.Black;

        /// <summary>
        /// ���� ������ �����
        /// </summary>
        private readonly IBrush CircleColor = Brushes.Black;

        /// <summary>
        /// ������ ����� - ������� �� ������ ���������
        /// </summary>
        private const double LightsRadusPercent = 0.85;

        /// <summary>
        /// ������ ����� �������
        /// </summary>
        private const double CaseLinesWidth = 5; 

        /// <summary>
        /// ������ ����� ������
        /// </summary>
        private const double CirclesLinesWidth = 2;

        /// <summary>
        /// ������ ����������
        /// </summary>
        private const double LedRadius = 2;

        /// <summary>
        /// ���������� ����� ������������
        /// </summary>
        private const double LedSpacing = 1.5;

        /// <summary>
        /// ���������� ������� �����
        /// </summary>
        private readonly IBrush RedLightOnColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF0000");

        /// <summary>
        /// ���������� ����� �����
        /// </summary>
        private readonly IBrush YellowLightOnColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFFF00");

        /// <summary>
        /// ���������� ������ �����
        /// </summary>
        private readonly IBrush GreenLightOnColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#00FF00");

        /// <summary>
        /// ����������� ������� �����
        /// </summary>
        private readonly IBrush RedLightOffColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#500000");

        /// <summary>
        /// ����������� ����� �����
        /// </summary>
        private readonly IBrush YellowLightOffColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#666300");

        /// <summary>
        /// ����������� ������ �����
        /// </summary>
        private readonly IBrush GreenLightOffColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#035200");

        #region ���������� ������

        /// <summary>
        /// �������� ���������� ������� ����
        /// </summary>
        public static readonly StyledProperty<bool> IsRedOnProperty = AvaloniaProperty.Register<TrafficLightsCanvas, bool>(nameof(IsRedOn));

        /// <summary>
        /// �����-�� ������� �����
        /// </summary>
        public bool IsRedOn
        {
            get { return GetValue(IsRedOnProperty); }
            set { SetValue(IsRedOnProperty, value); }
        }

        /// <summary>
        /// �������� ���������� ����� ����
        /// </summary>
        public static readonly StyledProperty<bool> IsYellowOnProperty = AvaloniaProperty.Register<TrafficLightsCanvas, bool>(nameof(IsYellowOn));

        /// <summary>
        /// �����-�� ������� �����
        /// </summary>
        public bool IsYellowOn
        {
            get { return GetValue(IsYellowOnProperty); }
            set { SetValue(IsYellowOnProperty, value); }
        }

        /// <summary>
        /// �������� ���������� ����� ����
        /// </summary>
        public static readonly StyledProperty<bool> IsGreenOnProperty = AvaloniaProperty.Register<TrafficLightsCanvas, bool>(nameof(IsGreenOn));

        /// <summary>
        /// �����-�� ������� �����
        /// </summary>
        public bool IsGreenOn
        {
            get { return GetValue(IsGreenOnProperty); }
            set { SetValue(IsGreenOnProperty, value); }
        }

        #endregion

        public TrafficLightsCanvas()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ����� ���������
        /// </summary>
        public override void Render(DrawingContext context)
        {
            // ������ � ������ ��������������  
            var width = Width;
            var height = Height;

            // �������������� �����
            var centerX = width / 2;

            // ������������ ������ �����
            var centerRedY = 1 * height / 4;
            var centerYellowY = 2 * height / 4;
            var centerGreenY = 3 * height / 4;

            // ������� �������� �����
            var lightsContoursRaduses = LightsRadusPercent * width / 2;

            // ������ ������ ���������
            var casePen = new Pen(CaseColor, CaseLinesWidth, lineCap: PenLineCap.Square);
            context.DrawRectangle(casePen, new Rect(0, 0, width, height));

            // ������ �����
            var circlePen = new Pen(CircleColor, CirclesLinesWidth, lineCap: PenLineCap.Square);
            
            DrawLights(context, centerX - lightsContoursRaduses, centerRedY - lightsContoursRaduses, 2 * lightsContoursRaduses, IsRedOn ? RedLightOnColor : RedLightOffColor);
            DrawCircle(context, centerX, centerRedY, lightsContoursRaduses, circlePen); // ������ ������ �������� ����

            // ������ ������ ������ ����
            DrawLights(context, centerX - lightsContoursRaduses, centerYellowY - lightsContoursRaduses, 2 * lightsContoursRaduses, IsYellowOn ? YellowLightOnColor : YellowLightOffColor);
            DrawCircle(context, centerX, centerYellowY, lightsContoursRaduses, circlePen);

            // ������ ������ ������� ����
            DrawLights(context, centerX - lightsContoursRaduses, centerGreenY - lightsContoursRaduses, 2 * lightsContoursRaduses, IsGreenOn ? GreenLightOnColor : GreenLightOffColor);
            DrawCircle(context, centerX, centerGreenY, lightsContoursRaduses, circlePen);

            base.Render(context);
        }

        /// <summary>
        /// ��������� �����
        /// </summary>
        private void DrawCircle(DrawingContext context, double x, double y, double radius, Pen pen)
        {
            context.DrawEllipse(Brushes.Transparent, pen, new Point(x, y), radius, radius);
        }

        /// <summary>
        /// ��������� ����������
        /// </summary>
        private void DrawLed(DrawingContext context, double x, double y, IBrush ledColor)
        {
            context.DrawEllipse(ledColor, new Pen(ledColor, 1, lineCap: PenLineCap.Square), new Point(x, y), LedRadius, LedRadius);
        }

        /// <summary>
        /// ��������� ������������ � ����� �����
        /// </summary>
        private void DrawLights(DrawingContext context, double x, double y, double side, IBrush lightColor)
        {
            for (double yPos = y; yPos <= y + side; yPos += 2 * LedRadius + LedSpacing)
            {
                for (double xPos = x; xPos <= x + side; xPos += 2 * LedRadius + LedSpacing)
                {
                    if (Math.Pow(xPos - x - 0.5 * side, 2) + Math.Pow(yPos - y - 0.5 * side, 2) <= Math.Pow(0.5 * (side - LedRadius), 2))
                    {
                        DrawLed(context, xPos, yPos, lightColor);
                    }
                }
            }
        }
    }
}
