using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalWallSpriteChange : MonoBehaviour
{
    //�d���I�t
    public Sprite offImage;
    //�d���I��
    public Sprite onImage;
    //spriteRenderer
    SpriteRenderer spriteRenderer;

    private bool isChange_ = true;

    // Start is called before the first frame update
    void Start()
    {
        isChange_ = true;
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

    public void SetIsChange(bool isChange)
    {
        isChange_ = isChange;
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
