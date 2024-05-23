using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int amountOfItems;
    [SerializeField] private GameObject dropPrefab;
    [SerializeField] private List<ItemData> possibleDrop;
    [SerializeField] private List<ItemData> dropList;

    private void Awake()
    {
        dropList = new List<ItemData>();
    }

    public void GenerateDrop()
    {
        foreach (var drop in possibleDrop)
        {
            if (Random.Range(0, 100) <= drop.dropChance)
            {
                dropList.Add(drop);
            }
        }

        if (dropList.Count - 1 < 0) return;

        for (int i = 0; i < amountOfItems; i++)
        {
            var index = Random.Range(0, dropList.Count - 1);
            if (index < 0) return;
            var randomItem = dropList[index];
            DropItem(randomItem);
        }
    }

    public void DropItem(ItemData item)
    {
        var pos = transform.position + new Vector3(0, 1);
        var parent = PlayerManager.Instance.item.transform;
        var newDrop = Instantiate(dropPrefab, pos, Quaternion.identity, parent);
        if (!newDrop.TryGetComponent(out ItemObject itemObject)) return;
        var randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(10, 15));
        itemObject.Setup(item, randomVelocity);
    }
}