using System.Collections;
using UnityEngine;
public class Spawner : MonoBehaviour
{
    [SerializeField] private float _spawnTime;
    [SerializeField] private GameObject zombieType1;
    [SerializeField] private GameObject zombieType2;
    [SerializeField] private GameObject zombieType3;
    private float timeLeft;
    private void Awake() => StartCoroutine(Spawn());
    private IEnumerator Spawn()
    {
        timeLeft += _spawnTime;
        if (timeLeft < 60)
        {
            float[] arr = RandomOrg();
            Instantiate(zombieType1, new Vector3(arr[0], -28f, arr[1]), Quaternion.identity);
        }
        else if (timeLeft > 60 && timeLeft < 120)
        {
            float[] arr = RandomOrg();
            Instantiate(zombieType1, new Vector3(arr[0], -28f, arr[1]), Quaternion.identity);
            float[] arr2 = RandomOrg();
            Instantiate(zombieType2, new Vector3(arr2[0], -28f, arr2[1]), Quaternion.identity);
            float[] arr3 = RandomOrg();

        }
        else if (timeLeft > 120 && timeLeft < 180)
        {
            _spawnTime = 10;

            float[] arr3 = RandomOrg();
            Instantiate(zombieType3, new Vector3(arr3[0], -28f, arr3[1]), Quaternion.identity);
        }
        if (timeLeft > 180 && timeLeft < 500)
        {
            float[] arr = RandomOrg();
            Instantiate(zombieType1, new Vector3(arr[0], -28f, arr[1]), Quaternion.identity);
            float[] arr2 = RandomOrg();
            Instantiate(zombieType2, new Vector3(arr2[0], -28f, arr2[1]), Quaternion.identity);
            float[] arr3 = RandomOrg();
            Instantiate(zombieType3, new Vector3(arr3[0], -28f, arr3[1]), Quaternion.identity);
        }
        yield return new WaitForSeconds(_spawnTime);
        StartCoroutine(Spawn());
    }
    private float[] RandomOrg()
    {
        float[] spawnPoints = new float[] { -19f, 35f, 67f, 38f, -17.59f, -65f };
        float zPos = 0, xPos = spawnPoints[Random.Range(0, spawnPoints.Length)];

        if (xPos == -19f || xPos == 35f) zPos = 55f;
        else if (xPos == 67f) zPos = 4f;
        else if (xPos == 37f || xPos == -17.59f) zPos = -41.8f;
        else if (xPos == -65f) zPos = 1.37f;

        float[] arr = new[] { xPos, zPos };
        return arr;
    }
}