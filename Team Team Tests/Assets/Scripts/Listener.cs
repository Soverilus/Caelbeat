using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
[RequireComponent(typeof(AudioSource))]
public class Listener : MonoBehaviour {
    AudioSource _audioSource;
    [SerializeField]
    float bandMultiplier = 10;
    float[] _samples = new float[512];
    float[] _freqBand = new float[8];
    float[] _bandBuff = new float[8];
    float[] _buffLerp = new float[8];

    float[] _fBandMax = new float[8];
    public float[] _audBand = new float[8];
    public float[] _audBuff = new float[8];

    void Start() {
        _audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        GetSpectrumAudio();
        MakeFreqBand();
        BandBuff();
        CreateAudBands();
    }

    void GetSpectrumAudio() {
        _audioSource.GetSpectrumData(_samples, 0, FFTWindow.Hamming);
    }

    void CreateAudBands() {
        for (int i = 0; i < 8; i++) {
            if (_freqBand[i] > _fBandMax[i]) {
                _fBandMax[i] = _freqBand[i];
            }
            _audBand[i] = (_freqBand[i] / _fBandMax[i]);
            _audBuff[i] = (_bandBuff[i] / _fBandMax[i]);
        }
    }

    void BandBuff() {
        for (int g = 0; g < 8; g++) {
            if (_freqBand[g] > _bandBuff[g]) {
                _bandBuff[g] = _freqBand[g];
                _buffLerp[g] = 0.005f;
            }
            if (_freqBand[g] < _bandBuff[g]) {
                _bandBuff[g] -= _buffLerp[g];
                _buffLerp[g] *= 1.25f;
            }
        }
    }

    void MakeFreqBand() {

        /* 22050 / 512 currents bands = 43 hz/samp
         * 20 - 60
         * 60 - 250
         * 250 - 500
         * 500 - 2000
         * 2000 - 4000
         * 4000 - 6000
         * 6000 - 20000
         * 
         * 0 - 2 = 87hz      : 0 - 87
         * 1 - 4 = 172hz     : 87 - 258
         * 2 - 8 = 344hz     : 259 - 602
         * 3 - 16 = 688hz    : 603 - 1290
         * 4 - 32 = 1376hz   : 1291 - 2666
         * 5 - 64 = 2752hz   : 2667 - 5418
         * 6 - 128 = 5504hz  : 5419 - 10922
         * 7 - 256 = 11008hz : 10923 - 21930 (+2*87)
         * 
        */

        int count = 0;

        for (int i = 0; i < 8; i++) {

            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            /*
             * something Peer Play suggested : using powers of 
             * 2, the very last number will miss two sets of 87 
             * hertz, so we add that here.
             */
            if (i == 7) {
                sampleCount += 2;
            }

            // the following averages the hertz range in each for i.
            for (int j = 0; j < sampleCount; j++) {
                average += _samples[count] * (count + 1);
                count++;
            }

            average /= count;

            _freqBand[i] = average * bandMultiplier;
        }


    }
}
