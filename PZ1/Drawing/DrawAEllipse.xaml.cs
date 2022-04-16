using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PZ1.Drawing
{
    /// <summary>
    /// Interaction logic for DrawAEllipse.xaml
    /// </summary>
    public partial class DrawAEllipse : Window
    {
        public static List<string> Color;
        private string fillColor;
        private string borderColor;
        private double ellipseheight;
        private double ellipsewidth;
        private double ellipseborderThickness;
        private bool draw;
        public DrawAEllipse()
        {
            InitializeComponent();
            Color = new List<string>();
            Color.Add("Red");
            Color.Add("Blue");
            Color.Add("Green");
            Color.Add("Yelow");
            Color.Add("Pink");
            Color.Add("Gray");
            Color.Add("Brown");
            Color.Add("White");
            Color.Add("Black");
            FillColor = "";
            BorderColor = "";
            Draw = false;
            FillColorComboBox.ItemsSource = Color;
            BorderColorComboBox.ItemsSource = Color;
        }

        public string FillColor { get => fillColor; set => fillColor = value; }
        public string BorderColor { get => borderColor; set => borderColor = value; }
        public double EllipseHeight { get => ellipseheight; set => ellipseheight = value; }
        public double EllipseWidth { get => ellipsewidth; set => ellipsewidth = value; }
        public double EllipseBorderThickness { get => ellipseborderThickness; set => ellipseborderThickness = value; }
        public bool Draw { get => draw; set => draw = value; }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Draw = false;
            this.Close();
        }

        private void ButtonDraw_Click(object sender, RoutedEventArgs e)
        {
            if (!Double.TryParse(WidthTextBox.Text, out ellipsewidth) || !Double.TryParse(HeightTextBox.Text, out ellipseheight) || !Double.TryParse(BorderThicknessTextBox.Text, out ellipseborderThickness) || BorderColorComboBox.SelectedIndex == -1 || FillColorComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Proverite da li su sva polja ispravno popunjena i pokušajte ponovo.");
            }
            else
            {
                Draw = true;
                BorderColor = BorderColorComboBox.SelectedItem.ToString();
                FillColor = FillColorComboBox.SelectedItem.ToString();
                //EllipseWidth = double.Parse(WidthTextBox.Text);
                //EllipseHeight = double.Parse(HeightTextBox.Text);
                //EllipseBorderThickness = double.Parse(BorderThicknessTextBox.Text);
                this.Close();
            }
        }
    }
}
