using System;
using ZeepSDK.Chat;
using ZeepSDK.ChatCommands;

namespace TNRD.Zeepkist.MedalTimes;

public class AuthorTimeChatCommand : ILocalChatCommand
{
    public string Prefix => "/";
    public string Command => "at";
    public string Description => "Posts the author time as a chat message";

    public void Handle(string arguments)
    {
        string time = TimeSpan.FromSeconds(PlayerManager.Instance.currentMaster.setupScript.GlobalLevel.TimeAuthor)
            .ToString("mm\\:ss\\.fff");

        ChatApi.SendMessage("Author time: " + time);
    }
}
