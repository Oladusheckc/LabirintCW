using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labirint.LabirintsClasses
{
    internal class PerfectLabirint : LabirintBase
    {
        public PerfectLabirint(int heightCellMap, int widthCellMap, int scale, PictureBox pictureBox, bool displaySpecialDots, bool displayWay)
    : base(heightCellMap, widthCellMap, scale, pictureBox,displaySpecialDots,displayWay)
        {
        }
        public override void Generate()
        {
            base.Generate();
            Random rnd = new Random();
            Stack<Cell> wayCells = new Stack<Cell>();
            Cell cl;
            bool[,] itsVisited = new bool[widthCellMap, heightCellMap];
            int numbVisited = 0;
            wayCells.Push(startCell);
            cl = wayCells.Peek();
            itsVisited[cl.X, cl.Y] = true;
            numbVisited++;
            while (((widthCellMap - 1) / 2) * ((heightCellMap - 1) / 2) > numbVisited)
            {
                cl = wayCells.Peek();
                List<Cell> cellForRandomPick = new List<Cell>();
                if (cl.X < widthCellMap - 2 && !itsVisited[cl.X + 2, cl.Y])
                {
                    cellForRandomPick.Add(Cells[cl.X + 2, cl.Y]);
                }
                if (cl.Y < heightCellMap - 2 && !itsVisited[cl.X, cl.Y + 2])
                {
                    cellForRandomPick.Add(Cells[cl.X, cl.Y + 2]);
                }
                if (cl.X > 1 && !itsVisited[cl.X - 2, cl.Y])
                {
                    cellForRandomPick.Add(Cells[cl.X - 2, cl.Y]);
                }
                if (cl.Y > 1 && !itsVisited[cl.X, cl.Y - 2])
                {
                    cellForRandomPick.Add(Cells[cl.X, cl.Y - 2]);
                }
                if (cellForRandomPick.Count != 0)
                {

                    Cell pickedCell = cellForRandomPick[rnd.Next(cellForRandomPick.Count)];
                    Cells[(cl.X + pickedCell.X) / 2, (cl.Y + pickedCell.Y) / 2].color = Color.White;
                    wayCells.Push(pickedCell);
                    numbVisited++;
                    itsVisited[wayCells.Peek().X, wayCells.Peek().Y] = true;
                }
                else
                {
                    wayCells.Pop();
                }
            }
        }
    }
}
