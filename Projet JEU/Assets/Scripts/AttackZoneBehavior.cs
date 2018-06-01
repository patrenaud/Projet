using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZoneBehavior : MonoBehaviour
{
    public bool m_Attackable = false;

    private void OnTriggerStay(Collider a_Other)
    {
        if (a_Other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            a_Other.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
            m_Attackable = true;
        }
        else if (a_Other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Enemy Attack");

            // ICI LE DÉGUB NE PART PAS. JE DOIS TROUVER UN MOYEN DE TERMINER LE TOUR DES ENNEMIES

            a_Other.gameObject.transform.localScale -= new Vector3(1, 0, 1);
            gameObject.GetComponentInParent<EnemyController>().m_IsPlaying = false;
            gameObject.transform.localScale = Vector3.zero;
        }
    }

    private void OnTriggerExit(Collider a_Other)
    {
        if (a_Other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            a_Other.gameObject.GetComponent<Renderer>().material.color = Color.white;
            m_Attackable = false;
        }
    }
}
