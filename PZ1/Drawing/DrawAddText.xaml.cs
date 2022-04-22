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
    /// Interaction logic for DrawAddText.xaml
    /// </summary>
    public partial class DrawAddText : Window
    {
        public static List<string> Color;
        public static List<string> Size;
        private double text;
        private string textColor;
        private string textSize;
        private bool draw;
        public DrawAddText()
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

            Size = new List<string>();
            Size.Add("2");
            Size.Add("4");
            Size.Add("6");
            Size.Add("8");
            Size.Add("10");

            //text = "";
            textColor = "";
            textSize = "";
            Draw = false;
            TextColorComboBox.ItemsSource = Color;
            TextSizeComboBox.ItemsSource = Size;

        }

        public bool Draw { get => draw; set => draw = value; }
        public double Text { get => text; set => text = value; }
        public string TextColor { get => textColor; set => textColor = value; }
        public string TextSize { get => textSize; set => textSize = value; }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Draw = false;
            this.Close();
        }

        private void ButtonDraw_Click(object sender, RoutedEventArgs e)
        {
            if (!Double.TryParse(TTBox.Text, out text) || TextColorComboBox.SelectedIndex == -1 || TextSizeComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Proverite da li su sva polja ispravno popunjena i pokušajte ponovo.");
            }
            else
            {
                Draw = true;
                TextColor = TextColorComboBox.SelectedItem.ToString();
                TextSize = TextSizeComboBox.SelectedItem.ToString();
          
                this.Close();
            }
        }
    
    }
}
