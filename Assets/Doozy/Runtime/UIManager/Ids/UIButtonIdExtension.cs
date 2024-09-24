// Copyright (c) 2015 - 2022 Doozy Entertainment. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

//.........................
//.....Generated Class.....
//.........................
//.......Do not edit.......
//.........................

using System.Collections.Generic;
// ReSharper disable All
namespace Doozy.Runtime.UIManager.Components
{
    public partial class UIButton
    {
        public static IEnumerable<UIButton> GetButtons(UIButtonId.CampaignSelect id) => GetButtons(nameof(UIButtonId.CampaignSelect), id.ToString());
        public static bool SelectButton(UIButtonId.CampaignSelect id) => SelectButton(nameof(UIButtonId.CampaignSelect), id.ToString());

        public static IEnumerable<UIButton> GetButtons(UIButtonId.Codex id) => GetButtons(nameof(UIButtonId.Codex), id.ToString());
        public static bool SelectButton(UIButtonId.Codex id) => SelectButton(nameof(UIButtonId.Codex), id.ToString());

        public static IEnumerable<UIButton> GetButtons(UIButtonId.Debug id) => GetButtons(nameof(UIButtonId.Debug), id.ToString());
        public static bool SelectButton(UIButtonId.Debug id) => SelectButton(nameof(UIButtonId.Debug), id.ToString());

        public static IEnumerable<UIButton> GetButtons(UIButtonId.General id) => GetButtons(nameof(UIButtonId.General), id.ToString());
        public static bool SelectButton(UIButtonId.General id) => SelectButton(nameof(UIButtonId.General), id.ToString());

        public static IEnumerable<UIButton> GetButtons(UIButtonId.MainMenuNavigation id) => GetButtons(nameof(UIButtonId.MainMenuNavigation), id.ToString());
        public static bool SelectButton(UIButtonId.MainMenuNavigation id) => SelectButton(nameof(UIButtonId.MainMenuNavigation), id.ToString());

        public static IEnumerable<UIButton> GetButtons(UIButtonId.Merchant id) => GetButtons(nameof(UIButtonId.Merchant), id.ToString());
        public static bool SelectButton(UIButtonId.Merchant id) => SelectButton(nameof(UIButtonId.Merchant), id.ToString());

        public static IEnumerable<UIButton> GetButtons(UIButtonId.PauseMenu id) => GetButtons(nameof(UIButtonId.PauseMenu), id.ToString());
        public static bool SelectButton(UIButtonId.PauseMenu id) => SelectButton(nameof(UIButtonId.PauseMenu), id.ToString());
    }
}

namespace Doozy.Runtime.UIManager
{
    public partial class UIButtonId
    {
        public enum CampaignSelect
        {
            Delete,
            UseCampaign
        }

        public enum Codex
        {
            Bestiary,
            Characters,
            Location,
            Lore,
            Quests,
            Tutorials
        }

        public enum Debug
        {
            FastTravel,
            Merchant,
            OpenDebug,
            PlayerStat,
            PrimarySkill,
            SoulSkill
        }

        public enum General
        {
            Back,
            Next,
            No,
            Previous,
            Yes
        }

        public enum MainMenuNavigation
        {
            Credits,
            Extras,
            Play,
            Quit,
            Settings
        }

        public enum Merchant
        {
            Purchase
        }

        public enum PauseMenu
        {
            Resume,
            ToMainMenu
        }    
    }
}