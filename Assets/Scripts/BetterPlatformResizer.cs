using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterPlatformResizer : Resizer
{
    Transform platform;
    List<Transform> leftDecors = new List<Transform>();
    List<Transform> rightDecors = new List<Transform>();
    
    // Start is called before the first frame update
    void Start()
    {
        platform = this.transform.GetChild(1);
        foreach (Transform child in transform) {
            if (child.gameObject.name.StartsWith("leftdecoration")) {
                leftDecors.Add(child);
            }
            else if (child.gameObject.name.StartsWith("rightdecoration")) {
                rightDecors.Add(child);
            }
        }
    }

    public override void Expand()
    {
        Resize(1);
    }

    public override void Shrink()
    {
        Resize(-1);
    }

    void Resize(int mult)
    {
        platform.localScale += new Vector3(0.3f,0f,0f) * mult;
        Vector3 move = new Vector3(0.5f,0f,0f) * mult;
        foreach (Transform left in leftDecors) {
            left.position -= move;
        }
        foreach (Transform right in rightDecors) {
            right.position += move;
        }
    }
}
