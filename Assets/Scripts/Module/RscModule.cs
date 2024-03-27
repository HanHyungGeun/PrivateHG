using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eRscType
{
    Unit   = 0,
    Object = 1
}

public class RscModule : MonoBehaviour
{
    private static RscModule instance;
    public static RscModule Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<RscModule>();

            return instance;
        }
    }

    [SerializeField]
    private GameObject[] UnitPrefabs;
    [SerializeField]
    private GameObject[] ObjectPrefabs;

    private Dictionary<eRscType, Dictionary<int, ObjectPool>> mPoolMap;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this as RscModule;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }


        mPoolMap = new Dictionary<eRscType, Dictionary<int, ObjectPool>>
        {
            { eRscType.Unit, new Dictionary<int, ObjectPool>() },
            { eRscType.Object, new Dictionary<int, ObjectPool>() }
        };

        for (int i = 0; i < UnitPrefabs.Length; i++) GetPool(eRscType.Unit, i);
        for (int i = 0; i < ObjectPrefabs.Length; i++) GetPool(eRscType.Object, i);
    }


    public ObjectPool GetPool(eRscType poolType, int idx = 0)
    {
        ObjectPool retObjPool = null;

        if (mPoolMap[poolType].TryGetValue(idx, out retObjPool))
        {

        }
        else
        {
            if (poolType.Equals(eRscType.Unit))
            {
                retObjPool = ObjectPool.Create(UnitPrefabs[idx], poolType + "_Pool_" + idx, 3);
                retObjPool.SetData(eRscType.Unit, idx);
            }
            else if (poolType.Equals(eRscType.Object))
            {
                retObjPool = ObjectPool.Create(ObjectPrefabs[idx], poolType + "_Pool_" + idx, 3);
                retObjPool.SetData(eRscType.Object, idx);
            }

            retObjPool.transform.SetParent(this.transform);
            retObjPool.SetModule(this);
            mPoolMap[poolType].Add(idx, retObjPool);
        }
        return retObjPool;
    }
}


public class PoolData : MonoBehaviour
{
    private RscModule mRscModule;
    private eRscType mType;
    private int mIdx;

    public void SetData(eRscType t, int i)
    {
        mType = t;
        mIdx = i;
    }

    private void OnDisable()
    {
        AddPool();
    }

    public void AddPool() => mRscModule?.GetPool(mType, mIdx).Add(this.gameObject);
    public void SetMoudle(RscModule module) => mRscModule = module;

    public eRscType GetRscType() => mType;
    public int GetRscIdx() => mIdx;
}
