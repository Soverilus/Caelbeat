using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualiser : MonoBehaviour {
    [SerializeField]
    Listener myListener;
    public int bandArray;
    public float level, myScale = 1, maxScale;

    public bool _useBuffer = true;

    void Start() {

    }

    void Update() {
        CalculateBandLevel();
        ChangeShapeBasedonLevel();
    }

    void CalculateBandLevel() {
        if (_useBuffer)
            level = myListener._audBuff[bandArray];
        else
            level = myListener._audBand[bandArray];
    }

    void ChangeShapeBasedonLevel() {
        transform.localScale = new Vector3((level * maxScale) + myScale, (level * maxScale) + myScale, (level * maxScale) + myScale);
    }
}
