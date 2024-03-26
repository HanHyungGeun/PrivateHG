using UnityEngine;
using System.Collections.Generic;

[AddComponentMenu("Object Pool/ObjectPool")]
public class ObjectPool : MonoBehaviour
{
	private static GameObject Parent;

	public GameObject Source;
	public List<GameObject> ObjectList;

    private eRscType Type;
    private int Idx;

	public static ObjectPool Create(GameObject resource, string name, int PoolSize)
	{
		if (Parent == null)  Parent = new GameObject("Object Pool");
		 
		GameObject PoolObj = new GameObject(name);
		PoolObj.transform.parent = Parent.transform;

		ObjectPool Pool = PoolObj.AddComponent<ObjectPool>();
		Pool.Init(resource, PoolSize);
		return Pool;
	}
     

	public void Init(GameObject resource, int PoolSize)
	{
		ObjectList = new List<GameObject>();
		Source = resource;

		for (int i = 0; i < PoolSize; i++)
		{
			ObjectList.Add(createObject());
		}
	}

    public void SetData(eRscType t , int i)
    {
        Type = t;
        Idx = i;

        for(int c = 0; c < ObjectList.Count; c++)
        {
            ObjectList[c].GetComponent<PoolData>().SetData(Type, Idx);
        }
    }

    public void SetModule(RscModule module)
    { 
        for (int c = 0; c < ObjectList.Count; c++)
        {
            ObjectList[c].GetComponent<PoolData>().SetMoudle(module);
        }
    }

    public GameObject GetObject()
    {
        GameObject Obj = null;
        getPoolObject(out Obj);
        Obj.transform.localPosition = Source.transform.localPosition;
        Obj.transform.rotation = Source.transform.rotation;
        Obj.SetActive(true);

        return Obj;
    }
    public GameObject GetSourceObject()
    {
        return Source;
    }

    public void Add(GameObject obj)
    {
        if (!this)
        {
            Destroy(obj);
            return;
        }

        if (ObjectList.Contains(obj) == false)
        {
            if(obj.activeSelf)
                obj.transform.SetParent(this.transform);
    
            obj.transform.position = Vector3.zero;
            obj.transform.rotation = Quaternion.identity;
            obj.SetActive(false);
            ObjectList.Add(obj);
        }
        return;
    }


    private GameObject createObject()
	{
		GameObject Obj = Instantiate(Source) as GameObject;
        Obj.SetActive(false);
        Obj.name = Source.name;
        Obj.transform.SetParent(this.transform);
		Obj.transform.position = Vector3.zero;
        Obj.AddComponent<PoolData>().SetData(Type,Idx);
		return Obj;
	}
    private void getPoolObject(out GameObject obj )
    {
        if (ObjectList.Count > 0)
        {
            obj = ObjectList[0];
            if (obj == null)
            { 
                obj = createObject();
            }
            ObjectList.RemoveAt(0);
        }
        else
        {
            obj = createObject();
        }
    }
 
    private void destoryPool()
	{
		int Count = ObjectList.Count;

		for (int i = 0; i < Count; i++)
		{
			Destroy(ObjectList[i]);
		}
		ObjectList.Clear();
	}
	private void OnDestroy()
	{
        destoryPool();
	}
}
 