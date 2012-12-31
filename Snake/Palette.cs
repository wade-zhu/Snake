using System;
using System.Collections;
using System.Drawing;
using System.Timers;

namespace Snake
{
    // 主画板，用于绘制游戏
    class Palette
    {
        private int _width = 20; // 宽度
        private int _height = 20; // 高度
        private Color _bgColor; // 背景色
        private Graphics _gpPalette; //  画布
        private ArrayList _blocks; // 蛇块列表
        private Direction _direction; // 前进方向
        private bool _directAble; // 是否可改变方向
        private Timer timerBlock; // 更新器
        private Block _food; // 当前食物
        private int _size = 20; // 单位大小
        private int _level = 0;// 游戏等级
        private bool _isGameOver = false; //是否有戏结束
        private int[] _speed = new int[] { 500, 450, 400, 350, 300, 250, 200, 150, 100, 50 };// 游戏速度

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

        // 设定方向
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

        // 开始游戏
        public void Start()
        {
            this._food = GetFood();// 生成一个食物
            // 初始化计时器
            timerBlock = new System.Timers.Timer(_speed[this._level]);
            timerBlock.Elapsed += new System.Timers.ElapsedEventHandler(OnBlockTimedEvent);
            timerBlock.AutoReset = true;
            timerBlock.Start();
        }

        // 定时更新
        private void OnBlockTimedEvent(object source, ElapsedEventArgs e)
        {
            this.Move();// 前进一个单位
            if (this.CheckDead()) // 检测是否有戏结束
            {
                this.timerBlock.Stop();
                this.timerBlock.Dispose();
                System.Windows.Forms.MessageBox.Show("Score: " + this._blocks.Count + Environment.NewLine + "Level: " + this._level, "Game Over");
            }
            else // 检查升级
            {
                int level = this._blocks.Count / 10 % 10; // 每增长10个升一级
                if (this._level != level)
                {
                    this._level = level;
                    this.timerBlock.Interval = this._speed[_level];//更新计时器速度
                }
            }
        }
        // 检查是否有戏结束
        private bool CheckDead()
        {
            Block head = (Block)(this._blocks[0]);
            if (head.Point.X < 0 || head.Point.Y < 0 || head.Point.X >= this._width || head.Point.Y >= this._height)//检查是否超出画布范围
                return true;
            for (int i = 1; i < this._blocks.Count; i++) //检查是否撞上自己
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

        // 生成下一个食物，也就是下一节蛇块
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
                for(int i=0;i<this._blocks.Count;i++)// 检查食物所在坐标是否和贪吃蛇冲突
                {
                    Block b = (Block)(this._blocks[i]);
                    if (b.Point.X == x && b.Point.Y == y)// 有冲突时，再随机找一个坐标
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

        // 前进一节
        private void Move()
        {
            Point p; // 下一个坐标位置
            Block head = (Block)(this._blocks[0]);
            if (this._direction == Direction.Up)
                p = new Point(head.Point.X, head.Point.Y - 1);
            else if (this._direction == Direction.Down)
                p = new Point(head.Point.X, head.Point.Y + 1);
            else if (this._direction == Direction.Left)
                p = new Point(head.Point.X - 1, head.Point.Y);
            else
                p = new Point(head.Point.X + 1, head.Point.Y);

            // 生成新坐标，将来成为蛇头
            Block b = new Block(Color.Red, this._size, p);

            
            if (this._food.Point != p)
                this._blocks.RemoveAt(this._blocks.Count - 1);// 如果下一个坐标不是当前食物坐标，那么从蛇块信息列表中删除最后一个蛇块
            else
                this._food = this.GetFood();//如果下一个坐标和食物坐标重合，那么就生成一个新食物

            this._blocks.Insert(0, b);// 把下一个坐标插入到蛇块信息列表的第一个，使其成为蛇头

            this.PaintPalette(this._gpPalette); // 更新画板

            this._directAble = true; // 标示可接受下一个方向
        }

        // 更新画板
        public void PaintPalette(Graphics gp)
        {
            gp.Clear(this._bgColor);
            this._food.Paint(gp);
            foreach (Block b in this._blocks)
                b.Paint(gp);
        }
    }

    // 枚举四个方向
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }
}
