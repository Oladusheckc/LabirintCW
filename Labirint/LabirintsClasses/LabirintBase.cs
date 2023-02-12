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
        class LinkedCell
        {
            public LinkedCell(Cell cell, LinkedCell link)
            {
                this.cell = cell;
                this.link = link;
            }
            public bool visited = false;
            public Cell cell;
            public LinkedCell link;
            public override bool Equals(object? obj)
            {
                if(cell.Equals((obj as LinkedCell).cell))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        protected void searchWay()
        {
            int[,] waypoint = new int[widthCellMap, heightCellMap];
            Stack<LinkedCell> CellVisited = new Stack<LinkedCell>();
            Queue<LinkedCell> waySearch = new Queue<LinkedCell>();
            waySearch.Enqueue(new LinkedCell(startCell, null));
            while (waySearch.Count != 0)
            {
                LinkedCell cell = waySearch.Dequeue();
                if(!CellVisited.Contains(cell))
                {
                    if (cell.cell.X < widthCellMap && Cells[cell.cell.X + 1, cell.cell.Y].color != Color.Black)
                    {
                        waySearch.Enqueue(new LinkedCell(Cells[cell.cell.X + 1, cell.cell.Y], cell));
                    }
                    if (cell.cell.Y < heightCellMap && Cells[cell.cell.X, cell.cell.Y + 1].color != Color.Black)
                    {
                        waySearch.Enqueue(new LinkedCell(Cells[cell.cell.X, cell.cell.Y + 1], cell));
                    }
                    if (cell.cell.X > 0 && Cells[cell.cell.X - 1, cell.cell.Y].color != Color.Black)
                    {
                        waySearch.Enqueue(new LinkedCell(Cells[cell.cell.X - 1, cell.cell.Y], cell));
                    }
                    if (cell.cell.Y > 0 && Cells[cell.cell.X, cell.cell.Y - 1].color != Color.Black)
                    {
                        waySearch.Enqueue(new LinkedCell(Cells[cell.cell.X, cell.cell.Y - 1], cell));
                    }
                }
                CellVisited.Push(cell);
                if (cell.cell.X == endCell.X && cell.cell.Y == endCell.Y)
                {
                    break;
                }
            }
            Stack<Cell> way = new Stack<Cell>();
            LinkedCell LastCell = CellVisited.Pop();
            while (true)
            {
                LastCell.cell.color = Color.PowderBlue; 
                way.Push(LastCell.cell);
                if (LastCell.cell.X == startCell.X && LastCell.cell.Y == startCell.Y)
                {
                    break;
                }
                LastCell = LastCell.link;
            }
            foreach (Cell cl in way)
            {
                Cells[cl.X, cl.Y] = cl;
            }
        }

    }
}

