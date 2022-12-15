using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labirint.LabirintsClasses
{
    internal class CellMapPicture
    {
        protected struct Cell
        {
            public int X { get; }
            public int Y { get; }
            public Color color { get; set; }
            public Cell(int LocalX, int LocalY, Color color)
            {
                this.X = LocalX;
                this.Y = LocalY;
                this.color = color;
            }
        }
        PictureBox pictureBox;
        protected int heightCellMap;
        protected int widthCellMap;
        protected int heightPixelMap;
        protected int widthPixelMap;
        protected Cell[,] Cells;
        public CellMapPicture(int heightCellMap, int widthCellMap, int scale, PictureBox pictureBox)
        {
            this.heightCellMap = heightCellMap;
            this.widthCellMap = widthCellMap;
            this.Cells = new Cell[this.widthCellMap, this.heightCellMap];
            this.pictureBox = pictureBox;
            this.heightPixelMap = this.heightCellMap * scale;
            this.widthPixelMap = this.widthCellMap * scale;
        }

        public virtual void Generate()
        {
            for (int i = 0; i < widthCellMap; i++)
            {
                for (int j = 0; j < heightCellMap; j++)
                {
                    Cells[i, j] = new Cell(i, j, i % 2 + j % 2 == 1 ? Color.Black : Color.White);
                }
            }
        }
        public virtual void Draw()
        {
            pictureBox.Image = new Bitmap(widthPixelMap, heightPixelMap);
            Graphics g = Graphics.FromImage(pictureBox.Image);
            Pen pen = new Pen(Color.Aqua, 5f);
            g.FillRectangle(new SolidBrush(Color.Black), 0, 0, widthPixelMap, heightPixelMap);
            for (int i = 0; i < widthCellMap; i++)
            {
                for (int j = 0; j < heightCellMap; j++)
                {
                    g.FillRectangle(new SolidBrush(Cells[i, j].color),
                        (int)((widthPixelMap / (float)widthCellMap) * Cells[i, j].X),
                        (int)((heightPixelMap / (float)heightCellMap) * Cells[i, j].Y),
                        (int)(widthPixelMap / (float)widthCellMap),
                        (int)(heightPixelMap / (float)heightCellMap));
                }
            }
            pictureBox.Update();
        }
        public void Save()
        {
            SaveFileDialog savedialog = new SaveFileDialog();
            savedialog.Title = "Сохранить картинку как...";
            savedialog.OverwritePrompt = true;
            savedialog.CheckPathExists = true;
            savedialog.Filter = "Image Files(*.BMP)|*.BMP|Image Files(*.JPG)|*.JPG|Image Files(*.GIF)|*.GIF|Image Files(*.PNG)|*.PNG|All files (*.*)|*.*";
            if (savedialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pictureBox.Image.Save(savedialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                }
                catch
                {
                    MessageBox.Show("Невозможно сохранить изображение", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


    }
}
