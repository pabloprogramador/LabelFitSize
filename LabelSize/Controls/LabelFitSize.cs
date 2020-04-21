using System;
using Xamarin.Forms;

using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace LabelSize.Controls
{
    public class LabelFitSize : ContentView
    {

        #region TextColorProperty
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(LabelFitSize), default(Color));

        public Color TextColor
        {
            get
            {
                return (Color)GetValue(TextColorProperty);
            }
            set
            {
                SetValue(TextColorProperty, value);
            }
        }
        #endregion

        #region TextProperty
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(LabelFitSize), default(string));

        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }
        #endregion

        #region BoldProperty
        public static readonly BindableProperty BoldProperty = BindableProperty.Create(nameof(Bold), typeof(bool), typeof(LabelFitSize), default(bool));

        public bool Bold
        {
            get
            {
                return (bool)GetValue(BoldProperty);
            }
            set
            {
                SetValue(BoldProperty, value);
            }
        }
        #endregion

        public LabelFitSize()
        {

            SKCanvasView canvasView = new SKCanvasView();
            canvasView.PaintSurface += OnCanvasViewPaintSurface;
            Content = canvasView;
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            string text = Text;

            // Create an SKPaint object to display the text
            SKPaint textPaint = new SKPaint
            {
                //Style = SKPaintStyle.Stroke,
                //StrokeWidth = 1,
                FakeBoldText = Bold,
                Color = TextColor.ToSKColor()
            };



            // Adjust TextSize property so text is 95% of screen width
            float textWidth = textPaint.MeasureText(text);
            textPaint.TextSize = 0.95f * info.Width * textPaint.TextSize / textWidth;

            // Find the text bounds
            SKRect textBounds = new SKRect();
            textPaint.MeasureText(text, ref textBounds);

            // Calculate offsets to center the text on the screen
            float xText = info.Width / 2 - textBounds.MidX;
            float yText = info.Height / 2 - textBounds.MidY;

            // And draw the text
            canvas.DrawText(text, xText, yText, textPaint);
        }
    }
}