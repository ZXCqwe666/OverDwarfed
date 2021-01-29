using UnityEngine;

[CreateAssetMenu]
public class BuildingData : ScriptableObject
{
    public int recipeId;
    public int maxHealth;
    public Sprite buildingSprite;
    public Vector2 size;
    public Vector2 colliderSize;
    public Vector2 colliderOffset;

    public bool isProductionBuilding;
    public int[] recipeIds;

    public bool isTurret;
    public float damage, aimRadius, reloadTime;
}