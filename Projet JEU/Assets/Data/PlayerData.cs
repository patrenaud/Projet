using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Data", fileName = "PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    [SerializeField]
    private float m_MeleeAttackRange = 1;
    public float MeleeAttackRange
    {
        get { return m_MeleeAttackRange; }
    }

    [SerializeField]
    private float m_RangeAttackRange = 1;
    public float RangeAttackRange
    {
        get { return m_RangeAttackRange; }
    }

    [SerializeField]
    private float m_MoveSpeed = 1f;
    public float MoveSpeed
    {
        get { return m_MoveSpeed; }
    }

    [SerializeField]
    private float m_MaxHealth = 100;
    public float MaxHealth
    {
        get { return m_MaxHealth; }
    }

    [SerializeField]
    private float m_HealthRegenAbility = 0.25f;
    public float HealthRegenAbility
    {
        get { return m_HealthRegenAbility; }
    }
}