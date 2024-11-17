using UnityEngine;
public class SwitchGun : MonoBehaviour
{
    [SerializeField] private GameObject[] weapons;
    private int currentWeaponIndex = 0;
    void Start()
    {
        SwitchWeapon(currentWeaponIndex);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchWeapon(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchWeapon(3);
        }
    }

    void SwitchWeapon(int index)
    {
        foreach (GameObject weapon in weapons)
        {
            weapon.SetActive(false);
        }

        weapons[index].SetActive(true);
        currentWeaponIndex = index;
    }
}