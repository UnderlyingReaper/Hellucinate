using System;
using UnityEngine;

public class AddItemToPlayer : MonoBehaviour
{
    Item _item;
    public GameObject playerItem;

    void Start()
    {
        _item = GetComponent<Item>();
        _item.OnItemPickedUp += EnableItem;
    }

    public void EnableItem(object sender, Item.ItemPickUpInfo e)
    {
        playerItem.SetActive(true);
        e.inventorySystem.RemoveItem(_item.item_ID);
    }
}
