using UnityEngine;

public abstract class ItemEffect : ScriptableObject
{
    public abstract void ExecuteEffect(GameObject from, GameObject to);
}