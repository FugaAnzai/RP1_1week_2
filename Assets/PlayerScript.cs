using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerScript : MonoBehaviour
{
    private Vector3 start_ = Vector3.zero;
    private Vector3 end_ = Vector3.zero;
    private float moveT_ = 0;
    private bool isMove = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (Input.GetKeyDown(KeyCode.D) && !isMove)
        {
            isMove = true;
            SetPlayerLerp(this.transform.position, this.transform.position + new Vector3(1, 0, 0));
            this.GetComponent<SpriteRenderer>().flipX = false;
        }

        if(Input.GetKeyDown(KeyCode.A) && !isMove)
        {
            isMove = true;
            SetPlayerLerp(this.transform.position, this.transform.position + new Vector3(-1, 0, 0));
            this.GetComponent<SpriteRenderer>().flipX = true;

        }

        if (Input.GetKeyDown(KeyCode.W) && !isMove)
        {
            isMove = true;
            SetPlayerLerp(this.transform.position, this.transform.position + new Vector3(0, 1, 0));

        }

        if (Input.GetKeyDown(KeyCode.S) && !isMove)
        {
            isMove = true;
            SetPlayerLerp(this.transform.position, this.transform.position + new Vector3(0, -1, 0));

        }

        PlayerLerp();

    }

    private void PlayerLerp()
    {
        if (isMove)
        {
            moveT_ += 0.05f;

            if(moveT_ <= 1.0f)
            {
                this.transform.position = Vector3.Lerp(start_,end_,moveT_);
            }

            if(moveT_ > 1.0f)
            {
                moveT_ = 0.0f;
                isMove = false;
            }

        }
    }

    private void SetPlayerLerp(Vector3 start,Vector3 end)
    {
        start_ = start; end_ = end;
    }
}
