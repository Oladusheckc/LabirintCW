using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labirint.LabirintsClasses
{
    internal class WickerLabirint : LabirintBase
    {
        public WickerLabirint(int heightCellMap, int widthCellMap, int scale, PictureBox pictureBox, bool displaySpecialDots, bool displayWay)
: base(heightCellMap, widthCellMap, scale, pictureBox, displaySpecialDots, displayWay)
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
            List<Cell> deadEnds = new List<Cell>();
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
            for (int i = 1; i <= widthCellMap-1; i+=2) 
            {
                for (int j = 1; j < heightCellMap-1; j+=2) 
                {
                    cl = Cells[i, j];
                    int countWalls = 0;
                    if (Cells[cl.X + 1, cl.Y].color == Color.Black)
                    {
                        countWalls++;
                    }
                    if (Cells[cl.X, cl.Y + 1].color == Color.Black)
                    {
                        countWalls++;
                    }
                    if (Cells[cl.X - 1, cl.Y].color == Color.Black)
                    {
                        countWalls++;
                    }
                    if (Cells[cl.X, cl.Y - 1].color == Color.Black)
                    {
                        countWalls++;
                    }
                    if(countWalls>2)
                    {
                        deadEnds.Add(Cells[cl.X, cl.Y]);
                    }
                }
            }
            while(deadEnds.Count !=0)
            {
                cl = deadEnds[0];
                List<Cell> cellForRandomPick = new List<Cell>();

                if (Cells[cl.X + 1, cl.Y].color == Color.Black && cl.X < widthCellMap - 2)
                {
                    cellForRandomPick.Add(Cells[cl.X + 2, cl.Y]);
                }
                if (Cells[cl.X, cl.Y + 1].color == Color.Black && cl.Y < heightCellMap - 2)
                {
                    cellForRandomPick.Add(Cells[cl.X, cl.Y + 2]);
                }
                if (Cells[cl.X - 1, cl.Y].color == Color.Black && cl.X > 1)
                {
                    cellForRandomPick.Add(Cells[cl.X - 2, cl.Y]);
                }
                if (Cells[cl.X, cl.Y - 1].color == Color.Black && cl.Y > 1)
                {
                    cellForRandomPick.Add(Cells[cl.X, cl.Y - 2]);
                }
                if(cellForRandomPick.Count == 0)
                {
                    deadEnds.RemoveAt(0);
                    continue;
                }
                else
                {
                    Cell clq = new Cell();
                    bool skipRnd = false;
                    for(int i = 1; i < deadEnds.Count; i++)
                    {
                        for (int j = 0; j < cellForRandomPick.Count; j++)
                        {
                            if (cellForRandomPick[j].X == deadEnds[i].X && cellForRandomPick[j].Y == deadEnds[i].Y)
                            {
                                clq = cellForRandomPick[j];
                                deadEnds.RemoveAt(i);
                                skipRnd = true;
                                break; 
                            }
                        }
                    }
                    if(!skipRnd)
                    {
                        clq = cellForRandomPick[rnd.Next(cellForRandomPick.Count)];
                    }
                    Cells[(cl.X + clq.X) / 2, (cl.Y + clq.Y) / 2].color = Color.White;
                    deadEnds.RemoveAt(0);
                }
            }

        }
    }
}
