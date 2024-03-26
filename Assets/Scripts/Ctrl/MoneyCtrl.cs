using UnityEngine;
using UniRx;
using UniRx.Triggers;
using TMPro;

public class MoneyCtrl : MonoBehaviour
{
    private TextMeshProUGUI mTMP_Money;
    private CounterCtrl mCountCtrl;
    private int mMoney = 0;
    private void Awake()
    {
        mTMP_Money = this.GetComponent<TextMeshProUGUI>();
        mCountCtrl = FindAnyObjectByType<CounterCtrl>();

        this.UpdateAsObservable()
           .Where(_ => (mCountCtrl.GetSellCount() > 0))
           .Subscribe(_ =>
           {
               addMoney();
               tmpUpdate();
               mCountCtrl.SetSellCount(mCountCtrl.GetSellCount() - 1);
           });

        tmpUpdate();
    }

    private void addMoney() => mMoney += 10;
    private void tmpUpdate()
    {
        mTMP_Money.text = mMoney.ToString();
    }

}
