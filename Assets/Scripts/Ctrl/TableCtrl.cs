using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableCtrl : MonoBehaviour
{
    private Transform mTableObj;
    private Transform mTrashObj;
    private bool mIsUse = false;

    private void Awake()
    {
        mTableObj = this.transform.GetChild(0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (GetIsUse() == false)
            return;

        if(collision.transform.CompareTag("Player"))
        {
            collision.transform.GetComponent<UnitCtrl>().SetIsTrash(true);
            mTrashObj.SetParent(collision.transform);
            mTrashObj.transform.localPosition = Vector3.up + (Vector3.forward * 0.35f);
            SetIsUse(false);
        }
    }

    public Vector3 GetTrashPos() => mTableObj.position+ Vector3.up *2.5f;
    public Vector3 GetSitPos() => mTableObj.position + (Vector3.left * 1.5f);
    public bool GetIsUse() => mIsUse;

    public void SetIsUse(bool isUse) => mIsUse = isUse;
    public void SetTrash(GameObject trash) => mTrashObj = trash.transform;
}
