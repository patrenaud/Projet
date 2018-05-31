﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public List<GameObject> m_Characters = new List<GameObject>();
    public Button m_AttackButton;
    public Button m_MoveButton;
    public Button m_AbilityButton;
    public bool m_SwitchCharacter = false;
    private int m_Turn = 1;

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
                m_Characters[m_Turn - 1].gameObject.GetComponent<Renderer>().material.color = Color.white;
                m_Characters[m_Turn].gameObject.GetComponent<Renderer>().material.color = Color.red;
                Debug.Log(m_Characters[m_Turn]);
                //*******   Le Script doit envoyer un appel pour débuter le tour au joueur/ennemi actif */

                m_Turn++;
            }
            else
            {
                m_Turn = 1;
                m_Characters[m_Characters.Count - 1].gameObject.GetComponent<Renderer>().material.color = Color.white;                
                Debug.Log(m_Characters[m_Turn - 1]);
                //*******   Le Script doit envoyer un appel pour débuter le tour au joueur */

                // Les boutons sont activés pour le tour du joueur
                m_AttackButton.interactable = true;
                m_MoveButton.interactable = true;
                m_AbilityButton.interactable = true;

            }
            m_SwitchCharacter = false;
        }
    }

    // Le End Button appel cette focntion pour passer au prochain object de la liste
    public void ActivateSwitchCharacter()
    {
        m_SwitchCharacter = true;
    }
}
