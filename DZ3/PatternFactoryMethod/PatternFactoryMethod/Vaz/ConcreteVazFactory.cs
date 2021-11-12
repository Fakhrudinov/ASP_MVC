using System;
using System.Collections.Generic;
using static PatternFactoryMethod.Vaz.EnumVazModels;

namespace PatternFactoryMethod.Vaz
{
    public class ConcreteVazFactory : BaseClassForCarFactory
    {
        private VazModels _model;

        public ConcreteVazFactory(VazModels newModel) : base("Завод ВАЗ")
        {
            if (!Enum.IsDefined(typeof(VazModels), newModel))
            {
                throw new ArgumentException($"Некорректная модель машины, не можем такую произвести",
                    nameof(newModel));
            }

            // Устанавливаем значение.
            _model = newModel;
        }

        public override BaseClassForCar[] Create(int count)
        {
            // Создаем коллекцию для хранения машин
            List<BaseClassForCar> cars = new List<BaseClassForCar>();

            // Создаем машину и добавляем в коллекцию.
            for (int i = 0; i < count; i++)
            {
                Vaz vaz = new Vaz(_model.ToString());
                cars.Add(vaz);
            }

            // Возвращаем готовые машины.
            return cars.ToArray();
        }
    }
}
