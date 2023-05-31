using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMove : MonoBehaviour
{
    private Vector3 start;

    // Start is called before the first frame update
    void Start()
    {
        start = new Vector3(38.39f, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += new Vector3(-0.001f, 0, 0);
    }

    private void OnBecameInvisible()
    {
        this.transform.position = start * Time.deltaTime;
    }

}
