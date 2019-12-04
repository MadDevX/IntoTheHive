using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetRandomPlayer : ITargetUpdatable
{
    private Random _random;

    public TargetRandomPlayer()
    {
        _random = new Random();
    }

    private void Shuffle(Collider2D[] array)
    {
        var length = array.Length;
        for (int t = 0; t < length; t++)
        {
            var tmp = array[t];
            int r = Random.Range(t, length);
            array[t] = array[r];
            array[r] = tmp;
        }
    }
    public Transform GetTarget(Collider2D[] hits)
    {
        Shuffle(hits);

        for (int i = 0; i < hits.Length; i++)
        {
            var player = hits[i].GetComponent<PlayerInstaller>();
            if (player != null)
            {
                var Target = player.transform;
                Debug.Log($"Found random target: {Target.name}");
                return Target;
            }
        }

        return null;
    }
}
