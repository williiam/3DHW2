using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public Rigidbody rb;

    void fixedUpdate()
    {
        rb.AddForce(0, 0, 50);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -7)
        {
            //摧毀自己
            Destroy(gameObject);
        }
    }
}
