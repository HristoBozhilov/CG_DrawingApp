using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace Draw.src.Model
{
    [Serializable]
    public class EllipseShape : Shape
    {
        public EllipseShape(RectangleF rect) : base(rect)
        { }


        public EllipseShape(EllipseShape ellipse) : base(ellipse)
        { }


        public override bool Contains(PointF point)
        {
            float xRadius = Width / 2f;
            float yRadius = Height / 2f;
            float xCenter = Location.X + xRadius;
            float yCenter = Location.Y + yRadius;

            if (point.X < Location.X || point.X >= Location.X + Width ||
                point.Y < Location.Y || point.Y >= Location.Y + Height)
            {
                return false;
            }

            return Math.Pow((point.X - xCenter), 2) / (xRadius * xRadius) +
                Math.Pow((point.Y - yCenter), 2) / (yRadius * yRadius) <= 1;

        }


        /// <summary>
        /// Частта, визуализираща конкретния примитив.
        /// </summary>
        public override void DrawSelf(Graphics grfx)
        {
            base.DrawSelf(grfx);

            GraphicsState state = grfx.Save();

            Matrix matrix = grfx.Transform.Clone();
            matrix.Multiply(TransformationMatrix);

            grfx.Transform = matrix;

            FillColor = Color.FromArgb(Opacity, FillColor);
            grfx.FillEllipse(new SolidBrush(FillColor), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
            grfx.DrawEllipse(new Pen(StrokeColor, StrokeThickness), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);

            grfx.Restore(state);
        }
    }

}
