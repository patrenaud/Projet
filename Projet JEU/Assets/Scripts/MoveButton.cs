using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveButton : MonoBehaviour
{
    

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray rayon = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit Hitinfo;

            if (Physics.Raycast(rayon, out Hitinfo, 100f, LayerMask.GetMask("Enemy")))
            {
                Debug.Log("Attack");
            }
            else if (Physics.Raycast(rayon, out Hitinfo, 100f, LayerMask.GetMask("Ground")))
            {
                Debug.Log("Move");
            }
        }
    }
}
