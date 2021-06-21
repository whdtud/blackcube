using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour {

    public void UpdateValue(float value)
    {
        AudioListener.volume = value;
    }
}
