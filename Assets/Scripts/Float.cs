using DG.Tweening;
using UnityEngine;

public class Float : MonoBehaviour
{
    [SerializeField]
    private float FloatRange = 1f;

    [SerializeField]
    private float FloatTime = 2f;

    private bool toB;
    private Vector3 positionA, positionB;

    private Tweener tweener;
    
    
    // Start is called before the first frame update
    void Start()
    {
        var offset = new Vector3(0, FloatRange, 0);
        positionA = transform.position + offset;
        positionB = transform.position - offset;
        toB = true;

        tweener = DOTween.To(() => transform.position, (val) => transform.position = val, positionB, FloatTime / 2.0f);
        tweener.SetEase(Ease.InOutQuad);
        tweener.SetLink(gameObject);
        tweener.onComplete += OnTweenComplete;
    }

    void OnTweenComplete()
    {
        toB = !toB;
        var target = toB ? positionB : positionA;
        
        tweener = DOTween.To(() => transform.position, (val) => transform.position = val, target, FloatTime);
        tweener.SetEase(Ease.InOutQuad);
        tweener.SetLink(gameObject);
        tweener.onComplete += OnTweenComplete;
    }
}
