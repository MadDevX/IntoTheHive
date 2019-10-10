using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLoop.Internal
{
    public static class Utility
    {
        public static bool RefreshEventList<T>(EventList<T> source, EventList<T> destination)
        {
            if(source.IsDirty)
            {
                var srcSubs = source.Subscribers;
                var destSubs = destination.Subscribers;
                var srcCount = srcSubs.Count;
                var destCount = destSubs.Count;
                var limit = Mathf.Min(srcCount, destCount);
                int i;
                for (i = 0; i < limit; i++)
                {
                    destSubs[i] = srcSubs[i];
                }
                //Destination smaller than source
                if(limit != srcCount)
                {
                    while(i < srcCount)
                    {
                        destSubs.Add(srcSubs[i]);
                        i++;
                    }
                }
                //Destination larger than source
                else
                {
                    destSubs.RemoveRange(i, destCount - i);
                }
                source.IsDirty = false;
                return true;

            }
            else
            {
                return false;
            }
        }
    }
}