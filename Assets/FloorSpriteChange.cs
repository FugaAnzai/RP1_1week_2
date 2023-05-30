using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSpriteChange : MonoBehaviour
{
    //�d���I�t
    public Sprite offImage;
    //�d���I��
    public Sprite onImage;
    //spriteRenderer
    SpriteRenderer spriteRenderer;

    private bool isChange_ = false;
    private bool isChangeReady_ = false;
    public int[] number_;

    // Start is called before the first frame update
    void Start()
    {
        isChange_ = false;
        isChangeReady_ = false;
        number_ = new int[8];
        for (int i = 0; i < 8; i++)
        {
            number_[i] = 0;
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeSprite();
    }

    public bool GetIsChange()
    {
        return isChange_;
    }

    public void SetIsChange()
    {
        isChange_ = isChangeReady_;
    }

    public void SetIsChangeReady(bool isChangeReady)
    {
        isChangeReady_ = isChangeReady;
    }

    void ChangeSprite()
    {
        if (!isChange_)
        {
            spriteRenderer.sprite = offImage;
        }

        if (isChange_)
        {
            spriteRenderer.sprite = onImage;
        }
    }
}
