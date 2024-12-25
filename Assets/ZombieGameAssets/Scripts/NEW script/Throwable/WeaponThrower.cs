using System.Collections;
using UnityEngine;

public class WeaponThrower : MonoBehaviour, IWeapon
{
    [Header("Settings")]
    [SerializeField] private int _currentAmmo = 3;
    [SerializeField] private float _throwDelay = 2f;
    [SerializeField] private float _throwForce;

    [Header("Game Objects")]
    [SerializeField] private GameObject _throwableObject;
    [SerializeField] private Transform _cameraTransform;

    [SerializeField] private GameObject _knifeObject;
    [SerializeField] private GameObject _grenadeObject;

    private bool canThrow = true;

    private void OnEnable()
    {
        Actions.OnTakeWeapon += ChangeWeapon;
    }

    private void OnDisable()
    {
        Actions.OnTakeWeapon -= ChangeWeapon;
    }


    public void Attack()
    {
        if (Input.GetButtonDown("Fire1") && _currentAmmo > 0 && canThrow)
        {
            canThrow = false;
            StartCoroutine(StartThrow());
        }
    }

    private IEnumerator StartThrow()
    {
        _currentAmmo--;
        if (_currentAmmo == 0 && _throwableObject.name == "bayonet")
        {
            _knifeObject.SetActive(false);
        }
        else if (_currentAmmo == 0 && _throwableObject.name == "grenade")
        {
            _grenadeObject.SetActive(false);
        }
        ThrowObject();
        yield return new WaitForSeconds(_throwDelay);
        canThrow = true;
    }

    private void ThrowObject()
    {
        GameObject weapon;

        if (_throwableObject.name == "bayonet")
        {
            weapon = Instantiate(_throwableObject, transform);

        }
        else
        {
            weapon = Instantiate(_throwableObject, transform.position, Quaternion.identity);

        }


        Rigidbody rb = weapon.GetComponent<Rigidbody>();

        WeaponThrowType weaponThrowType = weapon.GetComponent<WeaponThrowType>();

        //calculate direction
        Vector3 forceDirection = Camera.main.transform.forward;

        // only if knife
        if (weaponThrowType.throwingWeaponType == WeaponThrowType.ThrowingWeaponType.Knife)
        {
            _throwForce = 20f;

            RaycastHit hit;

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 100f))
            {
                forceDirection = (hit.point - transform.position).normalized;
            }
        }
        else
        {
            _throwForce = 20f;
        }

        rb.isKinematic = false;
        rb.useGravity = true;
        rb.linearVelocity = forceDirection * _throwForce;
    }

    private void ChangeWeapon(GameObject gameObject)
    {
        _throwableObject = gameObject;
        _currentAmmo = 3;
    }


}
