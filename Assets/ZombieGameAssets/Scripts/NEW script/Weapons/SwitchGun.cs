using UnityEngine;
using UnityEngine.UI;

public class SwitchGun : MonoBehaviour
{
    [SerializeField] private GameObject[] weapons;       // Массив оружия
    [SerializeField] private Image hudWeapon;            // HUD элемент для автомата и пистолета
    [SerializeField] private Image hudKnife;             // HUD элемент для топора
    [SerializeField] private Image hudGrene;             // HUD элемент для гранаты
    [SerializeField] private Sprite[] weaponSprites;     // Массив спрайтов

    private int currentWeaponIndex = 0;
    private int lastFirearmIndex = 0; // 0 для автомата, 2 для пистолета

    void Start()
    {
        SwitchWeapon(currentWeaponIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(0); // Автомат
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(1); // Топор
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchWeapon(2); // Пистолет
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchWeapon(3); // Граната
        }
    }

    void SwitchWeapon(int index)
    {
        // Отключаем все оружия
        foreach (GameObject weapon in weapons)
        {
            weapon.SetActive(false);
        }

        // Активируем выбранное оружие
        weapons[index].SetActive(true);

        // Обновляем HUD для оружия
        if (hudWeapon != null)
        {
            if (index == 0) // Автомат выбран
            {
                hudWeapon.sprite = weaponSprites[0]; // Активный спрайт автомата
                lastFirearmIndex = 0;
            }
            else if (index == 2) // Пистолет выбран
            {
                hudWeapon.sprite = weaponSprites[2]; // Активный спрайт пистолета
                lastFirearmIndex = 2;
            }
            else // Переключились на топор или гранату
            {
                if (lastFirearmIndex == 0) // Последним был автомат
                {
                    hudWeapon.sprite = weaponSprites[1]; // Неактивный спрайт автомата
                }
                else if (lastFirearmIndex == 2) // Последним был пистолет
                {
                    hudWeapon.sprite = weaponSprites[3]; // Неактивный спрайт пистолета
                }
            }
        }

        // Обновляем HUD для топора
        if (hudKnife != null)
        {
            if (index == 1) // Топор выбран
            {
                hudKnife.sprite = weaponSprites[5]; // Активный спрайт топора
            }
            else
            {
                hudKnife.sprite = weaponSprites[4]; // Неактивный спрайт топора
            }
        }

        // Обновляем HUD для гранаты
        if (hudGrene != null)
        {
            if (index == 3) // Граната выбрана
            {
                hudGrene.sprite = weaponSprites[7]; // Активный спрайт гранаты
            }
            else
            {
                hudGrene.sprite = weaponSprites[6]; // Неактивный спрайт гранаты
            }
        }

        currentWeaponIndex = index;
        CharacterWeapons.SelectedWeaponId = index;
    }
}