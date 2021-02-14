using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    private const float flashDuration = 0.1f;
    private int hp;
    public int maxHp;

    private SpriteRenderer rend;
    private Material defaultMaterial, whiteFlashMatherial;

    private void Start()
    {
        InitializeHealth();
        rend = GetComponent<SpriteRenderer>();
        defaultMaterial = rend.material;
        whiteFlashMatherial = Resources.Load<Material>("whiteFlash");
    }
    public virtual void InitializeHealth()
    {
        hp = maxHp;
    }
    public void TakeDamage(int _amount)
    {
        StartCoroutine(WhiteFlashEffect());
        hp -= _amount;
        if (hp <= 0)
            Die();
    }
    public void Heal(int _amount)
    {
        hp += _amount;
        if (hp > maxHp)
            hp = maxHp;
    }
    private void Die()
    {
        WaveSpawner.instance.enemyCount -= 1;
        Destroy(gameObject);
    }
    private IEnumerator WhiteFlashEffect()
    {
        rend.material = whiteFlashMatherial;
        yield return new WaitForSeconds(flashDuration);
        rend.material = defaultMaterial;
    }
}
