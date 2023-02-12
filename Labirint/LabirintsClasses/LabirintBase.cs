using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Labirint.LabirintsClasses
{
    internal class LabirintBase : CellMapPicture
    {
        protected Cell startCell;
        protected Cell endCell;
        protected bool displaySpecialDots;
        protected bool displayWay;
        int scale;
        public LabirintBase(int heightCellMap, int widthCellMap, int scale, PictureBox pictureBox, bool displaySpecialDots, bool displayWay)
        : base((heightCellMap * 2 + 1), (widthCellMap * 2 + 1), scale, pictureBox)
        {
            this.displaySpecialDots = displaySpecialDots;
            this.displayWay = displayWay;
            this.scale = scale;
        }
        public override void Generate()
        {

            for (int i = 0; i < widthCellMap; i++)
            {
                for (int j = 0; j < heightCellMap; j++)
                {
                    Cells[i, j] = new Cell(i, j, i % 2 + j % 2 == 2 ? Color.White : Color.Black);
                }
            }
            startCell = Cells[1, 1];
            endCell = Cells[widthCellMap - 2, heightCellMap - 2];
        }
        public void MouseClickOnLabirint(PictureBox pb, MouseEventArgs e)
        {
            double k = Math.Min(1.0 * pb.Width / widthPixelMap, 1.0 * pb.Height / heightPixelMap);
            Cell bufCell = Cells[(int)((e.Location.X - (pb.Width / 2 - widthPixelMap / 2 * k)) / k) / scale,
        (int)((e.Location.Y - (pb.Height / 2 - heightPixelMap / 2 * k)) / k) / scale];
            if (bufCell.color != Color.Black)
            {
                foreach (Cell cl in Cells)
                {
                    if (cl.color != Color.Black)
                    {
                        Cells[cl.X, cl.Y].color = Color.White;
                    }
                }
                if (e.Button == MouseButtons.Left)
                {
                    startCell = bufCell;
                }
                else if (e.Button == MouseButtons.Right)
                {
                    endCell = bufCell;
                }
                Draw();
            }

        }
        public override void Draw()
        {
            if (displayWay)
            {
                searchWay();
            }
            if (displaySpecialDots)
            {
                Cells[startCell.X, startCell.Y].color = Color.Green;
                Cells[endCell.X, endCell.Y].color = Color.Red;
            }
            base.Draw();
        }
        protected void searchWay()
        {
            int[,] waypoint = new int[widthCellMap, heightCellMap];
            foreach(Cell cl in Cells)
            {
                if(cl.color == Color.Black)
                {
                    waypoint[cl.X, cl.Y] = -1;
                }
                else
                {
                    waypoint[cl.X, cl.Y] = 0;
                }
            }
            void recWave(int nowX, int nowY)
            {

                if (nowX < widthCellMap && waypoint[nowX + 1, nowY] == 0)
                {
                    waypoint[nowX + 1, nowY] = waypoint[nowX, nowY] + 1;
                    recWave(nowX + 1, nowY);
                }
                if (nowY < heightCellMap && waypoint[nowX, nowY + 1] == 0)
                {
                    waypoint[nowX, nowY + 1] = waypoint[nowX, nowY] + 1;
                    recWave(nowX, nowY + 1);
                }
                if (nowX > 0 && waypoint[nowX - 1, nowY] == 0)
                {
                    waypoint[nowX - 1, nowY] = waypoint[nowX, nowY] + 1;
                    recWave(nowX - 1, nowY);
                }
                if (nowY > 0 && waypoint[nowX, nowY - 1] == 0)
                {
                    waypoint[nowX, nowY - 1] = waypoint[nowX, nowY] + 1;
                    recWave(nowX, nowY - 1);
                }
            }
            recWave(startCell.X, startCell.Y);
            Stack<Cell> way = new Stack<Cell>();
            Cell point = new Cell(endCell.X, endCell.Y, Color.PowderBlue);
            for (int i = waypoint[endCell.X, endCell.Y]; i > 1; i--) 
            {
                if (point.X < widthCellMap - 1 && waypoint[point.X + 1, point.Y] == i - 1) 
                {
                    point = new Cell(point.X + 1, point.Y, Color.PowderBlue);
                    way.Push(point);
                }
                else if (point.Y < heightCellMap - 1 && waypoint[point.X, point.Y + 1] == i - 1) 
                {
                    point = new Cell(point.X, point.Y + 1, Color.PowderBlue);
                    way.Push(point);
                }
                else if (point.X > 0 && waypoint[point.X - 1, point.Y] == i - 1) 
                {
                    point = new Cell(point.X - 1, point.Y, Color.PowderBlue);
                    way.Push(point);
                }
                else if (point.Y > 0 && waypoint[point.X, point.Y - 1] == i - 1) 
                {
                    point = new Cell(point.X, point.Y - 1, Color.PowderBlue);
                    way.Push(point);
                }
            }
            foreach (Cell cl in way)
            {
                Cells[cl.X, cl.Y] = cl;
            }
        }
        
    }
}

