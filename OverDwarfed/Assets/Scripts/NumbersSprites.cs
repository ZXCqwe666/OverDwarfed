using UnityEngine;

public class NumbersSprites : MonoBehaviour
{
    public static NumbersSprites instance;
    public Sprite[] numbers;
    private void Awake()
    {
        instance = this;
    }
}