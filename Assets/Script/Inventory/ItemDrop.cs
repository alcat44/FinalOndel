using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public int Index = 0, id, dropIndex;
    public Item item;
    public static ItemDrop Instance;
    public List<GameObject> droppedItems = new List<GameObject>();

    [Header("Game Object")]
    public GameObject itemObject;
    public GameObject itemDrop;
    public GameObject KerakTelor;
    public GameObject KertasMusik;
    public GameObject Ondel;
    public GameObject ondelboss;
    public GameObject ondel1;
    public GameObject ondel2;
    
    public GameObject lantai31;
    public GameObject lantai32;
    public GameObject lantai33;

    [Header("Position")]
    public Transform player;
    public Transform pintuTerakhir;
    public Transform telor;
    public Transform kelapa;
    public Transform beras;
    public Vector3 itemPlacementPosition;

    [Header("Text")]
    public GameObject dropText;
    public GameObject successText;

    [Header("Bool")]
    public bool interactable = false;
    public bool Misi1 = false;
    public bool Misi2 = false;
    public bool Misi3 = false;
    public bool anidio = false;

    [Header("Sound Effects")]
    public AudioSource audioSource;
    public AudioClip dropSound;

    private void Awake()
    {
        Instance = this;
    }

    void Drop(int Index)
    {
        dropIndex = droppedItems.Count;
        
        var item = InventoryManager.Instance.Items[Index];
        id = InventoryManager.Instance.Id;

        InventoryManager.Instance.Remove(item);
        itemDrop = Instantiate(itemObject, itemPlacementPosition, Quaternion.identity);
        droppedItems.Add(itemDrop);
        Destroy(itemObject);
        interactable = false;

        PlayDropSound();
        
        foreach (var droppedItem in droppedItems)
        {
            ItemPickUp itemPickUp = droppedItem.GetComponent<ItemPickUp>();
            if (itemPickUp != null)
            {
                itemPickUp.enabled = false;
            }
        }
    }

    void DropLantern(int Index)
    {
        var item = InventoryManager.Instance.Items[Index];
        InventoryManager.Instance.Remove(item);
        
        itemDrop = Instantiate(itemObject, new Vector3(player.position.x, player.position.y - 2, player.position.z - 1), Quaternion.identity);
        droppedItems.Add(itemDrop);
        Destroy(itemObject);

        PlayDropSound();
    }

    void DropKerak(int Index)
    {
        dropIndex = droppedItems.Count;
        
        var item = InventoryManager.Instance.Items[Index];
        id = InventoryManager.Instance.Id;

        InventoryManager.Instance.Remove(item);
        itemDrop = Instantiate(itemObject, new Vector3(4.62300014f, 19.8390007f, 15.5059996f), Quaternion.Euler(-70.328f, 15.36913f , 73.747f));
        droppedItems.Add(itemDrop);
        Destroy(itemObject);
        interactable = false;
        successText.SetActive(true);
        Invoke("HideSuccessText", 2.0f);

        PlayDropSound();
        
        foreach (var droppedItem in droppedItems)
        {
            ItemPickUp itemPickUp = droppedItem.GetComponent<ItemPickUp>();
            if (itemPickUp != null)
            {
                itemPickUp.enabled = false;
            }
        }
    }

    void DropOndel(int Index)
    {
        dropIndex = droppedItems.Count;

        var item = InventoryManager.Instance.Items[Index];
        id = InventoryManager.Instance.Id;

        InventoryManager.Instance.Remove(item);
        itemDrop = Instantiate(itemObject, new Vector3(-50.8790016f,5.654f,8.40100002f), Quaternion.Euler(-1.272f, 59.98f, -0.417f));
    
    // Mengubah scale itemDrop menjadi setengah dari ukuran awal
    itemDrop.transform.localScale *= 1.35f;
    
    droppedItems.Add(itemDrop);
    Destroy(itemObject);
    //Destroy(gameObject);
    interactable = false;
    successText.SetActive(true);
    Invoke("HideSuccessText", 2.0f);
    ondelboss.SetActive(false);
    ondel2.SetActive(false);
    ondel1.SetActive(false);

    foreach (var droppedItem in droppedItems)
    {
        ItemPickUp itemPickUp = droppedItem.GetComponent<ItemPickUp>();
        if (itemPickUp != null)
        {
            itemPickUp.enabled = false;
        }
    }
    }

    void HideSuccessText()
    {
        successText.SetActive(false); // Sembunyikan teks berhasil
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("KerakTelorPlacement") && (InventoryManager.Instance.Id == 7 || InventoryManager.Instance.Id == 8 || InventoryManager.Instance.Id == 9))
        {
            if (InventoryManager.Instance.equip)
            {
                dropText.SetActive(true);
                interactable = true;
                itemPlacementPosition = other.transform.position;
            }
        }

        if (other.CompareTag("Misi2") && (InventoryManager.Instance.Id == 10))
        {
            if (InventoryManager.Instance.equip)
            {
                dropText.SetActive(true);
                interactable = true;
                itemPlacementPosition = other.transform.position;
            }
        }

        if (other.CompareTag("Misi3") && (InventoryManager.Instance.Id == 12))
        {
            if (InventoryManager.Instance.equip)
            {
                dropText.SetActive(true);
                interactable = true;
                itemPlacementPosition = other.transform.position;
                anidio = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("KerakTelorPlacement") || (other.CompareTag("Misi2")))
        {
            dropText.SetActive(false);
            interactable = false;
        }
    }

    private void CheckRewardConditions()
    {
        if (!Misi1 && droppedItems[0] != null && droppedItems[1] != null && droppedItems[2] != null)
        {
            if (Vector3.Distance(droppedItems[0].transform.position, itemPlacementPosition) < 0.5f &&
                Vector3.Distance(droppedItems[1].transform.position, itemPlacementPosition) < 0.5f &&
                Vector3.Distance(droppedItems[2].transform.position, itemPlacementPosition) < 0.5f)
            {
                Debug.Log("All objects are in position. Instantiating new object.");
                Instantiate(KerakTelor, new Vector3(-31.6745014f,2.80493784f,6.62536097f), Quaternion.Euler(-70.406f, -24.826f , 73.641f));
                Misi1 = true;
                Destroy(droppedItems[0]);
                Destroy(droppedItems[1]);
                Destroy(droppedItems[2]);
            }
        }

        if (!Misi2 && droppedItems[3] != null)
        {
            if (Vector3.Distance(droppedItems[3].transform.position, itemPlacementPosition) < 3.5f)
            {
                Debug.Log("All objects are in position. Instantiating new object.");
                Instantiate(KertasMusik, new Vector3(5.152f, 20.5f, 15.184f), Quaternion.Euler(90, 0, 270));
                Misi2 = true;
                lantai31.SetActive(false);
                lantai32.SetActive(false);
                lantai33.SetActive(false);
            }
        }

        if (!Misi3 && droppedItems[4] != null)
        {
            if (Vector3.Distance(droppedItems[4].transform.position, itemPlacementPosition) < 2.5f)
            {
                Debug.Log("All objects are in position. Instantiating new object.");
                pintuTerakhir.rotation = Quaternion.Euler(-90, 0, -94.312f);
                InventoryManager.Instance.objective.text = ("⊙ Get out of the museum");
                Misi3 = true;
            }
        }
    }

    void Update()
    {
        id = InventoryManager.Instance.Id;
        itemObject = InventoryManager.Instance.itemInstance;
        Index = InventoryManager.Instance.Index;

        if (interactable && Input.GetKeyDown(KeyCode.F) && InventoryManager.Instance.equip)
        {
            if (InventoryManager.Instance.Id == 12)
            {
                DropOndel(Index);
            }
            if (InventoryManager.Instance.Id == 10)
            {
                DropKerak(Index);
            }
            if (InventoryManager.Instance.Id != 12 || InventoryManager.Instance.Id != 10)
            {
                Drop(Index);
            }
            interactable = false;
            InventoryManager.Instance.equip = false;
        }

        if (!interactable && Input.GetKeyDown(KeyCode.F) && InventoryManager.Instance.equip && id == 2)
        {
            interactable = false;
            DropLantern(Index);
            InventoryManager.Instance.equip = false;
        }

                if ((!Misi1 || !Misi2 || !Misi3) && InventoryManager.Instance.instantiatedItems.Count > 0)
        {
            CheckRewardConditions();
        }
    }

    // Method to play the drop sound effect
    private void PlayDropSound()
    {
        if (audioSource != null && dropSound != null)
        {
            audioSource.PlayOneShot(dropSound);
        }
    }
}

