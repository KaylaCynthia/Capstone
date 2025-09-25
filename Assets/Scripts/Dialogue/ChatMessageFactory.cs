using System.Collections.Generic;
using UnityEngine;

public static class ChatMessageFactory
{
    private static Dictionary<string, Sprite> characterPortraits;

    static ChatMessageFactory()
    {
        LoadCharacterPortraits();
    }

    private static void LoadCharacterPortraits()
    {
        characterPortraits = new Dictionary<string, Sprite>();
        Sprite[] portraits = Resources.LoadAll<Sprite>("Portraits/");
        foreach (Sprite portrait in portraits)
            characterPortraits[portrait.name] = portrait;
    }

    public static ChatMessage Create(string message, DialogueState state)
    {
        Sprite portrait = GetPortrait(state.CurrentSpeaker, state.CurrentPortraitTag);

        if (state.IsPlayerSpeaking)
        {
            return new PlayerMessage(message, portrait);
        }
        else
        {
            return new NPCMessage(state.CurrentSpeaker, message, portrait);
        }
    }

    private static Sprite GetPortrait(string speaker, string portraitTag)
    {
        string key = $"{speaker}_{portraitTag}";
        if (characterPortraits.ContainsKey(key)) return characterPortraits[key];
        if (characterPortraits.ContainsKey(speaker)) return characterPortraits[speaker];
        if (characterPortraits.ContainsKey("default")) return characterPortraits["default"];
        return null;
    }
}