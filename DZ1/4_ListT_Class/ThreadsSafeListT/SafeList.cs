using System.Collections.Generic;


namespace ThreadsSafeListT
{
    public class SafeList<T>
    {
        protected List<T> _list = new List<T>();
        protected static object _lock = new object();

        /// <summary>
        /// Добавление элемента в List<T>
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            lock (_lock)
            {
                _list.Add(item);
            }
        }

        /// <summary>
        /// Удаление элемента из List<T>
        /// результат удаления bool значение выполнения операции
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            lock (_lock)
            {
                return _list.Remove(item);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Clone().GetEnumerator();
        }

        /// <summary>
        /// Создание копии List<T>
        /// </summary>
        /// <returns></returns>
        public List<T> Clone()
        {
            List<T> newList = new List<T>();

            lock (_lock)
            {
                _list.ForEach(x => newList.Add(x));
            }

            return newList;
        }
    }
}
