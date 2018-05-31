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


    private void Awake()
    {
        m_MovePrefab.SetActive(false);
        m_AttackPrefab.SetActive(false);

    }

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
        else if (m_CanAbility)
        {
            Ability();
        }
    }

    private void FixedUpdate()
    {

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
            else if (Physics.Raycast(rayon, out Hitinfo, 500f, LayerMask.GetMask("UI")))
            {
                Debug.Log("Cancel Move");
                m_CanMove = false;
            }
            else if (Physics.Raycast(rayon, out Hitinfo, 500f, LayerMask.GetMask("Ground")) && Physics.Raycast(rayon, out Hitinfo, 500f, LayerMask.GetMask("Interractible")))
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
                // If bool de trigger de l'enemy est actif
                //if (GetComponent<EnemyBehavior>().m_Attackable)
                {
                    Debug.Log("Attack");
                    m_CanAttack = false;
                    m_AttackButton.interactable = false;
                    m_AttackPrefab.SetActive(false);
                }
            }
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


    #region Activatables
    public void ActivateMove()
    {
        m_CanMove = true;
        Debug.Log("CanMove");
        m_MovePrefab.SetActive(true);
        //Instantiate(m_MovePrefab, transform.gameObject.transform.position, transform.gameObject.transform.rotation);
    }

    public void ActivateAttack()
    {
        m_CanAttack = true;
        Debug.Log("CanAttack");
        m_AttackPrefab.SetActive(true);
    }

    public void ActivateHabilty()
    {
        m_CanAbility = true;
        Debug.Log("Hability");
    }
    #endregion
}
