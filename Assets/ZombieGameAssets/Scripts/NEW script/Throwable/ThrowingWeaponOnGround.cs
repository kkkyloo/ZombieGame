using UnityEngine;

public class ThrowingWeaponOnGround : MonoBehaviour
{
    private bool canTake = false;
    [SerializeField] private GameObject _prefab;

    [SerializeField] private GameObject _knifeObject;
    [SerializeField] private GameObject _grenadeObject;

    private void Update()
    {
        if (canTake && Input.GetKeyDown(KeyCode.F))
        {
            Actions.OnTakeWeapon?.Invoke(_prefab);
            if (_prefab.name == "bayonet")
            {
                _grenadeObject.SetActive(false);
                _knifeObject.SetActive(true);
            } else
            {
                _knifeObject.SetActive(false);
                _grenadeObject.SetActive(true);
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter()
    {
        canTake = true;
        Debug.Log("You can take ThrowingWeapon");
    }

    private void OnTriggerExit()
    {
        canTake = false;
        Debug.Log("You can NOT take ThrowingWeapon");
    }

}
