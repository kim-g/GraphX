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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphiX
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Core.Background = new SolidColorBrush(Colors.Black);

            Core.AddToShapes(new Grid()
            {
                Background = new SolidColorBrush(Colors.Gray),
                Margin = new Thickness(10)
            }, "", "WorkOld");

            Core.AddToShapes(new Rectangle()
            {
                Fill = new SolidColorBrush(Colors.Transparent),
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            }, "WorkOld");

            Core.AddToShapes(new Grid()
            {
                Background = new SolidColorBrush(Colors.Gray),
                Margin = new Thickness(10)
            }, "", "Work");

            Core.AddToShapes(new Rectangle()
            {
                Fill = new SolidColorBrush(Colors.Transparent),
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            }, "Work");

            Core.DeleteElement("WorkOld");

            //MyList.Items.Clear();
            MyList.Items.Add("Первый");
            MyList.Items.Add("Второй");
            MyList.Items.Add("Третий");
            MyList.Items.Add("Четвёртый");
        }

        private void Core_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void Down_Click(object sender, RoutedEventArgs e)
        {
            MyList.SelectedIndex++;
        }

        private void Up_Click(object sender, RoutedEventArgs e)
        {
            MyList.SelectedIndex--;
        }
    }
}
