using System.Collections;
using UnityEngine;

public class SelfDestruction : MonoBehaviour
{
    [SerializeField] private float TimeBeforeDestruction = 2f;

    private void Start()
    {
        StartCoroutine(SelfDestruct());
    }

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(TimeBeforeDestruction);

        Destroy(gameObject);
    }
    
}
