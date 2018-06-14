using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Upgrades : MonoBehaviour
{
    public PlayerController m_PlayerController;


    public void ActivateRangedAttack()
    {
        m_PlayerController.m_RangeAttack = true;
    }

    public void ReturnToGame()
    {
        m_PlayerController.m_UpgradeCanvas.gameObject.SetActive(false);
        m_PlayerController.m_LevelUpButton.gameObject.SetActive(false);
        m_PlayerController.m_XpBar.value = 0f;
        m_PlayerController.m_CanAttackBoss = true;
    }
}
