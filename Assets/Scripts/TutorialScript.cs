using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{

    public TextMeshProUGUI NubikText;
    public GameObject ArrowFinish;
    public GameObject ArrowFuel;
    public GameObject ArrowMoney;
    public PlayerController playerController;

    private Image _fadeImage;
    private Animator _fadeAnimator;
    bool TutorialFinshed = false;
    Color color;

    string[] Texts = {"Привет!", "Меня зовут... э-э...","Нубик!",
        "Мне нужно добраться до финиша!",
        "Следи за топливом в вагонетке!",
        "Убивай мобов и получай изумруды...",
        "... чтобы прокачать свою вагонетку!",
        "Удачи!!!",
        "Удачи!!!"};

    int iterator = 1;

    void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        if (SaveSystem.Instance.GameData.isTutorialShown)
        {
           
            TutorialFinshed =true;
            ArrowFuel.SetActive(false);
            ArrowFinish.SetActive(false);
            ArrowMoney.SetActive(false);
            GameObject.Find("FadeTutorial").SetActive(false);
            GameObject.Find("TutorialNubik").SetActive(false);

            return;
        }


        NubikText.text = Texts[0];
        _fadeImage = GameObject.Find("FadeTutorial").GetComponent<Image>();
        _fadeAnimator = GameObject.Find("FadeTutorial").GetComponent<Animator>();
        _fadeAnimator.SetBool("IsOn", true);
        
        ArrowFuel.SetActive(false);
        ArrowFinish.SetActive(false);
        ArrowMoney.SetActive(false);

     
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TutorialFinshed)
        {
            playerController.IsInputBlocked = false;
            return;
        }
            
        playerController.IsInputBlocked = true;
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            
            NubikText.text = Texts[iterator];
            iterator++;
        }

        if (iterator==4)
        {
            ArrowFinish.SetActive(true);
            ArrowFinish.GetComponent<Animation>().Play();
        }
        else
        {
            ArrowFinish.SetActive(false);
        }

        if (iterator==5)
        {
            ArrowFuel.SetActive(true);
            ArrowFuel.GetComponent<Animation>().Play(); 
        }
        else
            ArrowFuel.SetActive(false);

        if (iterator==6)
        {
            ArrowMoney.SetActive(true);
            ArrowMoney.GetComponent<Animation>().Play();
        }
        else
            ArrowMoney.SetActive(false);

        if (iterator >= Texts.Length)
        {
            //Debug.Log(iterator + ": iterator");
            GameObject.Find("TutorialNubik").GetComponent<Animator>().SetBool("Disappear", true);
            _fadeAnimator.SetBool("IsOff", true);
            _fadeAnimator.SetBool("IsOn", false);
            TutorialFinshed = true;
            playerController.IsInputBlocked = true;
            SaveSystem.Instance.GameData.isTutorialShown = true;
            SaveSystem.Instance.SaveGame();
        }

        

    }
}
