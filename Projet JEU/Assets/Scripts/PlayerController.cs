using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public bool m_CanMove = false;
    public bool m_CanAttack = false;
    public bool m_CanAbility = false;
    public Button m_AttackButton;
    public Button m_MoveButton;
    public Button m_AbilityButton;
    public float m_MoveSpeed = 5f;
    public GameObject m_MovePrefab;
    public GameObject m_AttackPrefab;
    public Vector3 m_Position;


    private Vector3 m_NormalAttackZone;


    private void Awake()
    {
        m_MovePrefab.SetActive(false);
        m_NormalAttackZone = m_AttackPrefab.transform.localScale;
        m_AttackPrefab.transform.localScale = Vector3.zero;
    }


    private void Update()
    {
        // Lorsque les bools sont activés par les boutons, les fonctions respectives sont appelées
        if (m_CanMove)
        {
            Move();
        }
        else if (m_CanAttack)
        {
            Attack();
        }
        else if (m_CanAbility)
        {
            Ability();
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
                ApplyDamage();
            }
            else if (Physics.Raycast(rayon, out Hitinfo, 500f, LayerMask.GetMask("UI")))
            {
                Debug.Log("Cancel Move"); // TO DO !!!! Doit annuler le Move lorsque l'on click de nouveau sur le bouton
                m_CanMove = false; // TO DO
            }
            // Permet le move seulement si le click est dans la zone de move et sur le ground 
            else if (Physics.Raycast(rayon, out Hitinfo, 500f, LayerMask.GetMask("Ground")) && Physics.Raycast(rayon, out Hitinfo, 500f, LayerMask.GetMask("MoveZone")))
            {
                Debug.Log("Move");
                m_Position.x = Hitinfo.point.x;
                m_Position.z = Hitinfo.point.z;
                m_Position.y = transform.position.y;

                StartCoroutine(MovetoPoint());
                m_CanMove = false;
                m_MoveButton.interactable = false;
                m_MovePrefab.SetActive(false);
            }
        }
    }

    // Permet le déplacment du joueur 
    private IEnumerator MovetoPoint()
    {
        float Timer = 0f;
        {
            transform.LookAt(m_Position);
            while (Vector3.Distance(m_Position, transform.position) > 0)
            {
                transform.position = Vector3.Lerp(transform.position, m_Position, Timer);
                Timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
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
                ApplyDamage();
            }
        }
    }

    public void ApplyDamage()
    {
        // DUM THING WONG
        // On peut attaquer un ennemi out of range si l'un d'entre eux est in range ***** Problème de Raycast
        if (GameObject.Find("Enemy").GetComponent<EnemyBehavior>().m_Attackable || GameObject.Find("Enemy2").GetComponent<EnemyBehavior>().m_Attackable)
        {
            Debug.Log("Attack");
            // Place to Apply damage
            m_CanAttack = false;
            m_AttackButton.interactable = false;
            m_AttackPrefab.transform.localScale = Vector3.zero;
        }
    }

    public void Ability()
    {
        Debug.Log(" Hability ");
        m_CanAbility = false;
        m_AbilityButton.interactable = false;
    }

    public void EndTurn()
    {

        Debug.Log("End Turn");
    }


    // Cette région permet aux boutons d'appaler ces fonctions. Les Booleens sont activés et permettent les Move/Attack/Ability/EndTurn
    #region Activatables
    public void ActivateMove()
    {
        if (m_CanMove)
        {
            m_MovePrefab.SetActive(false);
            m_CanMove = false;
        }
        else if (!m_CanMove)
        {
            m_CanMove = true;
            Debug.Log("CanMove");
            m_MovePrefab.SetActive(true);
        }
    }
    public void ActivateAttack()
    {
        if (m_CanAttack)
        {
            m_AttackPrefab.transform.localScale = Vector3.zero;
            m_CanAttack = false;
        }
        else if (!m_CanAttack)
        {
            m_CanAttack = true;
            Debug.Log("CanAttack");
            m_AttackPrefab.transform.localScale = m_NormalAttackZone;
        }
    }
    public void ActivateHabilty()
    {
        m_CanAbility = true;
        Debug.Log("Hability");
    }
    #endregion
}
