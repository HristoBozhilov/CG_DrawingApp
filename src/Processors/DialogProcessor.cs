using Draw.src.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Rectangle = System.Drawing.Rectangle;

namespace Draw
{
    [Serializable]
    /// <summary>
    /// Класът, който ще бъде използван при управляване на диалога.
    /// </summary>
    public class DialogProcessor : DisplayProcessor
    {
        #region Constructor

        public DialogProcessor()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Избран елемент.
        /// </summary>
        public List<Shape> selection = new List<Shape>();
        public List<Shape> Selection
        {
            get { return selection; }
            set { selection = value; }
        }

        /// <summary>
        /// Дали в момента диалога е в състояние на "влачене" на избрания елемент.
        /// </summary>
        private bool isDragging;
        public bool IsDragging
        {
            get { return isDragging; }
            set { isDragging = value; }
        }

        /// <summary>
        /// Последна позиция на мишката при "влачене".
        /// Използва се за определяне на вектора на транслация.
        /// </summary>
        private PointF lastLocation;
        public PointF LastLocation
        {
            get { return lastLocation; }
            set { lastLocation = value; }
        }

       

        private List<Shape> copyList = new List<Shape>();
        public List<Shape> CopyList
        {
            get { return copyList; }
            set { copyList = value; }
        }
       #endregion
        /// <summary>
        /// Добавя примитив - правоъгълник на произволно място върху клиентската област.
        /// </summary>
        public void AddRandomRectangle()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);

            RectangleShape rect = new RectangleShape(new Rectangle(x, y, 100, 200));
            rect.FillColor = Color.White;
            rect.StrokeColor = Color.Black;

            ShapeList.Add(rect);
        }

        /// <summary>
        /// Добавя примитив - елипса на произволно място върху клиентската област.
        /// </summary>
        public void AddRandomEllipse()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);

            EllipseShape ellipse = new EllipseShape(new Rectangle(x, y, 100, 200));
            ellipse.FillColor = Color.White;
            ellipse.StrokeColor = Color.Black;

            ShapeList.Add(ellipse);

        }

        /// <summary>
        /// Добавя примитив - квадрат на произволно място върху клиентската област.
        /// </summary>
        public void AddRandomSquare()
        {
            Random random = new Random();
            int x = random.Next(100, 1000);
            int y = random.Next(100, 600);

            SquareShape square = new SquareShape(new Rectangle(x, y, 150, 150));
            square.FillColor = Color.White;
            square.StrokeColor = Color.Black;

            ShapeList.Add(square);

        }

        /// <summary>
        /// Добавя примитив - триъгълник на произволно място върху клиентската област.
        /// </summary>
        public void AddRandomTriangle()
        {
            Random random = new Random();
            int x = random.Next(100, 1000);
            int y = random.Next(100, 600);

            TriangleShape triangle = new TriangleShape(new Rectangle(x, y, 200, 300));

            triangle.FillColor = Color.White;
            triangle.StrokeColor = Color.Black;

            ShapeList.Add(triangle);

        }

        /// <summary>
        /// Добавя примитив - кръг на произволно място върху клиентската област.
        /// </summary>

        public void AddRandomCircle()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);

            
            Circle circle = new Circle(new Rectangle(x, y, 100, 100));

            circle.TransformationMatrix.RotateAt(0,
                new PointF(circle.Rectangle.X + circle.Width / 2, circle.Rectangle.Y + circle.Height / 2));

            circle.FillColor = Color.White;
            circle.StrokeColor = Color.Black;

            ShapeList.Add(circle);
        }

        /// <summary>
        /// Проверява дали дадена точка е в елемента.
        /// Обхожда в ред обратен на визуализацията с цел намиране на
        /// "най-горния" елемент т.е. този който виждаме под мишката.
        /// </summary>
        /// <param name="point">Указана точка</param>
        /// <returns>Елемента на изображението, на който принадлежи дадената точка.</returns>
        public Shape ContainsPoint(PointF point)
        {
            for (int i = ShapeList.Count - 1; i >= 0; i--)
            {
                Shape shape = ShapeList[i];

                // Create a copy of the shape's transformation matrix
                Matrix transformationMatrix = shape.TransformationMatrix.Clone();

                // Invert the transformation matrix
                transformationMatrix.Invert();

                // Transform the point using the inverted matrix
                PointF[] transformedPoints = new PointF[] { point };
                transformationMatrix.TransformPoints(transformedPoints);
                PointF transformedPoint = transformedPoints[0];

                // Check if the transformed point is inside the shape
                if (shape.Contains(transformedPoint))
                {
                    return shape;
                }
            }
            return null;
        }

        /// <summary>
        /// Транслация на избраният елемент на вектор определен от <paramref name="p>p</paramref>
        /// </summary>
        /// <param name="p">Вектор на транслация.</param>
        public void TranslateTo(PointF point)
        {
            if (selection.Count > 0)
            {
                foreach (Shape item in Selection)
                {
                    if (item.TransformationMatrix != null)
                    {
                        item.TransformationMatrix.RotateAt(-item.Rotate,
                            new PointF(item.Rectangle.X + item.Width / 2, item.Rectangle.Y + item.Height / 2));

                        item.Location = new PointF(
                            item.Location.X + point.X - lastLocation.X,
                            item.Location.Y + point.Y - lastLocation.Y);

                        item.TransformationMatrix.RotateAt(item.Rotate,
                            new PointF(item.Rectangle.X + item.Width / 2, item.Rectangle.Y + item.Height / 2));
                    }
                }
                lastLocation = point;
            }
        }

        public void RotatePrimitive(int angle)
        {
            if (selection.Count > 0)
            {
                foreach (Shape item in Selection)
                {
                    Matrix matrix = new Matrix(
                        item.TransformationMatrix.Elements[0],
                        item.TransformationMatrix.Elements[1],
                        item.TransformationMatrix.Elements[2],
                        item.TransformationMatrix.Elements[3],
                        item.TransformationMatrix.Elements[4],
                        item.TransformationMatrix.Elements[5]
                    );

                    matrix.RotateAt(angle,
                        new PointF(item.Location.X + item.Width / 2,
                                   item.Location.Y + item.Height / 2));

                    item.TransformationMatrix = matrix;
                }
            }
        }



        /// <summary>
        /// Групиране на примитиви
        /// </summary>

        public void GroupPrimitives()
        {
            if (Selection.Count < 2)
                return;

            float minX = float.PositiveInfinity;
            float minY = float.PositiveInfinity;
            float maxX = float.NegativeInfinity;
            float maxY = float.NegativeInfinity;

            foreach (Shape item in Selection)
            {
                minX = Math.Min(minX, item.Location.X);
                minY = Math.Min(minY, item.Location.Y);
                maxX = Math.Max(maxX, item.Location.X + item.Width);
                maxY = Math.Max(maxY, item.Location.Y + item.Height);
            }

            int groupX = (int)minX;
            int groupY = (int)minY;
            int groupWidth = (int)(maxX - minX);
            int groupHeight = (int)(maxY - minY);

            GroupShape group = new GroupShape(new Rectangle(groupX, groupY, groupWidth, groupHeight));
            group.groupOfShapes = new List<Shape>(Selection);

            foreach (Shape item in group.groupOfShapes)
                ShapeList.Remove(item);

            Selection.Clear();
            Selection.Add(group);
            ShapeList.Add(group);
        }

        /// <summary>
        /// Разгрупиране на примитиви.
        /// </summary>
        public void UnGroupPrimitives()
        {
            var groupsToUngroup = Selection.OfType<GroupShape>().ToList();

            foreach (var group in groupsToUngroup)
            {
                ShapeList.AddRange(group.groupOfShapes);
                ShapeList.Remove(group);
                Selection.Remove(group);
                Selection.AddRange(group.groupOfShapes);
            }

            Selection.RemoveAll(shape => shape is GroupShape);
            GC.Collect();
        }




        /// <summary>
        /// Задаване на цвят на контура на примитив. 
        /// </summary>
        public void SetStrokeColor(Color color)
        {

            foreach (Shape item in selection)
                item.StrokeColor = color;
        }

        /// <summary>
        /// Задаване на цвят на вътрешността на примитив. 
        /// </summary>
        public void SetFillColor(Color color)
        {

            foreach (Shape item in selection)
                item.FillColor = color;

        }

        /// <summary>
        /// Задаване на прозрачност на примитив 
        /// </summary>
        public void SetOpacity(int opacity)
        {

            if (selection.Count > 0)
                foreach (Shape item in selection)
                    item.Opacity = opacity;
        }

        /// <summary>
        /// Задаване на дебелина на контур на примитив
        /// </summary>
        public void SetStrokeThickness(float width)
        {
            if (selection.Count > 0)
                foreach (Shape item in selection)
                    item.StrokeThickness = width;
        }

        // Метод за промяна на височина на примитв
        public void ChangeHeight(int height)
        {
            foreach (Shape item in selection)
            {
                item.Height = height;

            }

        }

        // Метод за промяна на широчина на примитив
        public void ChangeWidth(int width)
        {
            foreach (Shape item in selection)
            {
                item.Width = width;
            }


        }

        // Метод за промяна на дебелина на линия
        public void ChangeLineThickness(int line)
        {
            foreach (Shape item in selection)
            {
                line = item.LineThickness;
            }


        }


        internal void Delete()
        {
            foreach (Shape s in Selection)
            {
                ShapeList.Remove(s);
            }
        }


        public override void DrawShape(Graphics grfx, Shape item)
        {
            //For every item in subshape visualize
            base.DrawShape(grfx, item);

            if (selection.Contains(item))
            {
                GraphicsState state = grfx.Save();

                Matrix matrix = grfx.Transform.Clone();
                matrix.Multiply(item.TransformationMatrix);

                grfx.Transform = matrix;

                grfx.DrawRectangle(
                    new Pen(Color.Black, 3),
                    item.Location.X - 3,
                    item.Location.Y - 3,
                    item.Width + 6,
                    item.Height + 6

                    );
                grfx.Restore(state);
            }

        }




    }



}

