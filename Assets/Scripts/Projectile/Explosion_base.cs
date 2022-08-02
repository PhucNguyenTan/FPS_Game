using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Explosion_base : MonoBehaviour
{
    float _radius;
    float _duration;

    Color _startColor;
    Material _mate;
    Renderer _render;
    AnimationCurve _curveAlpha;

    private void Awake()
    {
        _render = transform.GetChild(0).transform.GetComponent<Renderer>();
        _mate = _render.materials[0];
    }

    private void Start()
    {
        _mate.color = _startColor;
        Vector3 scale = new Vector3(1f,1f,1f) * _radius;
        transform.localScale = Vector3.zero;
        _mate.DOFade(0f, _duration).SetEase(_curveAlpha);
        transform.DOScale(scale, _duration).OnComplete(
                () => Destroy(this.gameObject));
    }

    public Explosion_base SetRadius(float radius)
    {
        _radius = radius;
        return this;
    }

    public Explosion_base SetExplosionData(Explosion_data data)
    {
        _radius = data.BlastRadius;
        _duration = data.Duration;
        _curveAlpha = data.AlphaFadingCurve;
        _startColor = data.BlastColor;
        return this;
    }
}
