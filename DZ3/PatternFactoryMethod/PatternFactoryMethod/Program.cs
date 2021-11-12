using PatternFactoryMethod.Gaz;
using PatternFactoryMethod.Vaz;
using System;
using System.Collections.Generic;
using static PatternFactoryMethod.Gaz.EnumGazModels;
using static PatternFactoryMethod.Vaz.EnumVazModels;

namespace PatternFactoryMethod
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Pattern: Factory Method");

            // Создаем коллекцию заводов для производства машин, мы можем хранить их в одной коллекции.
            List<BaseClassForCarFactory> factorys = new List<BaseClassForCarFactory>
            {
                new ConcreteGazFactory(GazModels.Волга),
                new ConcreteGazFactory(GazModels.Газель),
                new ConcreteVazFactory(VazModels.Нива)
            };

            // Создаем коллекцию для хранения машин. здесь могут храниться любые классы, унаследованные от BaseClassForCar.
            List<BaseClassForCar> cars = new List<BaseClassForCar>();

            //количество экземпляров
            int count = 1;

            // По очереди запускаем фабрики производства машин.
            foreach (BaseClassForCarFactory concreteFactory in factorys)
            {
                // Вызываем фабричный метод для создания машины.
                BaseClassForCar[] newCar = concreteFactory.Create(count);

                // Добавляем созданную машину в общую коллекцию.
                cars.AddRange(newCar);
            }

            // Выводим данные о машинах на экран.
            foreach (BaseClassForCar car in cars)
            {
                Console.WriteLine(car);
            }

            Console.ReadLine();
        }
    }
}
