using UnityEngine;

public class boop : MonoBehaviour
{
    [Range(0.001f, 0.01f)]
    public float Amount = 0.002f;
    [Range(1f, 30f)]

    public float Freq = 10.0f;
    [Range(10f, 100f)]
    public float Smooth = 10.0f;

    Vector3 StartPos;

    void Start()
    {
        StartPos = transform.localPosition;
    }

    void Update()
    {

        CheckForHeadbobTriger();
        StopHeadBob();
    }

    private void CheckForHeadbobTriger()
    {
        float inputMagnitude = new Vector3(Mathf.Abs(PlayerMovement.horizontalInput), 0, Mathf.Abs(PlayerMovement.verticalInput)).magnitude;

        if (inputMagnitude >0)
            StartHeadBob();
    }

    private Vector3 StartHeadBob()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * Freq) * Amount * 1.4f, Smooth * Time.deltaTime);
        pos.x += Mathf.Lerp(pos.y, Mathf.Cos(Time.time * Freq / 2) * Amount * 1.6f, Smooth * Time.deltaTime);

        transform.localPosition += pos;
        return pos;

    }

    private void StopHeadBob()
    {
        if(transform.localPosition == StartPos) return;
        transform.localPosition = Vector3.Lerp(transform.localPosition, StartPos, 1 * Time.deltaTime);
    }
    









}