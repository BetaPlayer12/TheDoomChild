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
namespace Doozy.Runtime.UIManager.Containers
{
    public partial class UIView
    {
        public static IEnumerable<UIView> GetViews(UIViewId.CampaignSelect id) => GetViews(nameof(UIViewId.CampaignSelect), id.ToString());
        public static void Show(UIViewId.CampaignSelect id, bool instant = false) => Show(nameof(UIViewId.CampaignSelect), id.ToString(), instant);
        public static void Hide(UIViewId.CampaignSelect id, bool instant = false) => Hide(nameof(UIViewId.CampaignSelect), id.ToString(), instant);

        public static IEnumerable<UIView> GetViews(UIViewId.Codex id) => GetViews(nameof(UIViewId.Codex), id.ToString());
        public static void Show(UIViewId.Codex id, bool instant = false) => Show(nameof(UIViewId.Codex), id.ToString(), instant);
        public static void Hide(UIViewId.Codex id, bool instant = false) => Hide(nameof(UIViewId.Codex), id.ToString(), instant);

        public static IEnumerable<UIView> GetViews(UIViewId.Debug id) => GetViews(nameof(UIViewId.Debug), id.ToString());
        public static void Show(UIViewId.Debug id, bool instant = false) => Show(nameof(UIViewId.Debug), id.ToString(), instant);
        public static void Hide(UIViewId.Debug id, bool instant = false) => Hide(nameof(UIViewId.Debug), id.ToString(), instant);

        public static IEnumerable<UIView> GetViews(UIViewId.Gameplay id) => GetViews(nameof(UIViewId.Gameplay), id.ToString());
        public static void Show(UIViewId.Gameplay id, bool instant = false) => Show(nameof(UIViewId.Gameplay), id.ToString(), instant);
        public static void Hide(UIViewId.Gameplay id, bool instant = false) => Hide(nameof(UIViewId.Gameplay), id.ToString(), instant);

        public static IEnumerable<UIView> GetViews(UIViewId.General id) => GetViews(nameof(UIViewId.General), id.ToString());
        public static void Show(UIViewId.General id, bool instant = false) => Show(nameof(UIViewId.General), id.ToString(), instant);
        public static void Hide(UIViewId.General id, bool instant = false) => Hide(nameof(UIViewId.General), id.ToString(), instant);

        public static IEnumerable<UIView> GetViews(UIViewId.Intro id) => GetViews(nameof(UIViewId.Intro), id.ToString());
        public static void Show(UIViewId.Intro id, bool instant = false) => Show(nameof(UIViewId.Intro), id.ToString(), instant);
        public static void Hide(UIViewId.Intro id, bool instant = false) => Hide(nameof(UIViewId.Intro), id.ToString(), instant);

        public static IEnumerable<UIView> GetViews(UIViewId.Loading id) => GetViews(nameof(UIViewId.Loading), id.ToString());
        public static void Show(UIViewId.Loading id, bool instant = false) => Show(nameof(UIViewId.Loading), id.ToString(), instant);
        public static void Hide(UIViewId.Loading id, bool instant = false) => Hide(nameof(UIViewId.Loading), id.ToString(), instant);

        public static IEnumerable<UIView> GetViews(UIViewId.MainMenu id) => GetViews(nameof(UIViewId.MainMenu), id.ToString());
        public static void Show(UIViewId.MainMenu id, bool instant = false) => Show(nameof(UIViewId.MainMenu), id.ToString(), instant);
        public static void Hide(UIViewId.MainMenu id, bool instant = false) => Hide(nameof(UIViewId.MainMenu), id.ToString(), instant);

        public static IEnumerable<UIView> GetViews(UIViewId.Merchant id) => GetViews(nameof(UIViewId.Merchant), id.ToString());
        public static void Show(UIViewId.Merchant id, bool instant = false) => Show(nameof(UIViewId.Merchant), id.ToString(), instant);
        public static void Hide(UIViewId.Merchant id, bool instant = false) => Hide(nameof(UIViewId.Merchant), id.ToString(), instant);

        public static IEnumerable<UIView> GetViews(UIViewId.PlayerHUD id) => GetViews(nameof(UIViewId.PlayerHUD), id.ToString());
        public static void Show(UIViewId.PlayerHUD id, bool instant = false) => Show(nameof(UIViewId.PlayerHUD), id.ToString(), instant);
        public static void Hide(UIViewId.PlayerHUD id, bool instant = false) => Hide(nameof(UIViewId.PlayerHUD), id.ToString(), instant);

        public static IEnumerable<UIView> GetViews(UIViewId.Store id) => GetViews(nameof(UIViewId.Store), id.ToString());
        public static void Show(UIViewId.Store id, bool instant = false) => Show(nameof(UIViewId.Store), id.ToString(), instant);
        public static void Hide(UIViewId.Store id, bool instant = false) => Hide(nameof(UIViewId.Store), id.ToString(), instant);
        public static IEnumerable<UIView> GetViews(UIViewId.Transistion id) => GetViews(nameof(UIViewId.Transistion), id.ToString());
        public static void Show(UIViewId.Transistion id, bool instant = false) => Show(nameof(UIViewId.Transistion), id.ToString(), instant);
        public static void Hide(UIViewId.Transistion id, bool instant = false) => Hide(nameof(UIViewId.Transistion), id.ToString(), instant);
    }
}

namespace Doozy.Runtime.UIManager
{
    public partial class UIViewId
    {
        public enum CampaignSelect
        {
            Info,
            Main,
            Video
        }

        public enum Codex
        {
            Bestiary,
            Characters,
            Location,
            Lore,
            MainCodex,
            Quest,
            Tutorial
        }

        public enum Debug
        {
            CombatArtsDebug,
            MainDebug,
            PlayerStatDebug,
            PrimarySkillDebug,
            SoulSkillDebug,
            Window
        }

        public enum Gameplay
        {
            Cinematic,
            CinematicVideo,
            GameOver,
            HUD,
            Pause,
            Store
        }

        public enum General
        {
            Confirmation
        }

        public enum Intro
        {
            TradeMark
        }

        public enum Loading
        {
            BlackFade,
            SplashArt
        }

        public enum MainMenu
        {
            Credits,
            Extras,
            NavigationPanel,
            NavigationVideo,
            Settings,
            SplashScreen
        }

        public enum Merchant
        {
            GeneralShop,
            PurchaseConfirmation
        }

        public enum PlayerHUD
        {
            Health,
            QuickItem,
            SoulEssence
        }

        public enum Store
        {
            Bestiary,
            Codex,
            CombatArt,
            Inventory,
            Map,
            PlayerStats,
            SoulSkill,
            StatsMain,
            VideoTransition
        }
        public enum Transistion
        {
            Fade
        }    
    }
}