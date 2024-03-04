using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Draw
{
    [Serializable]
    /// <summary>
    /// Базовия клас на примитивите, който съдържа общите характеристики на примитивите.
    /// </summary>
    public abstract class Shape
	{
		#region Constructors
		
		public Shape()
		{
		}
		
		public Shape(RectangleF rect)
		{
			rectangle = rect;
		}
		
		public Shape(Shape shape)
		{
			this.Height = shape.Height;
			this.Width = shape.Width;
			this.Location = shape.Location;
			this.rectangle = shape.rectangle;
			
			this.FillColor =  shape.FillColor;
		}
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Обхващащ правоъгълник на елемента.
		/// </summary>
		private RectangleF rectangle;		
		public virtual RectangleF Rectangle {
			get { return rectangle; }
			set { rectangle = value; }
		}
		
		/// <summary>
		/// Широчина на елемента.
		/// </summary>
		public virtual float Width {
			get { return Rectangle.Width; }
			set { rectangle.Width = value; }
		}
		
		/// <summary>
		/// Височина на елемента.
		/// </summary>
		public virtual float Height {
			get { return Rectangle.Height; }
			set { rectangle.Height = value; }
		}
		
		/// <summary>
		/// Горен ляв ъгъл на елемента.
		/// </summary>
		public virtual PointF Location {
			get { return Rectangle.Location; }
			set { rectangle.Location = value; }
		}
		
		/// <summary>
		/// Цвят на елемента.
		/// </summary>
		private Color fillColor;		
		public virtual Color FillColor {
			get { return fillColor; }
			set { fillColor = value; }
		}

        private Color strokeColor;

        /// <summary>
        /// Цвят на контура на елемента.
        /// </summary>
        public virtual Color StrokeColor
        {
            get { return strokeColor; }
            set { strokeColor = value; }
        }

        private int opacity = 255;

        /// <summary>
        /// Прозрачност на елемента.
        /// </summary>
        public virtual int Opacity
        {
            get { return opacity; }
            set { opacity = value; }
        }

        private float strokeThickness = 1f;

        /// <summary>
        /// Дебелина на контура на елемент.
        /// </summary>
        public virtual float StrokeThickness
        {
            get { return strokeThickness; }
            set { strokeThickness = value; }
        }

        private int lineThickness;
        public virtual int LineThickness
        {
            get { return lineThickness; }
            set { lineThickness = value; }
        }

        private Matrix transformationMatrix = new Matrix();

        public virtual Matrix TransformationMatrix
        {
            get { return transformationMatrix; }
            set { transformationMatrix = value; }

        }

        private int rotate;
        public virtual int Rotate
        {

            get { return rotate; }
            set { rotate = value; }
        }


        #endregion


        /// <summary>
        /// Проверка дали точка point принадлежи на елемента.
        /// </summary>
        /// <param name="point">Точка</param>
        /// <returns>Връща true, ако точката принадлежи на елемента и
        /// false, ако не пренадлежи</returns>
        public virtual bool Contains(PointF point)
		{
			return Rectangle.Contains(point.X, point.Y);
		}
		
		/// <summary>
		/// Визуализира елемента.
		/// </summary>
		/// <param name="grfx">Къде да бъде визуализиран елемента.</param>
		public virtual void DrawSelf(Graphics grfx)
		{
			// shape.Rectangle.Inflate(shape.BorderWidth, shape.BorderWidth);
		}

        public virtual object Clone()
        {
            // Create a new instance of the same type as the current object
            Shape newShape = (Shape)Activator.CreateInstance(this.GetType());

            // Set the properties of the new instance 
            newShape.Location = this.Location;
            newShape.Height = this.Height;
            newShape.Width = this.Width;
            newShape.StrokeThickness = this.StrokeThickness;
            newShape.FillColor = this.FillColor;
            newShape.TransformationMatrix = this.TransformationMatrix;

            return newShape;
        }

    }
}
