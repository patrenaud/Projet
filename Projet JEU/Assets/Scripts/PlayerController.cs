using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public bool m_CanMove = false;
    public bool m_CanAttack = false;
    public bool m_CanHability = false;
    public Button m_AttackButton;
    public Button m_MoveButton;
    public Button m_AbilityButton;


    private void Update()
    {
        if (m_CanMove)
        {
            Move();
        }
        else if (m_CanAttack)
        {
            Attack();
        }
        else if (m_CanHability)
        {
            //Hability();
        }


    }

    public void Move()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray rayon = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit Hitinfo;

            if (Physics.Raycast(rayon, out Hitinfo, 500f, LayerMask.GetMask("Player")))
            {
                Debug.Log("Invalid Move");

            }
            else if (Physics.Raycast(rayon, out Hitinfo, 500f, LayerMask.GetMask("Enemy")))
            {
                Debug.Log("Invalid Move");

            }
            else if (Physics.Raycast(rayon, out Hitinfo, 500f, LayerMask.GetMask("Ground")))
            {
                Debug.Log("Move");
                m_CanMove = false;
                m_MoveButton.interactable = false;
            }
        }
    }

    public void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray rayon = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit Hitinfo;

            if (Physics.Raycast(rayon, out Hitinfo, 500f, LayerMask.GetMask("Enemy")))
            {
                Debug.Log("Attack");
                m_CanAttack = false;
                m_AttackButton.interactable = false;
            }
        }
    }

    public void EndTurn()
    {

        Debug.Log("End Turn");
    }


    #region Activatables
    public void ActivateMove()
    {
        m_CanMove = true;
        Debug.Log("CanMove");
    }

    public void ActivateAttack()
    {
        m_CanAttack = true;
        Debug.Log("CanAttack");
    }

    public void ActivateHabilty()
    {
        m_CanHability = true;
        Debug.Log("Hability");
    }
    #endregion
}
