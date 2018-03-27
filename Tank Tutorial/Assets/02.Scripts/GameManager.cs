﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int m_NumRoundsToWin = 5;
    public float m_StartDelay = 3f;
    public float m_EndDelay = 3f;
    public CameraControl m_CameraControl;
    public Text m_MessageText;
    public GameObject[] m_TankList;
    public TankManager[] m_Tanks;
    public int m_ChosenTank;
    public GameObject m_TankSelect;
    public GameObject m_PlayPanel;
    public GameObject m_EnemyTank;

    private int m_RoundNumber;
    private WaitForSeconds m_StartWait;
    private WaitForSeconds m_EndWait;
    private TankManager m_RoundWinner;
    private TankManager m_GameWinner;
    private bool isTankSelected = false;

    private void Start()
    {
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        StartCoroutine(GameStart());
    }

    private IEnumerator GameStart()
    {
        m_MessageText.text = "TANKS!";
        m_PlayPanel.SetActive(false);
        yield return m_StartWait;

        ShowTankSelect();
    }

    public void ButtonClickToStart()
    {
        m_TankSelect.SetActive(false);
        SpawnPlayerTank();
        SpawnEnemyTank();
        SetCameraTargets();
        StartCoroutine(GameLoop());
    }

    private void ShowTankSelect()
    {
        m_ChosenTank = -1;
        m_MessageText.text = "\n\n\n\n" + "SELECT TANK!";
        m_TankSelect.SetActive(true);
    }


    private void SpawnPlayerTank()
    {
        int playerNumber = 0;
        m_TankList[m_ChosenTank].SetActive(true);
        m_TankList[m_ChosenTank].transform.position = new Vector3(m_Tanks[playerNumber].m_SpawnPoint.position.x, m_Tanks[playerNumber].m_SpawnPoint.position.y, m_Tanks[playerNumber].m_SpawnPoint.position.z);
        m_TankList[m_ChosenTank].transform.rotation = 
            new Quaternion(m_Tanks[playerNumber].m_SpawnPoint.rotation.x, m_Tanks[playerNumber].m_SpawnPoint.rotation.y, m_Tanks[playerNumber].m_SpawnPoint.rotation.z, m_Tanks[playerNumber].m_SpawnPoint.rotation.w);
        m_Tanks[playerNumber].m_Instance = m_TankList[m_ChosenTank];
        m_Tanks[playerNumber].m_PlayerNumber = playerNumber + 1;
        m_Tanks[playerNumber].Setup();
    }

    private void SpawnEnemyTank()
    {
        int EnemyNumber = 1;
        m_EnemyTank.SetActive(true);
        m_EnemyTank.transform.position = new Vector3(m_Tanks[EnemyNumber].m_SpawnPoint.position.x, m_Tanks[EnemyNumber].m_SpawnPoint.position.y, m_Tanks[EnemyNumber].m_SpawnPoint.position.z);
        m_EnemyTank.transform.rotation = 
            new Quaternion(m_Tanks[EnemyNumber].m_SpawnPoint.rotation.x, m_Tanks[EnemyNumber].m_SpawnPoint.rotation.y, m_Tanks[EnemyNumber].m_SpawnPoint.rotation.z, m_Tanks[EnemyNumber].m_SpawnPoint.rotation.w);
        m_Tanks[EnemyNumber].m_Instance = m_EnemyTank;
        m_Tanks[EnemyNumber].m_PlayerNumber = EnemyNumber + 1;
        m_Tanks[EnemyNumber].Setup();
    }


    private void SetCameraTargets()
    {
        Transform[] targets = new Transform[m_Tanks.Length];

        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = m_Tanks[i].m_Instance.transform;
        }

        m_CameraControl.m_Targets = targets;
    }


    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        if (m_GameWinner != null)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            StartCoroutine(GameLoop());
        }
    }


    private IEnumerator RoundStarting()
    {
        ResetAllTanks();
        DisableTankControl();


        m_CameraControl.SetStartPositionAndSize();

        m_RoundNumber++;
        m_MessageText.text = "ROUND " + m_RoundNumber;

        yield return m_StartWait;
    }


    private IEnumerator RoundPlaying()
    {
        EnableTankControl();

        m_MessageText.text = string.Empty;

        while (!OneTankLeft())
        {
            yield return null;
        }
    }


    private IEnumerator RoundEnding()
    {
        DisableTankControl();

        m_RoundWinner = null;

        m_RoundWinner = GetRoundWinner();

        if (m_RoundWinner != null)
            m_RoundWinner.m_Wins++;

        m_GameWinner = GetGameWinner();

        string message = EndMessage();
        m_MessageText.text = message;

        yield return m_EndWait;
    }


    private bool OneTankLeft()
    {
        int numTanksLeft = 0;

        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_Instance.activeSelf)
                numTanksLeft++;
        }

        return numTanksLeft <= 1;
    }


    private TankManager GetRoundWinner()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_Instance.activeSelf)
                return m_Tanks[i];
        }

        return null;
    }


    private TankManager GetGameWinner()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_Wins == m_NumRoundsToWin)
                return m_Tanks[i];
        }

        return null;
    }


    private string EndMessage()
    {
        string message = "DRAW!";

        if (m_RoundWinner != null)
            message = m_RoundWinner.m_ColoredPlayerText + " WINS THE ROUND!";

        message += "\n\n\n\n";

        for (int i = 0; i < m_Tanks.Length; i++)
        {
            message += m_Tanks[i].m_ColoredPlayerText + ": " + m_Tanks[i].m_Wins + " WINS\n";
        }

        if (m_GameWinner != null)
            message = m_GameWinner.m_ColoredPlayerText + " WINS THE GAME!";

        return message;
    }


    private void ResetAllTanks()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].Reset();
        }
    }


    private void EnableTankControl()
    {
        m_PlayPanel.SetActive(true);
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].EnableControl();
        }
    }


    private void DisableTankControl()
    {
        m_PlayPanel.SetActive(false);
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].DisableControl();
        }
    }

    public void ChooseLightTank()
    {
        m_ChosenTank = 0;

        isTankSelected = true;
    }

    public void ChooseNormalTank()
    {
        m_ChosenTank = 1;

        isTankSelected = true;
    }

    public void ChooseHeavyTank()
    {
        m_ChosenTank = 2;

        isTankSelected = true;
    }
}
