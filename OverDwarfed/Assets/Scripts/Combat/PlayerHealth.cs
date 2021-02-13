using System.Collections;
using UnityEngine;
public class PlayerHealth : MonoBehaviour
{
    private const float flashDuration = 0.1f;
    public int hp;
    public int maxHp;
    //public Action characterDied; // maybe death will be handled here too

    private SpriteRenderer rend;
    private Material defaultMaterial, whiteFlashMatherial;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        defaultMaterial = rend.material;
        whiteFlashMatherial = Resources.Load<Material>("Materials/whiteFlash");
        hp = maxHp;
    }
    public void TakeDamage(int _amount)
    {
        StartCoroutine(WhiteFlashEffect());
        hp -= _amount;
        if (hp <= 0)
            Debug.Log("you died"); //DEATH LOGIC
    }
    public void Heal(int _amount)
    {
        hp += _amount;
        if (hp > maxHp)
            hp = maxHp;
    }
    private IEnumerator WhiteFlashEffect()
    {
        rend.material = whiteFlashMatherial;
        yield return new WaitForSeconds(flashDuration);
        rend.material = defaultMaterial;
    }
}

