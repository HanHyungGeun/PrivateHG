using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyManager : MonoBehaviour
{
    private List<GameObject> mEnemyList       = new List<GameObject>();
    private WaitForSeconds   mWait_SpawnDelay = new WaitForSeconds(1f);
    private WaitForSeconds   mWait_EnemyUseTime = new WaitForSeconds(3f);

    private Transform   mMyTrans;
    private TableCtrl   mTableCtrl;
    private CounterCtrl mCountCtrl;

    private const int mEnemyCountMax = 10;

    private void Awake()
    {
        mMyTrans   = this.transform;
        mTableCtrl = FindAnyObjectByType<TableCtrl>();
        mCountCtrl = FindAnyObjectByType<CounterCtrl>();

        StartCoroutine(cSpawnUpdate());
    }

    private void sortEnemy()
    {
        for (int i = 0; i < mEnemyList.Count; i++)
        {
            int idx = i;
            mEnemyList[idx].transform.DOKill();
            mEnemyList[idx].transform.DOMove(mMyTrans.position + (Vector3.back * 1.5f * i), 2f)
                .SetEase(Ease.Linear)
                .SetDelay(0.5f)
                .OnComplete(() =>
            { 
                mEnemyList[idx].GetComponent<UnitCtrl>().SetIsRun(false);
            });
            mEnemyList[idx].GetComponent<UnitCtrl>().SetIsRun(true);
        }
    }
     

    private IEnumerator cSpawnUpdate()
    {
        yield return mWait_SpawnDelay;
        while (true)
        {
            if (mEnemyList.Count < mEnemyCountMax)
            {
                GameObject newEnemy = RscModule.Instance.GetPool(eRscType.Unit, (int)eUnitType.Enemy).GetObject();
                UnitCtrl newUnit = newEnemy.GetComponent<UnitCtrl>();
                newEnemy.transform.SetParent(mMyTrans);
                newEnemy.transform.position = mMyTrans.position + (Vector3.back * 20);
                newEnemy.transform.DOMove(mMyTrans.position + (Vector3.back * 1.5f * mEnemyList.Count), 5f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    newUnit.SetIsRun(false);
                });
                newUnit.SetIsRun(true);
                mEnemyList.Add(newEnemy);
                yield return mWait_SpawnDelay;
            }
            else
            {
                if (mCountCtrl.GetIsBurger())
                {
                    GameObject enemy = mEnemyList[0];
                    Transform burger = mCountCtrl.GetBurger();
                    burger.transform.DOKill();
                    burger.transform.SetParent(enemy.transform);
                    burger.transform.localPosition = (Vector3.up * 0.5f) + (Vector3.forward * 0.35f);

                    enemy.transform.DOMove(mTableCtrl.GetSitPos(), 2f)
                        .SetEase(Ease.Linear)
                        .OnStart(() => enemy.GetComponent<UnitCtrl>().SetIsRun(true))
                        .OnUpdate(() =>
                        {
                            Vector3 dir = mTableCtrl.GetSitPos() - enemy.transform.position;
                            dir.Normalize();
                            enemy.transform.rotation = Quaternion.Lerp(enemy.transform.rotation, Quaternion.Euler(0, Quaternion.LookRotation(dir).eulerAngles.y, 0), 5 * Time.deltaTime);
                        })
                        .OnComplete(() =>
                        {
                            enemy.GetComponent<UnitCtrl>().SetIsRun(false);
                            mTableCtrl.SetIsUse(true);
                        });
                    mEnemyList.RemoveAt(0);
                    mCountCtrl.SortBurger();
                    mCountCtrl.SetSellCount(mCountCtrl.GetSellCount() + 1);
                    sortEnemy();

                    yield return mWait_EnemyUseTime;
                    GameObject trash = RscModule.Instance.GetPool(eRscType.Object, (int)eObjectType.Trash).GetObject();
                    trash.transform.SetParent(mTableCtrl.transform);
                    trash.transform.position = mTableCtrl.GetTrashPos();
                    mTableCtrl.SetTrash(trash.gameObject);

                    burger.SetParent(null);
                    burger.gameObject.SetActive(false);
                    yield return mWait_SpawnDelay;
                    enemy.gameObject.SetActive(false);
                    yield return StartCoroutine(cCheckTrash());
                }
                else
                {
                    yield return null;
                }
            }
        }
    }

    private IEnumerator cCheckTrash()
    {
        while (mTableCtrl.GetIsUse())
        {
            yield return null;
        }
    }


}
