using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mine_finder
{
    public partial class Form1 : Form
    {
        public static Boolean[,] GetRand_mine(int total_mine)
        {
            Boolean[,] maps = new Boolean[5, 5];
            Random r = new Random();
            for (int i = 0; i < total_mine; i++)
            {
                int x = r.Next(0, 5);
                int y = r.Next(0, 5);
                maps[y, x] = true;
            }
            return maps;
        }
        Boolean[,] maps = GetRand_mine(5);
        Button smile;
        MineButton[,] bnt = new MineButton[5, 5];
        static int width = 50;
        static int height = 50;
        static int total_mine = 0;
        
        public Form1()
        {
            InitializeComponent();

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (maps[i, j]) { total_mine++; }
                }
            }

            smile = new Button();
            smile.Size = new Size(30, 30);
            smile.Font = new Font("bntribntl", 12, FontStyle.Bold);
            smile.Location = new Point(150,10);
            smile.Text = ":)";
            smile.MouseDown += Smile_Click;

            Label nummine = new Label();
            nummine.Text = "total_mine = "+total_mine.ToString();
            nummine.Location = new Point(200, 15);

            Controls.Add(nummine);
            Controls.Add(smile);
            InitGame();
        }
         private void Form1_Click(object sender, MouseEventArgs e)
        {
            MineButton btn = sender as MineButton;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    // Left click
                    if (btn.Mine)
                    {
                        //죽음 게임 종료
                        smile.Text = ":(";
                        btn.Text = "*";
                        for (int i = 0; i < 5; i++)
                        {
                            for (int y = 0; y < 5; y++)
                            {
                                bnt[i, y].MouseDown -= Form1_Click;
                                Label complete = new Label();
                                complete.Text = "YOU DIE.";
                                complete.Font = new Font("bntribntl", 20, FontStyle.Bold);
                                complete.Size = new Size(150, 35);
                                complete.Location = new Point(100, 150);

                                Controls.Add(complete);
                                complete.BringToFront();
                            }
                        }
                    }
                    else
                    {
                        btn.showHint();
                        btn.Enabled = false;
                    }
                    break;

                case MouseButtons.Right:
                    // Right click
                    btn.Text = "#";
                    if (ChkFlag()) {
                        Label complete = new Label();
                        complete.Text = "complete";
                        complete.Font = new Font("bntribntl", 20, FontStyle.Bold);
                        complete.Size = new Size(140, 35);
                        complete.Location = new Point(100,150);
                        
                        Controls.Add(complete);
                        complete.BringToFront();
                    }
                    break;
            }   
        }
        private void Smile_Click(object sender, MouseEventArgs e)
        {
            Application.Restart();
        }
        public void InitGame()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int y = 0; y < 5; y++)
                {
                    bnt[i, y] = new MineButton();
                    bnt[i, y].Mine = maps[i, y];
                    bnt[i, y].Text = "  ";
                    bnt[i, y].TabStop = false;
                    bnt[i, y].Size = new Size(width, height);
                    bnt[i, y].Font = new Font("bntribntl", 15, FontStyle.Bold);
                    bnt[i, y].Location = new Point(i * 50 + 50, y * 50 + 50);
                    bnt[i, y].Enabled = true;
                    bnt[i, y].MouseDown += new MouseEventHandler(Form1_Click);
                    Controls.Add(bnt[i, y]);
                }
            }
            for (int i = 0; i < 5; i++)
            {
                for (int y = 0; y < 5; y++)
                {
                    try { if (bnt[i - 1, y].Mine) { bnt[i, y].Mhint++; } } catch { }
                    try { if (bnt[i + 1, y].Mine) { bnt[i, y].Mhint++; } } catch { }
                    try { if (bnt[i, y - 1].Mine) { bnt[i, y].Mhint++; } } catch { }
                    try { if (bnt[i, y + 1].Mine) { bnt[i, y].Mhint++; } } catch { }
                    try { if (bnt[i - 1, y - 1].Mine) { bnt[i, y].Mhint++; } } catch { }
                    try { if (bnt[i - 1, y + 1].Mine) { bnt[i, y].Mhint++; } } catch { }
                    try { if (bnt[i + 1, y - 1].Mine) { bnt[i, y].Mhint++; } } catch { }
                    try { if (bnt[i + 1, y + 1].Mine) { bnt[i, y].Mhint++; } } catch { }
                }
            }
        }
        public Boolean ChkFlag()
        {
            int flag_cnt=0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (maps[i, j]) {
                        if (bnt[i, j].Text == "#") {
                            flag_cnt++;
                        }
                    }
                }
            }
            if (total_mine == flag_cnt)
            {
                return true;
            }
            return false;
        }
    }
    public partial class MineButton : Button
    {
        private Boolean mine;
        private int hint;
        public Boolean Mine
        {
            get { return mine; }
            set { mine = value; }
        }
        public int Mhint
        {
            get { return hint; }
            set { hint = value; }
        }
        public void hideText() { Text = ""; }
        public void showHint() { Text = Mhint.ToString(); }
        public MineButton()
        {
            Mhint = 0;
        }
    }
}
