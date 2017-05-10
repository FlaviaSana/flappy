using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Media;

namespace flappy
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            label2.Visible = false;
        }
        List<int> Pipe1= new List<int>();//conducta 1
        List<int> Pipe2= new List<int>();//conducta 2
        int PipeWidth = 60; //latimea conductei
        int PipeDifferentY=310;//diferenta dintre doua conducte
        int PipeDifferentX = 300;//locul liber
        bool start = true;//variabila logica
        bool running;
        int step = 15;//viteza
        int OriginalX, OriginalY;//pozitia initiala pe x si y
        bool ResetPipes = false;//resetarea de la start
        int points;
        bool InPipe = false;
        int score;
        int ScoreDifferent;//diferenta de scor 

        Graphics gr;

        private void Die()
        {
            running = false;
            timer2.Enabled = false;
            timer3.Enabled = false;
            button1.Visible = true;
            button1.Enabled = true;
            ReadAndShowScore();
            points = 0;
            pictureBox1.Location = new Point(OriginalX, OriginalY);
            ResetPipes = true;
            Pipe1.Clear();
        }

        private void ReadAndShowScore()
        {
            using (StreamReader reader=new StreamReader("Score.ini"))
            {
                score=int.Parse(reader.ReadToEnd());
                reader.Close();
                if(int.Parse(label1.Text)==0 | int.Parse(label1.Text)>0)
                {
                    ScoreDifferent = score - int.Parse(label1.Text) + 1;//scor-cat am facut, plus 1.cat mai trebuia sa faca
                }
                if (score < int.Parse(label1.Text))
                {
                    MessageBox.Show(string.Format("          Scor: {1}          ", score, label1.Text), "FlappyBird", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    using (StreamWriter writer = new StreamWriter("Score.ini"))
                    {
                        writer.Write(label1.Text);
                        writer.Close();
                    }
                }
                if(score>int.Parse(label1.Text))
                {
                    MessageBox.Show(string.Format("Ai pierdut. Încearcă din nou!", ScoreDifferent, score), "FlappyBird", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                if(score == int.Parse(label1.Text))
                {
                    MessageBox.Show(string.Format("        Scor: {0} \n Încearcă din nou!", score, label1.Text, "FlappyBird", MessageBoxButtons.OK, MessageBoxIcon.Information));
                }
            }
        }

        private void StartGame()
        {
            ResetPipes = false;
            timer1.Enabled = true;
            timer2.Enabled = true;
            timer3.Enabled = true;
            Random random = new Random();
            int num= random.Next(40,this.Height-this.PipeDifferentY);
            int num1=num+this.PipeDifferentY;
            Pipe1.Clear();
            Pipe1.Add(this.Width);
            Pipe1.Add(num);
            Pipe1.Add(this.Width);
            Pipe1.Add(num1);

            num = random.Next(40, (this.Height - PipeDifferentY));
            num1 = num + PipeDifferentY;
            Pipe2.Clear();
            Pipe2.Add(this.Width + PipeDifferentX);
            Pipe2.Add(num);
            Pipe2.Add(this.Width + PipeDifferentX);
            Pipe2.Add(num1);

            button1.Visible = false;//ascunde
            button1.Enabled = false;//nu poti apasa
            running = true;
            Focus();//se concentreaza pe form
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            gr = this.CreateGraphics();
            

            OriginalX = pictureBox1.Location.X;//stocheaza de la pasare locatia x
            OriginalY = pictureBox1.Location.Y;
            if (!File.Exists("Score.ini"))
            {
                File.Create("Score.ini").Dispose();//creeaza si nu ramane legat de el
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Invalidate();

        }
        private void button1_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {

            if(Pipe1[0] + PipeWidth <= 0 | start == true)
            {
                Random rnd = new Random();
                int px = this.Width;
                int py = rnd.Next(40, (this.Height - PipeDifferentY));
                var p2x=px;
                var p2y=py+PipeDifferentY;
                Pipe1.Clear();
                Pipe1.Add(px);
                Pipe1.Add(py);
                Pipe1.Add(p2x);
                Pipe1.Add(p2y);

            }
            else
            {
                Pipe1[0]=Pipe1[0]-2;
                Pipe1[2]=Pipe1[2]-2;
            }
             if(Pipe2[0] + PipeWidth <= 0)
            {
                Random rnd = new Random();
                int px = this.Width;
                int py=rnd.Next(40,(this.Height-PipeDifferentY));
                var p2x=px;
                var p2y=py+PipeDifferentY;
                int[] p1 = { px, py, p2x, p2y };
                Pipe2.Clear();
                Pipe2.Add(px);
                Pipe2.Add(py);
                Pipe2.Add(p2x);
                Pipe2.Add(p2y);
            }
            else
            {
                Pipe2[0]=Pipe2[0]-2;
                Pipe1[2]=Pipe1[2]-2;
            }
            if(start==true)
            {
                start = false;
            }
        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if(!ResetPipes && Pipe1.Any() && Pipe2.Any())
            {
                //prima de sus
                e.Graphics.FillRectangle(Brushes.White, new Rectangle(Pipe1[0], 0, PipeWidth, Pipe1[1]));
                e.Graphics.FillRectangle(Brushes.White, new Rectangle(Pipe1[0]-5, Pipe1[3]-PipeDifferentY,0,0));
                //prima de jos
                e.Graphics.FillRectangle(Brushes.White, new Rectangle(Pipe1[2], Pipe1[3], PipeWidth, this.Height-Pipe1[3]));
                e.Graphics.FillRectangle(Brushes.White, new Rectangle(Pipe1[2] - 5, Pipe1[3], 0, 0));
                //a doua de sus
                e.Graphics.FillRectangle(Brushes.White, new Rectangle(Pipe2[0], 0, PipeWidth, Pipe2[1]));
                e.Graphics.FillRectangle(Brushes.White, new Rectangle(Pipe2[0] - 5, Pipe2[3] - PipeDifferentY,0, 0));
                //a doua de jos
                e.Graphics.FillRectangle(Brushes.White, new Rectangle(Pipe2[2], Pipe2[3], PipeWidth,this.Height-Pipe2[3]));
                e.Graphics.FillRectangle(Brushes.White, new Rectangle(Pipe2[2] - 5, Pipe2[3], 0, 0));

            }
        }
        private void CheckForPoint()
        {
            Rectangle rec = pictureBox1.Bounds;
            Rectangle rec1 = new Rectangle(Pipe1[2] + 20, Pipe1[3] - PipeDifferentY, 15, PipeDifferentY);
            Rectangle rec2 = new Rectangle(Pipe2[2] + 20, Pipe2[3] - PipeDifferentY, 15, PipeDifferentY);
            Rectangle rec3 = new Rectangle(Pipe1[0] + 20, Pipe1[3] - PipeDifferentY, 15, PipeDifferentY);
            Rectangle rec4 = new Rectangle(Pipe2[0] + 20, Pipe2[3] - PipeDifferentY, 15, PipeDifferentY);
            Rectangle intersect1 = Rectangle.Intersect(rec,rec1);
            Rectangle intersect2 = Rectangle.Intersect(rec, rec2);
            Rectangle intersect3 = Rectangle.Intersect(rec, rec3);
            Rectangle intersect4 = Rectangle.Intersect(rec, rec4);
            if(!ResetPipes | start)
            {
                if (intersect1 != Rectangle.Empty | intersect2 != Rectangle.Empty | intersect3 != Rectangle.Empty | intersect4 != Rectangle.Empty)
                {
                    if(!InPipe)
                    {
                        if (intersect1 != Rectangle.Empty & ( intersect3 != Rectangle.Empty |intersect4 != Rectangle.Empty ))
                            points--;
                        if (intersect2 != Rectangle.Empty & (intersect3 != Rectangle.Empty | intersect4 != Rectangle.Empty))
                            points--;
                        points++;
                        SoundPlayer sp = new SoundPlayer(flappy.Properties.Resources.point_wav);
                        sp.Play();
                        InPipe = true;
                    }
                }
                else
                 InPipe = false;
            }
        }
        private void CheckForCollision()
        {
            Rectangle rec = pictureBox1.Bounds;
            Rectangle rec1 = new Rectangle(Pipe1[0], 0, PipeWidth, Pipe1[1]);
            Rectangle rec2 = new Rectangle(Pipe1[2], Pipe1[3], PipeWidth, this.Height-Pipe1[3]);
            Rectangle rec3 = new Rectangle(Pipe2[0], 0, PipeWidth, Pipe2[1]);
            Rectangle rec4 = new Rectangle(Pipe2[2], Pipe2[3], PipeWidth, this.Height - Pipe2[3]);
            Rectangle intersect1 = Rectangle.Intersect(rec, rec1);
            Rectangle intersect2 = Rectangle.Intersect(rec, rec2);
            Rectangle intersect3 = Rectangle.Intersect(rec, rec3);
            Rectangle intersect4 = Rectangle.Intersect(rec, rec4);
            if(!ResetPipes | start)
            {
                if(intersect1 != Rectangle.Empty | intersect2 != Rectangle.Empty | intersect3 != Rectangle.Empty | intersect4 != Rectangle.Empty)
                {
                    SoundPlayer sp = new SoundPlayer(flappy.Properties.Resources.collision_wav);
                    sp.Play();
                    Die();
                }
            }

            if (rec.Bottom >= pictureBox2.Top)
            {
                Die();
            }
               
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)//cand apasam pe tasta
        {
            switch(e.KeyCode)
            {
                case Keys.Space:
                    step=-10;
                    pictureBox1.Image= flappy.Properties.Resources.bird_straight;
                    break;
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            pictureBox1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y + step);
            if(pictureBox1.Location.Y<0)
            {
                pictureBox1.Location = new Point(pictureBox1.Location.X, 0);
            }
            if(pictureBox1.Location.Y+pictureBox1.Height>this.ClientSize.Height)
            {
                pictureBox1.Location = new Point(pictureBox1.Location.X, this.ClientSize.Height - pictureBox1.Height);
            }
            CheckForCollision();
            if(running)
            {
                CheckForPoint();
            }
            label1.Text = Convert.ToString(points);

          
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)//cand lasam
        {
            switch(e.KeyCode)
            {
                case Keys.Space:
                    step = 5;
                    pictureBox1.Image = flappy.Properties.Resources.bird_down;
                    break;
            }
        }


        private void timer4_Tick(object sender, EventArgs e)
        {
          
        }
    }
}
