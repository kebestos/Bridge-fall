﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class MenuManager : Manager<MenuManager>
{

    [Header("Panels")]
    [SerializeField] GameObject m_PanelMainMenu;
    [SerializeField] GameObject m_PanelInGameMenu;
    [SerializeField] GameObject m_PanelVictory;
    [SerializeField] GameObject m_PanelGameOver;

    List<GameObject> m_AllPanels;

    protected override IEnumerator InitCoroutine()
    {
        m_AllPanels = new List<GameObject>();
        m_AllPanels.Add(m_PanelMainMenu);
        m_AllPanels.Add(m_PanelInGameMenu);
        m_AllPanels.Add(m_PanelVictory);
        m_AllPanels.Add(m_PanelGameOver);
        yield break;
    }

    void OpenPanel(GameObject panel)
    {
        foreach (var item in m_AllPanels)
            if (item) item.SetActive(item == panel);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            EscapeButtonHasBeenClicked();
        }
    }
    
    public void EscapeButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new EscapeButtonClickedEvent());
    }

    public void PlayButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new PlayButtonClickedEvent());
    }

    public void ResumeButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new ResumeButtonClickedEvent());
    }

    public void MainMenuButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new MainMenuButtonClickedEvent());
    }

    //GameManager Events
    protected override void GameMenu(GameMenuEvent e)
    {
        OpenPanel(m_PanelMainMenu);
    }

    protected override void GamePlay(GamePlayEvent e)
    {
        OpenPanel(null);
    }

    protected override void GamePause(GamePauseEvent e)
    {
        OpenPanel(m_PanelInGameMenu);
    }

    protected override void GameResume(GameResumeEvent e)
    {
        OpenPanel(null);
    }

    protected override void GameOver(GameOverEvent e)
    {
        OpenPanel(m_PanelGameOver);
    }

    protected override void GameVictory(GameVictoryEvent e)
    {
        OpenPanel(m_PanelVictory);
    }

    public void MainMenuButtonReloadSceneHasBeenClicked()
    {
        EventManager.Instance.Raise(new ReloadSceneHasBeenClicked());
    }
}
