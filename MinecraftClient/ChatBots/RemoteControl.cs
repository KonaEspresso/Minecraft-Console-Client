﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinecraftClient.ChatBots
{
    /// <summary>
    /// Allow to perform operations using whispers to the bot
    /// </summary>

    public class RemoteControl : ChatBot
    {
        public override void GetText(string text)
        {
            text = GetVerbatim(text).Trim();
            string command = "", sender = "";
            if (IsPrivateMessage(text, ref command, ref sender) && Settings.Bots_Owners.Contains(sender.ToLower().Trim()))
            {
                string response = "";
                PerformInternalCommand(command, ref response);
                response = GetVerbatim(response);
                foreach (char disallowedChar in McClient.GetDisallowedChatCharacters())
                {
                    response = response.Replace(disallowedChar.ToString(), String.Empty);
                }
                if (response.Length > 0)
                {
                    SendPrivateMessage(sender, response);
                }
            }
            else if (Settings.RemoteCtrl_AutoTpaccept
                && IsTeleportRequest(text, ref sender)
                && (Settings.RemoteCtrl_AutoTpaccept_Everyone || Settings.Bots_Owners.Contains(sender.ToLower().Trim())))
            {
                SendText("/tpaccept");
            }
        }
    }
}
