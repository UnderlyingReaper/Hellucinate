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

    public void EnableItem(object sender, EventArgs e)
    {
        playerItem.SetActive(true);

        Inventory_System inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory_System>();
        inventory.RemoveItem(_item.item_ID);
    }
}
