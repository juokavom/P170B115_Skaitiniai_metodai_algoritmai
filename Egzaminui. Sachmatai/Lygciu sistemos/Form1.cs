﻿//Jokubas Akramas IFF-8/12 7 var.
//P170B115 Skaitiniai metodai ir algoritmai (6 kr.)
//Egzamino užduotis. Šachmatai.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Pvz1
{
    public partial class Form1 : Form
    {
        private static char[] abc = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
        private static int[,] A;
        private static PictureBox[,] pbErr;
        private static List<Figure> blacks;
        private static List<int[]> PATHS;
        private static King whiteKing;
        private static List<int[]> whiteKingPaths;
        bool ab = false, cd = false;
        private int counter;

        public Form1()
        {
            InitializeComponent();
            Initialize();

            button2.Enabled = false;
            button3.Enabled = false;
            checkBox1.Enabled = false;
        }
        private class Cell
        {
            public char Symbol { get; set; }
            public bool Visited { get; set; }
            public bool Valid { get; set; }
            public int[] Parent { get; set; }

            public Cell(bool valid)
            {
                Symbol = '*';
                Valid = valid;
                Visited = false;
            }

            public bool Visit(int[] parent)
            {
                if (Visited || !Valid) return false;
                Parent = parent;
                Visited = true;
                return true;
            }
        }
        private abstract class Figure
        {
            public int X;
            public int Y;
            public int Name;
            public List<int[]> possiblePaths;
            public PictureBox pb;

            public static void imageLocation(int x, int y, PictureBox pb)
            {
                pb.Location = new Point(5 + x * 50, 355 - y * 50);
            }
            public void addImage(string image)
            {
                pb = new PictureBox();
                imageLocation(X, Y, pb);
                pb.Size = new Size(40, 40);
                pb.Image = Image.FromFile(image);
                pb.Visible = true;
            }
            public void Move()
            {
                Random rnd = new Random();
                int move, x1, y1;
                do
                {
                    move = rnd.Next(0, possiblePaths.Count);
                    x1 = possiblePaths[move][0];
                    y1 = possiblePaths[move][1];
                } while (A[x1, y1] != 0);
                A[X, Y] = 0;
                X = x1;
                Y = y1;
                A[X, Y] = Name;
                imageLocation(X, Y, pb);
            }
            public void Remove()
            {
                A[X, Y] = 0;
                pb.Visible = false;
            }
            public abstract void GeneratePaths();
        }
        private class King : Figure
        {
            public King(int x, int y)
            {
                X = x;
                Y = y;
            }
            public King(int x, int y, int name, string image)
            {
                X = x;
                Y = y;
                Name = name;
                addImage(image);
            }

            public override void GeneratePaths()
            {
                possiblePaths = new List<int[]>();
                possiblePaths.Add(new int[] { X, Y, 0 });
                if (X != 7 && Y != 7) possiblePaths.Add(new int[] { X + 1, Y + 1, 1 });
                if (X != 0 && Y != 7) possiblePaths.Add(new int[] { X - 1, Y + 1, 1 });
                if (X != 7 && Y != 0) possiblePaths.Add(new int[] { X + 1, Y - 1, 1 });
                if (X != 0 && Y != 0) possiblePaths.Add(new int[] { X - 1, Y - 1, 1 });
                if (X != 7) possiblePaths.Add(new int[] { X + 1, Y, 1 });
                if (X != 0) possiblePaths.Add(new int[] { X - 1, Y, 1 });
                if (Y != 7) possiblePaths.Add(new int[] { X, Y + 1, 1 });
                if (Y != 0) possiblePaths.Add(new int[] { X, Y - 1, 1 });
            }
            public void GeneratePathsWithDIrection()
            {
                possiblePaths = new List<int[]>();
                if (X != 7 && Y != 7) possiblePaths.Add(new int[] { X + 1, Y + 1, 9 });
                if (X != 0 && Y != 7) possiblePaths.Add(new int[] { X - 1, Y + 1, 7 });
                if (X != 7 && Y != 0) possiblePaths.Add(new int[] { X + 1, Y - 1, 3 });
                if (X != 0 && Y != 0) possiblePaths.Add(new int[] { X - 1, Y - 1, 1 });
                if (X != 7) possiblePaths.Add(new int[] { X + 1, Y, 6 });
                if (X != 0) possiblePaths.Add(new int[] { X - 1, Y, 4 });
                if (Y != 7) possiblePaths.Add(new int[] { X, Y + 1, 8 });
                if (Y != 0) possiblePaths.Add(new int[] { X, Y - 1, 2 });
            }
        }
        private class Bishop : Figure
        {
            public Bishop(int x, int y, int name, string image)
            {
                X = x;
                Y = y;
                Name = name;
                addImage(image);
            }

            public override void GeneratePaths()
            {
                possiblePaths = new List<int[]>();
                if (X != 7 && Y != 7)
                {
                    int deltaX = 7 - X;
                    int deltaY = 7 - Y;
                    int length = (deltaX < deltaY) ? deltaX : deltaY;
                    possiblePaths.Add(new int[] { X, Y, 0 });
                    for (int i = 1; i <= length; i++)
                    {
                        possiblePaths.Add(new int[] { X + i, Y + i, 1 });
                    }
                }
                if (X != 0 && Y != 7)
                {
                    int deltaX = X;
                    int deltaY = 7 - Y;
                    int length = (deltaX < deltaY) ? deltaX : deltaY;
                    possiblePaths.Add(new int[] { X, Y, 0 });
                    for (int i = 1; i <= length; i++)
                    {
                        possiblePaths.Add(new int[] { X - i, Y + i, 1 });
                    }
                }
                if (X != 7 && Y != 0)
                {
                    int deltaX = 7 - X;
                    int deltaY = Y;
                    int length = (deltaX < deltaY) ? deltaX : deltaY;
                    possiblePaths.Add(new int[] { X, Y, 0 });
                    for (int i = 1; i <= length; i++)
                    {
                        possiblePaths.Add(new int[] { X + i, Y - i, 1 });
                    }
                }
                if (X != 0 && Y != 0)
                {
                    int deltaX = X;
                    int deltaY = Y;
                    int length = (deltaX < deltaY) ? deltaX : deltaY;
                    possiblePaths.Add(new int[] { X, Y, 0 });
                    for (int i = 1; i <= length; i++)
                    {
                        possiblePaths.Add(new int[] { X - i, Y - i, 1 });
                    }
                }
            }
        }
        private class Horse : Figure
        {
            public Horse(int x, int y, int name, string image)
            {
                X = x;
                Y = y;
                Name = name;
                addImage(image);
            }

            public override void GeneratePaths()
            {
                possiblePaths = new List<int[]>();
                possiblePaths.Add(new int[] { X, Y, 0 });
                if (X < 7 && Y < 6) possiblePaths.Add(new int[] { X + 1, Y + 2, 1 });
                if (X < 6 && Y < 7) possiblePaths.Add(new int[] { X + 2, Y + 1, 1 });
                if (X > 0 && Y < 6) possiblePaths.Add(new int[] { X - 1, Y + 2, 1 });
                if (X > 1 && Y < 7) possiblePaths.Add(new int[] { X - 2, Y + 1, 1 });
                if (X < 7 && Y > 1) possiblePaths.Add(new int[] { X + 1, Y - 2, 1 });
                if (X < 6 && Y > 0) possiblePaths.Add(new int[] { X + 2, Y - 1, 1 });
                if (X > 0 && Y > 1) possiblePaths.Add(new int[] { X - 1, Y - 2, 1 });
                if (X > 1 && Y > 0) possiblePaths.Add(new int[] { X - 2, Y - 1, 1 });
            }
        }
        private class Rook : Figure
        {
            public Rook(int x, int y, int name, string image)
            {
                X = x;
                Y = y;
                Name = name;
                addImage(image);
            }
            public override void GeneratePaths()
            {
                possiblePaths = new List<int[]>();
                if (X != 7)
                {
                    possiblePaths.Add(new int[] { X, Y, 0 });
                    for (int x1 = X + 1; x1 <= 7; x1++)
                    {
                        possiblePaths.Add(new int[] { x1, Y, 1 });
                    }
                }
                if (X != 0)
                {
                    possiblePaths.Add(new int[] { X, Y, 0 });
                    for (int x1 = X - 1; x1 >= 0; x1--)
                    {
                        possiblePaths.Add(new int[] { x1, Y, 1 });
                    }
                }
                if (Y != 7)
                {
                    possiblePaths.Add(new int[] { X, Y, 0 });
                    for (int y1 = Y + 1; y1 <= 7; y1++)
                    {
                        possiblePaths.Add(new int[] { X, y1, 1 });
                    }
                }
                if (Y != 0)
                {
                    possiblePaths.Add(new int[] { X, Y, 0 });
                    for (int y1 = Y - 1; y1 >= 0; y1--)
                    {
                        possiblePaths.Add(new int[] { X, y1, 1 });
                    }
                }
            }
        }
        private void InitModels()
        {
            A = new int[8, 8];
            pbErr = new PictureBox[8, 8];
            counter = 0;
            fillValues(A);
            Random rnd = new Random();
            List<int[]> pos = new List<int[]>();
            if (radioButton4.Checked) // a)
            {
                pos.Add(new int[] { 4, 7 });
                pos.Add(new int[] { 7, 7 });
                pos.Add(new int[] { 5, 7 });
                pos.Add(new int[] { 1, 7 });
            }
            if (radioButton3.Checked) // b)
            {
                for (int i = 0; i < 4; i++)
                {
                    int x, y;
                    do
                    {
                        x = rnd.Next(0, 8);
                        y = rnd.Next(0, 8);
                    } while (pos.Contains(new int[] { x, y }) || (x == 4 && y == 0));
                    pos.Add(new int[] { x, y });
                }

            }
            blacks = new List<Figure>();
            blacks.Add(new King(pos[0][0], pos[0][1], 1, "Data/BK.jpg"));
            blacks.Add(new Rook(pos[1][0], pos[1][1], 2, "Data/BR.jpg"));
            blacks.Add(new Bishop(pos[2][0], pos[2][1], 3, "Data/BB.jpg"));
            blacks.Add(new Horse(pos[3][0], pos[3][1], 4, "Data/BH.jpg"));
            blacks.ForEach(figure => { A[figure.X, figure.Y] = figure.Name; figure.GeneratePaths(); chart1.Controls.Add(figure.pb); });
            PATHS = combinePaths();

            whiteKing = new King(4, 0, 8, @"Data/WK.jpg");
            chart1.Controls.Add(whiteKing.pb);
            A[whiteKing.X, whiteKing.Y] = whiteKing.Name;

            for (int i = 0; i < 8; i++)
            {
                for (int u = 0; u < 8; u++)
                {
                    PictureBox pb1 = new PictureBox();
                    pb1.Location = new Point(0, 0);
                    Figure.imageLocation(i, u, pb1);
                    pb1.Size = new Size(40, 40);
                    pb1.Image = Image.FromFile("Data/checked.png");
                    pb1.Visible = false;
                    pbErr[i, u] = pb1;
                    chart1.Controls.Add(pbErr[i, u]);
                }
            }

            PictureBox pb = new PictureBox();
            pb.Location = new Point(0, 0);
            pb.Size = new Size(400, 400);
            pb.Image = Image.FromFile(@"Data/Board.jpg");
            pb.Visible = true;
            pb.BackColor = Color.Transparent;
            chart1.Controls.Add(pb);

            findWhiteKingPaths();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            bool moved = whiteKingMove();
            button2.Enabled = false;
            if (!moved) { richTextBox1.AppendText("---NUEITI NEįMANOMA!---\n"); return; }
            else if (whiteKing.Y == 7) { richTextBox1.AppendText("---SĖKMINGAI NUEITA!---\n"); return; }
            else button2.Enabled = true;
            //---
            if (checkBox1.Checked) pathVisibility(false, PATHS);
            //---
            Random rnd = new Random();
            int u = rnd.Next(0, blacks.Count);
            blacks[u].Move();
            blacks[u].GeneratePaths();
            PATHS = combinePaths();
            //---
            if (checkBox1.Checked) pathVisibility(true, PATHS);
            findWhiteKingPaths();
        }
        public List<int[]> combinePaths()
        {
            List<int[]> paths = new List<int[]>();

            for (int q = 0; q < blacks.Count; q++)
            {
                bool skip = false;
                if (blacks[q].Name == 1 || blacks[q].Name == 4) blacks[q].possiblePaths.ForEach(i => paths.Add(i));
                else
                {
                    blacks[q].possiblePaths.ForEach(i =>
                    {
                        if (i[0] == blacks[q].X && i[1] == blacks[q].Y) skip = false;
                        if (!skip)
                        {
                            int sum = 0;
                            for (int u = 0; u < blacks.Count; u++)
                            {
                                if (u == q) continue;
                                else
                                {
                                    if (blacks[u].X == i[0] && blacks[u].Y == i[1]) sum++;
                                }
                            }
                            skip = (sum > 0) ? true : false;

                            paths.Add(i);
                        }
                    });
                }
            }
            return paths;
        }
        private void findWhiteKingPaths()
        {
            whiteKing.GeneratePathsWithDIrection();
            whiteKingPaths = new List<int[]>();
            whiteKing.possiblePaths.ForEach(i =>
            {
                bool contains = false;
                if (radioButton2.Checked) PATHS.ForEach(q => { if (q[0] == i[0] && q[1] == i[1]) contains = true; }); //d) Nekerta juodu
                else if (radioButton1.Checked) PATHS.ForEach(q => { if (q[0] == i[0] && q[1] == i[1]) { if (A[i[0], i[1]] == 1 || q[2] == 1) contains = true; }; }); //c) Kerta juodus, iskyrus karaliu
                if (!contains)
                {
                    whiteKingPaths.Add(i);
                }
            });
            //---
            //whiteKingPaths.ForEach(i => richTextBox1.AppendText(string.Format("Possible: {0} {1}, direction: {2}\n", i[0], i[1], i[2])));
            //richTextBox1.AppendText("\n");
        }
        private static List<int[]> BFS(Cell[,] A, int[] start, int[] end, out bool success)
        {
            int X_min = 0;
            int X_max = 7;
            int Y_min = 0;
            int Y_max = 7;

            int currentIndex = 0;
            int endIndex = 1;

            int[][] visited = new int[A.GetLength(0) * A.GetLength(1)][];
            visited[0] = start;
            A[start[0], start[1]].Symbol = '-';
            A[start[0], start[1]].Visited = true;
            A[start[0], start[1]].Parent = new int[] { -1, -1 };

            while (true)
            {
                bool changed = false;
                int x = visited[currentIndex][0];
                int y = visited[currentIndex][1];

                if (x > X_min && x <= X_max)
                {
                    // # # #
                    // x o #
                    // # # #
                    if (A[x - 1, y].Visit(visited[currentIndex]))
                    {
                        visited[endIndex++] = new int[] { x - 1, y };
                        changed = true;
                    }
                }
                if (x < X_max && x >= X_min)
                {
                    // # # #
                    // # o x
                    // # # #
                    if (A[x + 1, y].Visit(visited[currentIndex]))
                    {
                        visited[endIndex++] = new int[] { x + 1, y };
                        changed = true;
                    }
                }
                if (y > Y_min && y <= Y_max)
                {
                    // # # #
                    // # o #
                    // # x #
                    if (A[x, y - 1].Visit(visited[currentIndex]))
                    {
                        visited[endIndex++] = new int[] { x, y - 1 };
                        changed = true;
                    }
                }
                if (y < Y_max && y >= Y_min)
                {
                    // # x #
                    // # o #
                    // # # #
                    if (A[x, y + 1].Visit(visited[currentIndex]))
                    {
                        visited[endIndex++] = new int[] { x, y + 1 };
                        changed = true;
                    }
                }
                if (x > X_min && x <= X_max && y > Y_min && y <= Y_max)
                {
                    // # # #
                    // # o #
                    // x # #
                    if (A[x - 1, y - 1].Visit(visited[currentIndex]))
                    {
                        visited[endIndex++] = new int[] { x - 1, y - 1 };
                        changed = true;
                    }
                }
                if (x > X_min && x <= X_max && y < Y_max && y >= Y_min)
                {
                    // x # #
                    // # o #
                    // # # #
                    if (A[x - 1, y + 1].Visit(visited[currentIndex]))
                    {
                        visited[endIndex++] = new int[] { x - 1, y + 1 };
                        changed = true;
                    }
                }
                if (x < X_max && x >= X_min && y > Y_min && y <= Y_max)
                {
                    // # # #
                    // # o #
                    // # # x
                    if (A[x + 1, y - 1].Visit(visited[currentIndex]))
                    {
                        visited[endIndex++] = new int[] { x + 1, y - 1 };
                        changed = true;
                    }
                }
                if (x < X_max && x >= X_min && y < Y_max && y >= Y_min)
                {
                    // # # x
                    // # o #
                    // # # #
                    if (A[x + 1, y + 1].Visit(visited[currentIndex]))
                    {
                        visited[endIndex++] = new int[] { x + 1, y + 1 };
                        changed = true;
                    }
                }
                if (currentIndex < visited.GetLength(0)) currentIndex++;
                if (A[end[0], end[1]].Visited) { success = true; break; }
                if (!changed) { success = false; return null; }
            }
            List<int[]> list = new List<int[]>();
            list.Add(new int[] { end[0], end[1] });
            for (Cell curr = A[end[0], end[1]]; curr.Parent[0] != -1; curr = A[curr.Parent[0], curr.Parent[1]])
            {
                curr.Symbol = '-';
                list.Add(new int[] { curr.Parent[0], curr.Parent[1] });
            }
            list.Reverse();
            return list;

        }
        private int[] BFSOLUTION(Cell[,] B)
        {
            List<List<int[]>> lists = new List<List<int[]>>();
            for (int i = 0; i < 7; i++)
            {
                if (B[i, 7].Valid)
                {
                    Cell[,] C = (Cell[,])B.Clone();
                    bool sucess;
                    List<int[]> lst = BFS(C, new int[] { whiteKing.X, whiteKing.Y }, new int[] { i, 7 }, out sucess);
                    if (sucess) lists.Add(lst);
                }
            }
            if (lists.Count == 0) return new int[] { 0, 0 };
            var min = lists.Min(i => i.Count);
            int[] value = { 0, 0 };
            lists.ForEach(i => { if (i.Count == min) value = i[1]; });
            return value;
        }
        private Cell[,] spreadMatrix()
        {
            Cell[,] B = new Cell[8, 8];
            for (int i = 0; i < B.GetLength(0); i++)
            {
                for (int u = 0; u < B.GetLength(1); u++)
                {
                    B[i, u] = new Cell(true);
                    bool condition = false;
                    if (radioButton2.Checked) PATHS.ForEach(q => { if (q[0] == i && q[1] == u) condition = true; }); //d) Nekerta juodu
                    else if (radioButton1.Checked) PATHS.ForEach(q => { if (q[0] == i && q[1] == u) { if (A[i, u] == 1 || q[2] == 1) condition = true; }; }); //c) Kerta juodus, iskyrus karaliu
                    if (condition)
                    {
                        B[i, u].Valid = false; B[i, u].Symbol = 'o';
                    }
                }
            }
            return B;
        }
        private bool whiteKingMove()
        {
            Cell[,] SM = spreadMatrix();
            int[] xy = BFSOLUTION(SM);
            printMatrix(SM);
            int selectedPath = 0;
            if (xy[0] != 0 && xy[1] != 0) selectedPath = -1;
            //---
            bool left = (whiteKing.X > 3) ? true : false;
            //I virsu
            if (selectedPath == 0) whiteKingPaths.ForEach(i => { if (i[2] == 8) selectedPath = i[2]; }); // Jei i virsu
            if (left)
            {
                if (selectedPath == 0) whiteKingPaths.ForEach(i => { if (i[2] == 7) selectedPath = i[2]; }); //Jei i virsu ir kaire
                if (selectedPath == 0) whiteKingPaths.ForEach(i => { if (i[2] == 9) selectedPath = i[2]; }); //Jei i virsu ir desine
            }
            else
            {
                if (selectedPath == 0) whiteKingPaths.ForEach(i => { if (i[2] == 9) selectedPath = i[2]; }); //Jei i virsu ir kaire
                if (selectedPath == 0) whiteKingPaths.ForEach(i => { if (i[2] == 7) selectedPath = i[2]; }); //Jei i virsu ir desine
            }
            //Esamoj Y
            if (left)
            {
                if (selectedPath == 0) whiteKingPaths.ForEach(i => { if (i[2] == 4) selectedPath = i[2]; }); //Jei i kaire
                if (selectedPath == 0) whiteKingPaths.ForEach(i => { if (i[2] == 6) selectedPath = i[2]; }); //Jei i desine
            }
            else
            {
                if (selectedPath == 0) whiteKingPaths.ForEach(i => { if (i[2] == 6) selectedPath = i[2]; }); //Jei i desine
                if (selectedPath == 0) whiteKingPaths.ForEach(i => { if (i[2] == 4) selectedPath = i[2]; }); //Jei i kaire
            }
            //Zemyn
            if (left)
            {
                if (selectedPath == 0) whiteKingPaths.ForEach(i => { if (i[2] == 1) selectedPath = i[2]; }); //Jei zemyn ir i kaire
                if (selectedPath == 0) whiteKingPaths.ForEach(i => { if (i[2] == 3) selectedPath = i[2]; }); //Jei zemyn ir i desine
            }
            else
            {
                if (selectedPath == 0) whiteKingPaths.ForEach(i => { if (i[2] == 3) selectedPath = i[2]; }); //Jei zemyn ir i desine
                if (selectedPath == 0) whiteKingPaths.ForEach(i => { if (i[2] == 1) selectedPath = i[2]; }); //Jei zemyn ir i kaire
            }
            if (selectedPath == 0) whiteKingPaths.ForEach(i => { if (i[2] == 2) selectedPath = i[2]; }); //Jei zemyn
            //---
            if (selectedPath == 0) return false;
            //---
            //richTextBox1.AppendText(string.Format("Selected path: {0}\n", selectedPath));
            //richTextBox1.AppendText("\n");
            //---
            A[whiteKing.X, whiteKing.Y] = 0;
            int[] nextPoint;
            if (selectedPath == -1)
            {
                nextPoint = xy;
            }
            else
            {
                nextPoint = whiteKingPaths.Find(i => i[2] == selectedPath);
            }

            Figure remove = null;
            blacks.ForEach(i => { if (i.X == nextPoint[0] && i.Y == nextPoint[1]) { i.Remove(); remove = i; }; }); //Jei kerta juodus
            if (remove != null) blacks.Remove(remove);

            whiteKing.X = nextPoint[0];
            whiteKing.Y = nextPoint[1];
            A[whiteKing.X, whiteKing.Y] = whiteKing.Name;
            Figure.imageLocation(whiteKing.X, whiteKing.Y, whiteKing.pb);
            richTextBox1.AppendText(string.Format("Ėjimas: {0}, Koordinatės: {1}{2}\n", ++counter, abc[whiteKing.X], whiteKing.Y));
            //---
            return true;
        }

        //KITI METODAi
        public void pathVisibility(bool val, List<int[]> paths)
        {
            paths.ForEach(i => pbErr[i[0], i[1]].Visible = val);
        }
        private void fillValues(int[,] A)
        {
            for (int i = 0; i < A.GetLength(0); i++)
            {
                for (int u = 0; u < A.GetLength(1); u++)
                {
                    A[i, u] = 0;
                }
            }

        }
        private void printMatrix(int[,] A)
        {
            for (int i = A.GetLength(1) - 1; i >= 0; i--)
            {
                for (int u = 0; u < A.GetLength(0); u++)
                {
                    Write(A[u, i].ToString());
                }
                WriteLine("");
            }

        }
        private void printMatrix(Cell[,] array)
        {
            string line = new string('-', array.GetLength(0) * 2 - 1);
            System.Diagnostics.Debug.WriteLine(line);
            for (int y = array.GetLength(1) - 1; y >= 0; y--)
            {
                for (int x = 0; x < array.GetLength(0); x++)
                {
                    System.Diagnostics.Debug.Write(string.Format("{0} ", array[x, y].Symbol));
                }
                System.Diagnostics.Debug.WriteLine("");
            }
            System.Diagnostics.Debug.WriteLine(line);
        }
        private void Write(string text)
        {
            System.Diagnostics.Debug.Write(text);
        }
        private void WriteLine(string text)
        {
            Write(text + "\n");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            pathVisibility(checkBox1.Checked, PATHS);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            ClearForm1();
        }
        public void ClearForm1()
        {
            chart1.Controls.Clear();
            richTextBox1.Clear();
            InitModels();
            button2.Enabled = true;
            pathVisibility(checkBox1.Checked, PATHS);
        }
        private void groupBox2_Enter(object sender, EventArgs e)
        {
            ab = true;
            enableButtons();
        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {
            cd = true;
            enableButtons();
        }
        private void enableButtons()
        {
            if (ab && cd)
            {
                button2.Enabled = true;
                button3.Enabled = true;
                checkBox1.Enabled = true;
                InitModels();
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            ClearForm1();
        }
    }
}