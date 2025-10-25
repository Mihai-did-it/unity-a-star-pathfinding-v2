using UnityEngine;
using System.Collections.Generic;

public class PathFinder : MonoBehaviour
{
    // Assignment 2: Implement AStar
    //
    // DO NOT CHANGE THIS SIGNATURE (parameter types + return type)
    // AStar will be given the start node, destination node and the target position, and should return 
    // a path as a list of positions the agent has to traverse to reach its destination, as well as the
    // number of nodes that were expanded to find this path
    // The last entry of the path will be the target position, and you can also use it to calculate the heuristic
    // value of nodes you add to your search frontier; the number of expanded nodes tells us if your search was
    // efficient
    //
    // Take a look at StandaloneTests.cs for some test cases
    public static (List<Vector3>, int) AStar(GraphNode start, GraphNode destination, Vector3 target)
    {
        List<Vector3> pathToTake = new List<Vector3>();
        int nodesExplored = 0;

        Dictionary<int, float> distanceSoFar = new Dictionary<int, float>();
        Dictionary<int, GraphNode> cameFromNode = new Dictionary<int, GraphNode>();
        Dictionary<int, Wall> howWeGotHere = new Dictionary<int, Wall>();

        List<GraphNode> openList = new List<GraphNode>();
        List<int> closedSet = new List<int>();

        distanceSoFar[start.GetID()] = 0f;
        openList.Add(start);

        while (openList.Count > 0)
        {
            // pick node with lowest cost estimate
            GraphNode best = openList[0];
            float bestCost = distanceSoFar[best.GetID()] + (best.GetCenter() - target).magnitude;

            foreach (var option in openList)
            {
                float optionCost = distanceSoFar[option.GetID()] + (option.GetCenter() - target).magnitude;
                if (optionCost < bestCost)
                {
                    best = option;
                    bestCost = optionCost;
                }
            }

            openList.Remove(best);
            closedSet.Add(best.GetID());
            nodesExplored++;

            if (best == destination)
            {
                GraphNode current = destination;
                while (cameFromNode.ContainsKey(current.GetID()))
                {
                    Wall wallUsed = howWeGotHere[current.GetID()];
                    pathToTake.Add(wallUsed.midpoint);
                    current = cameFromNode[current.GetID()];
                }

                pathToTake.Reverse();
                pathToTake.Add(target);
                return (pathToTake, nodesExplored);
            }

            foreach (GraphNeighbor neighbor in best.GetNeighbors())
            {
                GraphNode next = neighbor.GetNode();
                int nextID = next.GetID();
                if (closedSet.Contains(nextID)) continue;

                float tentative = distanceSoFar[best.GetID()] + (best.GetCenter() - next.GetCenter()).magnitude;

                if (!distanceSoFar.ContainsKey(nextID) || tentative < distanceSoFar[nextID])
                {
                    distanceSoFar[nextID] = tentative;
                    cameFromNode[nextID] = best;
                    howWeGotHere[nextID] = neighbor.GetWall();

                    if (!openList.Contains(next))
                        openList.Add(next);
                }
            }
        }

        return (new List<Vector3>() { target }, nodesExplored);
    }

    public Graph graph;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        EventBus.OnTarget += PathFind;
        EventBus.OnSetGraph += SetGraph;
    }

    void Update()
    {
    }

    public void SetGraph(Graph g)
    {
        graph = g;
    }
    // entry point
    public void PathFind(Vector3 target)
    {
        if (graph == null) return;

        // find start and destination nodes in graph
        GraphNode start = null;
        GraphNode destination = null;

        foreach (var node in graph.all_nodes)
        {
            if (Util.PointInPolygon(transform.position, node.GetPolygon()))
            {
                start = node;
            }

            if (Util.PointInPolygon(target, node.GetPolygon()))
            {
                destination = node;
            }
        }

        if (destination != null)
        {
            // only find path if destination is inside graph
            EventBus.ShowTarget(target);
            (List<Vector3> path, int expanded) = PathFinder.AStar(start, destination, target);

            Debug.Log("found path of length " + path.Count + " expanded " + expanded + " nodes, out of: " + graph.all_nodes.Count);
            EventBus.SetPath(path);
        }
    }
}