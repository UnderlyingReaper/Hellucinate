using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_System : MonoBehaviour
{

    [Header("Inventory")]
    public List<GameObject> slots;
    public List<string> itemIDs;

    void Start()
    {
        // Reset inventory in the beginning of the game
        for(int i = 0; i < itemIDs.Count; i++)
        {
            itemIDs[i] = "";
            slots[i].GetComponent<CanvasGroup>().DOFade(0, 0.3f);
            slots[i].GetComponent<Image>().sprite = null;
            slots[i].GetComponent<Image>().color = Color.black;
        }
    }

    public void AddItem(string desiredItemID, Sprite itemesSprite, Color color, Vector2 size)
    {
        for(int i = 0; i < itemIDs.Count; i++)
        {
            if(itemIDs[i] == desiredItemID) break;

            if(itemIDs[i] == "")
            {
                itemIDs[i] = desiredItemID;
                slots[i].GetComponent<RectTransform>().sizeDelta = size;
                slots[i].GetComponent<Image>().sprite = itemesSprite;
                slots[i].GetComponent<Image>().color = color;
                slots[i].GetComponent<CanvasGroup>().DOFade(1, 1);
                Debug.Log(desiredItemID + " Item Added");
                break;
            }
        }
    }

    public void RemoveItem(string desiredItemID)
    {
        for(int i = 0; i < itemIDs.Count; i++)
        {
            if(itemIDs[i] == desiredItemID)
            {
                itemIDs[i] = "";
                slots[i].GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
                slots[i].GetComponent<CanvasGroup>().DOFade(0, 0.3f);
                slots[i].GetComponent<Image>().sprite = null;
                slots[i].GetComponent<Image>().color = Color.black;
                Debug.Log(desiredItemID + " Item Removed");
                break;
            }
        }
    }

    public bool CheckForEmptySlot()
    {
        bool isAvailable = false;

        for(int i = 0; i < itemIDs.Count; i++)
        {
            if(itemIDs[i] == "")
            {
                isAvailable = true;
                break;
            }
        }
        return isAvailable;
    }

    public bool CheckForItem(string itemID)
    {
        bool doesHaveItem = false;

        for(int i = 0; i < itemIDs.Count; i++)
        {
            if(itemIDs[i] == itemID)
            {
                doesHaveItem = true;
                break;
            }
        }
        return doesHaveItem;
    }
}
