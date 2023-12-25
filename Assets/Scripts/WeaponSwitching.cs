using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public static int selectedWeapon = 0;

    void Start()
    {
        SelectWeapon();
    }


    
    public void AxeDown()
    {
        if(!Ak47.isReloading || !shotgun.isReloading)
        {
            selectedWeapon = 0;
            SelectWeapon();
        }
    }
    public void AkDown()
    {
        if (!Ak47.isReloading || !shotgun.isReloading)
        {
            selectedWeapon = 1;
            SelectWeapon();
        }


    }
    public void ShootGunDown()
    {
        if (!Ak47.isReloading || !shotgun.isReloading)
        {
            selectedWeapon = 2;
            SelectWeapon();
        }
    }

    private void SelectWeapon()
    {
        int i = 0;

        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }
}
