using UnityEngine;

public static class UIHelper
{
    public static bool CheckBackButton()
    {
        return Input.GetKeyUp(KeyCode.Escape);
    }
}
