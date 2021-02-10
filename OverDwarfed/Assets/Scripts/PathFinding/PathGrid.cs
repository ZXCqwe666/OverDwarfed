using System.Collections.Generic;
using Unity.Mathematics;

namespace PathFinder
{
    public class PathGrid
    {
        public Node[,] nodes;
        public int2 gridSize;

        public PathGrid(int2 _gridSize, bool[,] walkable_tiles)
        {
            gridSize = _gridSize;
            nodes = new Node[_gridSize.x, _gridSize.y];

            for (int x = 0; x < _gridSize.x; x++)
            for (int y = 0; y < _gridSize.y; y++)
            nodes[x, y] = new Node(walkable_tiles[x, y] ? 1.0f : 0.0f, x, y);
        }
        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX < 0 || checkX >= gridSize.x || checkY < 0 || checkY >= gridSize.y)
                    continue;
                else neighbours.Add(nodes[checkX, checkY]);     
            }
            return neighbours;
        }
        public void ChangeNode(int2 coord, bool walkable)
        {
            nodes[coord.x, coord.y].walkable = walkable;
        }
    }
}
