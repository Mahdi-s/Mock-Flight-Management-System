﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS363finalproject
{
    public partial class Form1 : Form
    {
        int WIDTH = 300, HEIGHT = 300, HAND = 150;
        int u; //in degree
        int cx, cy; //center of circle
        int x, y; //Hand coordinate
        int tx, ty, lim = 20;

        Bitmap bmp;
        Pen p;
        Graphics g;

        Boolean warning = false;

        public Form1()
        {
            InitializeComponent();
            topographicChecked.Checked = true;
            topographicChecked.Checked = false;
            airplane2.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //create Bitmap
            bmp = new Bitmap(WIDTH + 1, HEIGHT + 1);
            //background color
            this.BackColor = Color.Black;
            //center
            cx = WIDTH / 2;
            cy = HEIGHT / 2;

            //timer
            Timer t = new Timer();
            t.Interval = 5;// in millisec
            t.Tick += new EventHandler(this.t_Tick);
            t.Start();
        }

        private void t_Tick(object sender, EventArgs e)
        {
            //pen
            if (warning)
            {
                p = new Pen(Color.Red, 1f);
            }
            else
            {
                p = new Pen(Color.Green, 1f);
            }
            //graphics 
            g = Graphics.FromImage(bmp);
            //calculate x,y, coord of hand
            int tu = (u - lim) % 360;
            if(u >= 0 && u <= 180)
            {
                //right half
                // convert u into radians from degrees
                x = cx + (int)(HAND * Math.Sin(Math.PI * u / 180));
                y = cy - (int)(HAND * Math.Cos(Math.PI * u / 180));
            }
            else
            {
                x = cx - (int)(HAND * -Math.Sin(Math.PI * u / 180));
                y = cy - (int)(HAND * Math.Cos(Math.PI * u / 180));

            }
            if (tu >= 0 && tu <= 180)
            {
                //right half
                // convert u into radians from degrees
                tx = cx + (int)(HAND * Math.Sin(Math.PI * tu / 180));
                ty = cy - (int)(HAND * Math.Cos(Math.PI * tu / 180));
            }
            else
            {
                tx = cx - (int)(HAND * -Math.Sin(Math.PI * tu / 180));
                ty = cy - (int)(HAND * Math.Cos(Math.PI * tu / 180));

            }
            #region Radar Cirlces
            //draw circle
            g.DrawEllipse(p, 0, 0, WIDTH, HEIGHT);//Outer circle
            g.DrawEllipse(p, 30, 30, WIDTH - 60, HEIGHT - 60);
            g.DrawEllipse(p, 60, 60, WIDTH - 120, HEIGHT - 120);
            g.DrawEllipse(p, 90, 90, WIDTH - 180, HEIGHT - 180);
            g.DrawEllipse(p, 120, 120, WIDTH - 240, HEIGHT - 240); //smallest cirlce
            #endregion
            #region Line & Hand 
            //draw prepen line
            g.DrawLine(p, new Point(cx, 0), new Point(cx, HEIGHT));//up down
            g.DrawLine(p, new Point(0, cy), new Point(WIDTH, cy)); //left right

            //draw HAND
            g.DrawLine(new Pen(Color.Black, 1f), new Point(cx, cy), new Point(tx, ty));
            g.DrawLine(p, new Point(cx, cy), new Point(x, y));
            #endregion

            //runway lines
            g.DrawLine(p, new Point(120, 130), new Point(180, 130));
            g.DrawLine(p, new Point(120, 132), new Point(180, 132));
            g.DrawLine(p, new Point(120, 140), new Point(180, 140));
            g.DrawLine(p, new Point(120, 142), new Point(180, 142));

            //airspace entrance lines
            g.DrawLine(p, new Point(250, 40), new Point(260, 40));
            g.DrawLine(p, new Point(30, 250), new Point(40, 250));

            //load bitmap in picture
            pictureBox2.Image = bmp;
            //dispose
            p.Dispose();
            g.Dispose();

            //update
            u++;
            if (u == 360)
            {
                u = 0;
            }

        }

        Boolean flippedA1C1 = false;
        Boolean flippedA2C1_1 = false;
        Boolean flippedA2C1_2 = false;
        Boolean flippedA3C1 = false;
        Boolean needsUpdatingC1 = true;
        int countC1 = 0;
        private void Case1Button_Click(object sender, EventArgs e)
        {
            Timer t_case1 = new Timer();
            t_case1.Interval = 50; //millisecond
            t_case1.Tick += new EventHandler(this.case1_Tick);
            t_case1.Start();

            needsUpdatingC1 = true;
            countC1 = 0;
            infoStatus.Text = "Arriving";
            infoAltitude.Text = "2000";
            infoSpeed.Text = "75";
            infoHeading.Text = "0";

            airplane1.Location = new Point(330, 275);
            airplane1.Visible = true;
            if (flippedA1C1)
            {
                airplane1.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                flippedA1C1 = false;
            }

            airplane2.Location = new Point(220, 15);
            airplane2.Visible = true;
            if(flippedA2C1_1 || flippedA2C1_2)
            {
                flippedA2C1_1 = false;
                flippedA2C1_2 = false;
            }

            airplane3.Location = new Point(110,210);
            airplane3.Visible = true;
            if (flippedA3C1)
            {
                flippedA3C1 = false;
            }
        }
        private void case1_Tick(object sender, EventArgs e)
        {
            countC1++;

            //airplane 1 (selected airplane) lands on runway
            if(airplane1.Location.Y == 150 && !flippedA1C1)
            {
                airplane1.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                flippedA1C1 = true;
                airplane1.Left -= 1;
                int altitude = Convert.ToInt32(infoAltitude.Text);
                infoAltitude.Text = Convert.ToString(altitude - 12);
                infoHeading.Text = "270";
                if (countC1 % 15 == 0)
                {
                    int speed = Convert.ToInt32(infoSpeed.Text);
                    infoSpeed.Text = Convert.ToString(speed - 4);
                }
            }
            else if (airplane1.Location.Y > 150)
            {
                airplane1.Top -= 1;
                int altitude = Convert.ToInt32(infoAltitude.Text);
                infoAltitude.Text = Convert.ToString(altitude - 6);
                if (countC1 % 15 == 0)
                {
                    int speed = Convert.ToInt32(infoSpeed.Text);
                    infoSpeed.Text = Convert.ToString(speed - 2);
                }
            }
            else if(airplane1.Visible)
            {
                airplane1.Left -= 1;
                if (needsUpdatingC1)
                {
                    int altitude = Convert.ToInt32(infoAltitude.Text);
                    infoAltitude.Text = Convert.ToString(altitude - 12);
                    if (countC1 % 15 == 0)
                    {
                        int speed = Convert.ToInt32(infoSpeed.Text);
                        infoSpeed.Text = Convert.ToString(speed - 4);
                    }
                }
                if (airplane1.Location.X == 225)
                {
                    infoStatus.Text = "Landed";
                    infoAltitude.Text = "   0";
                    infoSpeed.Text = "0";
                    infoHeading.Text = "0";
                    needsUpdatingC1 = false;
                    airplane1.Visible = false;
                }
            }

            //airplane 2 exits on top left airspace exit point
            if(airplane2.Location.X == 330)
            {
                airplane2.Visible = false;
            }
            else if(airplane2.Location.X == 280 && !flippedA2C1_1)
            {
                airplane2.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                flippedA2C1_1 = true;
                airplane2.Top += 1;
            }
            else if(airplane2.Location.Y == 30 && !flippedA2C1_2)
            {
                airplane2.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                flippedA2C1_2 = true;
            }
            else if(airplane2.Location.X < 280)
            {
                airplane2.Left += 1;
            }
            else if(airplane2.Location.Y < 30)
            {
                airplane2.Top += 1;
            }
            else if(airplane2.Visible)
            {
                airplane2.Left += 1;
            }

            //airplane 3 leads to airspace exit point
            if(airplane3.Location.X == 330)
            {
                airplane3.Visible = false;
            }
            else if(airplane3.Location.Y == 30 && !flippedA3C1)
            {
                airplane3.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                flippedA3C1 = true;
                airplane3.Left += 1;
            }
            else if (airplane3.Location.Y > 30)
            {
                airplane3.Top -= 2;
            }
            else if (airplane3.Visible)
            {
                airplane3.Left += 1;
            }
        }
        private void Case2Button_Click(object sender, EventArgs e)
        {

        }

        private void Case3Button_Click(object sender, EventArgs e)
        {

        }

        private void Case4Button_Click(object sender, EventArgs e)
        {

        }

        private void TopographicChecked_CheckedChanged(object sender, EventArgs e)
        {
            if (topographicChecked.Checked)
            {
                pictureBox2.BackgroundImage = Properties.Resources.topomap;
            }
            else
            {
                pictureBox2.BackgroundImage = Properties.Resources.black;
            }
        }
    }
}
