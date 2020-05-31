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
                    particle.X = MousePositionX;
                    particle.Y = MousePositionY;
                     particle.Direction = Particle.rand.Next(360);
                    particle.Speed = 1 + Particle.rand.Next(10);
                    particle.Radius = 2 + Particle.rand.Next(20);
                }
                else
                {
                    var directionInRadians = particle.Direction / 180 * Math.PI;
                    particle.X += (float)(particle.Speed * Math.Cos(directionInRadians));
                    particle.Y -= (float)(particle.Speed * Math.Sin(directionInRadians));
                }
            }
             Random random = new Random();
            for (var i = 0; i < 10; ++i)
            {
                if (particles.Count < 300) // пока частиц меньше 300 генерируем новые
                {
                    var particle = ParticleImage.Generate();
                    int candy =random.Next(1,9);
                    switch (candy) {
                        case 1: particle.image = Properties.Resources.red;
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

                 //   particle.image = Properties.Resources.red;
                    particle.X = MousePositionX;
                    particle.Y = MousePositionY;
                    particles.Add(particle);
                }
                else
                {
                    break; // а если частиц уже 500 штук, то ничего не генерирую
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateState();
            using (var g = Graphics.FromImage(picDisplay.Image))
            {
                g.Clear(Color.Transparent);
                Render(g);
            }
            picDisplay.Invalidate();
        }

        private int MousePositionX = 0;
        private int MousePositionY = 0;
        private void picDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            MousePositionX = e.X;
            MousePositionY = e.Y;
        }
    }
}
