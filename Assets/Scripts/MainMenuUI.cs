using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour, ISystem
{
    public GameObject LeftArrow;
    public GameObject RightArrow;
    public Image LevelImage;
    public Image BlockOutImage;
    public Image LockImage;

    public Button PlayButton;

    int iterator = 0;
    private string[] LevelNames = { "1. Равнина", "2. Холмы", "3. Деревня", "4. Шахта", "5. Снежный биом", "6. Ад", "7. Грибной биом" };
    public Sprite[] LevelSprites;
    
    void Start()
    {

        //Init();
    }

    public void Init()
    {
        GameObject.Find("Fade").GetComponent<Animator>().Play("FadeOut");
        Time.timeScale = 1.0f;
        LeftArrow.GetComponent<Animator>().Play("LeftArrow");
        RightArrow.GetComponent<Animator>().Play("RightArrow");
        UpdateLevelInfo();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResetProgress()
    {
        SaveSystem.Instance.NewGame();
        UpgradeSystem.Instance.LoadData(SaveSystem.Instance.GameData);
    }

    public void PlayButtonPressed()
    {

        StartCoroutine(LoadLevel());
        
    }

    private IEnumerator LoadLevel()
    {
        GameObject.Find("Fade").GetComponent<Animator>().Play("FadeIn");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Level" + Convert.ToString(iterator + 1));
    }

    public void ButtonsDown()
    {
        GetComponent<Animator>().SetBool("GoDown", true);
        GetComponent<Animator>().SetBool("GoUp", false);

        GameObject.Find("LevelBG").GetComponent<Animator>().SetBool("IsUp", true);
        GameObject.Find("LevelBG").GetComponent<Animator>().SetBool("IsDown", false);
    }

    public void BackButtonDown()
    {
        GetComponent<Animator>().SetBool("GoUp", true);
        GetComponent<Animator>().SetBool("GoDown",false);

        GameObject.Find("LevelBG").GetComponent<Animator>().SetBool("IsDown", true);
        GameObject.Find("LevelBG").GetComponent<Animator>().SetBool("IsUp", false);
        Debug.Log("LL");
    }

    public void UpdateLevelInfo()
    {
        LeftArrow.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        RightArrow.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        LeftArrow.GetComponent<Button>().enabled = true;
        RightArrow.GetComponent<Button>().enabled = true;

        if (!SaveSystem.Instance.GameData.openedLevels.Contains(iterator+1))
        {
            PlayButton.interactable= false;
            BlockOutImage.enabled = true;
            LockImage.enabled = true;
        }
        else
        {
            PlayButton.interactable = true;
            BlockOutImage.enabled= false;
            LockImage.enabled = false;
        }

        if (iterator==0)
        {
            LeftArrow.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            LeftArrow.GetComponent<Button>().enabled = false;
        }
        if (iterator == LevelNames.Length-1)
        {
            RightArrow.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            RightArrow.GetComponent<Button>().enabled = false;
        }

        LevelImage.sprite = LevelSprites[iterator];
        GameObject.Find("LevelName").GetComponent<TextMeshProUGUI>().text = LevelNames[iterator];
    }

    public void LeftArrowPressed()
    {
        iterator = Mathf.Clamp(iterator - 1,0,LevelNames.Length-1);
        UpdateLevelInfo();
    }

    public void RightArrowPressed()
    {
        iterator = Mathf.Clamp(iterator + 1, 0, LevelNames.Length - 1);
        UpdateLevelInfo();
    }
}
