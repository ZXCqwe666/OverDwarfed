using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using PathFinder;

public class EnemyAI : MonoBehaviour
{
    private const float pathUpdateRate = 0.25f, damageOnTouchInterval = 1f, deagringRadius = 7f, wanderingDelay = 2f, wanderingRadius = 4f;
    private int damage = 1;

    private List<Vector3> pathPositions;
    private float speed = 4f;
    private Rigidbody2D rb;

    public List<int> targets;
    public States state;

    public LayerMask playerLayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pathPositions = new List<Vector3>();
        targets = new List<int>();
        //SetState(States.idle);  enable later when map 
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
        if (_state == States.chase)
        {
            speed = 4f;
            StopAllCoroutines();
            StartCoroutine(ChasePathUpdater());
            StartCoroutine(DamageOnTouch());
        }
        if (_state == States.idle)
        {
            speed = 2f;
            if (targets.Count != 0) targets.RemoveAt(0);
            StopAllCoroutines();
            StartCoroutine(WanderPathUpdater());
        }
    }
    private IEnumerator DamageOnTouch()
    {
        while (state == States.chase)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2f, playerLayer);
            foreach (Collider2D collider in colliders)
            {
                if (collider.TryGetComponent(out PlayerHealth health))
                    health.TakeDamage(damage);
            }
            yield return new WaitForSeconds(damageOnTouchInterval);
        }  
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
        while (state == States.chase)
        {
            float distanceToTarget = Vector3.Distance(transform.position, PlayersPositions.instance.playerPositions[targets[0]]);
            if (distanceToTarget < deagringRadius)
            {
                UpdatePath(PlayersPositions.instance.playerPositions[targets[0]]);
                yield return new WaitForSeconds(pathUpdateRate);
            }
            else SetState(States.idle);
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
    idle,
    chase
}