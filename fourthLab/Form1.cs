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
        // List<Particle> particles = new List<Particle>();
        DirectionEmiter emiter;
        public Form1()
        {
            InitializeComponent();
            picDisplay.Image = new Bitmap(picDisplay.Width, picDisplay.Height);
            emiter = new DirectionEmiter
            {
                ParticlesCount = 300,
                Position = new Point(picDisplay.Width / 2, picDisplay.Height / 2)
            };
        }

        private void UpdateState()
        {
            emiter.UpdateState();
        }

        private void Render(Graphics g)
        {
            emiter.Render(g);
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

        private void picDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void tbDirection_Scroll(object sender, EventArgs e)
        {
            emiter.Direction = tbDirection.Value;
        }

        private void tbSpread_Scroll(object sender, EventArgs e)
        {
            emiter.Spread = tbSpread.Value;
        }
    }
}
