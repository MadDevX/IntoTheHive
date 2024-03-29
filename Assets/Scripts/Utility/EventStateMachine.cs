﻿using System;
using System.Collections.Generic;


//Without arguments
public class EventStateMachine<EnumType> where EnumType : struct, IConvertible
{
    public EnumType State { get; private set; }

    private Dictionary<EnumType, Action> _onStateInitialized = new Dictionary<EnumType, Action>();
    private Dictionary<EnumType, Action> _onStateDisposed = new Dictionary<EnumType, Action>();

    public virtual void SetState(EnumType state)
    {
        GetOnDisposeEvent(State)?.Invoke();
        GetOnInitEvent(state)?.Invoke();
        State = state;
    }

    public EventStateMachine()
    {
        InitDictionaries();
    }

    /// <summary>
    /// Add method to be invoked when specified state is set.
    /// </summary>
    /// <param name="state">State to which initialization method will be added.</param>
    /// <param name="method">Method that will be invoked after specified state is set.</param>
    public void SubscribeToInit(EnumType state, Action method)
    {
        var evt = GetOnInitEvent(state);
        evt += method;
        SetOnInitEvent(state, evt);
    }

    public void UnsubscribeFromInit(EnumType state, Action method)
    {
        var evt = GetOnInitEvent(state);
        evt -= method;
        SetOnInitEvent(state, evt);
    }

    /// <summary>
    /// Add method to be invoked when specified state is replaced with another one.
    /// </summary>
    /// <param name="state">State to which dispose method will be added.</param>
    /// <param name="method">Method that will be invoked before specified state is replaced with another one.</param>
    public void SubscribeToDispose(EnumType state, Action method)
    {
        var evt = GetOnDisposeEvent(state);
        evt += method;
        SetOnDisposeEvent(state, evt);
    }

    public void UnsubscribeFromDispose(EnumType state, Action method)
    {
        var evt = GetOnDisposeEvent(state);
        evt -= method;
        SetOnDisposeEvent(state, evt);
    }

    private void InitDictionaries()
    {
        foreach (EnumType state in Enum.GetValues(typeof(EnumType)))
        {
            _onStateInitialized.Add(state, null);
            _onStateDisposed.Add(state, null);
        }
    }

    private Action GetOnInitEvent(EnumType state)
    {
        return _onStateInitialized[state];
    }

    private Action GetOnDisposeEvent(EnumType state)
    {
        return _onStateDisposed[state];
    }

    private void SetOnInitEvent(EnumType state, Action evt)
    {
        _onStateInitialized[state] = evt;
    }

    private void SetOnDisposeEvent(EnumType state, Action evt)
    {
        _onStateDisposed[state] = evt;
    }
}

//With arguments
public class EventStateMachine<EnumType, ActionArgs> where EnumType : struct, IConvertible
{
    public EnumType State { get; private set; }

    private Dictionary<EnumType, Action<ActionArgs>> _onStateInitialized = new Dictionary<EnumType, Action<ActionArgs>>();
    private Dictionary<EnumType, Action<ActionArgs>> _onStateDisposed = new Dictionary<EnumType, Action<ActionArgs>>();

    public void SetState(EnumType state, ActionArgs e)
    {
        GetOnDisposeEvent(State)?.Invoke(e);
        GetOnInitEvent(state)?.Invoke(e);
        State = state;
    }

    public EventStateMachine()
    {
        InitDictionaries();
    }

    public void SubscribeToInit(EnumType state, Action<ActionArgs> method)
    {
        var evt = GetOnInitEvent(state);
        evt += method;
        SetOnInitEvent(state, evt);
    }

    public void UnsubscribeFromInit(EnumType state, Action<ActionArgs> method)
    {
        var evt = GetOnInitEvent(state);
        evt -= method;
        SetOnInitEvent(state, evt);
    }

    public void SubscribeToDispose(EnumType state, Action<ActionArgs> method)
    {
        var evt = GetOnDisposeEvent(state);
        evt += method;
        SetOnDisposeEvent(state, evt);
    }

    public void UnsubscribeFromDispose(EnumType state, Action<ActionArgs> method)
    {
        var evt = GetOnDisposeEvent(state);
        evt -= method;
        SetOnDisposeEvent(state, evt);
    }

    private void InitDictionaries()
    {
        foreach (EnumType state in Enum.GetValues(typeof(EnumType)))
        {
            _onStateInitialized.Add(state, null);
            _onStateDisposed.Add(state, null);
        }
    }

    private Action<ActionArgs> GetOnInitEvent(EnumType state)
    {
        return _onStateInitialized[state];
    }

    private Action<ActionArgs> GetOnDisposeEvent(EnumType state)
    {
        return _onStateDisposed[state];
    }

    private void SetOnInitEvent(EnumType state, Action<ActionArgs> evt)
    {
        _onStateInitialized[state] = evt;
    }

    private void SetOnDisposeEvent(EnumType state, Action<ActionArgs> evt)
    {
        _onStateDisposed[state] = evt;
    }
}