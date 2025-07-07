using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSprite : MonoBehaviour
{
    public int layerIndex = 0;
    public bool hasShadow = false;
    public Color shadowColor = Color.black;
    public Vector2 shadowOffset = new Vector2(1, 1);
    private GameObject shadow;

    //public bool isFlip = false; // Test function

    private RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        if(hasShadow)
        {
            CreateShadow(shadowColor, shadowOffset);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (isFlip) Flip();
    }

    private void CreateShadow(Color color, Vector3 offset)
    {
        shadow = Instantiate(gameObject, transform);
        shadow.transform.SetParent(null);
        Destroy(shadow.GetComponent<CharacterSprite>());
        if (shadow.GetComponent<Image>() == null) return; // Image Empty Error
        Image shadowImage = shadow.GetComponent<Image>();
        shadowImage.color = color;

        shadow.transform.position = transform.position + offset;
        shadow.transform.SetParent(transform.parent);
        transform.SetParent(shadow.transform);
    }

    public void Flip()
    {
        Vector3 flipped = rectTransform.localScale;
        flipped.y *= -1;
        rectTransform.localScale = flipped;
        //Debug.Log("flip");
    }

    public void AddShadow(bool hasShadow)
    {
        this.hasShadow = hasShadow;
    }

    public void ShadowSettings(Color shadowColor, Vector2 shadowOffset)
    {
        this.shadowColor = shadowColor;
        this.shadowOffset = shadowOffset;
    }
}
