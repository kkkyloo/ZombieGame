using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public RectTransform crosshair;

    [SerializeField] float sizeState;
    [SerializeField] float sizeMove;
    [SerializeField] float sizeCurrent;

    [SerializeField] float speedSize;

    private void Update()
    {
        if (IsMoving)
        {
            sizeCurrent = Mathf.Lerp(sizeCurrent, sizeMove, Time.deltaTime * speedSize);
        }
        else
        {
            sizeCurrent = Mathf.Lerp(sizeCurrent, sizeState, Time.deltaTime * speedSize);
        }

        crosshair.sizeDelta = new Vector2(sizeCurrent, sizeCurrent);
    }

    bool IsMoving
    {
        get
        {
            if (
            PlayerMovement.horizontalInput != 0 ||
            PlayerMovement.verticalInput != 0
                )
                return true;
            else
                return false;
        }
    }
}
