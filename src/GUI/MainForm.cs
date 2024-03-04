using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Draw
{
    /// <summary>
    /// Върху главната форма е поставен потребителски контрол,
    /// в който се осъществява визуализацията
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Агрегирания диалогов процесор във формата улеснява манипулацията на модела.
        /// </summary>
        private DialogProcessor dialogProcessor = new DialogProcessor();

        public MainForm()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }

        /// <summary>
        /// Изход от програмата. Затваря главната форма, а с това и програмата.
        /// </summary>
        void ExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Събитието, което се прихваща, за да се превизуализира при изменение на модела.
        /// </summary>
        void ViewPortPaint(object sender, PaintEventArgs e)
        {
            dialogProcessor.ReDraw(sender, e);
        }

        /// <summary>
        /// Бутон, който поставя на произволно място правоъгълник със зададените размери.
        /// Променя се лентата със състоянието и се инвалидира контрола, в който визуализираме.
        /// </summary>
        void DrawRectangleSpeedButtonClick(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomRectangle();

            statusBar.Items[0].Text = "Последно действие: Рисуване на правоъгълник";

            viewPort.Invalidate();
        }

        /// <summary>
        /// Прихващане на координатите при натискането на бутон на мишката и проверка (в обратен ред) дали не е
        /// щракнато върху елемент. Ако е така то той се отбелязва като селектиран и започва процес на "влачене".
        /// Промяна на статуса и инвалидиране на контрола, в който визуализираме.
        /// Реализацията се диалогът с потребителя, при който се избира "най-горния" елемент от екрана.
        /// </summary>
        void ViewPortMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (pickUpSpeedButton.Checked)
            {
                Shape temp = dialogProcessor.ContainsPoint(e.Location);
                if (temp != null)
                {
                    if (dialogProcessor.Selection.Contains(temp))
                    {
                        dialogProcessor.Selection.Remove(temp);

                    }
                    else
                        dialogProcessor.Selection.Add(temp);
                }

                statusBar.Items[0].Text = "Последно действие: Селекция на примитив";
                dialogProcessor.IsDragging = true;
                dialogProcessor.LastLocation = e.Location;

                viewPort.Invalidate();

            }
        }

        /// <summary>
        /// Прихващане на преместването на мишката.
        /// Ако сме в режм на "влачене", то избрания елемент се транслира.
        /// </summary>
        void ViewPortMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (dialogProcessor.IsDragging)
            {
                if (dialogProcessor.Selection != null) statusBar.Items[0].Text = "Последно действие: Влачене";
                dialogProcessor.TranslateTo(e.Location);
                viewPort.Invalidate();
            }
        }

        /// <summary>
        /// Прихващане на отпускането на бутона на мишката.
        /// Излизаме от режим "влачене".
        /// </summary>
        void ViewPortMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            dialogProcessor.IsDragging = false;
        }

        /// <summary>
        /// Бутон, който поставя на произволно място елипса със зададените размери.
        /// Променя се лентата със състоянието и се инвалидира контрола, в който визуализираме.
        /// </summary>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            dialogProcessor.AddRandomEllipse();

            statusBar.Items[0].Text = "Последно действие: Рисуване на елипса";

            viewPort.Invalidate();

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

            dialogProcessor.AddRandomSquare();

            statusBar.Items[0].Text = "Последно действие: Рисуване на квадрат";

            viewPort.Invalidate();


        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {

            dialogProcessor.AddRandomCircle();

            statusBar.Items[0].Text = "Последно действие: Рисуване на кръг";

            viewPort.Invalidate();


        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {

            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                dialogProcessor.SetFillColor(colorDialog1.Color);
            }
            viewPort.Invalidate();

        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {

            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                dialogProcessor.SetStrokeColor(colorDialog1.Color);
            }
            viewPort.Invalidate();


        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {

            dialogProcessor.AddRandomTriangle();

            statusBar.Items[0].Text = "Последно действие: Рисуване на триъгълник";

            viewPort.Invalidate();


        }

        //A scroll used to set the opacity of an element
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            trackBar1.Minimum = 0;
            trackBar1.Maximum = 255;
            trackBar1.TickFrequency = 15;
            dialogProcessor.SetOpacity(trackBar1.Value);
            viewPort.Invalidate();

        }

        //A scroll used to set the stroke thickness of an element
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            trackBar2.Minimum = 1;
            trackBar2.Maximum = 15;
            dialogProcessor.SetStrokeThickness(trackBar2.Value);

            viewPort.Invalidate();

        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            dialogProcessor.GroupPrimitives();
            viewPort.Invalidate();
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            dialogProcessor.UnGroupPrimitives();
            viewPort.Invalidate();
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            string input = Interaction.InputBox("Enter a name of shape:");

            switch (input)
            {
                case "ellipse":
                    dialogProcessor.AddRandomEllipse();
                    statusBar.Items[0].Text = "Последно действие: Рисуване на елипса";
                    break;
                case "triangle":
                    dialogProcessor.AddRandomTriangle();
                    statusBar.Items[0].Text = "Последно действие: Рисуване на триъгълник";
                    break;
                case "square":
                    dialogProcessor.AddRandomSquare();
                    statusBar.Items[0].Text = "Последно действие: Рисуване на квадрагт";
                    break;
                case "circle":
                    dialogProcessor.AddRandomCircle();
                    statusBar.Items[0].Text = "Последно действие: Рисуване на кръг";
                    break;
                case "rectangle":
                    dialogProcessor.AddRandomRectangle();
                    statusBar.Items[0].Text = "Последно действие: Рисуване на правоъгълник";
                    break;
                case "елипса":
                    dialogProcessor.AddRandomEllipse();
                    statusBar.Items[0].Text = "Последно действие: Рисуване на елипса";
                    break;
                case "триъгълник":
                    dialogProcessor.AddRandomTriangle();
                    statusBar.Items[0].Text = "Последно действие: Рисуване на триъгълник";
                    break;
                case "квадрат":
                    dialogProcessor.AddRandomSquare();
                    statusBar.Items[0].Text = "Последно действие: Рисуване на квадрат";
                    break;
                case "кръг":
                    dialogProcessor.AddRandomCircle();
                    statusBar.Items[0].Text = "Последно действие: Рисуване на кръг";
                    break;
                case "правоъгълник":
                    dialogProcessor.AddRandomRectangle();
                    statusBar.Items[0].Text = "Последно действие: Рисуване на правоъгълник";
                    break;
                default:
                    break;
            }


            viewPort.Invalidate();
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            if (RotateTB.Text.Length != 0)
            {
                dialogProcessor.RotatePrimitive(int.Parse(RotateTB.ToString()));
                RotateTB.Clear();
                viewPort.Invalidate();
            }
            if (HeightTB.Text.Length != 0)
            {
                dialogProcessor.ChangeHeight(int.Parse(HeightTB.ToString()));
                HeightTB.Clear();
                viewPort.Invalidate();
            }
            if (WidthTB.Text.Length != 0)
            {
                dialogProcessor.ChangeWidth(int.Parse(WidthTB.ToString()));
                WidthTB.Clear();
                viewPort.Invalidate();
            }
        }

        private void rectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomRectangle();
            statusBar.Items[0].Text = "Последно действие: Рисуване на правоъгълник";
            viewPort.Invalidate();

        }

        private void ellipseToolStripMenuItem_Click(object sender, EventArgs e)
        {

            dialogProcessor.AddRandomEllipse();
            statusBar.Items[0].Text = "Последно действие: Рисуване на елипса";
            viewPort.Invalidate();
        }

        private void squareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomSquare();
            statusBar.Items[0].Text = "Последно действие: Рисуване на квадрат";
            viewPort.Invalidate();
        }

        private void circleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomCircle();
            statusBar.Items[0].Text = "Последно действие: Рисуване на кръг";
            viewPort.Invalidate();
        }

        private void triangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomTriangle();
            statusBar.Items[0].Text = "Последно действие: Рисуване на триъгълник";
            viewPort.Invalidate();
        }

        private List<Shape> clipboard = new List<Shape>();



        private void viewPort_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                // Delete selected shapes
                foreach (Shape shape in dialogProcessor.Selection.ToList())
                {
                    dialogProcessor.ShapeList.Remove(shape);
                }
                dialogProcessor.Selection.Clear();
            }

        }
  



        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // Set the initial directory and file name filter
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog.Filter = "PNG Files|*.png|JPEG Files|*.jpeg";

            // Show the file explorer dialog and get the result
            DialogResult dialogResult = saveFileDialog.ShowDialog();

            // Process the result
            if (dialogResult == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;

                try
                {
                    // Create a Bitmap to draw the shapes
                    Bitmap bitmap = new Bitmap(viewPort.Width, viewPort.Height);
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        // Draw the shapes onto the Bitmap
                        foreach (Shape shape in dialogProcessor.ShapeList)
                        {
                            // Check if the shape has a Color property
                            if (shape.FillColor != null && shape.StrokeColor != null)
                            {
                                // Set the shape's color before drawing
                                using (SolidBrush brush = new SolidBrush(shape.FillColor))
                                {
                                    // Draw the shape using the specified color
                                    shape.DrawSelf(graphics);
                                }
                            }
                            else
                            {
                                // Draw the shape without specifying a color
                                shape.DrawSelf(graphics);
                            }
                        }
                    }

                    // Determine the selected file format based on the file extension
                    ImageFormat imageFormat = ImageFormat.Png;
                    string fileExtension = Path.GetExtension(filePath);
                    if (string.Equals(fileExtension, ".jpeg", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(fileExtension, ".jpg", StringComparison.OrdinalIgnoreCase))
                    {
                        imageFormat = ImageFormat.Jpeg;
                    }

                    // Save the Bitmap as the selected image format
                    bitmap.Save(filePath, imageFormat);

                    statusBar.Items[0].Text = "Shapes saved to file: " + filePath;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error saving shapes to file: " + ex.Message);
                }
            }
        }


    }

}
