﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Draw
{
    [Serializable]
    /// <summary>
    /// Класът правоъгълник е основен примитив, който е наследник на базовия Shape.
    /// </summary>
    public class Circle : Shape
    {
        #region Constructor

        // creates an circle object within a specifies bounding rectangle
        public Circle(RectangleF rect) : base(rect)
        {
        }
        // creates a new circle obj that is a copy of an existing ellipse obj
        public Circle(RectangleShape rectangle) : base(rectangle)
        {
        }

        #endregion

        public override bool Contains(PointF point)
        {
            // Check if point is within the bounding rectangle of the circle
            if (base.Contains(point))
            {
                // Apply transformation matrix to account for any rotations or translations
                PointF[] pointFs = { point };
                TransformationMatrix.Invert();
                TransformationMatrix.TransformPoints(pointFs);
                TransformationMatrix.Invert();

                // Calculate distance between point and center of circle
                double x = base.Location.X + (base.Width / 2);
                double y = base.Location.Y + (base.Height / 2);
               double temp1 = Math.Pow(pointFs[0].X - x, 2);
                double temp2 = Math.Pow(pointFs[0].Y - y, 2);

                // Calculate radius of circle
                double radiusX = Math.Pow(base.Width / 2, 2);
                double radiusY = Math.Pow(base.Height / 2, 2);

              //Check if distance is less than or equal to radius
                double temp3 = temp1 / radiusX + temp2 / radiusY;
               if (temp3 <= 1)
                    return true;
                else
                    return false;
            }
            else
            {
                // If point is not within bounding rectangle, it cannot be within circle
                return false;
            }
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

