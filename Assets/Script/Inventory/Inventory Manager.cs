using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Item> Items = new List<Item>();
    public List<GameObject> instantiatedItems = new List<GameObject>();

    public Transform ItemContent;
    public GameObject InventoryItem, Inventory, Info, pickupText, dropText;
    public GameObject MapCanvas;
    public GameObject triggermap;
    public GameObject jumpscarlt1;
    
    //public GameObject Serundeng, Telor, BerasKetan, Lantern, Kertas, KerakTelor;
    public GameObject itemInstance;
    public Transform player;
    public Vector3 itemPlacementPosition;
    public TMP_Text objective;
    public Image MapImage;
    public Button NextMapButton, PreviousMapButton;
    public bool map;
    public bool isInventoryOpen = false;
    public bool equip = false;
    public bool Reward = false;
    public int Index, Id, currentMapIndex;

    public AudioSource audioSource; // Referensi untuk AudioSource
    public AudioClip updateSound;   // Referensi untuk AudioClip

    private void Awake()
    {
        Instance = this;
        Inventory.SetActive(false);
    }
    

    public void Add(Item item)
    {
        Items.Add(item);
        ListItems();
        if (item.id == 1 || item.id == 2 || item.id == 6 || item.id == 7|| item.id == 8|| item.id == 9 || item.id == 10 || item.id == 11 || item.id == 12)
        {
            UpdateObjective(item.id);
        }
    }

    public void Remove(Item item)
    {
        Items.Remove(item);
        ListItems();
    }

    public void ListItems()
{
    foreach (Transform item in ItemContent)
    {
        Destroy(item.gameObject);
    }

    foreach (var item in Items)
    {
        GameObject obj = Instantiate(InventoryItem, ItemContent);
        var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
        var itemNo = obj.transform.Find("ItemNo").GetComponent<TextMeshProUGUI>();

        itemIcon.sprite = item.icon;
        itemNo.text = (Items.IndexOf(item) + 1).ToString(); // Menambahkan 1 ke indeks item
    }
}

    private void UseMap()
{
    // Check if player has item with ID 1
    Item mapItem = Items.Find(item => item.id == 1);
    if (mapItem != null)
    {
        map = !map;

        // Activate the map canvas and show the first map image
        MapCanvas.gameObject.SetActive(map);
        currentMapIndex = 0;
        MapImage.sprite = mapItem.images[currentMapIndex]; // Assuming 'images' is a list of Sprites in the Item scriptable object

        // Remove any existing listeners to avoid stacking
        NextMapButton.onClick.RemoveAllListeners();
        PreviousMapButton.onClick.RemoveAllListeners();

        // Assign button functions to switch maps
        NextMapButton.onClick.AddListener(() => ShowNextMap(mapItem));
        PreviousMapButton.onClick.AddListener(() => ShowPreviousMap(mapItem));

        // Handle time scale and cursor visibility
        if (!map)
        {
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
    else
    {
        Debug.LogWarning("Player does not have the map item.");
    }
}

private void ShowNextMap(Item mapItem)
{
    if (currentMapIndex < mapItem.images.Count - 1)
    {
        currentMapIndex++;
        MapImage.sprite = mapItem.images[currentMapIndex];
    }
}

private void ShowPreviousMap(Item mapItem)
{
    if (currentMapIndex > 0)
    {
        currentMapIndex--;
        MapImage.sprite = mapItem.images[currentMapIndex];
    }
}


    public void UpdateObjective(int itemId)
    {
        Item Beras = Items.Find(item => item.id == 7);
        Item kelapa = Items.Find(item => item.id == 8);
        Item Telur = Items.Find(item => item.id == 9);
        if(Beras != null && kelapa != null && Telur != null)
        {
            objective.text = "⊙ Put the ingredients on the wajan in the 8-Icon Gallery, 1st floor!";
        }
        switch (itemId)
        {
            case 1:
                Destroy(triggermap);
                jumpscarlt1.SetActive(true);
                objective.text = "⊙ Press [M] to open the map\n⊙ Press [I] to open the inventory\n⊙ Tour the 1st floor";
                break;
            case 2:
                objective.text = "⊙ Match the clue number with the banner number on the 3rd floor! and go to the Gambang Kromong Gallery";
                break;
            case 6:
                objective.text = "⊙ Check out the photo and head over to the Traditional Food Gallery, 2nd floor!";
                break;
            case 10:
                objective.text = "⊙ Put the kerak telor on the plate in the Traditional Food Gallery, 2nd floor!";
                break;
            case 11:
            objective.text = "⊙ Take the lantern on the table!\n⊙ Match the clue number with the banner number on the 3rd floor! and go to the Gambang Kromong Gallery";
                break;
                case 12:
            objective.text = "⊙ Mission 4: Put the mini Ondel-ondel on the table in the traditional house in the 8-Icon Gallery, 1st floor!";
            audioSource.Play();
                break;
            default:
                break;
        }
    }

    public void UpdateItemInformation(Item item)
    {
        if (Info != null)
        {
            var itemInformation = Info.transform.Find("ItemInformation").GetComponent<TextMeshProUGUI>();
            var information = Info.transform.Find("Information").GetComponent<TextMeshProUGUI>();

            if (itemInformation != null)
            {
                itemInformation.text = item.itemName;
            }
            else
            {
                Debug.LogError("ItemInformation TextMeshProUGUI component not found in Info.");
            }

            if (information != null)
            {
                information.text = item.itemInformation;
            }
            else
            {
                Debug.LogError("Information TextMeshProUGUI component not found in Info.");
            }
        }
        else
        {
            Debug.LogError("Info GameObject is not assigned in the inspector.");
        }
    }

private void Update()
{
    if (Input.GetKeyDown(KeyCode.I))
    {
        ToggleInventory();
    }
    if (Input.GetKeyDown(KeyCode.M))
    {
        UseMap();
    }

    for (int i = 0; i < 10; i++)
    {
        if (Input.GetKeyDown(KeyCode.Alpha1 + i))
        {
            if (i < Items.Count) // Cek apakah slot index valid
            {
                UseItem(i); 
                UpdateItemInformation(Items[i]); // Memastikan informasi item diperbarui setiap kali item baru digunakan
                Info.SetActive(true); // Pastikan Info aktif untuk menampilkan item informasi
            }
        }
    }

    if (Input.GetKeyDown(KeyCode.Q))
    {
        Unequip();
    }
}

public void UseItem(int slotIndex)
{
    if (slotIndex < 0 || slotIndex >= Items.Count)
    {
        Debug.LogError("Invalid inventory slot index.");
        return;
    }

    var item = Items[slotIndex];
    Index = slotIndex;
    Id = item.id;
    if (item != null)
    {
        Debug.Log($"Using item: {item.itemName}");

        // Hapus item yang sudah diinstansiasi sebelumnya
        foreach (var instantiatedItem in instantiatedItems)
        {
            if (instantiatedItem != null)
            {
                Destroy(instantiatedItem);
            }
        }
        instantiatedItems.Clear();

        if (item.prefab != null && player != null)
        {
            itemInstance = Instantiate(item.prefab, player.position, Quaternion.Euler(0, 0, 0));
            itemInstance.transform.SetParent(player);

            instantiatedItems.Add(itemInstance);

            ItemPickUp itemPickUp = itemInstance.GetComponent<ItemPickUp>();

            if (itemPickUp != null)
            {
                itemPickUp.interactable = false;
                itemPickUp.pickupText = pickupText;
                itemPickUp.pickupText.SetActive(false);
            }
            else
            {
                Debug.LogError("ItemPickUp script not found on item instance.");
            }

            equip = true;
            Info.SetActive(true);
        }
        else
        {
            Debug.LogError("Item prefab or spawn point not set.");
        }

        UpdateItemInformation(item); // Perbarui informasi item setiap kali item digunakan
    }
}



    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        Inventory.SetActive(isInventoryOpen);
    }

    
    private void Unequip()
    {
        foreach (var instantiatedItem in instantiatedItems)
        {
            if (instantiatedItem != null)
            {
                Destroy(instantiatedItem);
            }
        }
        Id = 0;
        Index = 0;
        Info.SetActive(false);
        instantiatedItems.Clear();
        equip = false;
    }

    public void PutBaju(Vector3 position)
    {
        if (instantiatedItems.Count > 0)
        {
            GameObject itemToPlace = instantiatedItems[0];
            itemToPlace.transform.SetParent(null);
            itemToPlace.transform.position = position;

            instantiatedItems.RemoveAt(0);
            instantiatedItems.Add(itemToPlace); // Add the placed item back to the list
        }
        else
        {
            Debug.LogError("No item to place.");
        }
    }

    
}
