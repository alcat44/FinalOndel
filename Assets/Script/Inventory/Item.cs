using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="New Item", menuName ="Item/Create")]
public class Item : ScriptableObject
{
    public int id;
    public string itemName;
    public string itemInformation;
    public Sprite icon;
    public GameObject prefab;
    public List<Sprite> images = new List<Sprite>();
}
