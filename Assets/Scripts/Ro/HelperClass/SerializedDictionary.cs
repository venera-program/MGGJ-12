using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class SerializedDictionary<TKey, TValue> : Dictionary<TKey,TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    protected List<SerializedKeyValuePair<TKey, TValue>> KVP = new List<SerializedKeyValuePair<TKey, TValue>>();
    
    protected List<SerializedKeyValuePair<TKey, TValue>> backUPKVP = new List<SerializedKeyValuePair<TKey, TValue>>();

/// <summary>
/// Translating the things from the editor to be used in the class
/// </summary>
    public void OnAfterDeserialize()
    {
      this.Clear();
      backUPKVP.Clear();
      foreach(SerializedKeyValuePair<TKey, TValue> A in KVP){
        if(!this.ContainsKey(A.KEY)){
          this.Add(A.KEY, A.VALUE);
        } else {
          backUPKVP.Add(A);
        }
      }
      
   
      // maybe do an operation here to remove the things in backUPKVP that wasn't included in the dictionary?
    }

/// <summary>
/// Translating the things inside of the class to something to be used for the editor
/// </summary>
    public void OnBeforeSerialize(){
      KVP.Clear();
      foreach(KeyValuePair<TKey, TValue> A in this){
        KVP.Add(new SerializedKeyValuePair<TKey, TValue>(A.Key,A.Value));
      }
      foreach(SerializedKeyValuePair<TKey, TValue> A in backUPKVP){
        KVP.Add(A);
      }
    }

    public List<SerializedKeyValuePair<TKey, TValue>> GetKVPList(){
      return KVP;
    }

    
}

[Serializable]
public class SerializedKeyValuePair<TKey, TValue> {
  public TKey KEY;
  public TValue VALUE;

  public SerializedKeyValuePair(TKey _key, TValue _value){
    KEY = _key;
    VALUE = _value;
  }

}