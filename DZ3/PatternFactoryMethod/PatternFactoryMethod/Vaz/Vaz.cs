using System;

namespace PatternFactoryMethod.Vaz
{
    public class Vaz : BaseClassForCar
    {
        public string SerialNumber { get; private set; }

        public Vaz(string model) : base("Ваз", model)
        {
            // Создаем генератор случайных чисел для серийного номера
            Random rnd = new Random();
            SerialNumber = "vz" + rnd.Next(1000000, 9999999).ToString();
        }

        public override string ToString()
        {
            return $"машина {base.Model}, произведена заводом {FactoryName}, с номером {SerialNumber}";
        }
    }
}
