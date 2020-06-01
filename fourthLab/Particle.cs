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
            int alpha = (int)(k * 255);
            var color = Color.FromArgb(alpha, Color.Black);
            var b = new SolidBrush(color);
            g.FillEllipse(b, X - Radius, Y - Radius, Radius * 2, Radius * 2);
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
            new float[] {0, 0, 0, 0, 1F}}); // а сюда добавляется то сколько мы хотим прибавить к каждому каналу
                       
            ImageAttributes imageAttributes = new ImageAttributes();
            imageAttributes.SetColorMatrix(matrix);
            // метод рисования
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
    public abstract class EmiterBase
    {
        List<Particle> particles = new List<Particle>();
        // количество частиц эмитера храним в переменной
        int particleCount = 0;
        // и отдельной свойство которое возвращает количество частиц
        public int ParticlesCount
        {
            get
            {
                return particleCount;
            }
            set
            {
                // при изменении этого значения
                particleCount = value;
                // удаляем лишние частицы 
                if (value < particles.Count)
                {
                    particles.RemoveRange(value, particles.Count - value);
                }
            }
        }
        public abstract void ResetParticle(Particle particle);
        public abstract void UpdateParticle(Particle particle);
        public abstract Particle CreateParticle();
        // тут общая логика обновления состояния эмитера  
        public void UpdateState()
        {
            foreach (var particle in particles)
            {
                particle.Life -= 1;
                if (particle.Life < 0)
                {
                    ResetParticle(particle);
                }
                else
                {
                    UpdateParticle(particle);
                }
            }

            for (var i = 0; i < 10; ++i)
            {
                if (particles.Count < 300)
                {
                    particles.Add(CreateParticle());
                }
                else
                {
                    break;
                }
            }
        }

        public void Render(Graphics g)
        {
            foreach (var particle in particles)
            {
                particle.Draw(g);
            }
        }
    }
    public class PointEmiter : EmiterBase
    {
        public Point Position;

        public override Particle CreateParticle()
        {
            var particle = Particle.Generate();
            particle.X = Position.X;
            particle.Y = Position.Y;
            return particle;
        }

        public override void ResetParticle(Particle particle)
        {
            particle.Life = 20 + Particle.rand.Next(100);
            particle.Speed = 1 + Particle.rand.Next(10);
            particle.Direction = Particle.rand.Next(360);
            particle.Radius = 2 + Particle.rand.Next(20);
            particle.X = Position.X;
            particle.Y = Position.Y;
        }

        public override void UpdateParticle(Particle particle)
        {
            var directionInRadians = particle.Direction / 180 * Math.PI;
            particle.X += (float)(particle.Speed * Math.Cos(directionInRadians));
            particle.Y -= (float)(particle.Speed * Math.Sin(directionInRadians));
        }
    }
    public class DirectionEmiter : PointEmiter
    {
        public int Direction = 0; // направление частиц
        public int Spread = 10; // разброс частиц
        public override Particle CreateParticle()
        {
            var particle = ParticleImage.Generate();
            Random random = new Random();
            int candy = random.Next(1, 9);
            switch (candy)
            {
                case 1:
                    particle.image = Properties.Resources.red;
                    break;
                case 2:
                    particle.image = Properties.Resources.green;
                    break;
                case 3:
                    particle.image = Properties.Resources.blue;
                    break;
                case 4:
                    particle.image = Properties.Resources.turquoise;
                    break;
                case 5:
                    particle.image = Properties.Resources.purple;
                    break;
                case 6:
                    particle.image = Properties.Resources.heart;
                    break;
                case 7:
                    particle.image = Properties.Resources.stick;
                    break;
                case 8:
                    particle.image = Properties.Resources.lolipop;
                    break;
            }
            particle.Direction = this.Direction + Particle.rand.Next(-Spread / 2, Spread / 2);
            particle.X = Position.X;
            particle.Y = Position.Y;
            return particle;
        }

        public override void ResetParticle(Particle particle)
        {
            if (particle != null)
            {
                particle.Life = 20 + Particle.rand.Next(100);
                particle.Speed = 1 + Particle.rand.Next(10);
                particle.Direction = this.Direction + Particle.rand.Next(-Spread / 2, Spread / 2);
                particle.X = Position.X;
                particle.Y = Position.Y;
            }
        }
    }
}
