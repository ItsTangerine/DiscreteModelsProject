using DiscreteModelsProject.Extensions;
using DiscreteModelsProject.Lab1;

namespace DiscreteModelsProject.Lab4;

public static class Lab5
{
    public static void Start()
    {
        Console.WriteLine();
        Console.WriteLine("-------- Лабораторна робота №5 --------");
        Console.WriteLine();

        var graph = GraphEx.ReadGraphFromFile("Lab5/data.txt");
        var graph2 = GraphEx.ReadGraphFromFile("Lab5/data2.txt");
        var graph3 = GraphEx.ReadGraphFromFile("Lab5/data3.txt");
        
        Console.WriteLine("Граф 1:");
        graph.PrintGraph();
        Console.WriteLine("Граф 2:");
        graph2.PrintGraph();
        Console.WriteLine("Граф 3:");
        graph3.PrintGraph();
        
        var result = graph.Isomorphic(graph2);
        Console.WriteLine($"Графи 1 та 2: {(result ? "" : "не ")} iзоморфнi.");
        
        result = graph.Isomorphic(graph3);
        Console.WriteLine($"Графи 1 та 3: {(result ? "" : "не ")} iзоморфнi.");
    }
    
    private static bool Isomorphic(this Graph g1, Graph g2)
    {
        int vertices1 = g1.NumVertices;
        int vertices2 = g2.NumVertices;

        var adj1 = g1.ToAdjacencyMatrix();
        var adj2 = g2.ToAdjacencyMatrix();
        
        if (vertices1 != vertices2)
        {
            return false;
        }

        int[] usedVertices2 = new int[vertices2];

        for (int i = 0; i < vertices1; i++)
        {
            int degree1 = 0;
            int degree2 = 0;
            int candidate = -1;

            for (int j = 0; j < vertices2; j++)
            {
                if (usedVertices2[j] == 0 && adj1[i, i] == adj2[j, j])
                {
                    int k;
                    for (k = 0; k < vertices1; k++)
                    {
                        if (adj1[i, k] != adj1[k, i] && adj1[i, k] != 0 && adj1[i, k] == adj2[j, k])
                        {
                            degree1++;
                        }

                        if (adj2[j, k] != adj2[k, j] && adj2[j, k] != 0 && adj2[j, k] == adj1[i, k])
                        {
                            degree2++;
                        }
                    } if (degree1 == degree2 && (candidate == -1 || degree1 > degree2))
                    {
                        candidate = j;
                    }
                }
            }

            if (candidate == -1)
            {
                return false;
            }

            usedVertices2[candidate] = 1;
        }

        return true;
    }
}