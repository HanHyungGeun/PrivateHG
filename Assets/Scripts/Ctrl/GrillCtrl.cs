using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GrillCtrl : MonoBehaviour
{
    [SerializeField]
    private GameObject FinishPanelObj;

    private ParticleSystem   mFx_GrillEnable;
    private WaitForSeconds   mWaitEnable = new WaitForSeconds(1f);
    private List<GameObject> mCurrentBurgerList = new List<GameObject>();
    

    private bool mIsEnable;

    private void Awake()
    {
        mFx_GrillEnable = this.GetComponentInChildren<ParticleSystem>(true);
        mIsEnable = isCheckEnable();
        StartCoroutine(cGrillUpdate());
    }

    private void setEnableFx(bool isEnable)
    {
        mFx_GrillEnable.gameObject.SetActive(isEnable);
        mFx_GrillEnable.Play();
    }

    private bool isCheckEnable() =>
        mIsEnable = mCurrentBurgerList.Count < 4 ;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Player"))
        {
            if (mCurrentBurgerList.Count > 0)
            {
                if (other.transform.GetComponent<UnitCtrl>().IsPickup())
                    return;

                for (int i = 0; i < 2; i++)
                {
                    if (mCurrentBurgerList.Count > 0)
                    {
                        GameObject target = mCurrentBurgerList[0];
                        target.transform.DOKill();
                        target.transform.SetParent(other.transform);
                        target.transform.localPosition = (Vector3.up * (i + 1) * 0.5f) + (Vector3.forward * 0.35f);

                        mCurrentBurgerList.RemoveAt(0);
                    }
                }

                sortBurger();
                if (!mIsEnable)
                {
                    mIsEnable = true;
                    StartCoroutine(cGrillUpdate());
                }
            }
        }
    }

    private void sortBurger()
    {
        for(int i = 0; i < mCurrentBurgerList.Count; i++)
        {
            mCurrentBurgerList[i].transform.DOMove(FinishPanelObj.transform.position + getPosX(i) + getPosY(i),0.5f);
        }
    }

    private IEnumerator cGrillUpdate()
    {
        float curTime = 0f;
        float endTIme = 1f;
        setEnableFx(true);

        yield return mWaitEnable;

        while (mIsEnable)
        {
            curTime += Time.deltaTime;
            if(curTime >= endTIme)
            {
                curTime = 0f;

                int count = mCurrentBurgerList.Count;
                GameObject newBurger = RscModule.Instance.GetPool(eRscType.Object, (int)eObjectType.Burger).GetObject();
                newBurger.transform.SetParent(FinishPanelObj.transform);
                newBurger.transform.position = this.transform.position;
                newBurger.transform.DOJump(FinishPanelObj.transform.position + getPosX(count) + getPosY(count),1f,1, 0.5f);

                mCurrentBurgerList.Add(newBurger.gameObject);
                if (mCurrentBurgerList.Count >= 4)
                    break;
            }
            yield return null;
        }

        mIsEnable = false;
        setEnableFx(false);
    }

    private Vector3 getPosX(int count) =>  count % 2 == 0 ? new Vector2(-0.75f , 0) : new Vector2(0.75f , 0);
    private Vector3 getPosY(int count) => new Vector3(0, 0.75f, 0) * (count / 2) + new Vector3(0f,0.5f,0f); 

}
