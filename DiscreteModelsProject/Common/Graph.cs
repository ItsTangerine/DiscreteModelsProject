using DiscreteModelsProject.Extensions;

namespace DiscreteModelsProject.Lab1;

public class Graph
{
    public int NumVertices;
    public List<Edge>[] AdjacencyList;

    public Graph(int numVertices)
    {
        this.NumVertices = numVertices;
        AdjacencyList = new List<Edge>[numVertices];

        for (int i = 0; i < numVertices; i++)
        {
            AdjacencyList[i] = new List<Edge>();
        }
    }
    
    public List<Edge> GetAdjacentEdges(int vertex)
    {
        return AdjacencyList[vertex];
    }

    public void AddEdge(int source, int destination, int weight)
    {
        Edge edge = new Edge(source, destination, weight);
        AdjacencyList[source].Add(edge);
    }
}