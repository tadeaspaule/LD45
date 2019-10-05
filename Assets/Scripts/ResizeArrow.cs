using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeArrow : MonoBehaviour
{
    public Transform platform;
    public Transform otherArrow;
    float originalX;
    float firstXScale;
    float originalXscale;
    bool holdingDown;
    
    float plusScale = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        originalX = transform.position.x;
        originalXscale = platform.localScale.x;
        firstXScale = platform.localScale.x;
        if (this.gameObject.name.Equals("minus")) {
            plusScale *= -1f;
            Debug.Log("minus");
        }
    }

    void OnMouseDown()
    {
        holdingDown = true;
    }

    void OnMouseUp()
    {
        holdingDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (holdingDown) {
            platform.localScale += new Vector3(plusScale,0f,0f);
            float posMove = plusScale;
            if (plusScale < 0f) posMove *= -1f;
            transform.position += new Vector3(posMove*1.7f,0f,0f);
            // Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // float xDif = pos.x - originalX;
            // float w = platform.GetComponent<SpriteRenderer>().bounds.size.x;
            // Debug.Log($"w is {w}, xDif is {xDif}");
            // float scaleDif =  (w + xDif) / w;
            // Debug.Log(xDif);
            // platform.localScale = new Vector3(platform.localScale.x*scaleDif,platform.localScale.y,1f);
            // transform.position = new Vector3(pos.x,transform.position.y,0f);
            otherArrow.position = new Vector3(platform.position.x - (transform.position.x - platform.position.x),transform.position.y,0f);
        }
    }
}
