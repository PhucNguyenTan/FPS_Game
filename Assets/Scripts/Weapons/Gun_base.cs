using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_base : MonoBehaviour
{
    protected float range;
    protected float damage;
    protected float fireRate;

    private Vector3 InitialPos;
    private Quaternion InitialRot;

    [SerializeField]
    private AnimationCurve _curve;

    [SerializeField]
    private float amount = 5f;
    [SerializeField]
    private float maxAmount = 10f;

    [SerializeField]
    private float smooth = 10f;
    [SerializeField]
    private float currentMove; //run from 0-1
    private bool forwardDirection;
    [SerializeField]
    private float speed = 10f;

    private Vector3 MoveAnim;
    private Vector3 AimAnim;
    private Vector3 ShootAnim;

    

    private Quaternion RecoilAnim;

    private void Start()
    {
        InitialPos = transform.localPosition;
        InitialRot = transform.localRotation;
        currentMove = 0f;
    }

    public void MovementSway(Vector2 charMove)
    {
        Vector3 test = new Vector3(charMove.x, 0f, charMove.y);
        
        if(currentMove < 0f || currentMove > 1f)
        {
            FlipDirection();
        }

        if (forwardDirection)
        {
            currentMove += Time.deltaTime*speed;
        }
        else
        {
            currentMove -= Time.deltaTime*speed;
        }
        MoveAnim = Vector3.Lerp(InitialPos, test*0.1f + InitialPos, _curve.Evaluate(currentMove));
    }

    public void FlipDirection()
    {
        if (forwardDirection)
        {
            forwardDirection = false;
        }
        else
        {
            forwardDirection = true;
        }
    }

    public void RotationSway(Vector2 camMove)
    {
        float MoveX = Mathf.Clamp(camMove.x * amount, -maxAmount, maxAmount);
        float MoveY = Mathf.Clamp(camMove.y * amount, -maxAmount, maxAmount);

        Vector3 finalPos = new Vector3(MoveX, MoveY, 0f);

        AimAnim = Vector3.Lerp(InitialPos, finalPos + InitialPos, Time.deltaTime * smooth);
    }

    public void RecoilSway()
    {
        Quaternion RotY = Quaternion.Euler(10f, 10f ,0f);
        transform.localRotation = Quaternion.Slerp(InitialRot, InitialRot * RotY, Time.deltaTime);
    }

    public void ActuallyChangeDoChanges()
    {
        Vector3 finalOutcome = (AimAnim + MoveAnim) - InitialPos;
        transform.localPosition = finalOutcome;
    }
}

    
