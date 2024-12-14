using System.Collections.Generic;
using System.Linq;
using Unity.Burst.Intrinsics;
using UnityEngine;
public class Crosshair : MonoBehaviour
{
    public RectTransform crosshair;



    public Transform[] crosshairObj;

    [SerializeField] float sizeState;
    [SerializeField] float sizeMove;
    [SerializeField] float sizeMove2;
    [SerializeField] float sizeCurrent;

    [SerializeField] float speedSize;
    [SerializeField] float speedSize2;

    private float speed;


    private void OnEnable()
    {
        Actions.OnMove += ImpactMove;
        Actions.GunShoot += Impact;

        crosshairObj = GetComponentsInChildren<Transform>(false);
        crosshairObj = crosshairObj.Skip(1).ToArray();

    }
    private void OnDisable()
    {
        Actions.OnMove -= ImpactMove;
        Actions.GunShoot -= Impact;
    }


    private void Update()
    {
        if (Ak47Script.scope)
        {
            foreach (Transform obj in crosshairObj)
            {
                obj.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (Transform obj in crosshairObj)
            {
                obj.gameObject.SetActive(true);
            }
        }
    }




    void Impact()
    {
        speed = speedSize2;
        CrosshairChange(1, 1, sizeMove2);
    }

    void ImpactMove(float horizontalInput, float verticalInput)
    {
        speed = speedSize;
        CrosshairChange(horizontalInput, verticalInput, sizeMove);
    }

    private void CrosshairChange(float horizontalInput, float verticalInput, float sizeMove)
    {

        if (horizontalInput != 0 || verticalInput != 0)
        {
            sizeCurrent = Mathf.Lerp(sizeCurrent, sizeMove, Time.deltaTime * speedSize);
        }
        else
        {
            sizeCurrent = Mathf.Lerp(sizeCurrent, sizeState, Time.deltaTime * speedSize);
        }
        crosshair.sizeDelta = new Vector2(sizeCurrent, sizeCurrent);
    }
}
