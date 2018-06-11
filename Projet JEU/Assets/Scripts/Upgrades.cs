using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Upgrades : MonoBehaviour
{
    //public PlayerController m_PlayerController;


    public void ActivateRangedAttack()
    {
        //m_PlayerController.m_RangeAttack = true;
    }

	public void LoadMainScene()
	{
		SceneManager.LoadScene("Main",LoadSceneMode.Single);
	}
}
