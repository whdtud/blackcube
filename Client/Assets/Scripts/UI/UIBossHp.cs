using UnityEngine;
using UnityEngine.UI;

public class UIBossHp : MonoBehaviour
{
    public Slider HpSlider;
    public Slider HpHpSliderBG;

    void Update()
    {
        HpSlider.value = GameController.Instance.Boss.HpPercent;

        HpHpSliderBG.value = Mathf.Lerp(HpHpSliderBG.value, HpHpSliderBG.value, 0.05f);
    }
}
