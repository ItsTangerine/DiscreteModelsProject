using DiscreteModelsProject.Extensions;
using DiscreteModelsProject.Lab1;

namespace DiscreteModelsProject.Lab2;

public static class Lab2
{
    private const int Infinity = 999999;
    
    public static void Start()
    {
        Console.WriteLine();
        Console.WriteLine("-------- Лабораторна робота №2 --------");
        Console.WriteLine();

        var graph = GraphEx.ReadGraphFromFile("Lab3/data.txt");

        int[,]? eulerGraph = graph.GetEulerGraph();
        int distance = FindMinimumDistance(eulerGraph);
        Console.WriteLine($"Мінімальна дистанція для подорожі листоноші: {distance}.");
    }

    public static int[,]? GetEulerGraph(this Graph graph) => GetEulerGraph(graph.ToAdjacencyMatrix());

    private static int[,]? GetEulerGraph(int[,] graph)
    {
        int n = graph.GetLength(0);
        int[,] degreeMatrix = new int[n, n];
        int[,] eulerGraph = new int[n, n];
        int[] degree = new int[n];
        int oddCount = 0;
        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                if (graph[i, j] > 0)
                {
                    degree[i]++;
                    degree[j]++;
                    degreeMatrix[i, j] = degreeMatrix[j, i] = 1;
                    eulerGraph[i, j] = eulerGraph[j, i] = graph[i, j];
                }
            }

            if (degree[i] % 2 == 1)
            {
                oddCount++;
            }
        }

        if (oddCount == 0)
        {
            return eulerGraph;
        }

        if (oddCount == 2)
        {
            int startVertex = 0;
            for (int i = 0; i < n; i++)
            {
                if (degree[i] % 2 == 1)
                {
                    startVertex = i;
                    break;
                }
            }

            for (int i = startVertex + 1; i < n; i++)
            {
                if (degree[i] % 2 == 1)
                {
                    degreeMatrix[startVertex, i] = degreeMatrix[i, startVertex] = 1;
                    eulerGraph[startVertex, i] = eulerGraph[i, startVertex] = Infinity;
                    break;
                }
            }

            return eulerGraph;
        }
        else
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if (degreeMatrix[i, j] == 0)
                    {
                        degreeMatrix[i, j] = degreeMatrix[j, i] = 1;
                        eulerGraph[i, j] = eulerGraph[j, i] = Infinity;
                        int[,]? tempGraph = GetEulerGraph(eulerGraph);
                        if (tempGraph != null)
                        {
                            return tempGraph;
                        }

                        degreeMatrix[i, j] = degreeMatrix[j, i] = 0;
                        eulerGraph[i, j] = eulerGraph[j, i] = 0;
                    }
                }
            }

            return null;
        }
    }

    public static int FindMinimumDistance(int[,]? graph)
    {
        if (graph == null)
            return 0;

        int n = graph.GetLength(0);
        int[,] dist = new int[n, n];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                dist[i, j] = graph[i, j];
                if (dist[i, j] == 0 && i != j)
                {
                    dist[i, j] = Infinity;
                }
            }
        }

        for (int k = 0; k < n; k++)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (dist[i, k] != Infinity && dist[k, j] != Infinity && dist[i, k] + dist[k, j] < dist[i, j])
                    {
                        dist[i, j] = dist[i, k] + dist[k, j];
                    }
                }
            }
        }

        int minDistance = 0;
        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                if (graph[i, j] > 0)
                {
                    minDistance += dist[i, j];
                }
            }
        }

        return minDistance;
    }
}