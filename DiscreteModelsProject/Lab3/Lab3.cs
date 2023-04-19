using DiscreteModelsProject.Extensions;
using DiscreteModelsProject.Lab1;

namespace DiscreteModelsProject.Lab3;

public static class Lab3
{
    public static void Start()
    {
        Console.WriteLine();
        Console.WriteLine("-------- Лабораторна робота №3 --------");
        Console.WriteLine();

        var graph = GraphEx.ReadGraphFromFile("Lab3/data.txt");
        
        var path = new List<int>();
        bool[] visited = new bool[graph.NumVertices];
        visited[0] = true;

        int minCost = Int32.MaxValue;
        graph.BranchAndBound(path, ref minCost, 1, 0, new List<int> { 0 }, visited);

        Console.WriteLine("\nМінімальна ціна: " + minCost);
        Console.Write("Шлях: ");
        for (var i = 0; i < path.Count; i++)
        {
            Console.Write(path[i] + 1);
            
            if (i + 1 != path.Count) 
                Console.Write(" -> ");
        }
    }
    
    public static void BranchAndBound(this Graph graph, List<int> path, ref int minCost, int level, int cost, List<int> currentPath, bool[] visited)
    {
        var adj = graph.ToAdjacencyMatrix();
        if (level == graph.NumVertices)
        {
            if (cost + adj[currentPath[graph.NumVertices - 1], currentPath[0]] < minCost)
            {
                currentPath.Add(currentPath[0]);
                path.Clear();
                path.AddRange(currentPath);
                minCost = cost + adj[currentPath[graph.NumVertices - 1], currentPath[0]];
            }
        }
        else
        {
            for (int i = 0; i < graph.NumVertices; i++)
            {
                if (!visited[i])
                {
                    List<int> newPath = new List<int>(currentPath);
                    newPath.Add(i);
                    bool[] newVisited = (bool[])visited.Clone();
                    newVisited[i] = true;

                    int newCost = cost + adj[currentPath[level - 1], i];
                    if (newCost + graph.LowerBound(newVisited) < minCost)
                    {
                        graph.BranchAndBound(path, ref minCost, level + 1, newCost, newPath, newVisited);
                    }
                }
            }
        }
    }
    
    public static int LowerBound(this Graph graph, bool[] visited)
    {
        var adj = graph.ToAdjacencyMatrix();
        int cost = 0;
        for (int i = 0; i < graph.NumVertices; i++)
        {
            if (!visited[i])
            {
                int min = int.MaxValue;
                for (int j = 0; j < graph.NumVertices; j++)
                {
                    if (i != j && !visited[j] && adj[i, j] < min)
                    {
                        min = adj[i, j];
                    }
                }
                cost += min;
            }
        }
        return cost;
    }
}