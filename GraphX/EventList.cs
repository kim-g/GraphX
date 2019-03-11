using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphX
{
    class EventList<T> : List<T>
    {
        /// <summary>
        /// Событие, происходящее при любом изменении списка
        /// </summary>
        public event EventHandler ListChanged;

        /// <summary>
        /// Событие, происходящее при добавлении элементов в список
        /// </summary>
        public event EventHandler ListAdd;

        /// <summary>
        /// Событие, происходящее при удалении элементов из списка
        /// </summary>
        public event EventHandler ListRemove;

        /// <summary>
        /// Событие, происходящее при очистке списка
        /// </summary>
        public event EventHandler ListClear;

        /// <summary>
        /// Добавляет элемент в список
        /// </summary>
        /// <param name="item">Элемент</param>
        public new void Add(T item)
        {
            base.Add(item);
            OnListAdd(new EventArgs());
            OnListChange(new EventArgs());
        }

        /// <summary>
        /// Добавляет список в конец списка
        /// </summary>
        /// <param name="collection">Список для добавления</param>
        public new void AddRange(IEnumerable<T> collection)
        {
            base.AddRange(collection);
            OnListAdd(new EventArgs());
            OnListChange(new EventArgs());
        }

        /// <summary>
        /// Очищает список
        /// </summary>
        public new void Clear()
        {
            base.Clear();
            OnListRemove(new EventArgs());
            OnListClear(new EventArgs());
            OnListChange(new EventArgs());
        }

        /// <summary>
        /// Вставляет элемент на указанную позицию
        /// </summary>
        /// <param name="index">Позиция</param>
        /// <param name="item">Элемент</param>
        public new void Insert(int index, T item)
        {
            base.Insert(index, item);
            OnListAdd(new EventArgs());
            OnListChange(new EventArgs());
        }

        /// <summary>
        /// Вставляет элемент на указанную позицию
        /// </summary>
        /// <param name="index">Позиция</param>
        /// <param name="item">Элемент</param>
        public new void InsertRange(int index, IEnumerable<T> collection)
        {
            base.InsertRange(index, collection);
            OnListAdd(new EventArgs());
            OnListChange(new EventArgs());
        }

        /// <summary>
        /// Удалёет элемент из списка
        /// </summary>
        /// <param name="item"></param>
        public new void Remove(T item)
        {
            base.Remove(item);
            OnListRemove(new EventArgs());
            OnListChange(new EventArgs());
        }

        /// <summary>
        /// Удалёет все подходящие под условие элементы из списка
        /// </summary>
        /// <param name="item"></param>
        public new void RemoveAll(Predicate<T> match)
        {
            base.RemoveAll(match);
            OnListRemove(new EventArgs());
            OnListChange(new EventArgs());
        }

        /// <summary>
        /// Удалёет элемент с заданным индексом из списка
        /// </summary>
        /// <param name="item"></param>
        public new void RemoveAt(int index)
        {
            base.RemoveAt(index);
            OnListRemove(new EventArgs());
            OnListChange(new EventArgs());
        }

        /// <summary>
        /// Удалёет элемент с заданным индексом из списка
        /// </summary>
        /// <param name="item"></param>
        public new void RemoveRange(int index, int count)
        {
            base.RemoveRange(index, count);
            OnListRemove(new EventArgs());
            OnListChange(new EventArgs());
        }

        private void OnListChange(EventArgs e)
        {
            ListChanged?.Invoke(this, e);
        }

        private void OnListAdd(EventArgs e)
        {
            ListAdd?.Invoke(this, e);
        }

        private void OnListRemove(EventArgs e)
        {
            ListRemove?.Invoke(this, e);
        }

        private void OnListClear(EventArgs e)
        {
            ListClear?.Invoke(this, e);
        }
    }
}
