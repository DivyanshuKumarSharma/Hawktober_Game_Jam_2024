using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items;

    private void Awake()
    {
        if (items == null)
        {
            items = new List<Item>();
        }
    }

    public void AddItem(Item item)
    {
        items.Add(item);
    }

    public bool HasItem(string itemName)
    {
        foreach (Item item in items)
        {
            if (item.itemName == itemName)
            {
                return true;
            }
        }
        return false;
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
    }

    public int GetItemCount()
    {
        return items.Count;
    }


}
