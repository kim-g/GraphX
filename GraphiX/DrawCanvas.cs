using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphiX
{
    /// <summary>
    /// Grid с возможностью рисования объектов на нём.
    /// </summary>
    class DrawCanvas : Grid
    {
        UInt64 index = 0;
        
        // Массив элементов
        private Dictionary<string, FrameworkElement> Shapes = new Dictionary<string, FrameworkElement>();

        /// <summary>
        /// Выдаёт элемент по его имени
        /// </summary>
        /// <param name="ElementName">Имя элемента</param>
        /// <returns></returns>
        public FrameworkElement Element(string ElementName)
        {
            return ElementName == "" 
                ? null
                : Shapes[ElementName];
        }

        /// <summary>
        /// Проверяет наличие элемента
        /// </summary>
        /// <param name="ElementName"></param>
        /// <returns></returns>
        public bool ElementExists(string ElementName)
        {
            return Shapes.ContainsKey(ElementName);
        }

        /// <summary>
        /// Создаёт имя по-умолчанию для элемента
        /// </summary>
        /// <param name="element">Элемент</param>
        /// <returns></returns>
        private string NewName(FrameworkElement element)
        {
            string[] TypeN = element.GetType().ToString().Split('.');
            return TypeN[TypeN.Count()-1] + $"_{++index}";
        }

        /// <summary>
        /// Добавляет элемент в список и родителю. При отсутствии имени создаёт его. Выдаёт имя
        /// </summary>
        /// <param name="element">Объект для добавления</param>
        /// <param name="Parent">Элемент, на который добавляется новый элемент</param>
        /// <param name="ElementName">Имя добавляемого элемента</param>
        public string AddToShapes(FrameworkElement element, Panel Parent=null, string ElementName = null)
        {
            string ElN = ElementName == null
                ? NewName(element)
                : ElementName;
            while (ElementExists(ElN))
            {
                ElN = NewName(element);
            }

            Shapes.Add(ElN, element);
            element.Name = ElN;
            if (Parent == null)
                this.Children.Add(element);
            else
                Parent.Children.Add(element);
            return ElN;
        }

        /// <summary>
        /// Добавляет элемент в список и родителю. При отсутствии имени создаёт его. Выдаёт имя
        /// </summary>
        /// <param name="element">Объект для добавления</param>
        /// <param name="ElementName">Имя добавляемого элемента</param>
        public string AddToShapes(FrameworkElement element, string Parent, string ElementName = null)
        {
            return AddToShapes(element, (Panel)(Element(Parent)), ElementName);
        }



        /// <summary>
        /// Проверяет, может ли элемент принимать дочерние элементы
        /// </summary>
        /// <param name="elenment"></param>
        /// <returns></returns>
        public bool IsPanel(string elenment = null)
        {
            if (elenment == null) return true;
            return Element(elenment) is Panel;
        }

        /// <summary>
        /// Выдаёт список всех дочерних элементов первого поколения
        /// </summary>
        /// <param name="ElementName"></param>
        /// <returns></returns>
        public string[] ElementChildren(string ElementName = null)
        {
            if (!IsPanel(ElementName)) return null;

            List<string> Names = new List<string>();
            Panel Core = ElementName == null
                ? this
                : (Panel)Element(ElementName);

            foreach (FrameworkElement X in Core.Children)
                Names.Add(X.Name);

            return Names.ToArray();
        }

        /// <summary>
        /// Выдаёт список всех дочерних элементов свех поколений
        /// </summary>
        /// <param name="ElementName"></param>
        /// <returns></returns>
        public string[] ElementAllChildren(string ElementName = null)
        {
            if (!IsPanel(ElementName)) return null;

            List<string> Names = new List<string>();
            Panel Core = ElementName == null
                ? this
                : (Panel)Element(ElementName);

            AllChildren(Core, Names);

            return Names.ToArray();
        }

        /// <summary>
        /// Рекурсивно получает имена всех элементов в список
        /// </summary>
        /// <param name="element">Элемент-родитель</param>
        /// <param name="Names">Список элементов</param>
        private void AllChildren(Panel element, List<string> Names)
        {

            foreach (FrameworkElement X in element.Children)
            {
                Names.Add(X.Name);
                if (X is Panel)
                    AllChildren((Panel)X, Names);
            }
        }

        /// <summary>
        /// Рекурсивное удаление элемента и всех его потомков
        /// </summary>
        /// <param name="element">Имя удаляемого элемента</param>
        public void DeleteElement(string element)
        {
            FrameworkElement El = Element(element);
            DeleteElement(El);
        }

        /// <summary>
        /// Рекурсивное удаление элемента и всех его потомков
        /// </summary>
        /// <param name="element">Удаляемый элемент</param>
        public void DeleteElement(FrameworkElement element)
        {
            Shapes.Remove(element.Name);
            Panel Par = (Panel)(element.Parent);
            Par.Children.Remove(element);

            if (element is Panel)
            {
                List<string> ElementsToDelete = new List<string>();
                foreach (FrameworkElement X in ((Panel)element).Children)
                    ElementsToDelete.Add(X.Name);
                foreach (string X in ElementsToDelete)
                    DeleteElement(X);
            }
        }

        ///////////// Создание стандартных элементов /////////////

        public string Rectangle(string name, Brush fill, Brush stroke, Thickness margin, double stroke_width = 1, 
            int column=0, int column_span=1, int row=0, int row_span=0)
        {
            Rectangle Rect = new Rectangle()
            {
                Fill = fill,
                Stroke = stroke,
                StrokeThickness = stroke_width,
                Margin = margin
            };

            SetColumn(Rect, column);
            SetColumnSpan(Rect, column_span);
            SetRow(Rect, row);
            SetRowSpan(Rect, row_span);

            return AddToShapes(Rect);
        }
    }
}
