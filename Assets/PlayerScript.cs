using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame

    private void Update()
    {

    }

    public void Move(Vector2Int moveFrom,Vector2Int moveTo)
    {
        this.transform.position = new Vector3(moveFrom.x, moveFrom.y, 0) + new Vector3(moveTo.x, moveTo.y, 0);
    }

}
