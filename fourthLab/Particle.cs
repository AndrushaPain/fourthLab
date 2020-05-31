using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fourthLab
{
    class Particle
    {
        public int Radius; // радуис частицы
        public float X; // X координата положения частицы в пространстве
        public float Y; // Y координата положения частицы в пространстве
        public float Direction; // направление движения
        public float Speed; // скорость перемещения

        public static Random rand = new Random();

        // метод генерации частицы
        public static Particle Generate()
        {
           return new Particle
            {
                Direction = rand.Next(360),
                Speed = 1 + rand.Next(10),
                Radius = 2 + rand.Next(10)
            };
        }
    }
}
