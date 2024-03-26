using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCtrl : MonoBehaviour
{ 
    private void OnCollisionEnter(Collision collision)
    { 
        if (collision.transform.CompareTag("Player"))
        {
            UnitCtrl unitCtrl = collision.transform.GetComponent<UnitCtrl>();
            if (unitCtrl.IsTrash() == false)
                return;

            collision.transform.GetComponent<UnitCtrl>().SetIsTrash(false);
            collision.transform.GetChild(collision.transform.childCount - 1).GetComponent<PoolData>().AddPool();            
        }
    }

}
