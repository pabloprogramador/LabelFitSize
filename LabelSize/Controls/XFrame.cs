using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace LabelSize.Controls
{
    public class XFrame : ContentView
    {




        #region OpacityShadowProperty
        public static readonly BindableProperty OpacityShadowProperty = BindableProperty.Create(nameof(OpacityShadow), typeof(double), typeof(XFrame), (double)1);

        public double OpacityShadow
        {
            get
            {
                return (double)GetValue(OpacityShadowProperty);
            }
            set
            {
                SetValue(OpacityShadowProperty, value);
            }
        }
        #endregion

        #region BorderRadiusProperty
        public static readonly BindableProperty BorderRadiusProperty = BindableProperty.Create(nameof(BorderRadius), typeof(float), typeof(XFrame), (float)20);

        public float BorderRadius
        {
            get
            {
                return (float)GetValue(BorderRadiusProperty);
            }
            set
            {
                SetValue(BorderRadiusProperty, value);
            }
        }
        #endregion

        #region SizeShadowProperty
        public static readonly BindableProperty SizeShadowProperty = BindableProperty.Create(nameof(SizeShadow), typeof(float), typeof(XFrame), (float)70);

        public float SizeShadow
        {
            get
            {
                return (float)GetValue(SizeShadowProperty);
            }
            set
            {
                SetValue(SizeShadowProperty, value);
            }
        }
        #endregion

        #region ShowInnerProperty
        public static readonly BindableProperty ShowInnerProperty = BindableProperty.Create(nameof(ShowInner), typeof(bool), typeof(XFrame), true);

        public bool ShowInner
        {
            get
            {
                return (bool)GetValue(ShowInnerProperty);
            }
            set
            {
                SetValue(ShowInnerProperty, value);
            }
        }
        #endregion


        #region ShowOutProperty
        public static readonly BindableProperty ShowOutProperty = BindableProperty.Create(nameof(ShowOut), typeof(bool), typeof(XFrame), true);

        public bool ShowOut
        {
            get
            {
                return (bool)GetValue(ShowOutProperty);
            }
            set
            {
                SetValue(ShowOutProperty, value);
            }
        }
        #endregion

        #region BaseColorProperty
        public static readonly BindableProperty BaseColorProperty = BindableProperty.Create(nameof(BaseColor), typeof(Color), typeof(XFrame), Color.Gray);

        public Color BaseColor
        {
            get
            {
                return (Color)GetValue(BaseColorProperty);
            }
            set
            {
                SetValue(BaseColorProperty, value);
            }
        }
        #endregion



        private float SizeStroker;
        private SKCanvasView canvasView;

        public XFrame()
        {
            //this.HasShadow = false;
            //this.Padding = 0;
            
            this.BackgroundColor = Color.Transparent;
            
            

            canvasView = new SKCanvasView();
            canvasView.PaintSurface += OnCanvasViewPaintSurface;

            Console.WriteLine(":::::>>>>> COLOR " + BaseColor);


            InitAll();

            //Content = canvasView;
            // Draw Rectangle
        }

        async Task InitAll()
        {
            
            if (Content == null)
            {
                Console.WriteLine(":::::>>>>> Content null");
                await Task.Delay(100);
                await InitAll();
                return;
            }
            else
            {
                SizeStroker = (SizeShadow / 2);
                Console.WriteLine(":::::>>>>> SizeStroker " + SizeStroker);
                var _content = Content;
                var grid = new Grid();
                grid.Children.Add(canvasView);
                grid.Children.Add(_content);
                Content = grid;
            }
        }
        

        enum TypeShadow
        {
            top, down
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            


            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;
            canvas.Clear();

            float dxShadow = SizeStroker/2;


            if (ShowOut)
            {
                makeShadow(info, canvas, TypeShadow.down, dxShadow, dxShadow, Color.Black, BaseColor);
                makeShadow(info, canvas, TypeShadow.top, -dxShadow, -dxShadow, Color.White, BaseColor);
              

            }

            if (ShowInner)
            {
                var mask = makeBox(info, canvas, TypeShadow.top, -dxShadow, -dxShadow, Color.White, BaseColor);
                canvas.ClipRoundRect(mask, SKClipOperation.Intersect);

                makeInnerShadow(info, canvas, TypeShadow.down, -dxShadow, -dxShadow, Color.White, BaseColor);
                makeInnerShadow(info, canvas, TypeShadow.top, dxShadow, dxShadow, Color.Black, BaseColor);
            }




        }


        

        void makeInnerShadow(SKImageInfo info, SKCanvas canvas, TypeShadow typeShadow, float x, float y, Color color, Color backgroundColor)
        {

            

            SKPaint skPaint = new SKPaint()
            {
                Style = SKPaintStyle.Stroke,
                Color = color.ToSKColor().WithAlpha((byte)(0xFF * OpacityShadow)),
                StrokeWidth = SizeStroker,
                IsAntialias = true,
                ImageFilter = SKImageFilter.CreateDropShadow(
                                    x,
                                    y,
                                    SizeStroker,
                                    SizeStroker,
                                    color.ToSKColor().WithAlpha((byte)(0xFF * OpacityShadow)),
                         SKDropShadowImageFilterShadowMode.DrawShadowAndForeground)


            };

            SKRect skRectangle = new SKRect();
            if (!ShowOut)
            {
                skRectangle.Size = new SKSize(info.Width, info.Height);
                skRectangle.Location = new SKPoint(0, 0);
            }
            else
            {
                skRectangle.Size = new SKSize(info.Width - (SizeShadow * 2), info.Height - (SizeShadow * 2));
                skRectangle.Location = new SKPoint(SizeShadow, SizeShadow);
            }
           

            
            skRectangle.Inflate(SizeStroker, SizeStroker);
            canvas.DrawRoundRect(skRectangle, BorderRadius, BorderRadius, skPaint);
        }

        void makeShadow(SKImageInfo info, SKCanvas canvas, TypeShadow typeShadow, float x, float y, Color color, Color backgroundColor)
        {
            
            SKPaint skPaint = new SKPaint()
            {
                Style = SKPaintStyle.Fill,
                Color = backgroundColor.ToSKColor(),
                //StrokeWidth = 1,
                IsAntialias = true,

               
                ImageFilter = SKImageFilter.CreateDropShadow(
                                    x,
                                    y,
                                    SizeStroker,
                                    SizeStroker,
                                    color.ToSKColor().WithAlpha((byte)(0xFF * OpacityShadow)),
                         SKDropShadowImageFilterShadowMode.DrawShadowAndForeground)


            };
            var auxSize = SizeShadow;
            SKRect skRectangle = new SKRect();
            skRectangle.Size = new SKSize(info.Width - (auxSize * 2), info.Height - (auxSize * 2));

            skRectangle.Location = new SKPoint(auxSize, auxSize);




            canvas.DrawRoundRect(skRectangle, BorderRadius, BorderRadius, skPaint);
        }

        SKRoundRect makeBox(SKImageInfo info, SKCanvas canvas, TypeShadow typeShadow, float x, float y, Color color, Color backgroundColor)
        {
            
            SKPaint skPaint = new SKPaint()
            {
                Style = SKPaintStyle.Fill,
                Color = backgroundColor.ToSKColor(),
                //StrokeWidth = 20,
                IsAntialias = true,


            };

            SKRect skRectangle = new SKRect();
            if (!ShowOut)
            {
                skRectangle.Size = new SKSize(info.Width, info.Height);
                skRectangle.Location = new SKPoint(0, 0);
            }
            else
            {
                skRectangle.Size = new SKSize(info.Width - (SizeShadow * 2), info.Height - (SizeShadow * 2));
                skRectangle.Location = new SKPoint(SizeShadow, SizeShadow);
            }
            var mask = new SKRoundRect(skRectangle, BorderRadius, BorderRadius);
            canvas.DrawRoundRect(skRectangle, BorderRadius, BorderRadius, skPaint);
            //canvas.ClipRoundRect(mask, SKClipOperation.Difference);
            return mask;
        }
    }
}
