using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

public class CounterCtrl : MonoBehaviour
{
    [SerializeField]
    private GameObject GudeObj;
    private int mSellCount = 0;

    private void Awake()
    {
        this.UpdateAsObservable()
           .Where(_ => (this.transform.childCount >= 0))
           .Subscribe(_ => GudeObj.gameObject.SetActive(this.transform.childCount <= 0));
    }

    public void SetSellCount(int sellCount) => mSellCount = sellCount;
    public int GetSellCount() => mSellCount;
    public bool GetIsBurger() => this.transform.childCount > 0;
    public Transform GetBurger() => this.transform.GetChild(0);

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        { 
            UnitCtrl target = other.transform.GetComponent<UnitCtrl>();
            if (!target.IsPickup())
                return;

            int offset = target.GetCountOffset();
            int count = other.transform.childCount - offset;
              
            for (int i = 0; i < count; i++)
            {
                other.transform.GetChild(other.transform.childCount - 1).SetParent(this.transform);                
            }

            count = this.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                this.transform.GetChild(i).localPosition = new Vector3(0, i * 0.5f , 0);
            }
        }
    }

    public void SortBurger()
    {
        int count = this.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            this.transform.GetChild(i).transform.DOLocalMove(new Vector3(0, i * 0.5f, 0), 0.5f);
        }
    }

}
