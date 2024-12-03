using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterWeapons : MonoBehaviour
{
    public List<IWeapon> Weapons = new();
    public static int SelectedWeaponId { get;  set; } = 0;

    public float MaxDistanceToTarget = 250f;

    private void Awake()
    {
        Weapons = GetComponentsInChildren<IWeapon>(true).ToList();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            FireWeapons();
        }
    }

    public void FireWeapons()
    {
        Weapons[SelectedWeaponId].Attack();
    }


}

