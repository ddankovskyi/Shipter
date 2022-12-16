using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class Cannon : MonoBehaviour
{
    [SerializeField] Transform shootingPoint;
    [SerializeField] float mouseSensX = 0.003f;
    [SerializeField] float mouseSensY = 0.01f;
    [SerializeField] float maxAngle = 50f;
    [SerializeField] float shootPower = 100;


    [SerializeField] Cannonball cannonballPrefab;

    [SerializeField] float shootDelaySec = 1f;
    [SerializeField] float reloadDelaySec = 1f;
    [SerializeField] Animator _animator;
    bool _readyToShoot = true;

    bool _shootWhenReady = false;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 25;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
        MaybeShoot();
    }

    float _xRotation = 0f;
    float _xLastAngularSpeed = 0;
    float _maxA = 130;
    static readonly int Shoot1 = Animator.StringToHash("Shoot");

    void ProcessInputs()
    {
        Debug.Log(Mouse.current.delta.x.ReadValue());
        var deltaX = Mouse.current.delta.x.ReadValue() * mouseSensX;
        var deltaY = Mouse.current.delta.y.ReadValue() * mouseSensY;


        var maxDeltaX = _xLastAngularSpeed * Time.deltaTime + _maxA * Time.deltaTime * Time.deltaTime / 2;
        Debug.Log("maxDeltaX " + maxDeltaX);
        deltaX = Mathf.Clamp(deltaX, -maxDeltaX, maxDeltaX);
        _xLastAngularSpeed = Mathf.Abs(deltaX / Time.deltaTime);


        transform.Rotate(Vector3.up, deltaX);
        _xRotation -= deltaY;
        _xRotation = Mathf.Clamp(_xRotation, -maxAngle, 0);
        shootingPoint.localRotation = Quaternion.Euler(_xRotation, 0, 0);
    }

    void MaybeShoot()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (_readyToShoot)
                StartCoroutine(Shoot());
            else
                _shootWhenReady = true;
        }
        else if(_shootWhenReady && Mouse.current.leftButton.isPressed && _readyToShoot)
        {
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        _shootWhenReady = false;
        _readyToShoot = false;
        _animator.SetTrigger(Shoot1);
        yield return new WaitForSeconds(shootDelaySec); // no  time for binding animations and delays :(

        var ball = Instantiate(cannonballPrefab, shootingPoint.position, shootingPoint.rotation, null);
        ball.GetComponent<Rigidbody>().AddForce(ball.transform.forward * shootPower, ForceMode.Impulse);

        Reload();
        yield return new WaitForSeconds(reloadDelaySec);
        _readyToShoot = true;
    }

    void Reload()
    {

    }
}