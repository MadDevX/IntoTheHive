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
            var network = hits[i].GetComponent<NetworkedReceiverInstaller>();
            if (player != null || network != null)
            {
                var Target = player?.transform ?? network.transform;               
                return Target;
            }
        }
        
        return null;
    }
}
