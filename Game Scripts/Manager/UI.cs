using UnityEngine;
using UnityEngine.UI;

public class UI : Singleton<UI>
{
    [SerializeField] Animation bannerAnimation;
    [SerializeField] Text bannerText;
    [SerializeField] Image bannerImage;

    //Map
    [SerializeField] Text distanceText;

    private int pauseSwitch = 1;

    private void Update()
    {
        //Update
        distanceText.text = ((int)Map.instance.distance / 10f).ToString();
    }

    public void GamePause()
    {
        if (!GameManager.isOver)
        {
            pauseSwitch = 1 - pauseSwitch;
            if (pauseSwitch == 0)
            {
                GameManager.instance.GamePause();
            }
            else if (pauseSwitch == 1)
            {
                GameManager.instance.GameResume();
            }
        }
    }
    public void BackHome()
    {
        LoadingManager.instance.QuickLevel("Menu");
    }

    /// <summary>
    /// 퀴즈 배너를 보여주는 메소드
    /// </summary>
    /// <param name="quizText">퀴즈 텍스트</param>
    /// <param name="quizImage">퀴즈 배너 이미지</param>
    public void ShowQuizBanner(string quizText = null, Sprite quizImage = null)
    {
        if (quizText != null && quizImage != null)
        {
            //make your scripts
        }
        else if (quizText != null)
        {
            //Enable & Disable
            bannerText.enabled = true;
            bannerImage.enabled = false;

            //Set
            bannerText.text = quizText;
            bannerImage.sprite = null;
        }
        else if (quizImage != null)
        {
            //Enable & Disable
            bannerText.enabled = false;
            bannerImage.enabled = true;

            //Set
            bannerText.text = string.Empty;
            bannerImage.sprite = quizImage;
        }
        bannerAnimation.Stop();
        bannerAnimation.Play("ShowAlert");
    }
}
