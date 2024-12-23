using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using YG;

public class UIManager : MonoBehaviour, IDataPersistence
{
    private Image _fadeImage;
    private Image _progressPathFill;
    private GameObject _newbeeHeadUI;
    private GameObject _leftPB;
    private GameObject _rightPB;

    private GameObject _player;
    private GameObject _start;
    private GameObject _finish;

    public TextMeshProUGUI _nuggetText;

    private GameObject _pauseMenu;
    private GameObject _gameOverMenu;
    private GameObject _upgradeMenu;

    private int score = 0;
    private uint moneyAmount = 0;

    private Animator _goldAmountAnimator;

    private Image _fuelImage;

    bool pausedState = false;
    void Start()
    {
        LoadData(SaveSystem.Instance.GameData);

        

        _progressPathFill = GameObject.Find("ProgressBarFill").GetComponent<Image>();
        _newbeeHeadUI = GameObject.Find("NewBeeHeadUI");
        _leftPB = GameObject.Find("LeftPB");
        _rightPB = GameObject.Find("RightPB");
        _player = GameObject.FindWithTag("Player");
        _start = GameObject.Find("_Start");
        _finish = GameObject.Find("_Finish");
        _pauseMenu = GameObject.Find("PauseMenu");
        _pauseMenu.SetActive(false);
        _fadeImage = GameObject.Find("FadeImage").GetComponent<Image>();
        _goldAmountAnimator = GameObject.Find("GoldAmount").GetComponent<Animator>();
        _nuggetText = GameObject.Find("NuggetText").GetComponent<TextMeshProUGUI>();

        _gameOverMenu = GameObject.Find("LevelEndMenu");
        _gameOverMenu.SetActive(false);

        _upgradeMenu = GameObject.Find("UpgradeMenu");
        _upgradeMenu.SetActive(false);

        _fadeImage.color = new Color(0, 0, 0, 0);

        //moneyAmount = SaveManager.Instance.saveData.MoneyAmount;
        _nuggetText.text = "x" + moneyAmount;

        _fuelImage = GameObject.Find("FuelFill").GetComponent<Image>();
    }

    private static UIManager _instance;

    public static UIManager Instance { get { return _instance; } }
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

    // Update is called once per frame
    void Update()
    {
        float playerLerpValue = (_player.transform.position.x - _start.transform.position.x) / (_finish.transform.position.x - _start.transform.position.x);
        _progressPathFill.fillAmount = playerLerpValue;
        _newbeeHeadUI.transform.position = Vector3.Lerp(_leftPB.transform.position, _rightPB.transform.position, playerLerpValue);
    }

    public void IncreaseGoldAmount()
    {
        _goldAmountAnimator.Play("UI_NuggetScaleAnim");
        moneyAmount++;

        _nuggetText.text = "x" + moneyAmount;
    }

    public void LoadHomeScreen()
    {
        StartCoroutine(LoadLevel("MainMenu"));
        Debug.Log("Loading Home Screen...");
    }

    public void Pause(bool isPaused)
    {
        pausedState = !pausedState;
        Time.timeScale = pausedState ? 0 : 1;
        _pauseMenu.SetActive(pausedState);
        _pauseMenu.transform.Find("SoundSlider").GetComponent<Slider>().value = AudioListener.volume;
        if (pausedState)
        {
            _fadeImage.raycastTarget = true;
            _fadeImage.color = new Color(0, 0, 0, 0.67f);
        }
        else
        {
            _fadeImage.raycastTarget = false;
            _fadeImage.color = new Color(0, 0, 0, 0);
        }
    }

    public void UpdateSoundSlider(float sliderValue)
    {
        Debug.Log(sliderValue);
        AudioListener.volume = sliderValue;
    }
    private IEnumerator LoadLevel(string name)
    {
        Time.timeScale = 1.0f;
        GameObject.Find("Fade").GetComponent<Animator>().Play("FadeIn");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(name);
    }

    public void Restart()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().name));
    }

    private void OnEnable()
    {
        
        YandexGame.RewardVideoEvent += AddRewardedCoins;
    }

    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= AddRewardedCoins;
    }

    private void AddRewardedCoins(int id)
    {
        SaveSystem.Instance.GameData.money += 15;
        SaveSystem.Instance.SaveGame();
        LoadData(SaveSystem.Instance.GameData);
        _nuggetText.text = "x" + moneyAmount;
    }

    public void ShowRewardedAdd()
    {
        YGAdsProvider.ShowRewardedAd(1);
        Debug.Log("Showing Rewarded Add");
    }

    public void ShowUpgrade(bool show)
    {
        _upgradeMenu.GetComponent<UpgradeManagerUI>().UpdateUpgradesView();
        _upgradeMenu.SetActive(show);
        
    }

    public void SetFuel(float amount)
    {
        _fuelImage.fillAmount = amount;
    }

    public void ShowLevelEnded(bool isFinished)
    {

        _gameOverMenu.SetActive(true);
        if (isFinished)
        {
            _gameOverMenu.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = "Уровень пройден!";
            var levelName = SceneManager.GetActiveScene().name;

            if (!SaveSystem.Instance.GameData.openedLevels.Contains(levelName[levelName.Length - 1] - '0' + 1))
            {
                SaveSystem.Instance.GameData.openedLevels.Add(levelName[levelName.Length - 1] - '0' + 1);
                SaveSystem.Instance.SaveGame();
            }
        }
        else
            _gameOverMenu.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = "Уровень провален!";
        _fadeImage.color = new Color(0, 0, 0, 0.67f);
        _fadeImage.raycastTarget = true;

        SaveData(ref SaveSystem.Instance.GameData);
        SaveSystem.Instance.SaveGame();
    }

    public void LoadData(GameData gameData)
    {
        moneyAmount = gameData.money;
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.money = moneyAmount;
    }
}
