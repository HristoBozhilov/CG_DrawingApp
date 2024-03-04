using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Draw.src.Model
{
    public class GroupShape : Shape
    {
        public GroupShape(RectangleF rect) : base(rect)
        {
        }

        public GroupShape(RectangleShape rectangle) : base(rectangle)
        {
        }

        public List<Shape> groupOfShapes = new List<Shape>();

        /// <summary>
        /// Проверка за принадлежност на точка point към правоъгълника.
        /// В случая на правоъгълник този метод може да не бъде пренаписван, защото
        /// Реализацията съвпада с тази на абстрактния клас Shape, който проверява
        /// дали точката е в обхващащия правоъгълник на елемента (а той съвпада с
        /// елемента в този случай).
        /// </summary>
        public override bool Contains(PointF point)
        {
            foreach (Shape item in groupOfShapes)
            {
                bool result = item.Contains(point);
                if (result == true)
                    return true;
            }
            return false;
        }

        public override PointF Location
        {
            get { return base.Location; }
            set
            {
                foreach (Shape item in groupOfShapes)
                {
                    item.Location = new PointF(item.Location.X - Location.X + value.X,
                        item.Location.Y - Location.Y + value.Y);

                }
                base.Location = new PointF(value.X, value.Y);

            }
        }

        public override Color FillColor
        {
            get => base.FillColor;
            set
            {
                base.FillColor = value;

                foreach (Shape item in groupOfShapes)
                {
                    item.FillColor = value;
                }
            }

        }

        public override Color StrokeColor
        {
            get => base.StrokeColor;
            set
            {
                base.StrokeColor = value;
                foreach (Shape item in groupOfShapes)
                {
                    item.StrokeColor = value;
                }

            }
        }

        public override int Opacity
        {
            get => base.Opacity;

            set
            {

                base.Opacity = value;
                foreach (Shape item in groupOfShapes)
                {
                    item.Opacity = value;
                }

            }
        }

        public override Matrix TransformationMatrix
        {
            get => base.TransformationMatrix;
            set
            {
                base.TransformationMatrix.Multiply(value);
                foreach (Shape item in groupOfShapes)
                {
                    item.TransformationMatrix.Multiply(value);
                }
            }
        }



        /// <summary>
        /// Частта, визуализираща конкретния примитив.
        /// </summary>
        public override void DrawSelf(Graphics grfx)
        {
            base.DrawSelf(grfx);
            foreach (Shape item in groupOfShapes)
            {
                item.DrawSelf(grfx);
            }

        }


    }
}
