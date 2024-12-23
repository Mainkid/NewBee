using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SaveData
{
    public int MaxLevelNum;
    public int MoneyAmount;
    public int FuelLevel;
    public int EngineLevel;
    public int DamageLevel;
    public int BounceLevel;
    public int ExplosionLevel;
}

public class SaveManager : MonoBehaviour
{
    private static SaveManager _instance;
    
    public static SaveManager Instance { get { return _instance; } }
    public SaveData saveData;
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

    void Start()
    {
        //PlayerPrefs.DeleteAll();
        Init();
    }

    public void Init()
    {
        DontDestroyOnLoad(gameObject);
        saveData = new SaveData();
        saveData.MaxLevelNum = PlayerPrefs.GetInt("MaxLevelNum");
        saveData.MoneyAmount = PlayerPrefs.GetInt("MoneyAmount");
        saveData.EngineLevel = PlayerPrefs.GetInt("EngineLevel") - 1;
        saveData.FuelLevel = PlayerPrefs.GetInt("FuelLevel")-1;
        saveData.ExplosionLevel = PlayerPrefs.GetInt("ExplosionLevel")-1;
        saveData.BounceLevel = PlayerPrefs.GetInt("BounceLevel")-1;
        saveData.DamageLevel = PlayerPrefs.GetInt("DamageLevel")-1;
    }
    void Update()
    {
        
    }

    public void Save()
    {
        PlayerPrefs.SetInt("MaxLevelNum", saveData.MaxLevelNum);
        PlayerPrefs.SetInt("MoneyAmount", saveData.MoneyAmount);
        PlayerPrefs.SetInt("FuelLevel", saveData.FuelLevel+1);
        PlayerPrefs.SetInt("ExplosionLevel", saveData.ExplosionLevel + 1);
        PlayerPrefs.SetInt("BounceLevel", saveData.BounceLevel + 1);
        PlayerPrefs.SetInt("DamageLevel", saveData.DamageLevel + 1);
        PlayerPrefs.SetInt("EngineLevel", saveData.EngineLevel + 1);
    }
}
