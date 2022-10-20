using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parkour System/New Parkour Action")]
public class ParkourAction : ScriptableObject
{
    [SerializeField] string animName;

    [SerializeField] float minHeight;
    [SerializeField] float maxHeight;

    public bool CheckIfPossible(ObstacleHitData hitData, Transform player)
    {
        float height = hitData.heightHit.point.y - player.position.y;
        if (height < minHeight || height > maxHeight)
            return false;

        return true;
    }

    public string AnimName => animName;
}
