using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Gun_base : MonoBehaviour
{
    protected float range;
    protected float damage;
    protected int fireRate; //in milisecond

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

    private float _currentMove; 
    private bool _forwardDirection;
    [SerializeField]
    private float speed = 10f;

    private bool _forwardRotation;
    private float _currentLook;
    private Vector3 _moveAnim;
    private Vector3 _moveBobAnim;
    private Vector3 _aimAnim;
    private Vector3 _shootAnim;

    private float maxDashSway = 0.25f;

    public bool CanShoot;

    private Vector2 _saveInput;
    private Vector3 _dashPosition;

    [SerializeField]  protected float _returnSpeed;
    [SerializeField] protected float _snappiness;
    //private Vector3 _targetRotation;
    //private Vector3 _currentRotation;
    //[SerializeField]
    //protected float _recoilX;
    //[SerializeField]
    //protected float _recoilY;
    //[SerializeField]
    //protected float _recoilZ;



    private Quaternion RecoilAnim;

    private void Awake()
    {
        CanShoot = true;
    }

    private void Start()
    {
        InitialPos = transform.localPosition;
        InitialRot = transform.localRotation;
        _currentMove = 0f;
        _currentLook = 0f;
        _forwardRotation = true;
        _forwardDirection = true;
    }

    public void MovementBob(Vector2 charMove, float bobIntensity)
    {
        float longerDirection = Mathf.Abs(charMove.x) >= Mathf.Abs(charMove.y) ? Mathf.Abs(charMove.x) : Mathf.Abs(charMove.y);
        Vector3 weaponBob = new Vector3(0f, -bobIntensity * longerDirection, 0f);
        
        if(_currentMove < 0f)
        {
            _currentMove = 0f;
            _forwardDirection =  FlipBool(_forwardDirection);
        }
        else if(_currentMove > 1f)
        {
            _currentMove = 1f;
            _forwardDirection = FlipBool(_forwardDirection);
        }

        if (_forwardDirection)
        {
            _currentMove += Time.deltaTime*speed;
        }
        else
        {
            _currentMove -= Time.deltaTime*speed;
        }
        _moveBobAnim = Vector3.Lerp(InitialPos, weaponBob*0.1f + InitialPos, _curve.Evaluate(_currentMove));
        
    }

    public bool FlipBool(bool flipBool)
    {
        if (flipBool)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void MovementSway(Vector2 moveInput)
    {
        Vector3 test = new Vector3(moveInput.x * 0.5f, 0f, moveInput.y * 0.5f);
        _moveAnim = Vector3.MoveTowards(Vector3.zero, test, Time.deltaTime);
    }

    //public void MovementSway(Vector2 moveInput)
    //{
    //    float MoveX = Mathf.Clamp(moveInput.x * amount, -maxAmount, maxAmount);
    //    float MoveY = Mathf.Clamp(moveInput.y * amount, -maxAmount, maxAmount);

    //    Vector3 finalPos = new Vector3(MoveX, MoveY, 0f);

    //    AimAnim = Vector3.Lerp(InitialPos, finalPos + InitialPos, Time.deltaTime * smooth);
    //}

    public void ResetPosition()
    {
        _moveAnim = Vector3.zero;
        _moveBobAnim = InitialPos;
        _currentMove = 0f;
        _forwardDirection = true;

    }

    public void ResetRotation()
    {
        _currentLook = 0f;
        transform.localRotation = InitialRot;
        _forwardRotation = true;

    }

    public void RecoilUpdate() //???
    {
        if (_currentLook < 0f)
        {
            _currentLook = 0f;
            _forwardRotation = FlipBool(_forwardRotation);
        }
        else if (_currentLook > 1f)
        {
            _currentLook = 1f;
            _forwardRotation = FlipBool(_forwardRotation);
        }

        if (_forwardRotation)
        {
            _currentLook += Time.deltaTime * _snappiness;
        }
        else
        {
            _currentLook -= Time.deltaTime * _returnSpeed;
        }
        Quaternion RotY = Quaternion.Euler(30f, 0f ,0f);
        transform.localRotation = Quaternion.Slerp(InitialRot, InitialRot * RotY, _currentLook);
        //Debug.Log(transform.localRotation.eulerAngles);
    }

    public void RecoilFire()
    {
        
        //_targetRotation = new Vector3(_recoilX, Random.Range(-_recoilY, _recoilY), Random.Range(-_recoilZ, _recoilZ));
    }

    public void ActuallyApplyDoChanges()
    {
        Vector3 finalOutcome = (_moveBobAnim + _moveAnim + _dashPosition);
        transform.localPosition = finalOutcome;
        //Debug.Log(finalOutcome);
    }

    public void Shot()
    {
        CanShoot = false;
    }

    //public void ToShootAgain() {
    //    StartCoroutine(WaitSeconds());
    //    CanShoot = true;
    //}

    

    //public IEnumerator WaitSeconds()
    //{
    //    yield return new WaitForSeconds(fireRate);
    //}

    public async void Wait()
    {
        await Task.Delay(fireRate);
        CanShoot = true;

    }

    public void DashSway(Vector2 dashVelPercent, int xDir, int yDir)
    {
        Vector3 xSway = Vector3.Lerp(Vector3.zero, new Vector3(maxDashSway * -xDir, 0f, 0f), dashVelPercent.x);
        Vector3 ySway = Vector3.Lerp(Vector3.zero, new Vector3(0f, 0f, maxDashSway * -yDir), dashVelPercent.y);
        _dashPosition = xSway + ySway;
    }
}

    
