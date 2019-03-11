using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace GraphX
{
    class WPF_Element
    {
        /// <summary>
        /// Привязка свойства объекта к свойству другого объекта OneWay
        /// </summary>
        /// <param name="BindSource">Элемент, с которого копируем свойства</param>
        /// <param name="Element">Элемент, которому устанавливаем привязку</param>
        /// <param name="Property">Свойство, откуда берём значение</param>
        /// <param name="DP">Свойство, которому устанавливается зависимость</param>
        public static void SetBinding(object BindSource, FrameworkElement Element, string Property,
            DependencyProperty DP)
        {
            Binding binding = new Binding();
            binding.Source = BindSource;
            binding.Path = new PropertyPath(Property);
            binding.Mode = BindingMode.OneWay;
            Element.SetBinding(DP, binding);
        }
    }
}
