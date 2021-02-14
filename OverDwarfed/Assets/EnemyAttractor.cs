using System.Collections;
using UnityEngine;

public class EnemyAttractor : MonoBehaviour
{
    private const float tickRate = 0.2f;
    private bool playerAlive;
    public LayerMask enemyLayer;

    private void Start()
    {
        SetAliveState(true);
    }
    private IEnumerator Attractor()
    {
        while (playerAlive)
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 5f, enemyLayer);

            foreach (Collider2D collider in enemies)
            {
                if (collider.TryGetComponent(out EnemyAI enemy))
                {
                    if (enemy.state != States.chase)
                        enemy.SetState(States.chase);     
                }
            }
            yield return new WaitForSeconds(tickRate);
        }
    }
    private void SetAliveState(bool isPlayerAlive)
    {
        if (playerAlive == false && isPlayerAlive)
        {
            playerAlive = isPlayerAlive;
            StartCoroutine(Attractor());
        }
        playerAlive = isPlayerAlive;
    }
}
