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

        int counter = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            counter++; // увеличиваю значение счетчика каждый вызов
            using (var g = Graphics.FromImage(picDisplay.Image))
            {
                g.Clear(Color.White);
                foreach (var particle in particles)
                {
                    particle.Draw(g);
                }
            }
            picDisplay.Invalidate();
        }
    }
}
