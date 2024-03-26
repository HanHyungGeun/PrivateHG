using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class CameraCtrl : MonoBehaviour
{
    [SerializeField]
    private Transform Target = null;

    private Transform mMyTrans;
    private Vector3   mOffsetPos;
    private float     mMoveSpeed = 5.0f;

    private void Start()
    {
        mMyTrans   = this.transform;
        mOffsetPos = mMyTrans.position - Target.position;

        this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButton(0))
            .Select(_ => convertPos(Target.transform.position, mOffsetPos))
            .Subscribe(pos => mMyTrans.position = Vector3.Lerp(mMyTrans.position , pos , Time.deltaTime * mMoveSpeed));        
    }

    private Vector3 convertPos(Vector3 a , Vector3 b)
    {
        return new Vector3(a.x + b.x, b.y, a.z + b.z);
    }
}


