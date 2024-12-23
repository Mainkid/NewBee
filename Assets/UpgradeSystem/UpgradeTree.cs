using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public enum UpgradeGroup
{
    None,
    Engine,
    Fuel,
    Damage,
    Bounce,
    Explosive
}

public class UpgradeTree : ScriptableObject
{
    private Dictionary<int, UpgradeItem> _upgradeItems = new Dictionary<int, UpgradeItem>();

    public Dictionary<int, UpgradeItem> UpgradeItems { get { return _upgradeItems; } }

    public Dictionary<UpgradeGroup, List<int>> UpgradeGroups = new Dictionary<UpgradeGroup, List<int>>()
    {
        {UpgradeGroup.Engine, new List<int>() {0,1,2,15,16,17 } },
        {UpgradeGroup.Fuel, new List<int>() {3,4,5,18,19,20 } },
        {UpgradeGroup.Damage, new List<int>() {6,7,8,21,22,23 } },
        {UpgradeGroup.Bounce, new List<int>() {9,10,11 } },
        {UpgradeGroup.Explosive, new List<int>() {12,13,14 } }
    };

    public Dictionary<UpgradeGroup, float> DefaultUpgradeValue = new Dictionary<UpgradeGroup, float>()
    {
        {UpgradeGroup.Fuel,2 },
        {UpgradeGroup.Engine,1.0f },
        {UpgradeGroup.Damage,0 },
        {UpgradeGroup.Bounce,0.25f },
        {UpgradeGroup.Explosive,0 }
    };

    public UpgradeTree()
    {
        UpgradeItem engineLvl1 = new UpgradeItem(10,"Двигатель\n Ур.1",new HashSet<int>(),1.25f);
        UpgradeItem engineLvl2 = new UpgradeItem(15, "Двигатель\n Ур.2", new HashSet<int>() { 0 },1.5f);
        UpgradeItem engineLvl3 = new UpgradeItem(20, "Двигатель\n Ур.3", new HashSet<int>() { 1 },1.85f);

        UpgradeItem fuelLvl1 = new UpgradeItem(5, "Топливо\n Ур.1", new HashSet<int>(),2);
        UpgradeItem fuelLvl2 = new UpgradeItem(10, "Топливо\n Ур.2", new HashSet<int>() { 3 },2.5f);
        UpgradeItem fuelLvl3 = new UpgradeItem(15, "Топливо\n Ур.3", new HashSet<int>() { 4 },3.0f);

        UpgradeItem damageLvl1 = new UpgradeItem(10, "Урон\n Ур.1", new HashSet<int>(), 1);
        UpgradeItem damageLvl2 = new UpgradeItem(15, "Урон\n Ур.2", new HashSet<int>() { 6 }, 2);
        UpgradeItem damageLvl3 = new UpgradeItem(20, "Урон\n Ур.3", new HashSet<int>() { 7 }, 3 );

        UpgradeItem bounceLvl1 = new UpgradeItem(10, "Прыжок\n Ур.1", new HashSet<int>() { 2, 5 },0.4f);
        UpgradeItem bounceLvl2 = new UpgradeItem(15, "Прыжок\n Ур.2", new HashSet<int>() { 9 }, 0.5f);
        UpgradeItem bounceLvl3 = new UpgradeItem(20, "Прыжок\n Ур.3", new HashSet<int>() { 10 }, 0.65f);

        UpgradeItem explosiveLvl1 = new UpgradeItem(10, "Взрыв\n Ур.1", new HashSet<int>() { 5, 8 },1.1f);
        UpgradeItem explosiveLvl2 = new UpgradeItem(15, "Взрыв\n Ур.2", new HashSet<int>() { 12 }, 1.2f);
        UpgradeItem explosiveLvl3 = new UpgradeItem(20, "Взрыв\n Ур.3", new HashSet<int>() { 13 }, 1.3f);

        UpgradeItem engineLvl4 = new UpgradeItem(15, "Двигатель\n Ур.4", new HashSet<int>() { 11, 14 },2.25f);
        UpgradeItem engineLvl5 = new UpgradeItem(20, "Двигатель\n Ур.5", new HashSet<int>() { 15 }, 2.8f);
        UpgradeItem engineLvl6 = new UpgradeItem(25, "Двигатель\n Ур.6", new HashSet<int>() { 16 }, 3.5f);

        UpgradeItem fuelLvl4 = new UpgradeItem(15, "Топливо\n Ур.4", new HashSet<int>() { 11, 14 },3.5f);
        UpgradeItem fuelLvl5 = new UpgradeItem(20, "Топливо\n Ур.5", new HashSet<int>() { 18 },4);
        UpgradeItem fuelLvl6 = new UpgradeItem(25, "Топливо\n Ур.6", new HashSet<int>() { 19 },5.5f);

        UpgradeItem damageLvl4 = new UpgradeItem(15, "Урон\n Ур.4", new HashSet<int>() { 11, 14 },4);
        UpgradeItem damageLvl5 = new UpgradeItem(20, "Урон\n Ур.5", new HashSet<int>() { 21 }, 5);
        UpgradeItem damageLvl6 = new UpgradeItem(25, "Урон\n Ур.6", new HashSet<int>() { 22 }, 6);

        _upgradeItems.Add(0, engineLvl1);
        _upgradeItems.Add(1, engineLvl2);
        _upgradeItems.Add(2, engineLvl3);

        _upgradeItems.Add(3, fuelLvl1);
        _upgradeItems.Add(4, fuelLvl2);
        _upgradeItems.Add(5, fuelLvl3);

        _upgradeItems.Add(6, damageLvl1);
        _upgradeItems.Add(7, damageLvl2);
        _upgradeItems.Add(8, damageLvl3);

        _upgradeItems.Add(9, bounceLvl1);
        _upgradeItems.Add(10, bounceLvl2);
        _upgradeItems.Add(11, bounceLvl3);

        _upgradeItems.Add(12, explosiveLvl1);
        _upgradeItems.Add(13, explosiveLvl2);
        _upgradeItems.Add(14, explosiveLvl3);

        _upgradeItems.Add(15, engineLvl4);
        _upgradeItems.Add(16, engineLvl5);
        _upgradeItems.Add(17, engineLvl6);

        _upgradeItems.Add(18, fuelLvl4);
        _upgradeItems.Add(19, fuelLvl5);
        _upgradeItems.Add(20, fuelLvl6);

        _upgradeItems.Add(21, damageLvl4);
        _upgradeItems.Add(22, damageLvl5);
        _upgradeItems.Add(23, damageLvl6);
    }
}
