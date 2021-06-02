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
            var network = hits[i].GetComponent<NetworkedReceiverInstaller>();
            if (player != null || network != null)
            {
                var Target = player?.transform ?? network.transform;
                Debug.Log($"Found furthest target: {Target.name}");
                return Target;
            }
        }

        return null;
    }
}
