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
    public Slider m_HealthBar;
    public Slider m_XpBar;
    public Button m_AttackButton;
    public Button m_MoveButton;
    public Button m_AbilityButton;
    public Button m_EndTurnButton;
    public Button m_Ability1;
    public Button m_Ability2;
    public Button m_Ability3;
    public Button m_Ability4;
    public Button m_LevelUpButton;
    public float m_MoveSpeed = 5f;
    public float m_MaxHealth = 100;
    [HideInInspector]
    public float m_CurrentHealth;
    public GameObject m_MovePrefab;
    public GameObject m_AttackPrefab;
    public Material m_PlayerMaterial;
    [HideInInspector]
    public Vector3 m_Position;

    private Vector3 m_NormalAttackZone;
    private bool m_CanAttackBoss;


    private void Awake()
    {
        m_MovePrefab.SetActive(false);
        m_NormalAttackZone = m_AttackPrefab.transform.localScale;
        m_AttackPrefab.transform.localScale = Vector3.zero;
        m_CurrentHealth = m_MaxHealth;
        m_HealthBar.value = 1;
        m_XpBar.value = 0f;
        m_Ability1.gameObject.SetActive(false);
        m_Ability2.gameObject.SetActive(false);
        m_Ability3.gameObject.SetActive(false);
        m_Ability4.gameObject.SetActive(false);
        m_LevelUpButton.gameObject.SetActive(false);
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
                    m_Turnmanager.m_Characters.Remove(Hitinfo.collider.gameObject);
                    Destroy(Hitinfo.collider.gameObject);
                    m_XpBar.value += 0.40f;

                    m_CanAttack = false;
                    m_AttackButton.interactable = false;
                    m_AttackPrefab.transform.localScale = Vector3.zero;
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

                StartCoroutine(MovetoPoint()); // Lorsque les conditions sont 
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
                    m_Turnmanager.m_Characters.Remove(Hitinfo.collider.gameObject);
                    Destroy(Hitinfo.collider.gameObject);  // Ceci est un fake pour montrer le kill de l'ennemi   
                    m_XpBar.value += 0.40f;

                    m_CanAttack = false;
                    m_AttackButton.interactable = false;
                    m_AttackPrefab.transform.localScale = Vector3.zero;
                }
            }
            else if (Physics.Raycast(rayon, out Hitinfo, 500f, LayerMask.GetMask("Boss")) && m_CanAttackBoss)
            {
                if (Hitinfo.collider.gameObject.GetComponent<EnemyController>().m_Attackable)
                {
                    //ApplyDamage();
                    m_Turnmanager.m_Characters.Remove(Hitinfo.collider.gameObject);
                    Destroy(Hitinfo.collider.gameObject);  // Ceci est un fake pour montrer le kill de l'ennemi   
                    m_XpBar.value += 0.40f;

                    m_CanAttack = false;
                    m_AttackButton.interactable = false;
                    m_AttackPrefab.transform.localScale = Vector3.zero;
                    Debug.Log("BOSS DEFEATED");
                }
            }
        }
    }

    public void ApplyDamage(float i_AttackDamage)
    {
        // Place to Apply damage
        m_CurrentHealth -= i_AttackDamage;
        m_HealthBar.value = m_CurrentHealth / m_MaxHealth;
        StartCoroutine(ApplyDamageAnim());
    }
    private IEnumerator ApplyDamageAnim()
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

    }
    public void ActivateAbility2()
    {

    }
    public void ActivateAbility3()
    {

    }
    public void ActivateAbility4()
    {
        m_HealthBar.value += 0.15f;
        m_Ability4.interactable = false;
    }
    #endregion


    public void LevelUp()
    {
        gameObject.transform.localScale += new Vector3(1, 0, 1);
        m_LevelUpButton.gameObject.SetActive(false);
        m_XpBar.value = 0f;
        m_CanAttackBoss = true;
    }
}
