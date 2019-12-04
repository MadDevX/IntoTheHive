using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetClosestPlayer : ITargetUpdatable
{
    public Transform GetTarget(Collider2D[] hits)
    {
        for (int i = 0; i < hits.Length; i++)
        {
            var player = hits[i].GetComponent<PlayerInstaller>();
            if (player != null)
            {
                var Target = player.transform;
                Debug.Log($"Found closest target: {Target.name}");
                return Target;
            }
        }

        return null;
    }
}
