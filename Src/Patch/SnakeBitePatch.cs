using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Cards;

namespace SnakeBiteSakiSfx.Patch;


[HarmonyPatch(typeof(Snakebite), "OnPlay")]
internal static class SnakeBitePatch
{
    static void Prefix(Snakebite __instance, PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CustomSfx.Instance?.Play("res://SnakeBiteBingyuanbanSakiSfx/Audio/SnakeBite.mp3", 1.5f);
    }
}