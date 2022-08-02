using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Explosion_base : MonoBehaviour
{
    float _radius;
    float _duration;

    private void Start()
    {
        Vector3 scale = new Vector3(1f,1f,1f) * _radius;
        transform.localScale = Vector3.zero;
        //transform.DOPunchScale(scale, _duration).OnComplete(() => Destroy(this.gameObject));
        transform.DOScale(scale, _duration / 2).OnComplete(
            () => transform.DOScale(Vector3.zero, _duration / 2).OnComplete(
                () => Destroy(this.gameObject)));
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
        return this;
    }
}
