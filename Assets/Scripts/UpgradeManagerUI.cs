using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[Serializable]
public class UpgradePairUI
{
    public int id;
    public UpgradeButton upgradeUI;
}

public class UpgradeManagerUI : MonoBehaviour,ISystem
{
    [SerializeField]
    public List<UpgradePairUI> UpgradeItemsUI;

    public GameObject _moneyContainer;
    public TextMeshProUGUI _moneyContainerAmount;
    public void Init()
    {
        UpdateUpgradesView();

        foreach (var upgrade in UpgradeItemsUI)
        {
            upgrade.upgradeUI.Id = upgrade.id;
        }
    }

    public void UpdateUpgradesView()
    {
        UpgradeSystem.Instance.GetActiveUpgrades();
        HashSet<int> activeUpgradesID = UpgradeSystem.Instance.GetActiveUpgrades();
        HashSet<int> readyToBuyUpgradesID = UpgradeSystem.Instance.GetAccessableUpgrades();
        HashSet<int> lockedUpgradesID = UpgradeSystem.Instance.GetLockedUpgrades();

        foreach (var id in activeUpgradesID)
        {
            UpgradeButton upgradeItemUI = (from pair in UpgradeItemsUI
                                           where pair.id == id
                                           select pair.upgradeUI).First();

            upgradeItemUI.buttonState = State.Bought;
        }

        foreach (var id in readyToBuyUpgradesID)
        {
            UpgradeButton upgradeItemUI = (from pair in UpgradeItemsUI
                                           where pair.id == id
                                           select pair.upgradeUI).First();

            upgradeItemUI.buttonState = State.Active;
        }

        foreach (var id in lockedUpgradesID)
        {
            UpgradeButton upgradeItemUI = (from pair in UpgradeItemsUI
                                           where pair.id == id
                                           select pair.upgradeUI).First();

            upgradeItemUI.buttonState = State.Locked;
        }


        foreach (var upgradeButton in UpgradeItemsUI)
        {
            upgradeButton.upgradeUI.UpdateState();
        }

        _moneyContainerAmount.text = Convert.ToString(SaveSystem.Instance.GameData.money);
        GameObject.Find("NuggetText").GetComponent<TextMeshProUGUI>().text = "x" + _moneyContainerAmount.text;
    }


   
}
