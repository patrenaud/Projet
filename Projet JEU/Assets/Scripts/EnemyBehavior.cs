using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{









    public bool m_Attackable = false;
    // m_Attackable means that we can press anywhere on the enemy that touches AttackZone to Attack it
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