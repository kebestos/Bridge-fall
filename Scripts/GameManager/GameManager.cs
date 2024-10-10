using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using UnityEngine.SceneManagement;

public enum GameState { gameMenu, gamePlay, gamePause, gameOver, gameVictory }

public class GameManager : Manager<GameManager>
{
    [Header("Initial Node")]    

    //Game State
    private GameState m_GameState;

    public bool IsPlaying { get { return m_GameState == GameState.gamePlay; } }

    // TIME SCALE
    private float m_TimeScale;
    public float TimeScale { get { return m_TimeScale; } }
    void SetTimeScale(float newTimeScale)
    {
        m_TimeScale = newTimeScale;
        Time.timeScale = m_TimeScale;
    }   

    //Nodes
    [SerializeField] private int m_NStartNode;

    private int m_NNode;
    public int NNode { get { return m_NNode; } }
    void DecrementNNode(int decrement)
    {
        SetNNode(m_NNode - decrement);
        Debug.Log(m_NNode);
        if (m_NNode == 0)
        {
            Over();
        }
    }

    void SetNNode(int nNode)
    {
        m_NNode = nNode;
        EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eNode = m_NNode });
    }    

    public override void SubscribeEvents()
    {
        base.SubscribeEvents();
        EventManager.Instance.AddListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
        EventManager.Instance.AddListener<PlayButtonClickedEvent>(PlayButtonClicked);
        EventManager.Instance.AddListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
        EventManager.Instance.AddListener<EscapeButtonClickedEvent>(EscapeButtonClicked);
        EventManager.Instance.AddListener<EndPointHasBeenReachEvent>(EndPointHasBeenReach);
        EventManager.Instance.AddListener<NodeHasBeenCreateEvent>(NodeHasBeenCreate);
        EventManager.Instance.AddListener<ReloadSceneHasBeenClicked>(ReloadHasbeenClick);
    }

    public override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();
        EventManager.Instance.RemoveListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
        EventManager.Instance.RemoveListener<PlayButtonClickedEvent>(PlayButtonClicked);
        EventManager.Instance.RemoveListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
        EventManager.Instance.RemoveListener<EscapeButtonClickedEvent>(EscapeButtonClicked);
        EventManager.Instance.AddListener<EndPointHasBeenReachEvent>(EndPointHasBeenReach);
        EventManager.Instance.AddListener<NodeHasBeenCreateEvent>(NodeHasBeenCreate);
        EventManager.Instance.AddListener<ReloadSceneHasBeenClicked>(ReloadHasbeenClick);
    }

   protected override IEnumerator InitCoroutine()
   {
        while (!MenuManager.Instance.IsReady) yield return null;

        Menu();
        yield break;
   }

    private void InitNewGame()
    {
        SetNNode(m_NStartNode);
    }

    private void NodeHasBeenCreate(NodeHasBeenCreateEvent e)
	{
        DecrementNNode(1);
	}

    private void EndPointHasBeenReach(EndPointHasBeenReachEvent e)
    {
        Victory();
    }

    //Buttons Events
    private void MainMenuButtonClicked(MainMenuButtonClickedEvent e)
    {
        Menu();
    }

    private void PlayButtonClicked(PlayButtonClickedEvent e)
    {
        Play();
    }

    private void ResumeButtonClicked(ResumeButtonClickedEvent e)
    {
        Resume();
    }

    private void EscapeButtonClicked(EscapeButtonClickedEvent e)
    {
        if (IsPlaying)
            Pause();
    }

    //Game State events
    private void Menu()
    {
        SetTimeScale(0);
        m_GameState = GameState.gameMenu;
        EventManager.Instance.Raise(new GameMenuEvent());
    }

    private void Play()
    {
        InitNewGame();
        SetTimeScale(1);
        m_GameState = GameState.gamePlay;
        EventManager.Instance.Raise(new GamePlayEvent());
    }

    private void Pause()
    {
        SetTimeScale(0);
        m_GameState = GameState.gamePause;
        EventManager.Instance.Raise(new GamePauseEvent());
    }

    private void Resume()
    {
        SetTimeScale(1);
        m_GameState = GameState.gamePlay;
        EventManager.Instance.Raise(new GameResumeEvent());
    }

    private void Over()
    {
        SetTimeScale(0);
        m_GameState = GameState.gameOver;
        EventManager.Instance.Raise(new GameOverEvent());
    }

    private void Victory()
    {
        SetTimeScale(0);
        m_GameState = GameState.gameVictory;
        EventManager.Instance.Raise(new GameVictoryEvent());
    }

    private void ReloadHasbeenClick(ReloadSceneHasBeenClicked e)
    {
        InitNewGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }    
}
