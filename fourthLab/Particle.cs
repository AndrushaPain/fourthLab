using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace fourthLab
{
     public class Particle
    {
        public int Radius; // радуис частицы
        public float X; // X координата положения частицы в пространстве
        public float Y; // Y координата положения частицы в пространстве
        public float Direction; // направление движения
        public float Speed; // скорость перемещения

        public float Life; // запас здоровья частицы

        public static Random rand = new Random();

        // метод генерации частицы
        public static Particle Generate()
        {
           return new Particle
            {
                Direction = rand.Next(360),
                Speed = 1 + rand.Next(10),
                Radius = 2 + rand.Next(20),
               Life = 20 + rand.Next(100),
           };
        }

        public virtual void Draw(Graphics g)
        {
            float k = Math.Min(1f, Life / 100);
            // рассчитываем значение альфа канала в шкале от 0 до 255
            // по аналогии с RGB, он используется для задания прозрачности
            int alpha = (int)(k * 255);

            // создаем цвет из уже существующего, но привязываем к нему еще и значение альфа канала
            var color = Color.FromArgb(alpha, Color.Black);
            var b = new SolidBrush(color);
            // нарисовали залитый кружок радиусом Radius с центром в X, Y
            g.FillEllipse(b, X - Radius, Y - Radius, Radius * 2, Radius * 2);

            // удалили кисть из памяти
            b.Dispose();
        }
    }
    public class ParticleImage : Particle
    {
        public Image image;

        public new static ParticleImage Generate()
        {
            return new ParticleImage
            {
                Direction = rand.Next(360),
                Speed = 1 + rand.Next(10),
                Radius = 2 + rand.Next(20),
                Life = 20 + rand.Next(100),
            };
        }

        public override void Draw(Graphics g)
        {
            float k = Math.Min(1f, Life / 100);

            // матрица преобразования цвета
            // типа аналога матрицы трансформации, но для цвета
            ColorMatrix matrix = new ColorMatrix(new float[][]{
            new float[] {1F, 0, 0, 0, 0}, // мультипликатор красного канала
            new float[] {0, 1F, 0, 0, 0}, // мультипликатор зеленого канала
            new float[] {0, 0, 1F, 0, 0}, // мультипликатор синего канала
            new float[] {0, 0, 0, k, 0}, // мультипликатор альфа канала, сюда прозрачность пихаем
            new float[] {0, 0, 0, 0, 1F}}); // а сюда пихаются то сколько мы хотим прибавить к каждому каналу

            // эту матрицу пихают в атрибуты
            ImageAttributes imageAttributes = new ImageAttributes();
            imageAttributes.SetColorMatrix(matrix);

            // ну и тут хитрый метод рисования
            g.DrawImage(image,
                // куда рисовать
                new Rectangle((int)(X - Radius), (int)(Y - Radius), Radius * 2, Radius * 2),
                // и какую часть исходного изображения брать, в нашем случае все изображения
                0, 0, image.Width, image.Height,
                GraphicsUnit.Pixel, // надо передать
                imageAttributes // наши атрибуты с матрицей преобразования
               );
        }
    }
}
