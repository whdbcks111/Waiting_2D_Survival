using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [Serializable]
    public struct SerializableKeyValuePair
    {
        public TKey key;
        public TValue value;

        public SerializableKeyValuePair(TKey key, TValue value)
        {
            this.key = key;
            this.value = value;
        }
    }

    [SerializeField]
    private List<SerializableKeyValuePair> _keyValuePairs = new();

    // save the dictionary to a list
    public void OnBeforeSerialize()
    {
    }

    // load dictionary from list
    public void OnAfterDeserialize()
    {
        Clear();
        foreach (var pair in _keyValuePairs)
        {
            Add(pair.key, pair.value);
        }
    }
}
