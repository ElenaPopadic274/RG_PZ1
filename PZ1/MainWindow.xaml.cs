using PZ1.Drawing;
using PZ1.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;

namespace PZ1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
   

    //Graf
    public struct Point  // struktura...koristim kao kljuc u recniku (dictionari)
    {
        public double x;
        public double y;
        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public partial class MainWindow : Window
    {
        //Graf
        public enum LineType
        {
            Verical, Horizontal,
        }

        //Crtanje
        public enum Shapes
        {
            Ellipse, Polygon, Text, NoShape,
        }

        //Graf
        public const double minLongitude = 19.727275;
        public const double maxLongitude = 19.950944;
        public const double minLatitude = 45.189725;
        public const double maxLatitude = 45.328735;

        //id, point Kolekcija svih cvorova sa kanvasu
        private static Dictionary<long, Model.Point> collectionOfNodes = new Dictionary<long, Model.Point>();
        //koristim za presek horizontalnih i vertikalnih linija, i ukoliko vod na zadatom mestu postoji, da se ne crta ponovo ->
        private static Dictionary<Point, LineType> horizontalLineOnPoint = new Dictionary<Point, LineType>();
        private static Dictionary<Point, LineType> verticalLineOnPoint = new Dictionary<Point, LineType>();
        public XmlEntities xmlEntities { get; set; }

        //Crtanje 
        private Shapes currShape;
        Point pointc;
        PointCollection polygonPoints = new PointCollection(); //Points for polygon
        UndoRedo undoRedo = new UndoRedo();
        private List<FrameworkElement> listAllShapes = new List<FrameworkElement>();
        public List<FrameworkElement> ListAllShapes { get => listAllShapes; set => listAllShapes = value; }

        public MainWindow()
        {
            InitializeComponent();
            this.xmlEntities = ParseXml();
            currShape = Shapes.NoShape;
        }

        //Graf
        private void Load_Grid(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadButton.IsEnabled = false;

            progressBar.Dispatcher.Invoke(() => progressBar.Value = 10, DispatcherPriority.Background);
            progressText.Text = "10 %";

            List<Ellipse> ellipses = new List<Ellipse>();

            ellipses = DrawSubstations(xmlEntities.Substations);
            foreach (var item in ellipses)
            {
                mapCanvas.Children.Add(item);
            }

            progressBar.Dispatcher.Invoke(() => progressBar.Value = 30, DispatcherPriority.Background);
            progressText.Text = "30 %";

            ellipses = DrawNodes(xmlEntities.Nodes);
            foreach (var item in ellipses)
            {
                mapCanvas.Children.Add(item);
            }

            progressBar.Dispatcher.Invoke(() => progressBar.Value = 60, DispatcherPriority.Background);
            progressText.Text = "70 %";


            ellipses = DrawSwitch(xmlEntities.Switches);
            foreach (var item in ellipses)
            {
                mapCanvas.Children.Add(item);
            }

            progressBar.Dispatcher.Invoke(() => progressBar.Value = 80, DispatcherPriority.Background);
            progressText.Text = "80 %";

            DrawLines(xmlEntities.Lines, mapCanvas);

            //Oznacavanje preseka vodova ->
            foreach (var point in horizontalLineOnPoint.Keys)
            {
                int step = 10;

                if (verticalLineOnPoint.ContainsKey(new Point(point.x, point.y)))
                {
                    Rectangle rectangle = new Rectangle();
                    rectangle.Height = 3;
                    rectangle.Width = 3;
                    rectangle.Fill = new SolidColorBrush(Color.FromRgb(66, 65, 11));
                    rectangle.SetValue(Canvas.LeftProperty, point.x + 3);
                    rectangle.SetValue(Canvas.TopProperty, point.y + 3);
                    mapCanvas.Children.Add(rectangle);
                }
            }
            progressBar.Dispatcher.Invoke(() => progressBar.Value = 100, DispatcherPriority.Background);
            progressText.Text = "100%";
        }
        public static List<Ellipse> DrawSubstations(List<SubstationEntity> substations)
        {
            List<Ellipse> substationEllipses = new List<Ellipse>();
            foreach (var item in substations)
            {
                if (collectionOfNodes.ContainsKey(item.Id))
                {
                    continue;
                }
                Ellipse ellipse = createNewEllipse(Color.FromRgb(94, 181, 54));

                var point = CreatePoint(item.Longitude, item.Latitude);

                ellipse.SetValue(Canvas.LeftProperty, point.X);
                ellipse.SetValue(Canvas.TopProperty, point.Y);
                ellipse.ToolTip = "\tSUBSTATION\nID: " + item.Id.ToString() + "\nName:" + item.Name;

                substationEllipses.Add(ellipse);
                collectionOfNodes.Add(item.Id, point);
            }
            return substationEllipses;
        }

        public static List<Ellipse> DrawNodes(List<NodeEntity> nodes)
        {
            List<Ellipse> nodeEllipses = new List<Ellipse>();
            foreach (var item in nodes)
            {
                if (collectionOfNodes.ContainsKey(item.Id))
                {
                    continue;
                }
                Ellipse elipse = createNewEllipse(Color.FromRgb(54, 175, 181));

                var point = CreatePoint(item.Longitude, item.Latitude);

                elipse.SetValue(Canvas.LeftProperty, point.X);
                elipse.SetValue(Canvas.TopProperty, point.Y);
                elipse.ToolTip = "\tNODE\nID: " + item.Id.ToString() + "\nName: " + item.Name;

                collectionOfNodes.Add(item.Id, point);
                nodeEllipses.Add(elipse);
            }
            return nodeEllipses;
        }

        public static List<Ellipse> DrawSwitch(List<SwitchEntity> switches)
        {
            List<Ellipse> switchEllipss = new List<Ellipse>();
            foreach (var item in switches)
            {
                if (collectionOfNodes.ContainsKey(item.Id))
                {
                    continue;
                }
                Ellipse elipse = createNewEllipse(Color.FromRgb(141, 54, 181));

                var point = CreatePoint(item.Longitude, item.Latitude);

                elipse.SetValue(Canvas.LeftProperty, point.X);
                elipse.SetValue(Canvas.TopProperty, point.Y);
                elipse.ToolTip = "\tSWITCH \nID: " + item.Id.ToString() + "\nName: " + item.Name + "\nStatus: " + item.Status;

                collectionOfNodes.Add(item.Id, point);
                switchEllipss.Add(elipse);
            }
            return switchEllipss;
        }

        public static void DrawLines(List<Model.LineEntity> lines, Canvas canvas)
        {
            Model.Point startNode = new Model.Point();
            Model.Point EndNode = new Model.Point();

            Model.Point currPoint = new Model.Point();
            Model.Point prevPoint = new Model.Point();

            foreach (var item in lines)
            {
                if (!collectionOfNodes.ContainsKey(item.FirstEnd) || !collectionOfNodes.ContainsKey(item.SecondEnd))
                {
                    continue;
                }
                startNode = collectionOfNodes[item.FirstEnd];
                EndNode = collectionOfNodes[item.SecondEnd];

                prevPoint.X = currPoint.X = startNode.X;
                prevPoint.Y = currPoint.Y = startNode.Y;

                int step = (currPoint.X > EndNode.X) ? -10 : 10; // razmak izmedju dva x-a na gridu je 10, zbog toga i korak -10 ili +10

                while (currPoint.X != EndNode.X) //crtamo po x
                {
                    currPoint.X += step;
                    if (!horizontalLineOnPoint.ContainsKey(new Point(currPoint.X, currPoint.Y)) || currPoint.X == EndNode.X)
                    {
                        Line l1 = createLine(prevPoint, currPoint);
                        l1.ToolTip = "\tLINE \nID: " + item.Id.ToString() + "\nName: " + item.Name + "\nFirstEnd: " + item.FirstEnd.ToString() + "\nSecondEnd: " + item.SecondEnd.ToString();
                        canvas.Children.Add(l1);
                        horizontalLineOnPoint[new Point(currPoint.X, currPoint.Y)] = LineType.Horizontal;
                    }
                    prevPoint.X = currPoint.X;
                }
                step = (currPoint.Y > EndNode.Y) ? -10 : 10; // razmak izmedju dva y-a na gridu je 10, zbog toga i korak -10 ili +10
                while (currPoint.Y != EndNode.Y) //crtamo po y
                {
                    currPoint.Y += step;
                    if (!verticalLineOnPoint.ContainsKey(new Point(currPoint.X, currPoint.Y)) || currPoint.Y == EndNode.Y)
                    {
                        Line l1 = createLine(prevPoint, currPoint);
                        l1.ToolTip = "\tLINE \nID: " + item.Id.ToString() + "\nName: " + item.Name + "\nFirstEnd: " + item.FirstEnd.ToString() + "\nSecondEnd: " + item.SecondEnd.ToString();
                        canvas.Children.Add(l1);
                        verticalLineOnPoint[new Point(currPoint.X, currPoint.Y)] = LineType.Verical;
                    }
                    prevPoint.Y = currPoint.Y;
                }
            }
        }

        private static Line createLine(Model.Point point1, Model.Point point2)
        {
            Line line = new Line();
            line.Stroke = new SolidColorBrush(Color.FromRgb(71, 252, 58));

            line.X1 = point1.X + 4.5;
            line.Y1 = point1.Y + 4.5;
            line.X2 = point2.X + 4.5;
            line.Y2 = point2.Y + 4.5;

            line.StrokeThickness = 1.5;

            return line;
        }

        private static Ellipse createNewEllipse(Color color)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Height = 9;
            ellipse.Width = 9;
            ellipse.Fill = new SolidColorBrush(color);
            return ellipse;
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Shape clickedShape = e.OriginalSource as Shape;
            if (clickedShape != null && clickedShape.GetType().Name.ToString() == "Ellipse")
            {
                DoubleAnimation widthAnimation = new DoubleAnimation
                {
                    From = 9,
                    To = 90,
                    Duration = TimeSpan.FromSeconds(1.5)
                };

                DoubleAnimation heightAnimation = new DoubleAnimation
                {
                    From = 9,
                    To = 90,
                    Duration = TimeSpan.FromSeconds(1.5)
                };

                mapCanvas.Children.Remove(clickedShape); //
                mapCanvas.Children.Add(clickedShape);    // ova i prethodna linija čisto da se objekat iscrta ispred svih drugih objekata....

                Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(Ellipse.WidthProperty));
                Storyboard.SetTarget(widthAnimation, clickedShape);

                Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(Ellipse.HeightProperty));
                Storyboard.SetTarget(heightAnimation, clickedShape);

                Storyboard s = new Storyboard();
                s.Children.Add(widthAnimation);
                s.Children.Add(heightAnimation);

                s.Completed += (t, r) => StoryboardCompleted(clickedShape);
                s.Begin();

            }
            else if (clickedShape != null && clickedShape.GetType().Name.ToString() == "Line")
            {
                if (System.Windows.Forms.MessageBox.Show("Oboji čvorove povezane ovim vodom drugacijom bojom od ostalih", "", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    string[] tool = clickedShape.ToolTip.ToString().Split('\n');

                    double FirstNodeId = double.Parse((tool[3].Split(':'))[1]);
                    double SecoundNodeId = double.Parse((tool[4].Split(':'))[1]);
                    Random rnd = new Random();
                    Color randomColor = Color.FromRgb(250, Convert.ToByte(rnd.Next(0, 250)), 0);
                    foreach (var node in mapCanvas.Children)
                    {
                        Shape ellipse = node as Shape;
                        if (ellipse.GetType().Name.ToString() == "Ellipse")
                        {
                            double nodeID = double.Parse(ellipse.ToolTip.ToString().Split('\n')[1].Split(':')[1]);

                            if (nodeID == FirstNodeId)
                            {
                                ellipse.Fill = new SolidColorBrush(randomColor);
                            }
                            else if (nodeID == SecoundNodeId)
                            {
                                ellipse.Fill = new SolidColorBrush(randomColor);
                            }
                        }
                    }
                }
            }
            //Crtanje
            if (currShape == Shapes.Polygon)
            {
                DrawAPolygon drawAPolygon = new DrawAPolygon();
                drawAPolygon.ShowDialog();
                if (drawAPolygon.Draw)
                {
                    DrawPolygon(drawAPolygon.FillColor, drawAPolygon.BorderColor, drawAPolygon.PolygonBorderThickness);
                    drawAPolygon.Draw = false;
                }
                else
                {
                    // polygonPoints.Clear(); 
                    polygonPoints = new PointCollection();
                }
                currShape = Shapes.NoShape;
            }
            else
            {
                if (clickedShape != null)
                {
                    ChangeShapeColor changeShapeColor = new ChangeShapeColor();
                    changeShapeColor.BorderThicknessTextBox.Text = clickedShape.StrokeThickness.ToString();
                    changeShapeColor.ShowDialog();
                    if (changeShapeColor.ApplyChange)
                    {
                        listAllShapes.Remove(clickedShape);
                        // Shape tempShape = clickedShape;
                        clickedShape.Fill = getColor(changeShapeColor.FillColor);
                        clickedShape.Stroke = getColor(changeShapeColor.BorderColor);
                        clickedShape.StrokeThickness = changeShapeColor.ShapeBorderThickness;
                        //PaintCanvas.Children.Remove(clickedShape);
                        // PaintCanvas.Children.Add(tempShape);  
                        listAllShapes.Add(clickedShape);
                    }
                }
            }
        }

        private void StoryboardCompleted(Shape e)
        {
            DoubleAnimation myDoubleAnimation2 = new DoubleAnimation();
            myDoubleAnimation2.From = 90;
            myDoubleAnimation2.To = 9;
            myDoubleAnimation2.Duration = new Duration(TimeSpan.FromSeconds(1.5));
            e.BeginAnimation(Ellipse.WidthProperty, myDoubleAnimation2);
            e.BeginAnimation(Ellipse.HeightProperty, myDoubleAnimation2);
        }

        private static Model.Point CreatePoint(double longitude, double latitude)
        {
            double ValueoOfOneLongitude = (maxLongitude - minLongitude) / 2200; //pravimo 2200delova (Longituda) jer nam je canvas 2200x2200 
            double ValueoOfOneLatitude = (maxLatitude - minLatitude) / 2200;  //isto kao gore

            double x = Math.Round((longitude - minLongitude) / ValueoOfOneLongitude); // koliko longituda stane u rastojanje izmedju trenutne i minimalne longitude
            double y = Math.Round((maxLatitude - latitude) / ValueoOfOneLatitude);

            x = x - x % 10; // zaokruzi na prvi broj deljiv sa 10, toliko ce nam biti rastojanje izmedju dva susedna x
            y = y - y % 10; // zaokruzi na prvi broj deljiv sa 10,, toliko ce nam biti rastojanje izmedju dva susedna y

            int cout = 0;

            while (true)
            {
                if (collectionOfNodes.Any(z => z.Value.X == x && z.Value.Y == y))
                {
                    if (cout == 0)
                    {
                        x += 10;
                        cout++;
                        continue;
                    }
                    else if (cout == 1)
                    {
                        x -= 20;
                        cout++;
                        continue;
                    }
                    else if (cout == 2)
                    {
                        x += 10; //vraxamo x na pocetnu vrednost
                        y += 10;
                        cout++;
                        continue;
                    }
                    else if (cout == 3)
                    {
                        y -= 20;
                        cout++;
                        continue;
                    }
                    else if (cout == 4)
                    {
                        x += 10;
                        cout++;
                        continue;
                    }
                    else if (cout == 5)
                    {
                        x -= 20;
                        cout++;
                        continue;
                    }
                    else if (cout == 6)
                    {
                        y += 20;
                        cout++;
                        continue;
                    }
                    else if (cout == 6)
                    {
                        x += 20;
                        cout++;
                        continue;
                    }
                    else
                    {
                        cout = 0;
                        continue;
                    }
                }
                else
                {
                    break;
                }
            }
            return new Model.Point
            {
                X = x,
                Y = y,
            };
        }

        public static XmlEntities ParseXml()
        {
            var filename = "Geographic.xml";
            var currentDirectory = Directory.GetCurrentDirectory();
            var purchaseOrderFilepath = System.IO.Path.Combine(currentDirectory, filename);

            StringBuilder result = new StringBuilder();

            //Load xml
            XDocument xdoc = XDocument.Load(filename);

            //Run query
            var substations = xdoc.Descendants("SubstationEntity")
                     .Select(sub => new SubstationEntity
                     {
                         Id = (long)sub.Element("Id"),
                         Name = (string)sub.Element("Name"),
                         X = (double)sub.Element("X"),
                         Y = (double)sub.Element("Y"),
                     }).ToList();

            double longit = 0;
            double latid = 0;

            foreach (var item in substations)
            {
                ToLatLon(item.X, item.Y, 34, out latid, out longit);
                item.Latitude = latid;
                item.Longitude = longit;
            }

            var nodes = xdoc.Descendants("NodeEntity")
                     .Select(node => new NodeEntity
                     {
                         Id = (long)node.Element("Id"),
                         Name = (string)node.Element("Name"),
                         X = (double)node.Element("X"),
                         Y = (double)node.Element("Y"),
                     }).ToList();

            foreach (var item in nodes)
            {
                ToLatLon(item.X, item.Y, 34, out latid, out longit);
                item.Latitude = latid;
                item.Longitude = longit;
            }

            var switches = xdoc.Descendants("SwitchEntity")
                     .Select(sw => new SwitchEntity
                     {
                         Id = (long)sw.Element("Id"),
                         Name = (string)sw.Element("Name"),
                         Status = (string)sw.Element("Status"),
                         X = (double)sw.Element("X"),
                         Y = (double)sw.Element("Y"),
                     }).ToList();

            foreach (var item in switches)
            {
                ToLatLon(item.X, item.Y, 34, out latid, out longit);
                item.Latitude = latid;
                item.Longitude = longit;
            }

            var lines = xdoc.Descendants("LineEntity")
                     .Select(line => new LineEntity
                     {
                         Id = (long)line.Element("Id"),
                         Name = (string)line.Element("Name"),
                         ConductorMaterial = (string)line.Element("ConductorMaterial"),
                         IsUnderground = (bool)line.Element("IsUnderground"),
                         R = (float)line.Element("R"),
                         FirstEnd = (long)line.Element("FirstEnd"),
                         SecondEnd = (long)line.Element("SecondEnd"),
                         LineType = (string)line.Element("LineType"),
                         ThermalConstantHeat = (long)line.Element("ThermalConstantHeat"),
                         Vertices = line.Element("Vertices").Descendants("Point").Select(p => new Model.Point
                         {
                             X = (double)p.Element("X"),
                             Y = (double)p.Element("Y"),
                         }).ToList()
                     }).ToList();

            foreach (var line in lines)
            {
                foreach (var point in line.Vertices)
                {
                    ToLatLon(point.X, point.Y, 34, out latid, out longit);
                    point.Latitude = latid;
                    point.Longitude = longit;
                }
            }
            return new XmlEntities { Substations = substations, Switches = switches, Nodes = nodes, Lines = lines };
        }


        //From UTM to Latitude and longitude in decimal
        public static void ToLatLon(double utmX, double utmY, int zoneUTM, out double latitude, out double longitude)
        {
            bool isNorthHemisphere = true;

            var diflat = -0.00066286966871111111111111111111111111;
            var diflon = -0.0003868060578;

            var zone = zoneUTM;
            var c_sa = 6378137.000000;
            var c_sb = 6356752.314245;
            var e2 = Math.Pow((Math.Pow(c_sa, 2) - Math.Pow(c_sb, 2)), 0.5) / c_sb;
            var e2cuadrada = Math.Pow(e2, 2);
            var c = Math.Pow(c_sa, 2) / c_sb;
            var x = utmX - 500000;
            var y = isNorthHemisphere ? utmY : utmY - 10000000;

            var s = ((zone * 6.0) - 183.0);
            var lat = y / (c_sa * 0.9996);
            var v = (c / Math.Pow(1 + (e2cuadrada * Math.Pow(Math.Cos(lat), 2)), 0.5)) * 0.9996;
            var a = x / v;
            var a1 = Math.Sin(2 * lat);
            var a2 = a1 * Math.Pow((Math.Cos(lat)), 2);
            var j2 = lat + (a1 / 2.0);
            var j4 = ((3 * j2) + a2) / 4.0;
            var j6 = ((5 * j4) + Math.Pow(a2 * (Math.Cos(lat)), 2)) / 3.0;
            var alfa = (3.0 / 4.0) * e2cuadrada;
            var beta = (5.0 / 3.0) * Math.Pow(alfa, 2);
            var gama = (35.0 / 27.0) * Math.Pow(alfa, 3);
            var bm = 0.9996 * c * (lat - alfa * j2 + beta * j4 - gama * j6);
            var b = (y - bm) / v;
            var epsi = ((e2cuadrada * Math.Pow(a, 2)) / 2.0) * Math.Pow((Math.Cos(lat)), 2);
            var eps = a * (1 - (epsi / 3.0));
            var nab = (b * (1 - epsi)) + lat;
            var senoheps = (Math.Exp(eps) - Math.Exp(-eps)) / 2.0;
            var delt = Math.Atan(senoheps / (Math.Cos(nab)));
            var tao = Math.Atan(Math.Cos(delt) * Math.Tan(nab));

            longitude = ((delt * (180.0 / Math.PI)) + s) + diflon;
            latitude = ((lat + (1 + e2cuadrada * Math.Pow(Math.Cos(lat), 2) - (3.0 / 2.0) * e2cuadrada * Math.Sin(lat) * Math.Cos(lat) * (tao - lat)) * (tao - lat)) * (180.0 / Math.PI)) + diflat;
        }


        //Crtanje
        private void EllipseButton_Click(object sender, RoutedEventArgs e)
        {
            currShape = Shapes.Ellipse;
        }

        private void TextButton_Click(object sender, RoutedEventArgs e)
        {
            currShape = Shapes.Text;
        }

        private void PolygonButton_Click(object sender, RoutedEventArgs e)
        {
            currShape = Shapes.Polygon;
            polygonPoints = new PointCollection();
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            undoRedo.Undo(this);
        }

        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            undoRedo.Redo(this);
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            if (mapCanvas.Children.Count != 0)
            {
                undoRedo.InsertAllShapeforUndoRedo(ListAllShapes);
                ListAllShapes.Clear();
                mapCanvas.Children.Clear();
            }
            currShape = Shapes.NoShape;
        }

        private void mapCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            pointc = e.GetPosition(this);
            switch (currShape)
            {
                case Shapes.NoShape:
                    System.Windows.MessageBox.Show("Niste izabrali oblik!", "Greška");
                    break;

                case Shapes.Ellipse:
                    DrawAEllipse drawAEllipse = new DrawAEllipse();
                    drawAEllipse.ShowDialog();
                    if (drawAEllipse.Draw)
                    {
                        DrawEllipse(drawAEllipse.EllipseWidth, drawAEllipse.EllipseHeight, drawAEllipse.FillColor, drawAEllipse.BorderColor, drawAEllipse.EllipseBorderThickness);
                    }
                    break;

                case Shapes.Text:
                    DrawAddText drawAddText = new DrawAddText();
                    drawAddText.ShowDialog();
                    if (drawAddText.Draw)
                    {
                        DrawAddText(drawAddText.RectangleWidth, drawAddText.RectangleHeight, drawAddText.FillColor, drawARectangle.BorderColor, drawAddText.RectangleBorderThickness);
                    }
                    break;

                case Shapes.Polygon:
                    pointc.y = pointc.y - 45;
                    polygonPoints.Add(pointc);
                    break;

                default:
                    return;
            }
        }

        private void DrawEllipse(double width,double height,string fillcolor,string bordercolor,double borderthickness)
        {
            Ellipse newEllipse = new Ellipse();
          
            newEllipse.SetValue(Canvas.LeftProperty, pointc.x);
            newEllipse.SetValue(Canvas.TopProperty, pointc.y - 45);
            newEllipse.Width = width;
            newEllipse.Height = height;
            newEllipse.Fill = getColor(fillcolor);
            newEllipse.Stroke = getColor(bordercolor);
            newEllipse.StrokeThickness = borderthickness;
           
            mapCanvas.Children.Add(newEllipse);
            ListAllShapes.Add(newEllipse);
            currShape = Shapes.NoShape;

            undoRedo.InsertShapeforUndoRedo(newEllipse);
        }
        /*
        private void DrawRectangle(double width, double height, string fillcolor, string bordercolor, double borderthickness)
        {
            Rectangle newRectangle = new Rectangle();
            
            newRectangle.SetValue(Canvas.LeftProperty, point.X);
            newRectangle.SetValue(Canvas.TopProperty, point.Y - 45);
            newRectangle.Width = width;
            newRectangle.Height = height;
            newRectangle.Fill = getColor(fillcolor);
            newRectangle.Stroke = getColor(bordercolor);
            newRectangle.StrokeThickness = borderthickness;

            mapCanvas.Children.Add(newRectangle);
            ListAllShapes.Add(newRectangle);
            currShape = Shapes.NoShape;
           
            undoRedo.InsertShapeforUndoRedo(newRectangle);
        }
        */
        private void DrawAddText()
        { 
        
        }

        private void DrawPolygon(string fillcolor, string bordercolor, double borderthickness)
        {
            Polygon newPolygon = new Polygon();

            newPolygon.Fill = getColor(fillcolor);
            newPolygon.Stroke = getColor(bordercolor);
            newPolygon.StrokeThickness = borderthickness;
            newPolygon.Points = polygonPoints;

            mapCanvas.Children.Add(newPolygon);
            ListAllShapes.Add(newPolygon);
            //polygonPoints.Clear();
            polygonPoints = new PointCollection();
            currShape = Shapes.NoShape;
           
            undoRedo.InsertShapeforUndoRedo(newPolygon);
        }

        private SolidColorBrush getColor(string color)
        {
            SolidColorBrush sb = null;

            switch(color)
            {
                case "Red":
                    sb = new SolidColorBrush(Colors.Red);
                    break;
                    
                case "Blue":
                    sb = new SolidColorBrush(Colors.Blue);
                    break;

                case "Green":
                    sb = new SolidColorBrush(Colors.Green);
                    break;

                case "Yelow":
                    sb = new SolidColorBrush(Colors.Yellow);
                    break;

                case "Pink":
                    sb = new SolidColorBrush(Colors.Pink);
                    break;

                case "Gray":
                    sb = new SolidColorBrush(Colors.Gray);
                    break;

                case "Brown":
                    sb = new SolidColorBrush(Colors.Brown);
                    break;

                case "White":
                    sb = new SolidColorBrush(Colors.White);
                    break;

                case "Black":
                    sb = new SolidColorBrush(Colors.Black);
                    break;
            }
            return sb;
        }
    }
}
