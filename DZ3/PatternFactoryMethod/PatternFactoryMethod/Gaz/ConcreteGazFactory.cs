using System;
using System.Collections.Generic;
using static PatternFactoryMethod.Gaz.EnumGazModels;

namespace PatternFactoryMethod.Gaz
{
    public class ConcreteGazFactory : BaseClassForCarFactory
    {
        private GazModels _model;

        public ConcreteGazFactory(GazModels newModel) : base("Завод Газ")
        {
            if (!Enum.IsDefined(typeof(GazModels), newModel))
            {
                throw new ArgumentException($"Некорректная модель машины, не можем такую произвести",
                    nameof(newModel));
            }

            // Устанавливаем значение.
            _model = newModel;
        }

        public override BaseClassForCar[] Create(int count)
        {
            // Создаем коллекцию для хранения машин.
            List<BaseClassForCar> cars = new List<BaseClassForCar>();

            // Создаем машину и добавляем в коллекцию.
            for (int i = 0; i < count; i++)
            {
                Gaz gaz = new Gaz(_model.ToString());
                cars.Add(gaz);
            }

            // Возвращаем готовые машины.
            return cars.ToArray();
        }
    }
}
