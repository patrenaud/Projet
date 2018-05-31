using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public bool m_Attackable = false;

    private void OnTriggerStay(Collider a_Other)
    {
        if (a_Other.gameObject.layer == LayerMask.NameToLayer("Interractible"))
        {
            a_Other.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
            m_Attackable = true;
        }
    }

    private void OnTriggerLeave(Collider a_Other)
    {
        if (a_Other.gameObject.layer == LayerMask.NameToLayer("Interractible"))
        {
            a_Other.gameObject.GetComponent<Renderer>().material.color = Color.white;
            m_Attackable = false;
        }
    }
}