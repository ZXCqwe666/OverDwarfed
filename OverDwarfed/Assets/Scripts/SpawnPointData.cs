using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpawnPointData : ScriptableObject
{
    public List<Enemy> enemiesToSpawn;
    public Sprite idleSprite;
    public RuntimeAnimatorController animController;
}
