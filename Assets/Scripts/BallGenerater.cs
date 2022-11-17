using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGenerater : MonoBehaviour
{
    private float nextActionTime = 0.0f;
    public float period = 0.15f;
    
    public GameObject smallBallPrefab;
    public GameObject midBallPrefab;
    public GameObject bigballPrefab;
    public GameObject smallCylinderPrefab;
    public GameObject midCylinderPrefab;
    public GameObject bigCylinderPrefab;

    IList<GameObject> prefabList = new List<GameObject>();

    // Start is called before the first frame update
    void Awake() {
        prefabList.Add(smallBallPrefab);
        prefabList.Add(midBallPrefab);
        prefabList.Add(bigballPrefab);
        prefabList.Add(smallCylinderPrefab);
        prefabList.Add(midCylinderPrefab);
        prefabList.Add(bigCylinderPrefab);
     }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextActionTime)
        {
            GameObject randomPrefab = getRandomPrefab();
            Vector3 pos = getRandomPosition();
            Vector3 force = getRandomForce();
            nextActionTime += period;
            GameObject p = Instantiate(randomPrefab, pos, transform.rotation);
            p.GetComponent<Rigidbody>().AddForce(0, 0, 1000);
        }
    }

    private Vector3 getRandomPosition()
    {
        float x = Random.Range(-2, 4);
        float y = Random.Range(3, 6);
        float z = Random.Range(35, 38);
        return new Vector3(x, y, z);
    }

    // 回傳隨機Z軸反方向力量
    private Vector3 getRandomForce()
    {
        Vector3 force = new Vector3(0, 0, Random.Range(-30, -50));
        return force;
    }
    // 回傳隨機Z軸反方向力量
    private GameObject getRandomPrefab()
    {
        int prefabIndex =  Random.Range(0, 5);
        return prefabList[prefabIndex];
    }
}
