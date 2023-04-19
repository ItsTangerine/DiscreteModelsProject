using DiscreteModelsProject.Extensions;

namespace DiscreteModelsProject.Lab1;

public static class Lab1
{
    public static void Start()
    {
        Console.WriteLine();
        Console.WriteLine("-------- Лабораторна робота №1 --------");
        Console.WriteLine();

        var graph = GraphEx.ReadGraphFromFile("Lab1/data.txt");

        var mst = graph.FindSpanningTree();
        Console.WriteLine("\nМінімальне покриваюче дерево графа:");
        for (int i = 0; i < mst.Count; i++)
        {
            Console.Write($"Edge: {mst[i].Source + 1} -> {mst[i].Destination + 1} ({mst[i].Weight}) ");
        }
        Console.WriteLine();
        Console.WriteLine($"Total weight: {mst.Sum(x => x.Weight)}");

        var maxSt = graph.FindSpanningTree(true);
        Console.WriteLine("\nМаксимальне покриваюче дерево графа:");
        for (int i = 0; i < maxSt.Count; i++)
        {
            Console.Write($"Edge: {maxSt[i].Source + 1} -> {maxSt[i].Destination + 1} ({maxSt[i].Weight}) ");
        }
        Console.WriteLine();
        Console.WriteLine($"Total weight: {mst.Sum(x => x.Weight)}");
    }
    
    public static List<Edge> FindSpanningTree(this Graph graph, bool max = false)
    {
        List<Edge> minimumSpanningTree = new List<Edge>();
        HashSet<int> visited = new HashSet<int>();

        visited.Add(0);

        while (visited.Count < graph.NumVertices)
        {
            Edge? minEdge = null;
            int minWeight = max ? int.MinValue : int.MaxValue;

            foreach (int visitedVertex in visited)
            {
                List<Edge> adjacentEdges = graph.GetAdjacentEdges(visitedVertex);
                foreach (Edge edge in adjacentEdges)
                {
                    if (!visited.Contains(edge.Destination) && (max ? edge.Weight > minWeight : edge.Weight < minWeight))
                    {
                        minEdge = edge;
                        minWeight = edge.Weight;
                    }
                }
            }

            if (minEdge != null)
            {
                minimumSpanningTree.Add(minEdge);
                visited.Add(minEdge.Destination);
            }
        }

        return minimumSpanningTree;
    }
}