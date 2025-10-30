using System;
using System.Collections.Generic;
using UnityEngine;

public class PassiveBindings : MonoBehaviour
{
    private readonly Dictionary<object, List<Action>> _unbinds = new();

    public void Bind(object owner, Action unbind)
    {
        if (!_unbinds.TryGetValue(owner, out var list))
        {
            list = new List<Action>();
            _unbinds[owner] = list;
        }
        list.Add(unbind);
    }

    public void Unbind(object owner)
    {
        if (_unbinds.TryGetValue(owner, out var list))
        {
            foreach (var unbind in list) 
                unbind?.Invoke();
            list.Clear();
            _unbinds.Remove(owner);
        }
    }

    private void OnDestroy()
    {
        foreach (var list in _unbinds.Values)
            foreach (var unbind in list) 
                unbind?.Invoke();
        _unbinds.Clear();
    }
}
