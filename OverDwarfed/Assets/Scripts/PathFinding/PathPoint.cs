namespace PathFinder
{
    public class PathPoint
    {
        public int x, y;

        public PathPoint()
        {
            x = 0;
            y = 0;
        }
        public PathPoint(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
        public PathPoint(PathPoint point)
        {
            x = point.x;
            y = point.y;
        }

        #region Operators

        public PathPoint Set(int _x, int _y)
        {
            x = _x;
            y = _y;
            return this;
        }
        public override int GetHashCode()
        {
            return x ^ y;
        }
        public override bool Equals(object obj)
        {
            PathPoint point = (PathPoint)obj;
            if (point is null) return false;
            return (x == point.x) && (y == point.y);
        }
        public bool Equals(PathPoint point)
        {
            if (point is null) return false;
            return (x == point.x) && (y == point.y);
        }
        public static bool operator == (PathPoint a, PathPoint b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is null) return false;
            if (b is null) return false;
            return a.x == b.x && a.y == b.y;
        }
        public static bool operator != (PathPoint a, PathPoint b)
        {
            return !(a == b);
        }
        #endregion
    }
}
