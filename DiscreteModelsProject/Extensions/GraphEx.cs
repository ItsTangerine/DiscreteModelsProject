using DiscreteModelsProject.Lab1;

namespace DiscreteModelsProject.Extensions;

public static class GraphEx
{
    public static int[,] ToAdjacencyMatrix(this Graph graph)
    {
        int[,] adjacencyMatrix = new int[graph.NumVertices, graph.NumVertices];

        foreach (var edges in graph.AdjacencyList)
        {
            foreach (var edge in edges)
            {
                adjacencyMatrix[edge.Source, edge.Destination] = edge.Weight;
            }
        }

        return adjacencyMatrix;
    }

    public static void PrintGraph(this Graph graph)
    {
        Console.WriteLine("Матриця суміжності графа:");
        int[,] adjacencyMatrix = graph.ToAdjacencyMatrix();

        for (int i = 0; i < graph.NumVertices; i++)
        {
            for (int j = 0; j < graph.NumVertices; j++)
            {
                Console.Write(adjacencyMatrix[i, j] + " ");
            }

            Console.WriteLine();
        }
    } 
    
    public static Graph ReadGraphFromFile(string filename)
    {
        string[] lines = File.ReadAllLines($"D:/Politeh/Politeh_8sem/ДМ/DiscreteModelsProject/DiscreteModelsProject{Path.DirectorySeparatorChar}{filename}");
        int numVertices = int.Parse(lines[0]);
        Graph graph = new Graph(numVertices);

        for (int i = 0; i < numVertices; i++)
        {
            string[] row = lines[i + 1].Split(' ');

            for (int j = 0; j < numVertices; j++)
            {
                int weight = int.Parse(row[j]);

                if (weight != 0)
                {
                    graph.AddEdge(i, j, weight);
                }
            }
        }

        return graph;
    }
}