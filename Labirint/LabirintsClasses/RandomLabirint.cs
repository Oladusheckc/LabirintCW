using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labirint.LabirintsClasses
{
    internal class RandomLabirint : LabirintBase
    {

        public RandomLabirint(int heightCellMap, int widthCellMap, int scale, PictureBox pictureBox, bool displaySpecialDots, bool displayWay)
        : base(heightCellMap, widthCellMap, scale, pictureBox,displaySpecialDots, displayWay)
        {

        }
        public override void Generate()
        {
            base.Generate();
            Random rnd = new Random();
            for (int i = 1; i < widthCellMap - 1; i++)
            {
                for (int j = 1 + i % 2; j < heightCellMap - 1; j += 2)
                {
                    Cells[i, j] = new Cell(i, j, rnd.Next(2) == 1 ? Color.White : Color.Black);
                }
            }
        }
    }
}
