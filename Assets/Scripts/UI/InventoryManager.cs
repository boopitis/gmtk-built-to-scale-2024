using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    private bool menuActivated;
    public ItemSlot[] itemSlots;

    public void ToggleMenu()
    {
        if (menuActivated)
        {
            Time.timeScale = 1;
            InventoryMenu.SetActive(false);
            menuActivated = false;
            DeselectAllSlots();
        }
        else
        {
            Time.timeScale = 0;
            InventoryMenu.SetActive(true);
            menuActivated = true;
        }

    }

    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
    {
        foreach (ItemSlot itemSlot in itemSlots)
        {
            if (itemSlot.isFull == false && itemSlot.itemName == itemName || itemSlot.quantity == 0)
            {
                int leftOverItems = itemSlot.AddItem(itemName, quantity, itemSprite, itemDescription);
                if (leftOverItems > 0)
                    leftOverItems = AddItem(itemName, leftOverItems, itemSprite, itemDescription);

                return leftOverItems;
            }
        }
        return quantity;
    }

    public void DeselectAllSlots()
    {
        foreach (ItemSlot itemSlot in itemSlots)
        {
            itemSlot.selectedShader.SetActive(false);
            itemSlot.thisItemSelected = false;
            itemSlot.itemDescriptionImage.enabled = false;
        }
    }
}
