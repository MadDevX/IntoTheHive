using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventList<T>
{
    public QuickList<T> Subscribers;
    public bool IsDirty { get; set; } = false;

    public EventList()
    {
        Subscribers = new QuickList<T>();
    }

    public void Subscribe(T subscriber)
    {
        Subscribers.Add(subscriber);
        IsDirty = true;
    }

    public void Unsubscribe(T subscriber)
    {
        if (Subscribers.Remove(subscriber))
        {
            IsDirty = true;
        }
    }
    
}


public class QuickList<T> : List<T>
{
    public QuickList(int capacity) : base(capacity) { }

    public new void RemoveAt(int index)
    {
        var t = this[index];
        var last = Count - 1;
        this[index] = this[last];
        this[last] = t;
        base.RemoveAt(last);
    }

    public new bool Remove(T obj)
    {
        var idx = IndexOf(obj);
        if (idx == -1) return false;
        RemoveAt(idx);
        return true;
    }
}
