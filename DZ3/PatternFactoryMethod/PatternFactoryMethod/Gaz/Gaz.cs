using System;

namespace PatternFactoryMethod.Gaz
{
    public class Gaz : BaseClassForCar
    {
        public string SerialNumber { get; private set; }

        public Gaz(string model) : base("Газ", model.ToString())
        {
            // Создаем генератор случайных чисел для серийного номера
            Random rnd = new Random();
            SerialNumber = rnd.Next(1000000, 9999999).ToString() + "GAZ";
        }

        public override string ToString()
        {
            return $"машина {base.Model}, произведена заводом {FactoryName}, с номером {SerialNumber}";
        }
    }
}
