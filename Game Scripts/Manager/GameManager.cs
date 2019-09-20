using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    //Delegate and Event Handler
    public delegate void GameManagerEvent();
    public static event GameManagerEvent GameManagerInitialize;
    public static event GameManagerEvent GameManagerInitialized;
    public static event GameManagerEvent EventGameOver;

    //Static Variable
    public static bool isOver = false;

    #region Game Cycle
    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        GameManagerInitialize?.Invoke();

        //Map에게 시작 연산
        Map.instance.Initialize();

        Initialized();

        //Set Variable
        isOver = false;
    }
    public void Initialized()
    {
        GameManagerInitialized?.Invoke();
    }
    public void GameResume()
    {
        Time.timeScale = 1;
    }
    public void GamePause()
    {
        Time.timeScale = 0;
    }
    public void GameOver()
    {
        isOver = true;
        EventGameOver?.Invoke();
    }
    #endregion

    #region Player Method
    public float GetPlayerHealthPer()
    {
        return ObjectManager.instance.player.GetHealthPer();
    }
    public float GetPlayerShieldPer()
    {
        return ObjectManager.instance.player.GetShieldPer();
    }
    #endregion
}
