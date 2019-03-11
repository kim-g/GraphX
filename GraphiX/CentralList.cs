using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphiX
{
    class CentralList : Grid
    {
        private Rectangle BackgroundRect = new Rectangle() { };

        // Свойства
        /// <summary>
        /// Цвет фона элемента
        /// </summary>
        public Brush Color
        {
            get { return BackgroundRect.Fill; }
            set { BackgroundRect.Fill = value; }
        }

        public Color Stroke
        {
            get { return ((SolidColorBrush)(BackgroundRect.Stroke)).Color; }
            set { BackgroundRect.Stroke = new SolidColorBrush(value); }
        }
    }
}
