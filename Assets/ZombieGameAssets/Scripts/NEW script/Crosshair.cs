using UnityEngine;
public class Crosshair : MonoBehaviour
{
    public RectTransform crosshair;

    [SerializeField] float sizeState;
    [SerializeField] float sizeMove;
    [SerializeField] float sizeCurrent;

    [SerializeField] float speedSize;
    private void OnEnable()
    {
        Actions.OnMove += CrosshairChange;
        //  Actions.GunShoot += Impact;

    }
    private void OnDisable()
    {
        Actions.OnMove -= CrosshairChange;
        //  Actions.GunShoot -= Impact;
    }

    private void CrosshairChange(float horizontalInput, float verticalInput)
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
