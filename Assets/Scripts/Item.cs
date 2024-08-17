using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private int quantity;
    [SerializeField] private Sprite sprite;

    [TextArea]
    [SerializeField] private string itemDescription;

    private InventoryManager inventoryManager;
    private Collider2D lastHit;

    private void Awake()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == lastHit) return;

        if (!other.gameObject.CompareTag("Player")) return;
        
        var leftOverItems = inventoryManager.AddItem(itemName, quantity, sprite, itemDescription);
        
        if (leftOverItems <= 0)
            Destroy(gameObject);
        else
            quantity = leftOverItems;
        lastHit = other;
    }
}
