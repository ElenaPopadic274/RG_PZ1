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
    /// Interaction logic for DrawAPolygon.xaml
    /// </summary>
    public partial class DrawAPolygon : Window
    {

        public static List<string> Color;
        private string fillColor;
        private string borderColor;
        private double polygonBorderThickness;
        private bool draw;

        public DrawAPolygon()
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
        public double PolygonBorderThickness { get => polygonBorderThickness; set => polygonBorderThickness = value; }
        public bool Draw { get => draw; set => draw = value; }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Draw = false;
            this.Close();
        }

        private void ButtonDraw_Click(object sender, RoutedEventArgs e)
        {
            if (!Double.TryParse(BorderThicknessTextBox.Text, out polygonBorderThickness) || BorderColorComboBox.SelectedIndex == -1 || FillColorComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Proverite da li su sva polja ispravno popunjena i pokušajte ponovo.");
            }
            else
            {
                Draw = true;
                BorderColor = BorderColorComboBox.SelectedItem.ToString();
                FillColor = FillColorComboBox.SelectedItem.ToString();
                // PolygonBorderThickness = double.Parse(BorderThicknessTextBox.Text);
                this.Close();
            }
        }

    }
}
