using UnityEngine.UI;
using UnityEngine;

public class sensivitty : MonoBehaviour
{
    public Slider slider2;
    private const float _multiplier = 20f;



    private void FixedUpdate()
    {
        PlayerCam.Damage(slider2.value);
    }
}
