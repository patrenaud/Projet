using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public TurnManager m_Turnmanager;
    public GameObject m_Player;
    public bool m_Attackable = false;
    public bool m_IsPlaying = false;
    public GameObject m_AttackZonePrefab;

    private Vector3 m_NormalAttackZone;

    private void Start()
    {
        m_NormalAttackZone = m_AttackZonePrefab.transform.localScale;
        m_AttackZonePrefab.transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        if (m_IsPlaying)
        {
            m_AttackZonePrefab.transform.localScale = m_NormalAttackZone;
            EnemyAttack();
        }
    }

    private void EnemyAttack()
    {
        m_IsPlaying = false; // Termine l'attaque de l'ennemi
        Debug.Log("EnemyAttack");
        //RaycastHit hit;
        StartCoroutine(EnemyAttackCoroutine());
        // Cette ligne permet de faire un Raycast de la grandeur du scale m'attackzoneprefab entre le player et l'ennemi.  Le size/2 trouve le rayon.
        if (Physics.Raycast(transform.position, (m_Player.transform.position - transform.position), m_AttackZonePrefab.transform.localScale.z / 2, LayerMask.GetMask("Player")))
        {
            Debug.DrawRay(transform.position, (m_Player.transform.position - transform.position), Color.red, m_AttackZonePrefab.transform.localScale.z / 2);
            m_Player.transform.localScale -= new Vector3(0.3f, 0f, 0.3f);
            Debug.Log("EnemyDamage");
            // ApplyDamage();
        }
    }

    private IEnumerator EnemyAttackCoroutine()
    {
        yield return new WaitForSeconds(2f);
        m_AttackZonePrefab.transform.localScale = Vector3.zero;
        m_Turnmanager.m_SwitchCharacter = true;
    }

    private void OnTriggerStay(Collider a_Other)
    {
        if (a_Other.gameObject.layer == LayerMask.NameToLayer("Interractible"))
        {
            gameObject.GetComponent<Renderer>().material.color = Color.yellow;
            m_Attackable = true;
        }
    }

    private void OnTriggerExit(Collider a_Other)
    {
        if (a_Other.gameObject.layer == LayerMask.NameToLayer("Interractible"))
        {
            gameObject.GetComponent<Renderer>().material.color = Color.white;
            m_Attackable = false;
        }
    }
}
