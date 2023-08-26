using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp10
{
    public partial class Form1 : Form
    {

        public Panel maze_panel = new Panel();
        int mazeSizeX = 0;
        int mazeSizeY = 0;
        int startx =0 ;
        int starty = 0;
        string harita = "";
        static int blockx = 25;
        static int blocky = 25;
        static int Q_TableSize = blockx * blocky;
        int targetx, targety;
        int[,] reward_table = new int[blockx, blocky];
        double[,] Q_table = new double[Q_TableSize, 8];
        TextBox startX = new TextBox();
        TextBox startY = new TextBox();
        TextBox finishX = new TextBox();
        TextBox finishY = new TextBox();
        Button basla = new Button();
        List<Panel> pathBlock = new List<Panel>();
        Panel settings = new Panel();

        public Form1()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            this.BackColor = Color.FromArgb(75, 75, 75);
            
            Random rand = new Random();
            
            for(int i =0;i<blockx;i++)
            {
               
                for (int j=0;j<blocky;j++)
                {
                                     
                    reward_table[i, j] = 0;
                    
                }
                
            }
            int countBlock = (blockx * blocky) * 20 / 100;
            for(int i=0;i<countBlock;i++)
            {
                int x = rand.Next(0, blockx);
                int y = rand.Next(0, blocky);
               // Console.WriteLine(x + "," + y);
                reward_table[x, y] = -5;
            }
            for(int i=0;i<reward_table.GetLength(0);i++)
            {
                for(int j=0;j<reward_table.GetLength(1);j++)
                {
                    harita += reward_table[i, j].ToString() + "\t"; 
                }
                harita += "\n"; 
            }

           
            
            
            settings.BackColor = Color.FromArgb(240, 147, 43);
            settings.Size = new Size(this.Size.Width * 19 / 100, this.Size.Height * 60 / 100);
            settings.Location = new Point(this.Width * 80 / 100, this.Height * 15 / 100);
            maze_panel.BackColor = Color.FromArgb(240, 147, 43);

            Label header1 = new Label();
            header1.Text = "Başlangıç Noktaları";
            header1.Size = new Size(settings.Width * 50 / 100, settings.Height * 5 / 100);
            header1.Font = new Font("Arial", 10);
            header1.Location = new Point(settings.Width * 10 / 100, settings.Height * 12 / 100);
            settings.Controls.Add(header1);
            Label header2 = new Label();
            header2.Text = "Bitiş Noktaları";
            header2.Font = new Font("Arial", 10);
            header2.Size = new Size(settings.Width * 50 / 100, settings.Height * 5 / 100);
            header2.Location = new Point(settings.Width * 10 / 100, settings.Height * 40 / 100);
            settings.Controls.Add(header2);

            startX.Location = new Point(settings.Width * 10 / 100, settings.Height * 18 / 100);
            startY.Location = new Point(settings.Width * 25 / 100, settings.Height * 18 / 100);
            startX.Size = new Size(settings.Width * 12 / 100, settings.Height * 10 / 100);
            startY.Size = new Size(settings.Width * 12 / 100, settings.Height * 10 / 100);

            settings.Controls.Add(startX);
            settings.Controls.Add(startY);

            finishX.Location = new Point(settings.Width * 10 / 100, settings.Height * 46 / 100);
            finishY.Location = new Point(settings.Width * 25 / 100, settings.Height * 46 / 100);
            finishX.Size = new Size(settings.Width * 12 / 100, settings.Height * 10 / 100);
            finishY.Size = new Size(settings.Width * 12 / 100, settings.Height * 10 / 100);

            basla.Size = new Size(settings.Width * 80 / 100, settings.Height * 20 / 100);
            basla.FlatStyle = FlatStyle.Flat;
            basla.BackColor = Color.FromArgb(235, 77, 75);
            basla.Text = "BAŞLA";
            basla.Font = new Font("Arial", 30);
            basla.ForeColor = Color.White;
            basla.Location = new Point(settings.Width * 10 / 100, settings.Height * 76 / 100);

            settings.Controls.Add(basla);
            settings.Controls.Add(finishX);
            settings.Controls.Add(finishY);

            maze_panel.Size = new Size(this.Size.Width*75/100, this.Size.Height*75/100);
            maze_panel.Location = new Point(this.Width * 3 / 100,this.Height * 7 / 100);


            mazeSizeX = maze_panel.Width;
            mazeSizeY = maze_panel.Height;
            this.Controls.Add(maze_panel);
            this.Controls.Add(settings);

            for(int i=0;i<Q_TableSize;i++)
            {
                for(int j=0;j<8; j++)
                {
                    Q_table[i, j] = 0;
                }
            }

            basla.Click += Basla_Click;
            
             
            

        }

        private void Basla_Click(object sender, EventArgs e)
        {
            startx = Convert.ToInt32(startX.Text);
            starty = Convert.ToInt32(startY.Text);
            targetx = Convert.ToInt32(finishX.Text);
            targety = Convert.ToInt32(finishY.Text);
            reward_table[targety, targetx] = 5;
            reward_table[startx, starty] = 0;
            Console.WriteLine("startX = " + startx + " startY= " + starty + " targetx= " + targetx + " targety= " + targety);
            block();
            qLearning q = new qLearning(reward_table,Q_table,blockx,blocky,targetx,targety,startx,starty,pathBlock, maze_panel);
            q.q_learning();
           
            


        }

        public void block()
        {
            
            int uzunluk = 0;
            int aralıkx = 0;
            int aralıky = 0;
            int uzunluky = 0;
            int blok_uzun = 0;
            int x = 0, y = 0;
            
            for (int i = 0; i < blocky ; i++)
            {

                aralıkx = 0;
               
                

                for (int j = 0; j < blockx; j++)
                {
                    Panel block = new Panel();
                    block.Size = new Size(maze_panel.Width / blockx, maze_panel.Height / blocky);
                    if (reward_table[i, j] == 0)
                    {
                        block.BackColor = Color.FromArgb(249, 202, 36);
                        if(i==starty && j==startx)
                            {
                            block.BackColor = Color.FromArgb(106, 176, 76);
                            }
                    }
                    else if(reward_table[i, j] == 5)
                    {
                        block.BackColor = Color.FromArgb(48, 51, 107);
                    }
                    
                    else
                    {
                        block.BackColor = Color.FromArgb(240, 147, 43);
                    }
                    block.Location = new Point(x + aralıkx, y + aralıky);
                    maze_panel.Controls.Add(block);
                    aralıkx += block.Size.Width + 1;
                    uzunluk = aralıkx;
                    blok_uzun = block.Size.Height;
                    mazeSizeX = block.Size.Width;
                    mazeSizeY = block.Size.Height;
                    pathBlock.Add(block);
                    // block.Visible = false;
                    //block.SendToBack();

                }
                
                aralıky += blok_uzun + 1;
                uzunluky = aralıky;

            }


            //Console.WriteLine(block_sizex + "," + block_sizey);
            maze_panel.Size = new Size(uzunluk, uzunluky);



        }
    }
}
