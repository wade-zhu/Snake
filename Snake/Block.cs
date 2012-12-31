using System;
using System.Drawing;

namespace Snake
{
    // 蛇块
    class Block
    {
        private Color _color; // 颜色
        private int _size; // 单位大小
        private Point _point; //坐标

        
        public Block(Color color, int size, Point p)
        {
            this._color = color;
            this._size = size;
            this._point = p;
        }

        public Point Point
        {
            get { return this._point; }
        }
        // 绘制自身到画布
        public virtual void Paint(Graphics g)
        {
            SolidBrush sb = new SolidBrush(_color);
            lock (g)
            {
                try
                {
                    g.FillRectangle(sb, this.Point.X * this._size, this.Point.Y * this._size, this._size - 1, this._size - 1);
                }
                catch 
                { }
            }
        }
    }
}
