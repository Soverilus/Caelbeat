using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerBehaviour : MonoBehaviour {
    [SerializeField]
    Visualiser[] allVis;
    float[] allBands;
    float level;
    void Start() {
        allBands = new float[allVis.Length];
    }

    void Update() {

    }


}
