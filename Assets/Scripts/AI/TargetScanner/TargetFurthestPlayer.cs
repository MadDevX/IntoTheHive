using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFurthestPlayer : ITargetUpdatable
{

    public Transform GetTarget(Collider2D[] hits)
    {
        for (int i = hits.Length - 1; i >= 0; i--)
        {
            var player = hits[i].GetComponent<PlayerInstaller>();
            if (player != null)
            {
                var Target = player.transform;
                Debug.Log($"Found furthest target: {Target.name}");
                return Target;
            }
        }

        return null;
    }
}
