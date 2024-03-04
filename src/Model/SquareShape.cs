using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Draw
{
    [Serializable]
    /// <summary>
    /// Класът правоъгълник е основен примитив, който е наследник на базовия Shape.
    /// </summary>
    public class SquareShape : Shape
    {
        #region Constructor

        
        public SquareShape(RectangleF rect) : base(rect)
        {
        }
       
        public SquareShape(RectangleShape rectangle) : base(rectangle)
        {
        }

        #endregion

        /// <summary>
        /// Проверка за принадлежност на точка point към правоъгълника.
        /// В случая на правоъгълник този метод може да не бъде пренаписван, защото
        /// Реализацията съвпада с тази на абстрактния клас Shape, който проверява
        /// дали точката е в обхващащия правоъгълник на елемента (а той съвпада с
        /// елемента в този случай).
        /// </summary>
        public override bool Contains(PointF point)
        {
            if (base.Contains(point))
            {
                // Check if the point is in the bounding rectangle first

                PointF[] transformedPoints = { point };
                TransformationMatrix.Invert();
                TransformationMatrix.TransformPoints(transformedPoints);
                TransformationMatrix.Invert();

                double halfSideLength = Width / 2.0;
                double centerX = Location.X + halfSideLength;
                double centerY = Location.Y + halfSideLength;
                double xDifference = Math.Abs(transformedPoints[0].X - centerX);
                double yDifference = Math.Abs(transformedPoints[0].Y - centerY);

                if (xDifference <= halfSideLength && yDifference <= halfSideLength)
                {
                    // If the point is within half the side length of the center in both x and y directions,
                    // then it is contained within the square
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                // If the point is not even within the bounding rectangle, then it cannot be contained within the square
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

            grfx.FillRectangle(new SolidBrush(FillColor), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
            grfx.DrawRectangle(new Pen(StrokeColor,StrokeThickness), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);

            grfx.Restore(state);
        }

    }
}

