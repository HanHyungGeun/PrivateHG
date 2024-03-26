using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;


public class JoyStickCtrl : MonoBehaviour
{
    [SerializeField]
    private UnitCtrl Target = null;

    private Transform mCircle;
    private Image mCircleImage;

    private Transform mOutCircle;
    private Image mOutCircleImage;

    private readonly float mCirclRangeMax  = 60.0f;
    private readonly float mInputOffsetMax = 200.0f;
    private float mCircleRange = 0;

    private Vector2 mPointA;
    private Vector2 mPointB;

    public static bool IsValid = false;

    private void Awake()
    {
        mCircle = this.transform.GetChild(0);
        mCircleImage = mCircle.GetComponent<Image>();

        mOutCircle = this.transform;
        mOutCircleImage = mOutCircle.GetComponent<Image>();
        setEnableJoyStick(false);

        this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButtonDown(0))
            .Subscribe(_ =>
            {
                setEnableJoyStick(true);
                IsValid = true;
                mPointA = Input.mousePosition;
                mOutCircle.position = mPointA;
            });

        this.UpdateAsObservable()
            .Where(_ => IsValid && Input.GetMouseButton(0))
            .Subscribe(_ => mPointB = Input.mousePosition);
         
        this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButtonUp(0))
            .Subscribe(_ =>
            {
                setEnableJoyStick(false);
                mPointA = mPointB = Vector2.zero;
                IsValid = false;
            });

        this.UpdateAsObservable()
          .Where(_ => IsValid)
          .Subscribe(_ =>
          {
              Vector2 offset;
              Vector2 direction;

              offset = mPointB - mPointA;
              direction = Vector2.ClampMagnitude(offset, 1.0f);

              float inputOffset = Vector2.Distance(offset, Vector2.zero);

              if (inputOffset > mInputOffsetMax)
              {
                  mCircleRange = mCirclRangeMax;
              }
              else
              {
                  mCircleRange = inputOffset * mCirclRangeMax / mInputOffsetMax;
              }

              mCircle.localPosition = direction * mCircleRange;

              // My Player Rotation
              offset = mPointB - mPointA;
              float angle = Mathf.Atan2(offset.x, offset.y) * Mathf.Rad2Deg;
              Quaternion rote = Quaternion.Euler(Vector3.up * angle);
              Target.SetMoveUpdate(rote);
          });
    }
     
    private void setEnableJoyStick(bool isEnable)
    {
        mCircleImage.enabled = mOutCircleImage.enabled = isEnable;
    }

}
