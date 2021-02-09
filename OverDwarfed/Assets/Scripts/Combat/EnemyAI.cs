using System.Collections.Generic;
using UnityEngine;
using PathFinder;

public class EnemyAI : MonoBehaviour
{
    private List<PathPoint> pathPoints;
    private List<Vector3> pathPositions; 
    private readonly float speed = 7f;

    private Vector3 lastClickPosition;
    private Camera mainCam;

    // wonder  using RandomPointInCircle + wonder radius  + mb wonder speed multiplier
    private void Start() // temp
    {
        mainCam = Camera.main;
        pathPoints = new List<PathPoint>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // current destination
        {
            lastClickPosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
            UpdatePath();
        }
        Move();
    }

    #region PathFinding

    private void Move()
    {
        if(pathPoints.Count != 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, pathPositions[0], speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, pathPositions[0]) < 0.1f)
                pathPositions.RemoveAt(0);   
        }
    }
    private void UpdatePath()
    {
        PathPoint startPoint = PointFromVector3(transform.position);
        PathPoint endPoint = PointFromVector3(lastClickPosition);

        pathPositions.Clear();
        foreach (PathPoint point in Pathfinding.FindPath(startPoint, endPoint))
            pathPositions.Add(PointToVector3(point));
    }
    private PathPoint PointFromVector3(Vector3 position)
    {
        return new PathPoint(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));
    }
    private Vector3 PointToVector3(PathPoint pathPoint)
    {
        return new Vector3(pathPoint.x + 0.5f , pathPoint.y + 0.5f);
    }
    #endregion
}