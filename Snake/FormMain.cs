using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Snake
{
    public partial class FormMain : Form
    {
        private Palette p;

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            // ���廭���������Լ�ÿ���߿�Ĵ�С
            int width, height, size;
            width = height = 20;
            size = 15;

            // �趨��Ϸ���ڵĴ�С
            this.pictureBox1.Width = width * size;
            this.pictureBox1.Height = height * size;
            this.Width = pictureBox1.Width + 30;
            this.Height = pictureBox1.Height + 60;

            // ����һ���»���(��ȣ��߶ȣ���λ��С������ɫ����ͼ�������Ϸ�ȼ�)
            p = new Palette(width, height, size, this.pictureBox1.BackColor, Graphics.FromHwnd(this.pictureBox1.Handle), 5);
            // ��Ϸ��ʼ
            p.Start();
        }

        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.W || e.KeyCode == Keys.Up) && p.Direction != Direction.Down)
            {
                p.Direction = Direction.Up;
                return;
            }
            if ((e.KeyCode == Keys.D || e.KeyCode == Keys.Right) && p.Direction != Direction.Left)
            {
                p.Direction = Direction.Right;
                return;
            }
            if ((e.KeyCode == Keys.S || e.KeyCode == Keys.Down) && p.Direction != Direction.Up)
            {
                p.Direction = Direction.Down;
                return;
            }
            if ((e.KeyCode == Keys.A || e.KeyCode == Keys.Left) && p.Direction != Direction.Right)
            {
                p.Direction = Direction.Left;
                return;
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (p != null)
            {
                p.PaintPalette(e.Graphics);
            }
        }
    }
}