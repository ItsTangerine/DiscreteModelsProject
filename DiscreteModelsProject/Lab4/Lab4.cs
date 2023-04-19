using DiscreteModelsProject.Extensions;
using DiscreteModelsProject.Lab1;

namespace DiscreteModelsProject.Lab4;

public static class Lab4
{
    public static void Start()
    {
        Console.WriteLine();
        Console.WriteLine("-------- Лабораторна робота №4 --------");
        Console.WriteLine();

        var graph = GraphEx.ReadGraphFromFile("Lab4/data.txt");
        
        var result = graph.FordFulkerson(0, 7);
        Console.WriteLine($"Максимальний потік: {result}");
    }
    
    private static int FordFulkerson(this Graph g, int s, int t)
    {
        var _v = g.NumVertices;
        int[,] rGraph = g.ToAdjacencyMatrix();

        int[] parent = new int[_v];
        int maxFlow = 0;

        while (g.BFS(rGraph, s, t, parent))
        {
            int pathFlow = int.MaxValue;
            for (int v = t; v != s; v = parent[v])
            {
                int u = parent[v];
                pathFlow = Math.Min(pathFlow, rGraph[u, v]);
            }

            for (int v = t; v != s; v = parent[v])
            {
                int u = parent[v];
                rGraph[u, v] -= pathFlow;
                rGraph[v, u] += pathFlow;
            }

            maxFlow += pathFlow;
        }

        return maxFlow;
    }

    private static bool BFS(this Graph graph, int[,] rGraph, int s, int t, int[] parent)
    {
        bool[] visited = new bool[graph.NumVertices];
        for (int i = 0; i < graph.NumVertices; ++i)
        {
            visited[i] = false;
        }

        Queue<int> q = new Queue<int>();
        q.Enqueue(s);
        visited[s] = true;
        parent[s] = -1;

        while (q.Count != 0)
        {
            int u = q.Dequeue();

            for (int v = 0; v < graph.NumVertices; ++v)
            {
                if (visited[v] == false && rGraph[u, v] > 0)
                {
                    q.Enqueue(v);
                    parent[v] = u;
                    visited[v] = true;
                }
            }
        }

        return visited[t] == true;
    }
}