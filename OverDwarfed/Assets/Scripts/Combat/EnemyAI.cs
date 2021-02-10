using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using PathFinder;

public class EnemyAI : MonoBehaviour
{
    private const float pathUpdateRate = 0.25f, deagringRadius = 7f, wanderingDelay = 2f, wanderingRadius = 4f;

    private List<Vector3> pathPositions; 
    private float speed = 4f, wanderSpeedMultiplier = 0.5f;
    private Rigidbody2D rb;

    public List<int>targets;
    public States state;

    private void Start() // temp
    {
        rb = GetComponent<Rigidbody2D>();
        pathPositions = new List<Vector3>();
        targets = new List<int>();
        Debug.Log(GetUnitOnCircle(Vector3.zero, -270, 1f));
        //SetState(States.idle);
    }
    private void FixedUpdate()
    {
        Move();
    }
    public void SetState(States _state)
    {
        state = _state;
        if (_state == States.chase)
        {
            speed = 4f;
            StopAllCoroutines();
            StartCoroutine(ChasePathUpdater());
        }
        if (_state == States.idle)
        {
            speed = 2f;
            if (targets.Count != 0) targets.RemoveAt(0);
            StopAllCoroutines();
            StartCoroutine(WanderPathUpdater());
        }
        if (_state == States.scared)
        {
            speed = 6f;
            StopAllCoroutines();
            StartCoroutine(ScaredPathUpdater());
        }
    }

    #region PathFinding

    private void Move()
    {
        if (pathPositions.Count != 0)
        {
            rb.velocity = (pathPositions[0] - transform.position).normalized * speed;
            if (Vector3.Distance(transform.position, pathPositions[0]) < 0.1f)
                pathPositions.RemoveAt(0);
        }
        else
            rb.velocity = Vector2.zero;  
    }
    private IEnumerator WanderPathUpdater()
    {
        while (state == States.idle)
        {
            yield return new WaitForSeconds(Random.Range(0.25f, 1f) * wanderingDelay);
            UpdatePath((Vector2)transform.position + Random.insideUnitCircle * wanderingRadius);
        } 
    }
    private IEnumerator ChasePathUpdater()
    {
        while (state == States.chase )
        {
            float distanceToTarget = Vector3.Distance(transform.position, PlayersPositions.instance.playerPositions[targets[0]]);
            if (distanceToTarget < deagringRadius)
            {
                UpdatePath(PlayersPositions.instance.playerPositions[targets[0]]);
                yield return new WaitForSeconds(pathUpdateRate);
            }
            else
                SetState(States.idle);
        }
    }
    private IEnumerator ScaredPathUpdater()
    {
        while (state == States.scared)
        {
            Vector3 direction = (transform.position - PlayersPositions.instance.playerPositions[targets[0]]).normalized;
            for(int i = 0; i < 10; i++)
            {
                UpdatePath(transform.position + direction * i );
                if (pathPositions.Count != 0)
                    yield break;
            }
            yield return new WaitForSeconds(pathUpdateRate / 2);
        }
    }
    private void UpdatePath(Vector3 endPosition)
    {
        PathPoint startPoint = PointFromVector3(transform.position);
        PathPoint endPoint = PointFromVector3(endPosition);

        pathPositions.Clear();
        foreach (PathPoint point in Pathfinding.FindPath(startPoint, endPoint))
            pathPositions.Add(PointToVector3(point));
        if (pathPositions.Count > 0)
            pathPositions[pathPositions.Count - 1] = endPosition;
    }
    private PathPoint PointFromVector3(Vector3 position)
    {
        return new PathPoint(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));
    }
    private Vector3 PointToVector3(PathPoint pathPoint)
    {
        return new Vector3(pathPoint.x + 0.5f , pathPoint.y + 0.5f);
    }
    private Vector2 GetUnitOnCircle(Vector3 center, float angleDegrees, float radius)
    {
        // initialize calculation variables
        float angleRadians = 0;
        Vector2 _returnVector;

        // convert degrees to radians
        angleRadians = angleDegrees * Mathf.PI / 180.0f;

        // get the 2D dimensional coordinates
        _returnVector.y = center.y + radius * Mathf.Sin(angleRadians);
        Debug.Log(Mathf.Sin(angleRadians));
        _returnVector.x = center.x + radius * Mathf.Cos(angleRadians);

        GetAngleFromUnitOnCircle(Vector3.zero, _returnVector, radius);
        // return the vector info
        return _returnVector;
    }
    private float GetAngleFromUnitOnCircle(Vector3 center, Vector3 pointOnCircle, float radius)
    {
        float radianDegrees = Mathf.Asin((pointOnCircle.y - center.y) / radius);
        float angle = radianDegrees *  Mathf.Rad2Deg;
        Debug.Log(angle);
        return angle;
    }
    #endregion
}
public enum States
{
    idle,
    chase,
    scared,
}