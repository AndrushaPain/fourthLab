using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fourthLab
{
    public partial class Form1 : Form
    {
        List<Particle> particles = new List<Particle>();
        public Form1()
        {
            InitializeComponent();
            picDisplay.Image = new Bitmap(picDisplay.Width, picDisplay.Height);
            for (var i = 0; i < 500; ++i)
            {
                var particle = Particle.Generate();
                // переношу частицы в центр изображения
                particle.X = picDisplay.Image.Width / 2;
                particle.Y = picDisplay.Image.Height / 2;
                // добавляю список
                particles.Add(particle);
            }
        }

        private void UpdateState()
        {
            foreach (var particle in particles)
            {
                particle.Life -= 1;
                if (particle.Life < 0)
                {
                    // восстанавливаю здоровье
                    particle.Life = 20 + Particle.rand.Next(100);
                    // пермещаю частицу в центр
                    particle.X = picDisplay.Image.Width / 2;
                    particle.Y = picDisplay.Image.Height / 2;
                    particle.Direction = Particle.rand.Next(360);
                    particle.Speed = 1 + Particle.rand.Next(10);
                    particle.Radius = 2 + Particle.rand.Next(10);
                }
                else
                {
                    var directionInRadians = particle.Direction / 180 * Math.PI;
                    particle.X += (float)(particle.Speed * Math.Cos(directionInRadians));
                    particle.Y -= (float)(particle.Speed * Math.Sin(directionInRadians));
                }
            }
        }

        private void Render(Graphics g)
        {
            foreach (var particle in particles)
            {
                particle.Draw(g);
            }
        }


        int counter = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateState();
            using (var g = Graphics.FromImage(picDisplay.Image))
            {
                g.Clear(Color.White);
                Render(g);
            }
            picDisplay.Invalidate();
        }
    }
}
