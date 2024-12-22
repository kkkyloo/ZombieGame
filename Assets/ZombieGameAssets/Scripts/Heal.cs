using System;
using UnityEngine;

public class Heal : MonoBehaviour, IWeapon
{
    private PlayerManager playerManager;
    [SerializeField] private float heal = 50;
    [SerializeField] private float count = 1;
    public GameObject _obj;

    public static event Action OnHeal;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerManager = GetComponentInParent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_obj != null)
        {
            if (count != 0)
            {
                _obj.SetActive(true);
            }
            else
            {
                _obj.SetActive(false);

            }
        }


    }

    public void Attack()
    {
        if (count != 0)
        {
            playerManager.TakeHeal(heal);
            count--;
            OnHeal?.Invoke();
        }



    }



}
