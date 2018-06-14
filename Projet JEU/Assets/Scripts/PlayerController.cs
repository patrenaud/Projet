﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public TurnManager m_Turnmanager;
    [HideInInspector]
    public bool m_CanMove = false;
    [HideInInspector]
    public bool m_CanAttack = false;
    [HideInInspector]
    public bool m_CanAbility = false;
    [HideInInspector]
    public bool m_EndTurn = false;
    public bool m_RangeAttack = false;
    public bool m_CanAttackBoss;
    public Slider m_HealthBar;
    public Slider m_XpBar;
    public Button m_AttackButton;
    public Button m_MeleeAttackButton;
    public Button m_RangeAttackButton;
    public Button m_MoveButton;
    public Button m_AbilityButton;
    public Button m_EndTurnButton;
    public Button m_Ability1;
    public Button m_Ability2;
    public Button m_Ability3;
    public Button m_Ability4;
    public Button m_LevelUpButton;
    [HideInInspector]
    public float m_CurrentHealth;
    public GameObject m_UpgradeCanvas;
    public GameObject m_MoveZone;
    public GameObject m_AttackZone;
    public GameObject m_RangeAttackZone;
    public Material m_PlayerMaterial;
    [HideInInspector]
    public Vector3 m_TargetPosition;
    

    [SerializeField]
    private PlayerData m_PlayerData;
    private Vector3 m_ScaleOfAttackZone;
    private Vector3 m_ScaleOfRangeAttackZone;
    private bool m_MeleeButtonIsPressed = false;
    private bool m_RangeButtonIsPressed = false;
    private float m_MoveSpeed;
    private float m_MaxHealth;
    private float m_HealthRegenAbility;

    private void Awake()
    {
        m_UpgradeCanvas.gameObject.SetActive(false);
        m_Ability1.gameObject.SetActive(false);
        m_Ability2.gameObject.SetActive(false);
        m_Ability3.gameObject.SetActive(false);
        m_Ability4.gameObject.SetActive(false);
        m_LevelUpButton.gameObject.SetActive(false);
        m_RangeAttackButton.gameObject.SetActive(false);
        m_MeleeAttackButton.gameObject.SetActive(false);
        m_MoveZone.SetActive(false);

        m_ScaleOfAttackZone = m_AttackZone.transform.localScale * m_PlayerData.MeleeAttackRange;
        m_ScaleOfRangeAttackZone = m_RangeAttackZone.transform.localScale * m_PlayerData.RangeAttackRange;
        m_AttackZone.transform.localScale = Vector3.zero;
        m_RangeAttackZone.transform.localScale = Vector3.zero;
    }

    private void Start()
    {
        m_MoveSpeed = m_PlayerData.MoveSpeed;
        m_MaxHealth = m_PlayerData.MaxHealth;
        m_HealthRegenAbility = m_PlayerData.HealthRegenAbility;

        m_CurrentHealth = m_MaxHealth;
        m_HealthBar.value = 1;
        m_XpBar.value = 0f;
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

        if (m_XpBar.value >= 1)
        {
            m_LevelUpButton.gameObject.SetActive(true);
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
                    //ApplyDamage(); // Le joueur peut attaquer un enemie dans sa zone de move seulement si elle est dans la zone d'attaque aussi
                    AttackEnd(Hitinfo);
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
                m_TargetPosition.x = Hitinfo.point.x;
                m_TargetPosition.z = Hitinfo.point.z;
                m_TargetPosition.y = transform.position.y;

                StartCoroutine(MovetoPoint()); // Lorsque les conditions sont
                m_CanMove = false;
                m_MoveButton.interactable = false;
                m_MoveZone.SetActive(false);
            }
        }
    }

    // Permet le déplacment du joueur 
    private IEnumerator MovetoPoint()
    {
        float Timer = 0f;
        {
            transform.LookAt(m_TargetPosition);
            while (Vector3.Distance(m_TargetPosition, transform.position) > 0)
            {
                transform.position = Vector3.Lerp(transform.position, m_TargetPosition, Timer);
                Timer += Time.deltaTime * m_MoveSpeed / 60;
                yield return new WaitForEndOfFrame();
            }
        }
    }

    // Permet l'attaque du joueur vers l'ennemi
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
                    //ApplyDamage();
                    AttackEnd(Hitinfo);
                }
            }
            else if (Physics.Raycast(rayon, out Hitinfo, 500f, LayerMask.GetMask("Boss")) && m_CanAttackBoss)
            {
                // This part is the condition to defeat the Boss
                if (Hitinfo.collider.gameObject.GetComponent<EnemyController>().m_Attackable)
                {
                    //ApplyDamage();
                    AttackEnd(Hitinfo);
                }
                else
                {
                    Debug.Log("Can't attack");
                }
            }
        }
    }

    private void AttackEnd(RaycastHit i_Hitinfo)
    {
        m_Turnmanager.m_Characters.Remove(i_Hitinfo.collider.gameObject);
        Destroy(i_Hitinfo.collider.gameObject);  // Ceci est un fake pour montrer le kill de l'ennemi   
        m_XpBar.value += 0.40f;

        m_CanAttack = false;
        m_AttackButton.interactable = false;
        m_AttackZone.transform.localScale = Vector3.zero;
        m_RangeAttackZone.transform.localScale = Vector3.zero;
        m_MeleeAttackButton.gameObject.SetActive(false);
        m_RangeAttackButton.gameObject.SetActive(false);
        m_MeleeButtonIsPressed = false;
        m_RangeButtonIsPressed = false;
    }

    public void ApplyDamage(float i_AttackDamage)
    {
        // Place to Apply damage
        m_CurrentHealth -= i_AttackDamage;
        m_HealthBar.value = m_CurrentHealth / m_MaxHealth;
        StartCoroutine(ApplyDamageFeedback());
    }
    private IEnumerator ApplyDamageFeedback()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        gameObject.GetComponent<Renderer>().material.color = m_PlayerMaterial.color;
    }

    public void Ability()
    {
        m_Ability1.gameObject.SetActive(true);
        m_Ability2.gameObject.SetActive(true);
        m_Ability3.gameObject.SetActive(true);
        m_Ability4.gameObject.SetActive(true);

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
            m_MoveZone.SetActive(false);
            m_CanMove = false;
        }
        else if (!m_CanMove)
        {
            m_CanMove = true;
            m_MoveZone.SetActive(true);
        }
    }
    public void ActivateAttack()
    {
        if (!m_RangeAttack)
        {
            if (m_CanAttack)
            {
                m_CanAttack = false;
                m_AttackZone.transform.localScale = Vector3.zero;
            }
            else if (!m_CanAttack)
            {
                m_CanAttack = true;
                m_AttackZone.transform.localScale = m_ScaleOfAttackZone;
            }
        }
        else
        {
            if (m_CanAttack)
            {
                m_CanAttack = false;

                m_MeleeAttackButton.gameObject.SetActive(false);
                m_RangeAttackButton.gameObject.SetActive(false);
            }
            else if (!m_CanAttack)
            {
                m_CanAttack = true;

                m_MeleeAttackButton.gameObject.SetActive(true);
                m_RangeAttackButton.gameObject.SetActive(true);
            }
        }
    }

    public void ActivateMeleeAttack()
    {
        m_MeleeButtonIsPressed = !m_MeleeButtonIsPressed;
        m_AttackZone.transform.localScale = m_MeleeButtonIsPressed ? m_ScaleOfAttackZone : Vector3.zero;
    }

    public void ActivateRangeAttack()
    {
        m_RangeButtonIsPressed = !m_RangeButtonIsPressed;
        m_RangeAttackZone.transform.localScale = m_RangeButtonIsPressed ? m_ScaleOfRangeAttackZone : Vector3.zero;
    }

    public void ActivateHabilty()
    {
        if (m_CanAbility)
        {
            m_Ability1.gameObject.SetActive(false);
            m_Ability2.gameObject.SetActive(false);
            m_Ability3.gameObject.SetActive(false);
            m_Ability4.gameObject.SetActive(false);
            m_CanAbility = false;
        }
        else if (!m_CanAbility)
        {
            m_CanAbility = true;
            m_Ability1.gameObject.SetActive(true);
            m_Ability2.gameObject.SetActive(true);
            m_Ability3.gameObject.SetActive(true);
            m_Ability4.gameObject.SetActive(true);
        }
    }
    public void ActivateAbility1()
    {
        // Has to be filled with Abilities
    }
    public void ActivateAbility2()
    {
        // Has to be filled with Abilities
    }
    public void ActivateAbility3()
    {
        // Has to be filled with Abilities
    }
    public void ActivateAbility4()
    {
        m_HealthBar.value += m_HealthRegenAbility;
        m_Ability4.interactable = false;
    }
    #endregion

    public void LevelUp()
    {
        m_UpgradeCanvas.gameObject.SetActive(true);
    }
}
