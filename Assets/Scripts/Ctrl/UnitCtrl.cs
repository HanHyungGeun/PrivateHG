using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class UnitCtrl : MonoBehaviour
{
    private Transform mMyTrans;
    private Animator  mAnimator;

    private readonly float mMoveSpeed = 3.85f;
    private int  mCountOffset;
    private bool mIsPickup = false;
    private bool mIsTrash = false;

    private void Awake()
    {
        mMyTrans  = transform;
        mAnimator = GetComponent<Animator>();

        mCountOffset = mMyTrans.childCount;

        if (this.GetComponent<ObjectType>()._UnitType == eUnitType.Player)
        {
            this.UpdateAsObservable()
               .Subscribe(_ => SetIsRun(JoyStickCtrl.IsValid));
        }

        this.UpdateAsObservable()
           .Where(_ => (mMyTrans.childCount >= mCountOffset))
           .Select(_ => (mMyTrans.childCount > mCountOffset) ? true : false )
            .Subscribe(isPickup => mIsPickup = isPickup);
    }

    public void SetMoveUpdate(Quaternion rote)
    {
        mMyTrans.rotation = rote;
        mMyTrans.Translate(Vector3.forward * mMoveSpeed * Time.deltaTime);
    }

    public void SetIsRun(bool isRun) => mAnimator.SetBool("isRun", isRun);
    public void SetIsTrash(bool isTrash) => mIsTrash = isTrash;

    public bool IsPickup() => mIsPickup;
    public bool IsTrash() => mIsTrash;
    public int  GetCountOffset() => mCountOffset;
}
