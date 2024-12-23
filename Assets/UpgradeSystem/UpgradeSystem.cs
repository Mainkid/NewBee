using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeSystem : MonoBehaviour, IDataPersistence, ISystem
{
    private UpgradeTree _upgradeTree;
    private HashSet<int> _activeUpgrades;

    private static UpgradeSystem _instance;
    public static UpgradeSystem Instance { get { return _instance; } }
    public HashSet<int> ActiveUpgrades { get { return _activeUpgrades; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void Init()
    {
        LoadData(SaveSystem.Instance.GameData);
        DontDestroyOnLoad(gameObject);

        _upgradeTree = new UpgradeTree();
    }

    public void LoadData(GameData gameData)
    {
        _activeUpgrades = new HashSet<int>(gameData.upgradesIDs);
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.upgradesIDs = _activeUpgrades.ToList();
    }

    public HashSet<int> GetActiveUpgrades()
    {
        return new HashSet<int>(_activeUpgrades);
    }
    public HashSet<int> GetAccessableUpgrades()
    {
        var result = new HashSet<int>();

        foreach (var keyValuePair in _upgradeTree.UpgradeItems)
        {
            if (_activeUpgrades.Contains(keyValuePair.Key))
                continue;

            bool isAccessable = true;

            foreach(var itemId in keyValuePair.Value.itemDependencies)
            {
                if (!_activeUpgrades.Contains(itemId))
                    isAccessable = false;
            }

            if (isAccessable)
                result.Add(keyValuePair.Key);
        }

        return result;
    }

    public HashSet<int> GetLockedUpgrades()
    {
        var result = new HashSet<int>();
        HashSet<int> notLockedUpgrades = GetActiveUpgrades();
        notLockedUpgrades.UnionWith(GetAccessableUpgrades());

        foreach (var keyValuePair in _upgradeTree.UpgradeItems)
        {
            if (!notLockedUpgrades.Contains(keyValuePair.Key))
                result.Add(keyValuePair.Key);
        }

        return result;
    }

    public float GetTopUpgradeValue(UpgradeGroup upgradeGroup)
    {
        var listOfUpgradeIds = _upgradeTree.UpgradeGroups[upgradeGroup].OrderByDescending(x => x).ToList();

        foreach ( var upgradeId in listOfUpgradeIds)
        {
            if (ActiveUpgrades.Contains(upgradeId))
                return _upgradeTree.UpgradeItems[upgradeId].value;
        }

        return _upgradeTree.DefaultUpgradeValue[upgradeGroup];
    }

    public int CountUpgradesOfGroup(UpgradeGroup upgradeGroup)
    {
        int ctr = 0;

        foreach (var upgradeId in _upgradeTree.UpgradeGroups[upgradeGroup])
        {
            if (ActiveUpgrades.Contains(upgradeId))
                ctr++;
        }
        return ctr;
    }
}
