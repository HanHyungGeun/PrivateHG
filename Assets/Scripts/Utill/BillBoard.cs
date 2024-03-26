using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    private Transform _CamTrans;
    private Transform _MyTrans;

    private void Start()
    {
        _MyTrans  = this.transform;
        _CamTrans = Camera.main.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        _MyTrans.LookAt(_MyTrans.position + _CamTrans.forward);
    }
}