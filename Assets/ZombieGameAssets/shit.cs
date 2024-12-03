//void SwitchWeapon(int index)
//{


//    if (hudWeapon != null)
//    {
//        if (index == 0) // Автомат выбран
//        {
//            hudWeapon.sprite = weaponSprites[0]; // Активный спрайт автомата
//            lastFirearmIndex = 0;
//        }
//        else if (index == 2) // Пистолет выбран
//        {
//            hudWeapon.sprite = weaponSprites[2]; // Активный спрайт пистолета
//            lastFirearmIndex = 2;
//        }
//        else // Переключились на топор или гранату
//        {
//            if (lastFirearmIndex == 0) // Последним был автомат
//            {
//                hudWeapon.sprite = weaponSprites[1]; // Неактивный спрайт автомата
//            }
//            else if (lastFirearmIndex == 2) // Последним был пистолет
//            {
//                hudWeapon.sprite = weaponSprites[3]; // Неактивный спрайт пистолета
//            }
//        }
//    }

//    // Обновляем HUD для топора
//    if (hudKnife != null)
//    {
//        if (index == 1) // Топор выбран
//        {
//            hudKnife.sprite = weaponSprites[5]; // Активный спрайт топора
//        }
//        else
//        {
//            hudKnife.sprite = weaponSprites[4]; // Неактивный спрайт топора
//        }
//    }

//    // Обновляем HUD для гранаты
//    if (hudGrene != null)
//    {
//        if (index == 3) // Граната выбрана
//        {
//            hudGrene.sprite = weaponSprites[7]; // Активный спрайт гранаты
//        }
//        else
//        {
//            hudGrene.sprite = weaponSprites[6]; // Неактивный спрайт гранаты
//        }
//    }

//    currentWeaponIndex = index;
//    CharacterWeapons.SelectedWeaponId = index;
//}