using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TurnManager : MonoBehaviour
{
    public List<GameObject> m_Characters = new List<GameObject>();
    public Button m_AttackButton;
    public Button m_MoveButton;
    public Button m_AbilityButton;
    public Button m_EndTurnButton;
    public bool m_SwitchCharacter = false;

    private int m_Turn = 1;

    private static TurnManager m_Instance; // Application d'un Singleton
    private static TurnManager Instance // Application d'un Singleton
    {
        get { return m_Instance; }
    }

    private void Awake() // Application d'un Singleton
    {
        if(m_Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            m_Instance = this;
        }        
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_SwitchCharacter = true;
        }

        if (m_SwitchCharacter)
        {
            if (m_Turn < m_Characters.Count)
            {
                m_Characters[m_Turn].GetComponent<EnemyController>().m_IsPlaying = true;
                m_Turn++;
            }
            else
            {
                m_Turn = 1;

                // Les boutons sont activés pour le tour du joueur
                m_AttackButton.interactable = true;
                m_MoveButton.interactable = true;
                m_AbilityButton.interactable = true;
                m_EndTurnButton.interactable = true;

            }
            m_SwitchCharacter = false;
        }

        if(m_Characters.Count <=1)
        {
            Debug.Log("Win the game");
            LoadLevel();            
        }
    }

    // Le End Button appel cette focntion pour passer au prochain object de la liste
    public void ActivateSwitchCharacter()
    {
        m_SwitchCharacter = true;
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("Upgrades",LoadSceneMode.Single);
    }
}
