using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public List<GameObject> m_Characters = new List<GameObject>();

    private bool m_SwitchCharacter = false;
    private int m_Turn = 1;

    private void Update()
    {
        if (m_SwitchCharacter == true)
        {
            if (m_Turn < m_Characters.Count)
            {
                m_Characters[m_Turn].gameObject.SetActive(false);
                m_Characters[m_Turn + 1].gameObject.SetActive(true);
				Debug.Log(m_Characters[m_Turn]);
                m_Turn++;                
            }
            else
            {
                m_Characters[m_Turn].gameObject.SetActive(false);
                m_Turn = 1;
                m_Characters[m_Turn].gameObject.SetActive(true);
                Debug.Log(m_Characters[m_Turn]);
            }

            m_SwitchCharacter = false;
        }
    }
}
