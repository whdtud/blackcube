using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using GooglePlayGames;

public static class PlatformHelper {

    private static string mLeaderBoardKey = "CgkI0-7VlYcbEAIQAQ";

    public static void Activate()
    {
        // 구글 플레이 활성화
        PlayGamesPlatform.Activate();

        // 로그인 창을 띄웁니다. 로그인 한적이 있다면 무시합니다.
        Social.localUser.Authenticate((bool success) =>
        {
            // handle success or failure
        });
    }

    public static void ShowLeaderBoard()
    {
        // 로그인이 안되어있다면 다시 로그인합니다.
        Social.localUser.Authenticate((bool success) =>
        {
            // handle success or failure
        });

        // 랭킹을 보여줍니다.
        ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(mLeaderBoardKey);
    }

    public static void ReportScore(int score)
    {
        Social.ReportScore(score, "CgkI0-7VlYcbEAIQAQ", (bool success) =>
        {
        });
    }
}
