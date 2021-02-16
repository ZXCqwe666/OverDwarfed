using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using PathFinder;

public class EnemyAI : MonoBehaviour
{
    private const float damageOnTouchInterval = 1f, deagringRadius = 7f, wanderingDelay = 2f, wanderingRadius = 4f;
    private int damage = 1;

    private const float chasePathUpdateRate = 0.25f, globalChasePathUpdateRate = 3f;

    private List<Vector3> pathPositions;
    private float speed = 4f;
    private Rigidbody2D rb; // mb dont use rigidbody

    public List<int> targets;
    public States state;

    public LayerMask playerLayer;

    public void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        pathPositions = new List<Vector3>();
        targets = new List<int>() { 0 };
    }
    private void FixedUpdate()
    {
        Move();
    }
    private void Move()
    {
        if (pathPositions.Count != 0)
        {
            rb.velocity = (pathPositions[0] - transform.position).normalized * speed;
            if (Vector3.Distance(transform.position, pathPositions[0]) < 0.1f)
                pathPositions.RemoveAt(0);
        }
        else rb.velocity = Vector2.zero;
    }

    #region StateLogic

    public void SetState(States _state)
    {
        state = _state;
        rb.interpolation = RigidbodyInterpolation2D.None;
        StopAllCoroutines();
        StartCoroutine(state.ToString() + "PathUpdater");
    }
    private IEnumerator DamageOnTouch()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2f, playerLayer);
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out PlayerHealth health))
                health.TakeDamage(damage);
        }
        yield return new WaitForSeconds(damageOnTouchInterval);
    }
    private IEnumerator IdlePathUpdater()
    {
        speed = 2f;

        while (state == States.Idle)
        {
            yield return new WaitForSeconds(Random.Range(0.25f, 1f) * wanderingDelay);
            UpdatePath((Vector2)transform.position + Random.insideUnitCircle * wanderingRadius);
        }
    }
    private IEnumerator ChasePathUpdater()
    {
        speed = 4f;
        StartCoroutine(DamageOnTouch());
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        while (state == States.Chase)
        {
            float distanceToTarget = Vector3.Distance(transform.position, PlayersPositions.instance.playerPositions[targets[0]]);
            if (distanceToTarget < deagringRadius)
            {
                UpdatePath(PlayersPositions.instance.playerPositions[targets[0]]); // bad indexing fix later
                yield return new WaitForSeconds(chasePathUpdateRate);
            }
            else SetState(States.Idle);
        }
    }
    private IEnumerator GlobalChasePathUpdater()
    {
        speed = 4f;

        while (state == States.GlobalChase)
        {
            UpdatePath(PlayersPositions.instance.playerPositions[targets[0]]); // bad indexing fix later
            if (pathPositions.Count == 0) SetState(States.Idle);

            yield return new WaitForSeconds(globalChasePathUpdateRate);
        }
    }
    #endregion

    #region PathFinding

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
        return new Vector3(pathPoint.x + 0.5f, pathPoint.y + 0.5f);
    }
    #endregion
}
public enum States
{
    Idle,
    Chase,
    GlobalChase,
}