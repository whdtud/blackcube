using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;

public class UIGameTimer : MonoBehaviour
{
    public Text Text;
    public Text TextBG;

    private const string TIMER_FORMAT = "{0:00.00}";

    public void SetColor(Color color)
    {
        Text.color = color;
    }

    public Color GetColor()
    {
        return Text.color;
    }

    void Update()
    {
        string text = string.Format(TIMER_FORMAT, GameController.Instance.GameTime);

        Text.text = text;
        TextBG.text = text;
    }

    public IEnumerator Co_Rest(Action callback)
    {
        while (true)
        {
            if (Text.color.r >= 0.99f)
                break;

            Text.color = Color.Lerp(Text.color, Color.red, 0.05f);

            yield return null;
        }

        callback?.Invoke();
    }
}
