using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum State
{
    Locked,
    Bought,
    Active
}

public class UpgradeButton : MonoBehaviour
{
    public State buttonState;
    public int Price;
    public int Id;

    private UpgradeManagerUI _upgradeManager;
    void Start()
    {
        _upgradeManager = GameObject.Find("UpgradeMenu").GetComponent<UpgradeManagerUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateState()
    {
        if (buttonState == State.Locked)
        {
            transform.Find("BackPanelUpgrade").GetComponent<Button>().interactable = false;
            transform.Find("BackPanelUpgrade").Find("EngineUpgrade").gameObject.SetActive(false);
            transform.Find("Nugget").gameObject.SetActive(false);
            transform.Find("Checkmark").gameObject.SetActive(false);
            transform.Find("BackPanelUpgrade").Find("Locked").gameObject.SetActive(true);
        }
        else if (buttonState == State.Active)
        {
            transform.Find("BackPanelUpgrade").GetComponent<Button>().interactable = true;
            transform.Find("BackPanelUpgrade").Find("EngineUpgrade").gameObject.SetActive(true);
            transform.Find("Nugget").gameObject.SetActive(true);
            transform.Find("Checkmark").gameObject.SetActive(false);
            transform.Find("BackPanelUpgrade").Find("Locked").gameObject.SetActive(false);
        }
        else if (buttonState == State.Bought)
        {
            transform.Find("BackPanelUpgrade").Find("EngineUpgrade").gameObject.SetActive(true);
            transform.Find("Nugget").gameObject.SetActive(false);
            transform.Find("Checkmark").gameObject.SetActive(true);
            transform.Find("BackPanelUpgrade").Find("Locked").gameObject.SetActive(false);
        }
    }

    public void BuyUpgrade()
    {
        if (buttonState != State.Active)
            return;

        if (Price > SaveSystem.Instance.GameData.money)
        {
            Debug.Log("Not enought money");
            GameObject.Find("Emeralds").GetComponent<Animator>().Play("NotEnoughMoney");
            return;
        }

        buttonState = State.Bought;

        UpgradeSystem.Instance.ActiveUpgrades.Add(Id);
        SaveSystem.Instance.GameData.money -= Convert.ToUInt32(Price);

        UpgradeSystem.Instance.SaveData(ref SaveSystem.Instance.GameData);
        SaveSystem.Instance.SaveGame();

        GetComponent<AudioSource>().Play();
        _upgradeManager.UpdateUpgradesView();


    }
}
