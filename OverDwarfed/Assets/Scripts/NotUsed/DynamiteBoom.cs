using System.Collections;
using UnityEngine;

public class DynamiteBoom : MonoBehaviour
{
    private int explosionStrength = 100;
    private const float timeBetweenShots = 0.005f;
    public LayerMask destuctableBlocksLayer;
    void Start()
    {
        StartCoroutine(Explode());
    }
    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(Random.Range(1f, 3f));

        for (int i = 0; i < explosionStrength; i++)
        {
            yield return new WaitForSeconds(timeBetweenShots);

            Vector2 direction = Random.insideUnitSphere;
            RaycastHit2D raycast = Physics2D.Raycast(transform.position, direction, 10f, destuctableBlocksLayer);
            if (raycast)
            {
                MiningManager.instance.Mine(raycast.point + direction * 0.1f, explosionStrength);
            }
        }
        Destroy(gameObject, 0.1f);
    }
}
