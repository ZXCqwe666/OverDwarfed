using UnityEngine;

public class EnemyAttrackter : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyAI enemy))
        {
            if (enemy.targets.Contains(0) == false)
            {
                enemy.targets.Add(0);
            }
            enemy.SetState(States.scared);
        }
    }
}
