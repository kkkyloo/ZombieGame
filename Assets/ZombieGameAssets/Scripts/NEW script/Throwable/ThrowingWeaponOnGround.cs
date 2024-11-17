using UnityEngine;

public class ThrowingWeaponOnGround : MonoBehaviour
{
    private bool canTake = false;
    [SerializeField] private GameObject _prefab;

    private void Update()
    {
        if (canTake && Input.GetKeyDown(KeyCode.F))
        {
            Actions.OnTakeWeapon?.Invoke(_prefab);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter()
    {
        canTake = true;
        Debug.Log("You can take ThrowingWeapon");
    }

    private void OnCollisionExit()
    {
        canTake = false;
        Debug.Log("You can NOT take ThrowingWeapon");
    }

}
