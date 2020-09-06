using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScreenType
{
    SPLASH,SCAN,QUESTION,REWARD,GENERATOR
}
public class ScreenManager : MonoBehaviour {
    [SerializeField] private GameScreen[] Screens;
    public static ScreenManager instance;
    private Dictionary<ScreenType,GameScreen> screenList = new Dictionary<ScreenType,GameScreen>();
	// Use this for initialization
	void Start () {
        foreach (GameScreen screen in Screens)
        {
            screenList.Add(screen.screenType,screen);
        }
	}
	
    public void OnScanButton()
    {
        screenList[ScreenType.SCAN].gameObject.SetActive(true);
        screenList[ScreenType.SPLASH].gameObject.SetActive(false);
    }

    public void OnQRGENButton()
    {
        screenList[ScreenType.GENERATOR].gameObject.SetActive(true);
        screenList[ScreenType.SPLASH].gameObject.SetActive(false);
    }

    public void ShowQuestionScreen()
    {
        screenList[ScreenType.QUESTION].gameObject.SetActive(true);
        screenList[ScreenType.SCAN].gameObject.SetActive(false);
    }
}
