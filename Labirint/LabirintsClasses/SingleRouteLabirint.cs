using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labirint.LabirintsClasses
{
    internal class SingleRouteLabirint : LabirintBase
    {
        public SingleRouteLabirint(int heightCellMap, int widthCellMap, int scale, PictureBox pictureBox, bool displaySpecialDots, bool displayWay)
: base(heightCellMap, widthCellMap, scale, pictureBox,displaySpecialDots,displayWay)
        {
        }
        public override void Generate()
        {
            for (int i = 0; i < widthCellMap; i++)
            {
                for (int j = 0; j < heightCellMap; j++)
                {
                    Cells[i, j] = new Cell(i, j, i % 4>0 && j % 4>0 ? Color.White : Color.Black);
                }
            }
            Random rnd = new Random();
            Stack<Cell> wayCells = new Stack<Cell>();
            Cell cl;
            bool[,] itsVisited = new bool[widthCellMap, heightCellMap];
            int numbVisited = 0;
            startCell = Cells[2,2];
            wayCells.Push(startCell);
            cl = wayCells.Peek();
            itsVisited[cl.X, cl.Y] = true;
            numbVisited++;
            while ((((widthCellMap - 1) / 2) * ((heightCellMap - 1) / 2))/4 > numbVisited)
            {
                cl = wayCells.Peek();
                List<Cell> cellForRandomPick = new List<Cell>();
                if (cl.X < widthCellMap - 4 && !itsVisited[cl.X + 4, cl.Y])
                {
                    cellForRandomPick.Add(Cells[cl.X + 4, cl.Y]);
                }
                if (cl.Y < heightCellMap - 4 && !itsVisited[cl.X, cl.Y + 4])
                {
                    cellForRandomPick.Add(Cells[cl.X, cl.Y + 4]);
                }
                if (cl.X > 3 && !itsVisited[cl.X - 4, cl.Y])
                {
                    cellForRandomPick.Add(Cells[cl.X - 4, cl.Y]);
                }
                if (cl.Y > 3 && !itsVisited[cl.X, cl.Y - 4])
                {
                    cellForRandomPick.Add(Cells[cl.X, cl.Y - 4]);
                }
                if (cellForRandomPick.Count != 0)
                {

                    Cell pickedCell = cellForRandomPick[rnd.Next(cellForRandomPick.Count)];
                    Cells[(cl.X + pickedCell.X) / 2, (cl.Y + pickedCell.Y) / 2].color = Color.White;
                    Cells[(cl.X + pickedCell.X) / 2 + ((cl.Y - pickedCell.Y) != 0 ? 1 : 0), (cl.Y + pickedCell.Y) / 2 + ((cl.X - pickedCell.X) != 0 ? 1 : 0)].color = Color.White;
                    Cells[(cl.X + pickedCell.X) / 2 - ((cl.Y - pickedCell.Y) != 0 ? 1 : 0), (cl.Y + pickedCell.Y) / 2 - ((cl.X - pickedCell.X) != 0 ? 1 : 0)].color = Color.White;
                    wayCells.Push(pickedCell);
                    numbVisited++;
                    itsVisited[wayCells.Peek().X, wayCells.Peek().Y] = true;
                }
                else
                {
                    wayCells.Pop();
                }
            }
            for(int i = 2;i<heightCellMap-2;i++)
            {
                for(int j = 2;j<widthCellMap-2;j++)
                {
                    if ((i % 2 + j % 2) == 0) Cells[j, i].color = Color.Black;
                }
            }
            recursDel(2, 2);
            void recursDel (int x,int y)
            {

                if (x > 0 && x < widthCellMap && y < (heightCellMap - 3) && Cells[x, y + 2].color == Color.Black && Cells[x, y + 3].color == Color.White && Cells[x + 1, y + 2].color == Color.White &&
                    Cells[x - 1, y + 2].color == Color.White)
                {
                    Cells[x, y + 1].color = Color.Black;
                    recursDel(x, y + 2);
                }
                if (x > 0 && x < widthCellMap && y > 2 &&Cells[x, y - 2].color == Color.Black && Cells[x, y - 3].color == Color.White && Cells[x + 1, y - 2].color == Color.White &&
                    Cells[x - 1, y - 2].color == Color.White)
                {
                    Cells[x, y - 1].color = Color.Black;
                    recursDel(x, y - 2);
                }
                if (y > 0 && y < heightCellMap && x < (widthCellMap - 3) && Cells[x + 2, y].color == Color.Black && Cells[x + 3, y].color == Color.White && Cells[x + 2, y + 1].color == Color.White &&
                    Cells[x + 2, y - 1].color == Color.White)
                {
                    Cells[x + 1, y].color = Color.Black;
                    recursDel(x + 2, y);
                }
                if (y > 0 && y < heightCellMap && x > 2 && Cells[x - 2, y].color == Color.Black && Cells[x - 3, y].color == Color.White && Cells[x - 2, y + 1].color == Color.White &&
                    Cells[x - 2, y - 1].color == Color.White) 
                {
                    Cells[x - 1, y].color = Color.Black;
                    recursDel(x - 2, y);
                }

            }
            startCell = Cells[1, 1];
            endCell = Cells[3, 1];
            Cells[2, 1].color = Color.Black;

        }
    }
}
