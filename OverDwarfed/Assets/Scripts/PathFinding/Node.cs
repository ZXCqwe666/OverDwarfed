namespace PathFinder
{
    public class Node
    {
        public bool walkable;
        public int gridX, gridY;
        public float penalty;

        public int gCost, hCost;
        public Node parent;

        public Node(float _price, int _gridX, int _gridY)
        {
            walkable = _price != 0f;
            penalty = _price;
            gridX = _gridX;
            gridY = _gridY;
        }
        public int FCost => gCost + hCost;
    }
}
