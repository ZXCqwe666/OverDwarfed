using UnityEngine;

[CreateAssetMenu]
public class BuildingData : ScriptableObject
{
    public int recipeId;
    public int maxHealth;
    public Sprite buildingSprite, buildingIcon;
    public Vector2 size;
    public Vector2 colliderSize;
    public Vector2 colliderOffset;

    public string buildingName;
    public string description;

    public bool isProductionBuilding;
    public int[] recipeIds;
}