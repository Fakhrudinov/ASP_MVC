using System;

namespace PatternFactoryMethod
{
    /// <summary>
    /// Базовый класс для любой машины.
    /// </summary>
    public abstract class BaseClassForCar
    {
        public string FactoryName { get; protected set; }
        public string Model { get; protected set; }

        public BaseClassForCar(string factoryName, string model)
        {
            // Проверяем входные данные на корректность.
            if (string.IsNullOrEmpty(factoryName))
            {
                throw new ArgumentNullException(nameof(factoryName));
            }

            if (string.IsNullOrEmpty(model))
            {
                throw new ArgumentNullException(nameof(model));
            }

            // Устанавливаем значения.
            FactoryName = factoryName;
            Model = model;
        }

        public override string ToString()
        {
            return FactoryName;
        }
    }
}
