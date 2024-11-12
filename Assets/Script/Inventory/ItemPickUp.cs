using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item item;
    public GameObject pickupText;
    public AudioSource pickup;  // Assign the audio source with the pickup sound effect in the editor
    public int Index, Id;  // Slot index of the item being picked up
    public Light itemLight;  // Assign the light component in the editor

    public bool interactable = false;

    void Start()
    {
        if (itemLight != null)
        {
            itemLight.enabled = true;  // Enable the light when the item is instantiated
        }
    }

    void Pickup()
    {
        // Add the item to the inventory
        InventoryManager.Instance.Add(item);

        // Set the item index in InventoryManager
        InventoryManager.Instance.Index = InventoryManager.Instance.Items.Count - 1;
        Index = InventoryManager.Instance.Index;
        InventoryManager.Instance.Id = Id;

        InventoryManager.Instance.UpdateItemInformation(item);

        // Destroy the picked-up item
        Destroy(gameObject);

        // Clear any instantiated items
        foreach (var instantiatedItem in InventoryManager.Instance.instantiatedItems)
        {
            if (instantiatedItem != null)
            {
                Destroy(instantiatedItem);
            }
        }
        InventoryManager.Instance.instantiatedItems.Clear();

        // Instantiate the item prefab if available
        if (item.prefab != null && InventoryManager.Instance.player != null)
        {
            if(InventoryManager.Instance.Id == 1 || InventoryManager.Instance.Id == 6 || InventoryManager.Instance.Id == 11)
            {
                InventoryManager.Instance.itemInstance = Instantiate(item.prefab, InventoryManager.Instance.player.position, Quaternion.Euler(0, 0, 0));
            }
            else
            {
                InventoryManager.Instance.itemInstance = Instantiate(item.prefab, InventoryManager.Instance.player.position, InventoryManager.Instance.player.rotation);
            }
            InventoryManager.Instance.itemInstance.transform.SetParent(InventoryManager.Instance.player);
            InventoryManager.Instance.equip = true;
            interactable = false;
            InventoryManager.Instance.instantiatedItems.Add(InventoryManager.Instance.itemInstance);
        }

        // Disable the ItemPickUp component on the newly instantiated item
        ItemPickUp itemPickUp = InventoryManager.Instance.itemInstance.GetComponent<ItemPickUp>();
        if (itemPickUp != null)
        {
            itemPickUp.pickupText = pickupText;
            itemPickUp.pickupText.SetActive(false);

            // Disable the light on the newly instantiated item
            if (itemPickUp.itemLight != null)
            {
                itemPickUp.itemLight.enabled = false;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            pickupText.SetActive(true);
            interactable = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            pickupText.SetActive(false);
            interactable = false;
        }
    }

    void Update()
    {
        Id = item.id;
        if (interactable && Input.GetKeyDown(KeyCode.E))
        {
            // Play the pickup sound effect
            if (pickup != null)
            {
                pickup.Play();
            }

            pickupText.SetActive(false);
            interactable = false;
            Pickup();

            // Disable the light when the item is picked up
            if (itemLight != null)
            {
                itemLight.enabled = false;
            }
        }
    }
}
