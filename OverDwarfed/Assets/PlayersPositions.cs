using System.Collections.Generic;
using UnityEngine;

public class PlayersPositions : MonoBehaviour
{
    public static PlayersPositions instance;
    public Dictionary<int,Vector3> playerPositions;

    private void Awake()
    {
        instance = this;
        playerPositions = new Dictionary<int, Vector3>()
        {
            {0, Vector3.zero},
            {1, Vector3.zero},
            {2, Vector3.zero},
            {3, Vector3.zero},
        };
    }
}
