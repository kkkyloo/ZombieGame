//void SwitchWeapon(int index)
//{


//    if (hudWeapon != null)
//    {
//        if (index == 0) // ������� ������
//        {
//            hudWeapon.sprite = weaponSprites[0]; // �������� ������ ��������
//            lastFirearmIndex = 0;
//        }
//        else if (index == 2) // �������� ������
//        {
//            hudWeapon.sprite = weaponSprites[2]; // �������� ������ ���������
//            lastFirearmIndex = 2;
//        }
//        else // ������������� �� ����� ��� �������
//        {
//            if (lastFirearmIndex == 0) // ��������� ��� �������
//            {
//                hudWeapon.sprite = weaponSprites[1]; // ���������� ������ ��������
//            }
//            else if (lastFirearmIndex == 2) // ��������� ��� ��������
//            {
//                hudWeapon.sprite = weaponSprites[3]; // ���������� ������ ���������
//            }
//        }
//    }

//    // ��������� HUD ��� ������
//    if (hudKnife != null)
//    {
//        if (index == 1) // ����� ������
//        {
//            hudKnife.sprite = weaponSprites[5]; // �������� ������ ������
//        }
//        else
//        {
//            hudKnife.sprite = weaponSprites[4]; // ���������� ������ ������
//        }
//    }

//    // ��������� HUD ��� �������
//    if (hudGrene != null)
//    {
//        if (index == 3) // ������� �������
//        {
//            hudGrene.sprite = weaponSprites[7]; // �������� ������ �������
//        }
//        else
//        {
//            hudGrene.sprite = weaponSprites[6]; // ���������� ������ �������
//        }
//    }

//    currentWeaponIndex = index;
//    CharacterWeapons.SelectedWeaponId = index;
//}