using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Rgr
{
    public partial class Form1 : Form
    {
        private int[,] _adjacencyMatrix;
        private int _numVertices;
        
        private int[] _distance;
        private int[] _previous;
        private List<int> _shortestPath;

        private int _sourceVertex = 0;
        private int _destinationVertex = 0;
        
        public Form1()
        {
            InitializeComponent();
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (_adjacencyMatrix == null)
                return;
            
            Graph_Paint(e);
        }
        
        private void Graph_Paint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            int startX = 400;
            int startY = 200;
            int radius = 120;
            
            Pen pen = new Pen(Color.Black, 2);
            Brush brush = new SolidBrush(Color.LightGray);
            Brush destinationBrush = new SolidBrush(Color.Red);
            Font font = new Font("Arial", 10);
            int diameter = 20;
            
            for (int i = 0; i < _numVertices; i++)
            {
                for (int j = i; j < _numVertices; j++)
                {
                    if (_adjacencyMatrix[i, j] != 0)
                    {
                        int x1 = (int)(startX + radius * Math.Cos(i * 2 * Math.PI / _numVertices));
                        int y1 = (int)(startY + radius * Math.Sin(i * 2 * Math.PI / _numVertices));
                        int x2 = (int)(startX + radius * Math.Cos(j * 2 * Math.PI / _numVertices));
                        int y2 = (int)(startY + radius * Math.Sin(j * 2 * Math.PI / _numVertices));
                        
                        int weight = _adjacencyMatrix[i, j]; 
                        g.DrawLine(pen, x1, y1, x2, y2);
                        g.DrawString(weight.ToString(), font, Brushes.Black, (x1 + x2) / 2, (y1 + y2) / 2);
                        
                        if (_shortestPath.Contains(i) && _shortestPath.Contains(j))
                        {
                            g.DrawCurve(new Pen(Color.Green, 4), new Point[] {new Point(x1, y1), new Point(x2, y2)});
                        }
                    }
                }
            }
            
            for (int i = 0; i < _numVertices; i++)
            {
                int x = (int)(startX + radius * Math.Cos(i * 2 * Math.PI / _numVertices));
                int y = (int)(startY + radius * Math.Sin(i * 2 * Math.PI / _numVertices));
                
                if (_shortestPath.Contains(i))
                {
                    g.FillEllipse(destinationBrush, x - diameter / 2, y - diameter / 2, diameter, diameter);
                    g.DrawEllipse(new Pen(Color.Green, 4), x - diameter / 2, y - diameter / 2, diameter, diameter);
                    g.DrawString((i+1).ToString(), font, Brushes.Black, x - diameter / 4, y - diameter / 4);
                }
                else
                {
                    g.FillEllipse(brush, x - diameter / 2, y - diameter / 2, diameter, diameter);
                    g.DrawEllipse(pen, x - diameter / 2, y - diameter / 2, diameter, diameter);
                    g.DrawString((i+1).ToString(), font, Brushes.Black, x - diameter / 4, y - diameter / 4);
                }
            }
            
        }

        private void Dijkstra(int sourceVertex)
        {
            bool[] visited = new bool[_numVertices];
            _distance[sourceVertex] = 0;

            for (int i = 0; i < _numVertices - 1; i++)
            {
                int currentVertex = MinimumDistanceVertex(visited);
                visited[currentVertex] = true;

                for (int j = 0; j < _numVertices; j++)
                {
                    if (!visited[j] && _adjacencyMatrix[currentVertex, j] != 0 &&
                        _distance[currentVertex] != int.MaxValue && _distance[currentVertex] + _adjacencyMatrix[currentVertex, j] < _distance[j])
                    {
                        _distance[j] = _distance[currentVertex] + _adjacencyMatrix[currentVertex, j];
                        _previous[j] = currentVertex;
                    }
                }
            }
        }

        private int MinimumDistanceVertex(bool[] visited)
        {
            int minDistance = int.MaxValue;
            int minDistanceVertex = -1;
            for (int i = 0; i < _numVertices; i++)
            {
                if (!visited[i] && _distance[i] <= minDistance)
                {
                    minDistance = _distance[i];
                    minDistanceVertex = i;
                }
            }
            return minDistanceVertex;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] lines = richTextBox1.Lines;
            int numVertices = int.Parse(lines[0]);
            _adjacencyMatrix = new int[numVertices, numVertices];

            for (int i = 0; i < numVertices; i++)
            {
                string[] row = lines[i + 1].Split(' ');

                for (int j = 0; j < numVertices; j++)
                {
                    _adjacencyMatrix[i, j] = int.Parse(row[j]);
                }
            }
            
            _numVertices = _adjacencyMatrix.GetLength(0);
            
            _distance = Enumerable.Repeat(int.MaxValue, _numVertices).ToArray();
            _previous = Enumerable.Repeat(-1, _numVertices).ToArray();
            
            _sourceVertex = int.Parse(textBox1.Text) - 1; 
            _destinationVertex = int.Parse(textBox2.Text) - 1; 
            
            Dijkstra(_sourceVertex);

            _shortestPath = new List<int>();
            int currentVertex = _destinationVertex;
            while (currentVertex != -1)
            {
                _shortestPath.Insert(0, currentVertex);
                currentVertex = _previous[currentVertex];
            }
            
            Invalidate();
        }
    }
}