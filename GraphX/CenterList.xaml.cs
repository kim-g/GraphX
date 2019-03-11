using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphX
{
    /// <summary>
    /// Логика взаимодействия для CenterList.xaml
    /// </summary>
    public partial class CenterList : UserControl
    {
        private int ElementsUp;
        private double ElementHeight;
        private double TextHeight;
        private Label[] Labels;
        private double OldHeight = 0;
        private int selectedindex = 0;
        private bool MousePressed = false;
        private Label PressedLabel;

        // Чётные элементы
        private Brush _EvenItemBackground = new SolidColorBrush(Colors.Transparent);
        private Brush _EvenItemForeground = new SolidColorBrush(Colors.Black);

        // Нечётные элементы
        private Brush _OddItemBackground = new SolidColorBrush(Colors.Transparent);
        private Brush _OddItemForeground = new SolidColorBrush(Colors.Black);

        // Центральный элемент
        private Brush _CentralItemBackground = new SolidColorBrush(Colors.Blue);
        private Brush _CentralItemForeground = new SolidColorBrush(Colors.White);

        // Выделенный элемент
        private Brush _SelectedItemBackground = new SolidColorBrush(Colors.Yellow);
        private Brush _SelectedItemForeground = new SolidColorBrush(Colors.Black);

        // Выделенный элемент
        private Brush _PressedItemBackground;
        private Brush _PressedItemForeground;

        // Список элементов
        private EventList<string> _items = new EventList<string>();

        /// <summary>
        /// Фоновый цвет чётных элементов
        /// </summary>
        public Brush EvenItemBackground
        {
            get { return _EvenItemBackground; }
            set
            {
                _EvenItemBackground = value;
                if (Labels == null) return;
                foreach (Label L in Labels)
                {
                    if ((((int)(L.Tag) & 1) == 0) && ((int)(L.Tag) != 0))
                        L.Background = _EvenItemBackground;
                }
            }
        }

        /// <summary>
        /// Цвет текста чётных элементов
        /// </summary>
        public Brush EvenItemForeground
        {
            get { return _EvenItemForeground; }
            set
            {
                _EvenItemForeground = value;
                if (Labels == null) return;
                foreach (Label L in Labels)
                {
                    if ((((int)(L.Tag) & 1) == 0) && ((int)(L.Tag) != 0))
                        L.Foreground = _EvenItemForeground;
                }
            }
        }

        /// <summary>
        /// Фоновый цвет нечётных элементов
        /// </summary>
        public Brush OddItemBackground
        {
            get { return _OddItemBackground; }
            set
            {
                _OddItemBackground = value;
                if (Labels == null) return;
                foreach (Label L in Labels)
                {
                    if ((((int)(L.Tag) & 1) != 0))
                        L.Background = _OddItemBackground;
                }
            }
        }

        /// <summary>
        /// Цвет текста нечётных элементов
        /// </summary>
        public Brush OddItemForeground
        {
            get { return _OddItemForeground; }
            set
            {
                _OddItemForeground = value;
                if (Labels == null) return;
                foreach (Label L in Labels)
                {
                    if ((((int)(L.Tag) & 1) != 0))
                        L.Foreground = _OddItemForeground;
                }
            }
        }

        /// <summary>
        /// Фоновый цвет центрального элемента
        /// </summary>
        public Brush CentralItemBackground
        {
            get { return _CentralItemBackground; }
            set
            {
                _CentralItemBackground = value;
                if (Labels == null) return;
                foreach (Label L in Labels)
                {
                    if (((int)(L.Tag) == 0))
                        L.Background = _CentralItemBackground;
                }
            }
        }

        /// <summary>
        /// Цвет текста центрального элемента
        /// </summary>
        public Brush CentralItemForeground
        {
            get { return _CentralItemForeground; }
            set
            {
                _CentralItemForeground = value;
                if (Labels == null) return;
                foreach (Label L in Labels)
                {
                    if (((int)(L.Tag) == 0))
                        L.Foreground = _CentralItemForeground;
                }
            }
        }

        /// <summary>
        /// Фоновый цвет выделенного элемента
        /// </summary>
        public Brush SelectedItemBackground
        {
            get { return _SelectedItemBackground; }
            set
            {
                _SelectedItemBackground = value;
                if (Labels == null) return;
                foreach (Label L in Labels)
                {
                    if (((int)(L.Tag) == 0))
                        L.Background = _SelectedItemBackground;
                }
            }
        }

        /// <summary>
        /// Цвет текста выделенного элемента
        /// </summary>
        public Brush SelectedItemForeground
        {
            get { return _SelectedItemForeground; }
            set
            {
                _SelectedItemForeground = value;
                if (Labels == null) return;
                foreach (Label L in Labels)
                {
                    if (((int)(L.Tag) == 0))
                        L.Foreground = _SelectedItemForeground;
                }
            }
        }

        public string Text
        {
            get { return TempLabel.Content.ToString(); }
            set { TempLabel.Content = value; }
        }

        public int SelectedIndex
        {
            get { return selectedindex; }
            set
            {
                int NewSI = value;
                if (NewSI < 0) NewSI = 0;
                if (NewSI >= Items.Count) NewSI = Items.Count - 1;
                selectedindex = NewSI;
                FillLabels();
                OnSelectionChanged(new EventArgs());
            }
        }

        public List<string> Items { get
            {
                return _items;
            }
            set
            {
                _items = (EventList<string>)value;
                SelectedIndex = selectedindex; // Чтобы прошла проверка правильности SelectedIndex
            }
        }

        /****** СОБЫТИЯ ******/
        /// <summary>
        /// Происходит при изменении ItemSelected
        /// </summary>
        public event EventHandler SelectionChanged;

        /// <summary>
        /// Происходит при добавлении элемента в список. Вызывается до ItemsChanged.
        /// </summary>
        public event EventHandler ItemsAdded;

        /// <summary>
        /// Происходит при удалении элемента из списка. Вызывается до ItemsChanged.
        /// </summary>
        public event EventHandler ItemsRemoved;

        /// <summary>
        /// Происходит при очистке списка. Вызывается до ItemsChanged.
        /// </summary>
        public event EventHandler ItemsClear;

        /// <summary>
        /// Происходит при изменении списка. Вызывается после частных событий, таких как ItemsAdded, ItemsRemoved и ItemsClear.
        /// </summary>
        public event EventHandler ItemsChanged;

        public CenterList()
        {
            InitializeComponent();
            SizeChanged += OnResize;
            _items.ListChanged += ListChanged;
            _items.ListAdd += (object sender, EventArgs e) => { OnItemsAdded(e); };
            _items.ListRemove += (object sender, EventArgs e) => { OnItemsRemoved(e); };
            _items.ListClear += (object sender, EventArgs e) => { OnItemsClear(e); };
            _items.ListChanged += (object sender, EventArgs e) => { OnItemsChanged(e); };

            SizeRefresh();
        }

        private void ListChanged(object sender, EventArgs e)
        {
            if (SelectedIndex >= _items.Count) SelectedIndex = _items.Count - 1;
            FillLabels();
        }

        /// <summary>
        /// Обновить элементы оформления.
        /// </summary>
        public void SizeRefresh()
        {
            Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            if (OldHeight == ActualHeight) return;
            OldHeight = ActualHeight;

            Size TextSize = Texts.MeasureTextSize("1py", FontFamily, FontStyle, FontWeight, FontStretch, FontSize);
            TextHeight = TextSize.Height;
            ElementHeight = TextHeight + 4;
            int Elements = Convert.ToInt32(Math.Truncate(ActualHeight / ElementHeight));

            // Нам нужно нечётное число
            Elements = (Elements & 1) == 0
                ? Elements - 1
                : Elements;

            //if (Elements == ElementsUp * 2 + 1) return;

            double Space = (ActualHeight - TextHeight * Elements) / Elements;
            ElementHeight = TextHeight + Space;

            ElementsUp = (Elements - 1) / 2;

            Content.RowDefinitions.Clear();
            for (int i = 0; i < Elements; i++)
                Content.RowDefinitions.Add(new RowDefinition()
                { Height = new GridLength(1, GridUnitType.Star) });
            Content.Children.Clear();

            double ElementPadding = Space / 2;

            Labels = Elements > 0 ? new Label[Elements] : null;
            for (int i = 0; i < Elements; i++)
            {
                Labels[i] = new Label()
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Padding = new Thickness(5, ElementPadding, 5, ElementPadding),
                    Background = ((i - ElementsUp) & 1) == 0
                        ? EvenItemBackground
                        : OddItemBackground,
                    Foreground = ((i - ElementsUp) & 1) == 0
                        ? EvenItemForeground
                        : OddItemForeground,
                    FontFamily = FontFamily,
                    FontSize = FontSize,
                    FontStretch = FontStretch,
                    FontStyle = FontStyle,
                    FontWeight = FontWeight,
                    Tag = i - ElementsUp,
                    Content = GetItem(i - ElementsUp),
                };
                Labels[i].MouseLeftButtonDown += LabelClick;
                Labels[i].MouseEnter += LabelEnter;
                Labels[i].MouseLeave += LabelLeave;
                Labels[i].MouseUp += LabelUp;
                if (i - ElementsUp == 0)
                {
                    Labels[i].Background = CentralItemBackground;
                    Labels[i].Foreground = CentralItemForeground;
                }
                Grid.SetRow(Labels[i], i);

                WPF_Element.SetBinding(this, Labels[i], "FontFamily", FontFamilyProperty);
                WPF_Element.SetBinding(this, Labels[i], "FontSize", FontSizeProperty);
                WPF_Element.SetBinding(this, Labels[i], "FontStretch", FontStretchProperty);
                WPF_Element.SetBinding(this, Labels[i], "FontStyle", FontStyleProperty);
                WPF_Element.SetBinding(this, Labels[i], "FontWeight", FontWeightProperty);

                Content.Children.Add(Labels[i]);
            }


            Text = "";//Elements.ToString();
        }

        private void LabelUp(object sender, MouseButtonEventArgs e)
        {
            if (!MousePressed) return;
            MousePressed = false;
            SelectedIndex += PressedLabel == null ? 0 : (int)PressedLabel.Tag;
            SetPressedItem(null);
        }

        private void LabelLeave(object sender, MouseEventArgs e)
        {
            if (!MousePressed) return;
            SetPressedItem(null);
        }

        private void LabelEnter(object sender, MouseEventArgs e)
        {
            if (!MousePressed) return;
            if ((string)(((Label)sender).Content) == "") return;

            SetPressedItem((Label)sender);
        }

        private void LabelClick(object sender, MouseButtonEventArgs e)
        {
            if ((string)(((Label)sender).Content) == "") return;
            MousePressed = true;
            SetPressedItem((Label)sender);
        }

        private void SetPressedItem(Label PressedItem)
        {
            if (PressedLabel != null)
            {
                PressedLabel.Background = _PressedItemBackground;
                PressedLabel.Foreground = _PressedItemForeground;
            }
            PressedLabel = PressedItem;
            if (PressedItem != null)
                if ((int)PressedItem.Tag == 0)
                    PressedLabel = null;
            if (PressedLabel == null)
            {
                _PressedItemBackground = null;
                _PressedItemForeground = null;
                return;
            }
            _PressedItemBackground = PressedLabel.Background;
            _PressedItemForeground = PressedLabel.Foreground;

            PressedLabel.Background = SelectedItemBackground;
            PressedLabel.Foreground = SelectedItemForeground;
        }

        /// <summary>
        /// Событие, возникающее при изменении размера.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResize(object sender, SizeChangedEventArgs e)
        {
            SizeRefresh();
        }

        public void FillLabels()
        {
            int j = -1;
            for (int i = SelectedIndex - ElementsUp; i <= SelectedIndex + ElementsUp; i++)
            {
                j++;
                if (i < 0)
                {
                    Labels[j].Content = "";
                    continue;
                }
                if (i >= Items.Count)
                {
                    Labels[j].Content = "";
                    continue;
                }

                Labels[j].Content = Items[i];
            }
        }

        private string GetItem(int Pos)
        {
            if (SelectedIndex + Pos < 0)
            {
                return "";
            }
            if (SelectedIndex + Pos >= Items.Count)
            {
                return "";
            }
            return Items[SelectedIndex + Pos].ToString();
        }

        /// <summary>
        /// Событие при изменении выделенного фрагмента.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnSelectionChanged(EventArgs e)
        {
            SelectionChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Событие при изменении списка элементов.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnItemsChanged(EventArgs e)
        {
            ItemsChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Событие при добавлении элемента в список.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnItemsAdded(EventArgs e)
        {
            ItemsAdded?.Invoke(this, e);
        }

        /// <summary>
        /// Событие при удалении элемента из списка.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnItemsRemoved(EventArgs e)
        {
            ItemsRemoved?.Invoke(this, e);
        }

        /// <summary>
        /// Событие при очистке списка.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnItemsClear(EventArgs e)
        {
            ItemsClear?.Invoke(this, e);
        }
    }
}
