using System;
using System.Collections;
using System.Drawing;
using System.Timers;

namespace Snake
{
    // �����壬���ڻ�����Ϸ
    class Palette
    {
        private int _width = 20; // ���
        private int _height = 20; // �߶�
        private Color _bgColor; // ����ɫ
        private Graphics _gpPalette; //  ����
        private ArrayList _blocks; // �߿��б�
        private Direction _direction; // ǰ������
        private bool _directAble; // �Ƿ�ɸı䷽��
        private Timer timerBlock; // ������
        private Block _food; // ��ǰʳ��
        private int _size = 20; // ��λ��С
        private int _level = 0;// ��Ϸ�ȼ�
        private bool _isGameOver = false; //�Ƿ���Ϸ����
        private int[] _speed = new int[] { 500, 450, 400, 350, 300, 250, 200, 150, 100, 50 };// ��Ϸ�ٶ�

        public Palette(int width, int height, int size, Color bgColor, Graphics g, int lvl)
        {
            this._width = width;
            this._height = height;
            this._bgColor = bgColor;
            this._gpPalette = g;
            this._size = size;
            this._level = lvl;
            this._blocks = new ArrayList();
            this._blocks.Insert(0, (new Block(Color.Red, this._size, new Point(width / 2, height / 2))));
            this._direction = Direction.Right;
        }

        // �趨����
        public Direction Direction
        {
            get { return this._direction; }
            set
            {
                if (this._directAble)
                {
                    this._direction = value;
                    this._directAble = false;
                }
            }
        }

        // ��ʼ��Ϸ
        public void Start()
        {
            this._food = GetFood();// ����һ��ʳ��
            // ��ʼ����ʱ��
            timerBlock = new System.Timers.Timer(_speed[this._level]);
            timerBlock.Elapsed += new System.Timers.ElapsedEventHandler(OnBlockTimedEvent);
            timerBlock.AutoReset = true;
            timerBlock.Start();
        }

        // ��ʱ����
        private void OnBlockTimedEvent(object source, ElapsedEventArgs e)
        {
            this.Move();// ǰ��һ����λ
            if (this.CheckDead()) // ����Ƿ���Ϸ����
            {
                this.timerBlock.Stop();
                this.timerBlock.Dispose();
                System.Windows.Forms.MessageBox.Show("Score: " + this._blocks.Count + Environment.NewLine + "Level: " + this._level, "Game Over");
            }
            else // �������
            {
                int level = this._blocks.Count / 10 % 10; // ÿ����10����һ��
                if (this._level != level)
                {
                    this._level = level;
                    this.timerBlock.Interval = this._speed[_level];//���¼�ʱ���ٶ�
                }
            }
        }
        // ����Ƿ���Ϸ����
        private bool CheckDead()
        {
            Block head = (Block)(this._blocks[0]);
            if (head.Point.X < 0 || head.Point.Y < 0 || head.Point.X >= this._width || head.Point.Y >= this._height)//����Ƿ񳬳�������Χ
                return true;
            for (int i = 1; i < this._blocks.Count; i++) //����Ƿ�ײ���Լ�
            {
                Block b = (Block)this._blocks[i];
                if (head.Point.X == b.Point.X && head.Point.Y == b.Point.Y)
                {
                    this._isGameOver = true;
                    return true;
                }
            }
            this._isGameOver = false;
            return false;
        }

        // ������һ��ʳ�Ҳ������һ���߿�
        private Block GetFood()
        {
            Block food = null;
            Random r = new Random();
            bool redo = false;
            while (true)
            {
                redo =false;
                int x = r.Next(this._width);
                int y = r.Next(this._height);
                for(int i=0;i<this._blocks.Count;i++)// ���ʳ�����������Ƿ��̰���߳�ͻ
                {
                    Block b = (Block)(this._blocks[i]);
                    if (b.Point.X == x && b.Point.Y == y)// �г�ͻʱ���������һ������
                    {
                        redo = true;
                    }
                }
                if (redo == false)
                {
                    food = new Block(Color.Black, this._size, new Point(x, y)); 
                    break;
                }
            }
            return food;
        }

        // ǰ��һ��
        private void Move()
        {
            Point p; // ��һ������λ��
            Block head = (Block)(this._blocks[0]);
            if (this._direction == Direction.Up)
                p = new Point(head.Point.X, head.Point.Y - 1);
            else if (this._direction == Direction.Down)
                p = new Point(head.Point.X, head.Point.Y + 1);
            else if (this._direction == Direction.Left)
                p = new Point(head.Point.X - 1, head.Point.Y);
            else
                p = new Point(head.Point.X + 1, head.Point.Y);

            // ���������꣬������Ϊ��ͷ
            Block b = new Block(Color.Red, this._size, p);

            
            if (this._food.Point != p)
                this._blocks.RemoveAt(this._blocks.Count - 1);// �����һ�����겻�ǵ�ǰʳ�����꣬��ô���߿���Ϣ�б���ɾ�����һ���߿�
            else
                this._food = this.GetFood();//�����һ�������ʳ�������غϣ���ô������һ����ʳ��

            this._blocks.Insert(0, b);// ����һ��������뵽�߿���Ϣ�б�ĵ�һ����ʹ���Ϊ��ͷ

            this.PaintPalette(this._gpPalette); // ���»���

            this._directAble = true; // ��ʾ�ɽ�����һ������
        }

        // ���»���
        public void PaintPalette(Graphics gp)
        {
            gp.Clear(this._bgColor);
            this._food.Paint(gp);
            foreach (Block b in this._blocks)
                b.Paint(gp);
        }
    }

    // ö���ĸ�����
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }
}
