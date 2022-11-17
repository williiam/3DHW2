using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderGenerater : MonoBehaviour
{
    private float nextActionTime = 0.0f;
    public float period = 0.5f;
    
    public Rigidbody cylinderPrefab;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextActionTime)
        {
            Vector3 pos = getRandomPosition();
            Vector3 force = getRandomForce();
            nextActionTime += period;
            Rigidbody p = Instantiate(cylinderPrefab, pos, transform.rotation);
            // p.AddForce(force);
            p.AddForce(0, 0, 1000);
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
}
