using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHighlight : MonoBehaviour
{
    public int index;
    private Image target;

    private void Awake()
    {
        target = GetComponent<Image>();
    }
    [Sirenix.OdinInspector.Button]
    public void HighLight()
    {
        target.DOKill(true);
        //target.transform.DOScale(0.95f, 0.1f).SetEase(Ease.OutCubic).SetUpdate(true);
        //target.transform.DOScale(1f, 0.4f).SetEase(Ease.OutElastic).SetUpdate(true);
        target.DOColor(Color.red, 0.5f).SetEase(Ease.Linear).OnComplete(() => {
            target.DOColor(Color.white, 0.5f).SetEase(Ease.Linear);
        });
    }

}
