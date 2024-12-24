using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchGun : MonoBehaviour
{
    public static SwitchGun Instance;

    [Header("Ссылки на TextMeshProUGUI для отображения патронов")]
    [SerializeField] private TextMeshProUGUI ammoText;      
    [SerializeField] private TextMeshProUGUI ammoTextTwo;   

    [Header("Хранилище оружия и HUD-спрайтов")]
    private List<GameObject> _weapons = new();
    [SerializeField] private Image[] _hudElements;
    [SerializeField] private Sprite[] _weaponSprites;

    public static bool CanSwitch = true;

    private int _currentWeaponIndex = 0;
    private int _prevIndex;
    private int _lastFirearmIndex = 0;

    private void Awake()
    {
        Instance = this;
        InitializeWeapons();
    }

    private void InitializeWeapons()
    {

        Heal.OnHeal += ChangeToPrev;


        Transform[] children = GetComponentsInChildren<Transform>(true);
        foreach (var child in children)
        {

            if (child == transform) 
                continue;


            if (child.GetComponent<IWeapon>() != null)
            {
                _weapons.Add(child.gameObject);
            }
        }


        SwitchWeapon(_currentWeaponIndex);


        SetAmmoText(0, 0);
    }

    private void OnDisable()
    {
        _weapons = null;
        Heal.OnHeal -= ChangeToPrev;
    }

    private void ChangeToPrev()
    {
        SwitchWeapon(_prevIndex);
    }

    private void Update()
    {
        if (!CanSwitch) return;

        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchWeapon(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchWeapon(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchWeapon(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4)) SwitchWeapon(3);
        else if (Input.GetKeyDown(KeyCode.Alpha5)) SwitchWeapon(4);
    }

    private void SwitchWeapon(int index)
    {

        if (index >= _weapons.Count || index == _currentWeaponIndex) 
            return;


        for (int i = 0; i < _weapons.Count; i++)
        {
            _weapons[i].SetActive(i == index);
        }

        UpdateHudElements(index);


        _prevIndex = _currentWeaponIndex;
        _lastFirearmIndex = index;
        _currentWeaponIndex = index;

        CharacterWeapons.SelectedWeaponId = index;


        SetAmmoText(0, 0);
    }

    private void UpdateHudElements(int index)
    {
        if (_hudElements == null || _weaponSprites == null) return;

        _hudElements[0].sprite = GetPrimaryHudSprite(index);
        _hudElements[1].sprite = (index == 1) ? _weaponSprites[5] : _weaponSprites[4];
        _hudElements[2].sprite = (index == 3) ? _weaponSprites[7] : _weaponSprites[6];
    }

    private Sprite GetPrimaryHudSprite(int index)
    {
        return index switch
        {
            0 => _weaponSprites[0],
            2 => _weaponSprites[2],
           
            _ => _lastFirearmIndex switch
            {
                0 => _weaponSprites[1],
                2 => _weaponSprites[3],
                _ => _hudElements[0].sprite
            }
        };
    }

    public void SetAmmoText(int currentAmmo, int totalAmmo)
    {
        if (ammoText != null)
            ammoText.text = currentAmmo.ToString();

        if (ammoTextTwo != null)
            ammoTextTwo.text = totalAmmo.ToString();
    }
}
