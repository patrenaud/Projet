﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Data", fileName = "PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    [SerializeField]
    private float m_MeleeAttackRange = 1;
    public float MeleeAttackRange
    {
        get { return m_MeleeAttackRange; } // Constant
    }

    [SerializeField]
    private float m_RangeAttackRange = 1;
    public float RangeAttackRange
    {
        get { return m_RangeAttackRange; } // Stat = Portée
    }

    [SerializeField]
    private float m_MoveSpeed = 1f;
    public float MoveSpeed
    {
        get { return m_MoveSpeed; } // Stat = Constant
    }

    [SerializeField]
    private float m_MaxHealth = 100;
    public float MaxHealth
    {
        get { return m_MaxHealth; } // Stat = Constitution
    }

    [SerializeField]
    private float m_HealthRegenAbility = 0.25f;
    public float HealthRegenAbility
    {
        get { return m_HealthRegenAbility; }
    }

    // Range Attack Damage (Perception)
    // Melee Attack Damage (Force)
    // % Chance to hit (Precision)
    // Move distance scale + Dodge Chance (Dexterity)
}