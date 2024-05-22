using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
}