using System;

namespace PatternFactoryMethod
{
    /// <summary>
    /// Базовый класс для фабрик производства машин.
    /// </summary>
    public abstract class BaseClassForCarFactory
    {
        public string Name { get; protected set; }

        public BaseClassForCarFactory(string name)
        {
            // Проверяем входные данные на корректность.
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            // Устанавливаем значение.
            Name = name;
        }

        public abstract BaseClassForCar[] Create(int count);

        public override string ToString()
        {
            return Name;
        }
    }
}
