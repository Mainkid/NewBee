using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeItem 
{
    [SerializeField]
    public int id;

    [SerializeField]
    public int price;

    [SerializeField]
    public string name;

    [SerializeField]
    public float value;

    [SerializeField]
    public HashSet<int> itemDependencies = new HashSet<int>();

    public UpgradeItem(int price, string name, HashSet<int> itemDependencies, float value)
    {
        this.price = price;
        this.name = name;
        this.itemDependencies = itemDependencies;
        this.value = value;
    }

}
