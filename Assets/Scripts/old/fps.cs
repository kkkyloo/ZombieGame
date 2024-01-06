using UnityEngine;
public class FpsControll : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 165;
    }
}