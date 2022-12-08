using Labirint;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using System.Security;
using System.Windows.Forms;

namespace Labirint
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(pictureBox1.Image);
            Pen pen = new Pen(Color.Aqua, 5f);
            g.DrawRectangle(pen, 0, 0, pictureBox1.Width - pen.Width / 2, pictureBox1.Height - pen.Width / 2);
            pictureBox1.Update();
            pictureBox1.Image.Save("testimg.png", System.Drawing.Imaging.ImageFormat.Png);*/
            int comboBoxSel = Convert.ToInt32(comboBox1.SelectedItem);
            PerfectLabirint map = new PerfectLabirint(comboBoxSel,comboBoxSel,10, pictureBox1,false,true);
            map.Generate();
            map.Draw();
            map.Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }

    class CellMapPicture
    {
        protected struct Cell
        {
            public int LocalX { get; }
            public int LocalY { get; }
            public Color color { get; set; }
            public Cell(int LocalX, int LocalY, Color color)
            {
                this.LocalX = LocalX;
                this.LocalY = LocalY;
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
        public void Draw()
        {
            pictureBox.Image = new Bitmap(widthPixelMap,heightPixelMap);
            Graphics g = Graphics.FromImage(pictureBox.Image);
            Pen pen = new Pen(Color.Aqua, 5f);
            g.FillRectangle(new SolidBrush(Color.Black), 0, 0, widthPixelMap, heightPixelMap);
            for (int i = 0; i < widthCellMap; i++)
            {
                for (int j = 0; j < heightCellMap; j++)
                {
                    g.FillRectangle(new SolidBrush(Cells[i,j].color),
                        (int)((widthPixelMap / (float)widthCellMap) * Cells[i, j].LocalX),
                        (int)((heightPixelMap / (float)heightCellMap) * Cells[i, j].LocalY),
                        (int)(widthPixelMap / (float)widthCellMap),
                        (int)(heightPixelMap / (float)heightCellMap));
                }
            }
            pictureBox.Update();
        }
        public void Save()
        {
            pictureBox.Image.Save("testimg.png", System.Drawing.Imaging.ImageFormat.Png);
        }

    }
    class LabirintBase : CellMapPicture
    {
        protected bool displaySpecialDots;
        protected Cell startCell;
        protected Cell endCell;
        public LabirintBase(int heightCellMap, int widthCellMap, int scale, PictureBox pictureBox, bool displaySpecialDots) 
        : base((heightCellMap * 2 + 1), (widthCellMap * 2 + 1), scale, pictureBox)
        {
            this.displaySpecialDots = displaySpecialDots;
        }
        public override void Generate()
        {
            for (int i = 0; i < widthCellMap; i++)
            {
                for (int j = 0; j < heightCellMap; j++)
                {
                    Cells[i, j] = new Cell(i, j, i % 2 + j % 2 == 2 ?  Color.White : Color.Black);
                }
            }
            if(displaySpecialDots)
            {
                Random rnd = new Random();
                int[] rndDot = { rnd.Next(widthCellMap), rnd.Next(heightCellMap) };
                while (Cells[rndDot[0], rndDot[1]].color != Color.White)
                {
                    rndDot[0] = rnd.Next(widthCellMap);
                    rndDot[1] = rnd.Next(heightCellMap);
                }
                Cells[rndDot[0], rndDot[1]].color = Color.Green;
                startCell = Cells[rndDot[0], rndDot[1]];
                while (Cells[rndDot[0], rndDot[1]].color != Color.White)
                {
                    rndDot[0] = rnd.Next(widthCellMap);
                    rndDot[1] = rnd.Next(heightCellMap);
                }
                Cells[rndDot[0], rndDot[1]].color = Color.Red;
                endCell = Cells[rndDot[0], rndDot[1]];
            }
            else
            {
                startCell = Cells[1, 1];
                endCell = Cells[widthCellMap - 2, heightCellMap - 2];
            }
        }
    }
}
class RandomLabirint :LabirintBase
{

    public RandomLabirint(int heightCellMap, int widthCellMap, int scale, PictureBox pictureBox, bool displaySpecialDots)
    : base(heightCellMap, widthCellMap, scale, pictureBox,displaySpecialDots)
    {
    }
    public override void Generate()
    {
        base.Generate();
        Random rnd = new Random();
        for (int i = 1; i < widthCellMap-1; i++)
        {
            for (int j = 1 + i % 2; j < heightCellMap-1; j+=2)
            {
                Cells[i, j] = new Cell(i, j, rnd.Next(2) == 1 ? Color.White : Color.Black);
            }
        }
    }
}
class PerfectLabirint : LabirintBase
{
    bool displayWay;
    public PerfectLabirint(int heightCellMap, int widthCellMap, int scale, PictureBox pictureBox, bool displaySpecialDots, bool displayWay)
: base(heightCellMap, widthCellMap, scale, pictureBox, displaySpecialDots)
    {
        this.displayWay = displayWay;
    }
    public override void Generate()
    {
        base.Generate();
        Stack<Cell> cells = new Stack<Cell>();
        bool [,] itsVisited = new bool[widthCellMap, heightCellMap];
        int numbVisited = 0;
        Random rnd = new Random();
        cells.Push(startCell);
        itsVisited[cells.Peek().LocalX, cells.Peek().LocalY] = true;
        numbVisited++;
        while (((widthCellMap - 1) / 2) * ((heightCellMap - 1) / 2) > numbVisited) 
        {  
            List<Cell> cellForRandomPick = new List<Cell>();
            if (cells.Peek().LocalX < widthCellMap - 2 && !itsVisited[cells.Peek().LocalX + 2, cells.Peek().LocalY]) 
            {
                cellForRandomPick.Add(Cells[cells.Peek().LocalX + 2, cells.Peek().LocalY]);
            }
            if (cells.Peek().LocalY < heightCellMap - 2 && !itsVisited[cells.Peek().LocalX, cells.Peek().LocalY + 2]) 
            {
                cellForRandomPick.Add(Cells[cells.Peek().LocalX, cells.Peek().LocalY + 2]);
            }
            if (cells.Peek().LocalX  > 1 && !itsVisited[cells.Peek().LocalX - 2, cells.Peek().LocalY]) 
            {
                cellForRandomPick.Add(Cells[cells.Peek().LocalX - 2, cells.Peek().LocalY]);
            }
            if (cells.Peek().LocalY  > 1 && !itsVisited[cells.Peek().LocalX, cells.Peek().LocalY - 2]) 
            {
                cellForRandomPick.Add(Cells[cells.Peek().LocalX, cells.Peek().LocalY - 2]);
            }
            if(displayWay)
            {
                if (endCell.Equals(cells.Peek()))
                {
                    List<Cell> cellsCopy = new List<Cell>();
                    cellsCopy = cells.ToList<Cell>(); 
                    for (int i = 0; i < cells.Count(); i++) 
                    {
                        Cells[cellsCopy[i].LocalX, cellsCopy[i].LocalY].color = Color.LightBlue;
                        if(i!=0)
                        {
                            Cells[(cellsCopy[i].LocalX+ cellsCopy[i-1].LocalX)/2, (cellsCopy[i-1].LocalY + cellsCopy[i].LocalY) / 2].color = Color.LightBlue;
                        }
                    }
                    Cells[startCell.LocalX,startCell.LocalY].color = Color.Green;
                    Cells[endCell.LocalX, endCell.LocalY].color = Color.Red;
                }
            }
            if (cellForRandomPick.Count != 0)
            {

                Cell pickedCell = cellForRandomPick[rnd.Next(cellForRandomPick.Count)];
                Cells[(cells.Peek().LocalX + pickedCell.LocalX) / 2, (cells.Peek().LocalY + pickedCell.LocalY) / 2].color = Color.White;
                cells.Push(pickedCell);
                numbVisited++;
                itsVisited[cells.Peek().LocalX, cells.Peek().LocalY] = true;

            }
            else
            {
                cells.Pop();
            }      
        }
    }
}