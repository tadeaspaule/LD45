using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlatform : Resizer
{
    Transform platform;
    
    // Start is called before the first frame update
    void Start()
    {
        platform = this.transform.GetChild(1);
    }

    public override void Expand()
    {
        platform.localScale += new Vector3(0.3f,0f,0f);
    }

    public override void Shrink()
    {
        platform.localScale -= new Vector3(0.3f,0f,0f);
    }
}
