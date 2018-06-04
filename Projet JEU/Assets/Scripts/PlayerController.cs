using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public TurnManager m_Turnmanager;
    public bool m_CanMove = false;
    public bool m_CanAttack = false;
    public bool m_CanAbility = false;
    public bool m_EndTurn = false;
    public Button m_AttackButton;
    public Button m_MoveButton;
    public Button m_AbilityButton;
    public Button m_EndTurnButton;
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
        else if (m_EndTurn)
        {
            EndTurn();
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
                Debug.Log("Invalid Move"); // Le joueur ne peut pas se déplacer sur lui-même

            }
            else if (Physics.Raycast(rayon, out Hitinfo, 500f, LayerMask.GetMask("Enemy")))
            {
                if (Hitinfo.collider.gameObject.GetComponent<EnemyController>().m_Attackable)
                {
                    ApplyDamage(); // Le joueur peut attaquer un enemie dans sa zone de move seulement si elle est dans la zone d'attaque aussi
                    m_Turnmanager.m_Characters.Remove(Hitinfo.collider.gameObject);
                    Destroy(Hitinfo.collider.gameObject);
                }
            }
            else if (Physics.Raycast(rayon, out Hitinfo, 500f, LayerMask.GetMask("UI")))
            {
                // Si le joueur click à nouveau sur le bouton Move, il annule son mouvement.
                m_CanMove = false;
            }
            // Permet le move seulement si le click est dans la MoveZone et sur le Ground 
            else if (Physics.Raycast(rayon, out Hitinfo, 500f, LayerMask.GetMask("Ground")) && Physics.Raycast(rayon, out Hitinfo, 500f, LayerMask.GetMask("MoveZone")))
            {
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
                if (Hitinfo.collider.gameObject.GetComponent<EnemyController>().m_Attackable)
                {
                    ApplyDamage();
                    m_Turnmanager.m_Characters.Remove(Hitinfo.collider.gameObject);
                    Destroy(Hitinfo.collider.gameObject);
                    // Ceci est un fake pour montrer le kill
                    gameObject.transform.localScale += new Vector3(1, 0, 1);
                }
            }
        }
    }

    public void ApplyDamage()
    {
        // Place to Apply damage
        m_CanAttack = false;
        m_AttackButton.interactable = false;
        m_AttackPrefab.transform.localScale = Vector3.zero;
    }

    public void Ability()
    {
        m_CanAbility = false;
        m_AbilityButton.interactable = false;
    }

    public void EndTurn()
    {
        // Ici on reset les buttons du joueur
        m_AttackButton.interactable = false;
        m_MoveButton.interactable = false;
        m_AbilityButton.interactable = false;
        m_EndTurnButton.interactable = false;
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
            m_AttackPrefab.transform.localScale = m_NormalAttackZone;
        }
    }
    public void ActivateHabilty()
    {
        m_CanAbility = true;
    }
    #endregion
}
