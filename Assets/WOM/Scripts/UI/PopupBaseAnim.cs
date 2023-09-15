using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PopupBaseAnim : MonoBehaviour
{
    private Transform target;

    private void Awake()
    {
        target = this.transform;
    }

    public void OnEnable()
    {
        target.transform.DOKill(true);

        Sequence scaleSequence = DOTween.Sequence();
        scaleSequence.Append(target.transform.DOScale(0.95f, 0.1f).SetEase(Ease.OutCubic).SetUpdate(true));
        scaleSequence.Append(target.transform.DOScale(1f, 0.4f).SetEase(Ease.OutElastic).SetUpdate(true));

        scaleSequence.OnComplete(() =>
        {

        });

    }

}
