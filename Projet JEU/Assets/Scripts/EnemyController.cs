using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
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
        }
        else
        {
            m_AttackZonePrefab.transform.localScale = Vector3.zero;
        }
    }
}
