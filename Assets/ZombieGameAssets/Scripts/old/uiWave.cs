using UnityEngine;

public class uiWave : MonoBehaviour
{
    [SerializeField] private GameObject text1;
    [SerializeField] private GameObject text0;

    [SerializeField] private GameObject text2;
    [SerializeField] private GameObject text3;
    [SerializeField] private GameObject ak;
    [SerializeField] private GameObject sg;

    void Start()
    {
        //ak.SetActive(true);
        Invoke("SelectWeapon1", 3);

        Invoke("SelectWeapon", 60);
        Invoke("SelectWeapon2", 120);
        Invoke("SelectWeapon3", 180);
    }


    private void SelectWeapon1()
    {
        text0.SetActive(false);
    }

    private void SelectWeapon()
    {
        //sg.SetActive(true);
        text1.SetActive(true);
    }
    private void SelectWeapon2()
    {

        text2.SetActive(true);
    }
    private void SelectWeapon3()
    {
        text3.SetActive(true);
    }
}
