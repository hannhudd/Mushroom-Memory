using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : MonoBehaviour
{
    [SerializeField] GameObject targetObject;
    [SerializeField] string targetMessage;
    public Color highlightColor = Color.yellow;
    private float scale;

    void Start()
    {
        scale = transform.localScale[0];
    }

    public void OnMouseEnter()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            sprite.color = highlightColor;
        }
    }

    public void OnMouseExit()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            sprite.color = Color.white;
        }
    }

    public void OnMouseDown()
    {
        transform.localScale = new Vector3(1.1f * scale, 1.1f * scale, 1.1f * scale);
    }

    public void OnMouseUp()
    {
        transform.localScale = new Vector3(scale, scale, scale);
        if (targetObject != null)
        {
            targetObject.SendMessage(targetMessage);
        }
    }
}
