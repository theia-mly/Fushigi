using Fushigi.Byml;
using Fushigi.course;
using Fushigi.gl;
using Fushigi.param;
using Fushigi.ui.modal;
using Fushigi.util;
using ImGuiNET;
using Silk.NET.Input;
using Silk.NET.SDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Fushigi.ui.widgets
{
    class AreaSettings
    {
        private static readonly Dictionary<string, string> Tilesets = new Dictionary<string, string>()
        {
            { "None", "" },
            { "Undefined", "Undefined" },
            { "Pipe-Rock Plateau (Grassy Overworld)", "HajimariSougen" },
            { "Pipe-Rock Plateau (Forest)", "HajimariMori" },
            { "Pipe-Rock Plateau (Grassy Savanna)", "Savanna" },
            { "Pipe-Rock Plateau (Rocky Savanna)", "SavannaSabaku" },
            { "Pipe-Rock Plateau (Mossy Underground)", "SavannaChitei" },
            { "Pipe-Rock Plateau (Puffy Sky)", "CommonSora" },
            { "Petal Isles (Sandy Overworld)", "HajimariKaigan" },
            { "Petal Isles (Rocky Underground)", "HajimariChika" },
            { "Petal Isles (Candy Overworld)", "MushiMachi" },
            { "Petal Isles (Candy Underground)", "MushiChika" },
            { "Fluff-Puff Peaks (Icy Overworld)", "YamaDokutu" },
            { "Fluff-Puff Peaks (Snowy Overworld (Purple))", "YamaTijyo" },
            { "Fluff-Puff Peaks (Snowy Overworld (Green))", "YamaJyoku" },
            { "Shining Falls (Overworld)", "WaKaigan" },
            { "Sunbaked Desert (Overworld)", "SabakuChijou" },
            { "Sunbaked Desert (Crystal Underground)", "SabakuChika" },
            { "Sunbaked Desert (Mansion)", "SabakuIseki" },
            { "Fungi Mines (Orange Forest)", "KinMoriAkarui" },
            { "Fungi Mines (Forest)", "KinMori" },
            { "Fungi Mines (Ghost Mansion)", "CommonYashiki" },
            { "Fungi Mines (Ruins)", "KinSlime" },
            { "Deep Magma Bog (Normal)", "NettaiYogan" },
            { "Deep Magma Bog (Glowing)", "NettaiResort" },
            { "Deep Magma Bog (Glowing Dark)", "NettaiResortDark" },
            { "Palace (Variation A)", "CommonToride" },
            { "Palace (Variation B)", "CommonTorideB" },
            { "Castle", "CastleSiro" },
            { "Factory", "CastleFactory" },
            { "Battleship", "CommonSenkan" },
            { "Search Party", "MiniCourseA" },
            { "Petal Isles (Underwater; Sandy Overworld Duplicate)", "CommonSuityuAsase" },
            { "Shining Falls (Underwater?; Grassy Savanna Duplicate) (Unused)", "WaAsase" },
            { "Petal Isles (Candy Overworld?; Grassy Savanna Duplicate) (Unused)", "MushiJyoku" },
            { "Bonus? (Has No Models) (Unused)", "CommonBonus" },
        };

        public static void Draw(ref bool continueDisplay, IPopupModalHost modalHost, AreaParam areaParam)
        {
            ImGui.SetNextWindowSize(new Vector2(500, 500), ImGuiCond.Once);

            // Window
            if (ImGui.Begin("Area Settings", ImGuiWindowFlags.NoDocking | ImGuiWindowFlags.NoCollapse))
            {
                // Close Button
                if (ImGui.Button("Close"))
                {
                    continueDisplay = false;

                    /// Remove empty strings from palette lists
                    //  from Transition palettes
                    var transPal = areaParam.EnvPaletteSetting.TransPaletteList;
                    if (transPal != null)
                    {
                        for (int i = 0; i < transPal.Count; i++)
                            if (transPal[i] == "") transPal.Remove(transPal[i]);

                        if (transPal.Count == 0) transPal = null;
                    }
                    areaParam.EnvPaletteSetting.TransPaletteList = transPal;

                    //  from Wonder palettes
                    var wonderPal = areaParam.EnvPaletteSetting.WonderPaletteList;
                    if (wonderPal != null)
                    {
                        for (int i = 0; i < wonderPal.Count; i++)
                            if (wonderPal[i] == "") wonderPal.Remove(wonderPal[i]);

                        if (wonderPal.Count == 0) wonderPal = null;
                    }
                    areaParam.EnvPaletteSetting.WonderPaletteList = wonderPal;

                    //  from Event palettes
                    var eventPal = areaParam.EnvPaletteSetting.EventPaletteList;
                    if (eventPal != null)
                    {
                        for (int i = 0; i < eventPal.Count; i++)
                            if (eventPal[i] == "") eventPal.Remove(eventPal[i]);

                        if (eventPal.Count == 0) eventPal = null;
                    }
                    areaParam.EnvPaletteSetting.EventPaletteList = eventPal;
                }

                // Setting Tabs
                if (ImGui.BeginTabBar("AreaSettingsTypes", ImGuiTabBarFlags.None))
                {
                    if (ImGui.BeginTabItem("Appearance"))
                    {
                        DrawAppearanceSettings(areaParam);
                        ImGui.EndTabItem();
                    }
                    if (ImGui.BeginTabItem("Music & Sound"))
                    {
                        DrawMusicSettings(areaParam);
                        ImGui.EndTabItem();
                    }
                    if (ImGui.BeginTabItem("Misc."))
                    {
                        DrawMiscSettings(areaParam);
                        ImGui.EndTabItem();
                    }
                    ImGui.EndTabBar();
                }
                
                ImGui.End();
            }
            
        }

        private static void DrawAppearanceSettings(AreaParam areaParam)
        {
            ImGui.SeparatorText("Terrain Settings");
            if (ImGui.BeginTable("SkinParam", 2))
            {
                AreaParam.AreaSkinParam skinParam = areaParam.SkinParam;
                string value = "";

                ImGui.TableNextRow();
                ImGui.TableSetColumnIndex(0);

                ImGui.Text("Field A");
                ImGui.TableSetColumnIndex(1);
                value = skinParam.FieldA;
                int index = Tilesets.Values.ToList().IndexOf(value);
                if (ImGui.Combo("##FieldA", ref index, Tilesets.Keys.ToArray(), Tilesets.Count(), 10)) 
                    skinParam.FieldA = Tilesets.Values.ToArray()[index];

                ImGui.TableNextColumn();

                ImGui.Text("Field B");
                ImGui.TableSetColumnIndex(1);
                value = skinParam.FieldB;
                index = Tilesets.Values.ToList().IndexOf(value);
                if (ImGui.Combo("##FieldB", ref index, Tilesets.Keys.ToArray(), Tilesets.Count(), 10))
                    skinParam.FieldB = Tilesets.Values.ToArray()[index];

                ImGui.TableNextColumn();

                ImGui.Text("Object Appearance");
                ImGui.TableSetColumnIndex(1);
                value = skinParam.Object;
                index = Tilesets.Values.ToList().IndexOf(value);
                if (ImGui.Combo("##Object", ref index, Tilesets.Keys.ToArray(), Tilesets.Count(), 10))
                    skinParam.Object = Tilesets.Values.ToArray()[index];

                ImGui.TableNextColumn();

                bool disableBgUnitDecoA = skinParam.DisableBgUnitDecoA;
                if (ImGui.Checkbox("Disable Decorations on Field A", ref disableBgUnitDecoA))
                {
                    skinParam.DisableBgUnitDecoA = disableBgUnitDecoA;
                }

                ImGui.EndTable(); 
            }

            ImGui.SeparatorText("Level Palettes");
            var initPal = areaParam.EnvPaletteSetting.InitPaletteBaseName;
            if (ImGui.TreeNode("Initial Palette"))
            {
                if (ImGui.InputText("##InitialPalette", ref initPal, 1024))
                {
                    areaParam.EnvPaletteSetting.InitPaletteBaseName = initPal;
                }
                ImGui.TreePop();
            }

            List<string> transPal = areaParam.EnvPaletteSetting.TransPaletteList;
            if (ImGui.TreeNode("Transition Palettes"))
            {
                if (ImGui.Button("Add Palette"))
                {
                    if (transPal is null) transPal = new List<string>();
                    transPal.Add("");
                    areaParam.EnvPaletteSetting.TransPaletteList = transPal;
                }

                if (transPal is not null)
                {
                    for (int i = 0; i < transPal.Count; i++)
                    {
                        var palette = transPal[i];
                        if (ImGui.InputText($"##TransitionPalette{i}", ref palette, 1024))
                            transPal[i] = palette;
                    }
                }

                ImGui.TreePop();
            }

            List<string> wonderPal = areaParam.EnvPaletteSetting.WonderPaletteList;
            if (ImGui.TreeNode("Wonder Palettes"))
            {
                if (ImGui.Button("Add Palette"))
                {
                    if (wonderPal is null) { wonderPal = new List<string>(); }
                    wonderPal.Add("");
                    areaParam.EnvPaletteSetting.WonderPaletteList = wonderPal;
                }

                if (wonderPal is not null)
                {
                    for (int i = 0; i < wonderPal.Count; i++)
                    {
                        var palette = wonderPal[i];
                        if (ImGui.InputText($"##WonderPalette{i}", ref palette, 1024))
                        {
                            wonderPal[i] = palette;
                        }
                    }
                }

                ImGui.TreePop();
            }

            List<string> eventPal = areaParam.EnvPaletteSetting.EventPaletteList;
            if (ImGui.TreeNode("Event Palettes"))
            {
                if (ImGui.Button("Add Palette"))
                {
                    if (eventPal is null) eventPal = new List<string>();
                    eventPal.Add("");
                    areaParam.EnvPaletteSetting.EventPaletteList = eventPal;
                }

                if (eventPal is not null)
                {
                    for (int i = 0; i < eventPal.Count; i++)
                    {
                        var palette = eventPal[i];
                        if (ImGui.InputText($"##EventPalette{i}", ref palette, 1024))
                        {
                            eventPal[i] = palette;
                        }
                    }
                }

                ImGui.TreePop();
            }
        }

        private static void DrawMusicSettings(AreaParam areaParam)
        {
            // TODO: Make this better by maybe loading these from a json
            ImGui.SeparatorText("Normal Music");
            {
                if (ImGui.BeginTable("##NormalMusicParam", 2))
                {
                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);

                    // BgmType
                    ImGui.Text("Background Track");
                    ImGui.TableNextColumn();
                    var bgmType = areaParam.BgmType is null ? "" : areaParam.BgmType;
                    if (ImGui.InputText("##BgmType", ref bgmType, 1024))
                        areaParam.BgmType = bgmType;

                    ImGui.TableNextColumn();

                    // EnvironmentSound
                    ImGui.Text("Environment Sound");
                    ImGui.TableNextColumn();
                    var environmentSound = areaParam.EnvironmentSound is null ? "" : areaParam.EnvironmentSound;
                    if (ImGui.InputText("##EnvironmentSound", ref environmentSound, 1024))
                        areaParam.EnvironmentSound = environmentSound;

                    ImGui.TableNextColumn();

                    // EnvironmentSoundEfx
                    ImGui.Text("Environment Sound Effects");
                    ImGui.TableNextColumn();
                    var environmentSoundEfx = areaParam.EnvironmentSoundEfx is null ? "" : areaParam.EnvironmentSoundEfx;
                    if (ImGui.InputText("##EnvironmentSoundEfx", ref environmentSoundEfx, 1024))
                        areaParam.EnvironmentSoundEfx = environmentSoundEfx;

                    ImGui.EndTable();
                }

                // BgmInterlock
                var bgmInterlock = areaParam.BgmInterlock;
                if (ImGui.Checkbox("Background Music Interlock", ref bgmInterlock))
                    areaParam.BgmInterlock = bgmInterlock;
            }

            ImGui.SeparatorText("Wonder Music");
            {
                if (ImGui.BeginTable("##WonderMusic", 2))
                {
                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);

                    // WonderBgmType
                    ImGui.Text("Wonder Background Track");
                    ImGui.TableNextColumn();
                    var wonderBgmType = areaParam.WonderBgmType is null ? "" : areaParam.WonderBgmType;
                    if (ImGui.InputText("##WonderBgmType", ref wonderBgmType, 1024))
                        areaParam.WonderBgmType = wonderBgmType;

                    ImGui.TableNextColumn();

                    // WonderBgmStartOffset
                    ImGui.Text("Wonder Background Track Starting Delay");
                    ImGui.TableNextColumn();
                    var wonderBgmStartOffset = areaParam.WonderBgmStartOffset;
                    if (ImGui.InputFloat("##WonderBgmStartOffset", ref wonderBgmStartOffset))
                        areaParam.WonderBgmStartOffset = wonderBgmStartOffset;

                    ImGui.TableNextColumn();

                    // WonderBgmStartOffset
                    ImGui.Text("Wonder Environment Sound");
                    ImGui.TableNextColumn();
                    var wonderEnvironmentSound = areaParam.WonderEnvironmentSound is null ? "" : areaParam.WonderEnvironmentSound;
                    if (ImGui.InputText("##WonderEnvironmentSound", ref wonderEnvironmentSound, 1024))
                        areaParam.WonderEnvironmentSound = wonderEnvironmentSound;

                    ImGui.TableNextColumn();

                    // WonderBgmStartOffset
                    ImGui.Text("Wonder Environment Sound Effects");
                    ImGui.TableNextColumn();
                    var wonderEnvironmentSoundEfx = areaParam.WonderEnvironmentSoundEfx is null ? "" : areaParam.WonderEnvironmentSoundEfx;
                    if (ImGui.InputText("##WonderEnvironmentSoundEfx", ref wonderEnvironmentSoundEfx, 1024))
                        areaParam.WonderEnvironmentSoundEfx = wonderEnvironmentSoundEfx;

                    ImGui.TableNextColumn();

                    // WonderBgmEfx
                    ImGui.Text("Wonder Environment Sound");
                    ImGui.TableNextColumn();
                    var wonderBgmEfx = areaParam.WonderBgmEfx is null ? "" : areaParam.WonderBgmEfx;
                    if (ImGui.InputText("##WonderBgmEfx", ref wonderBgmEfx, 1024))
                        areaParam.WonderBgmEfx = wonderBgmEfx;

                    ImGui.TableNextColumn();

                    // WonderSEKeyForTag
                    ImGui.Text("Wonder Sound Effect Key for Tag");
                    ImGui.TableNextColumn();
                    var wonderSEKeyForTag = areaParam.WonderSEKeyForTag is null ? "" : areaParam.WonderSEKeyForTag;
                    if (ImGui.InputText("##WonderSEKeyForTag", ref wonderSEKeyForTag, 1024))
                        areaParam.WonderSEKeyForTag = wonderSEKeyForTag;

                    ImGui.EndTable();
                }

                // BgmInterlockOfWonder
                var bgmInterlockOfWonder = areaParam.BgmInterlockOfWonder;
                if (ImGui.Checkbox("Wonder Background Music Interlock", ref bgmInterlockOfWonder))
                    areaParam.BgmInterlockOfWonder = bgmInterlockOfWonder;
            }

            ImGui.SeparatorText("Misc. Sound Settings");
            {
                if (ImGui.BeginTable("##MiscMusic", 2))
                {
                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);

                    // BgmString
                    ImGui.Text("Background Music Settings String");
                    ImGui.TableNextColumn();
                    var bgmString = areaParam.BgmString is null ? "" : areaParam.BgmString;
                    if (ImGui.InputText("##BgmString", ref bgmString, 1024))
                        areaParam.BgmString = bgmString;

                    ImGui.TableNextColumn();

                    // ExternalSoundAsset
                    ImGui.Text("External Sound Asset");
                    ImGui.TableNextColumn();
                    var externalSoundAsset = areaParam.ExternalSoundAsset is null ? "" : areaParam.ExternalSoundAsset;
                    if (ImGui.InputText("##ExternalSoundAsset", ref externalSoundAsset, 1024))
                        areaParam.ExternalSoundAsset = externalSoundAsset;

                    ImGui.TableNextColumn();

                    // DynamicResolutionQuality
                    ImGui.Text("Dynamic Resolution Quality");
                    ImGui.TableNextColumn();
                    var dynamicResolutionQuality = areaParam.DynamicResolutionQuality is null ? "" : areaParam.DynamicResolutionQuality;
                    if (ImGui.InputText("##DynamicResolutionQuality", ref dynamicResolutionQuality, 1024))
                        areaParam.DynamicResolutionQuality = dynamicResolutionQuality;

                    ImGui.TableNextColumn();

                    // PlayerRhythmJumpTiming
                    ImGui.Text("Player Rhythm Jump Timing");
                    ImGui.TableNextColumn();
                    var playerRhythmJumpTiming = areaParam.PlayerRhythmJumpTiming is null ? "" : areaParam.PlayerRhythmJumpTiming;
                    if (ImGui.InputText("##PlayerRhythmJumpTiming", ref playerRhythmJumpTiming, 1024))
                        areaParam.PlayerRhythmJumpTiming = playerRhythmJumpTiming;

                    ImGui.TableNextColumn();

                    // PlayerRhythmJumpBadgeTiming
                    ImGui.Text("Player Rhythm Jump Badge Timing");
                    ImGui.TableNextColumn();
                    var playerRhythmJumpBadgeTiming = areaParam.PlayerRhythmJumpBadgeTiming is null ? "" : areaParam.PlayerRhythmJumpBadgeTiming;
                    if (ImGui.InputText("##PlayerRhythmJumpBadgeTiming", ref playerRhythmJumpBadgeTiming, 1024))
                        areaParam.PlayerRhythmJumpBadgeTiming = playerRhythmJumpBadgeTiming;

                    ImGui.TableNextColumn();

                    // ExternalRhythmPatternSet
                    ImGui.Text("External Rhythm Pattern Set");
                    ImGui.TableNextColumn();
                    var externalRhythmPatternSet = areaParam.ExternalRhythmPatternSet is null ? "" : areaParam.ExternalRhythmPatternSet;
                    if (ImGui.InputText("##ExternalRhythmPatternSet", ref externalRhythmPatternSet, 1024))
                        areaParam.ExternalRhythmPatternSet = externalRhythmPatternSet;

                    ImGui.EndTable();
                }

                // UseMetalicPlayerSoundAsset
                var useMetalicPlayerSoundAsset = areaParam.UseMetalicPlayerSoundAsset;
                if (ImGui.Checkbox("Use Metalic Player Sound Asset", ref useMetalicPlayerSoundAsset))
                    areaParam.UseMetalicPlayerSoundAsset = useMetalicPlayerSoundAsset;

                // IsNeedCallWaterInSE
                var isNeedCallWaterInSE = areaParam.IsNeedCallWaterInSE;
                if (ImGui.Checkbox("Water-In Sound Effect Call needed", ref isNeedCallWaterInSE))
                    areaParam.IsNeedCallWaterInSE = isNeedCallWaterInSE;

                // IsNeedCallWaterInSE
                var isNotCallWaterEnvSE = areaParam.IsNotCallWaterEnvSE;
                if (ImGui.Checkbox("Don't call Water Environment Sound Effect", ref isNotCallWaterEnvSE))
                    areaParam.IsNotCallWaterEnvSE = isNotCallWaterEnvSE;
            }
        }

        private static void DrawMiscSettings(AreaParam areaParam)
        {
            if (ImGui.BeginTable("##MiscSettings", 2))
            {
                ImGui.TableNextRow();
                ImGui.TableSetColumnIndex(0);

                // BadgeMedleyEquipBadgeId
                ImGui.Text("Badge Medley Equip Badge ID");
                ImGui.TableNextColumn();
                var badgeMedleyEquipBadgeId = areaParam.BadgeMedleyEquipBadgeId is null ? "" : areaParam.BadgeMedleyEquipBadgeId;
                if (ImGui.InputText("##BadgeMedleyEquipBadgeId", ref badgeMedleyEquipBadgeId, 1024))
                    areaParam.BadgeMedleyEquipBadgeId = badgeMedleyEquipBadgeId;

                ImGui.TableNextColumn();

                // BackGroundAreaType
                ImGui.Text("Background Area Type");
                ImGui.TableNextColumn();
                var backGroundAreaType = areaParam.BackGroundAreaType is null ? "" : areaParam.BackGroundAreaType;
                if (ImGui.InputText("##BackGroundAreaType", ref backGroundAreaType, 1024))
                    areaParam.BackGroundAreaType = backGroundAreaType;

                ImGui.TableNextColumn();

                // EnvSetName
                ImGui.Text("Environment Set Name");
                ImGui.TableNextColumn();
                var envSetName = areaParam.EnvSetName is null ? "" : areaParam.EnvSetName;
                if (ImGui.InputText("##EnvSetName", ref envSetName, 1024))
                    areaParam.EnvSetName = envSetName;

                ImGui.EndTable();
            }

            // IsWaterArea
            var isWaterArea = areaParam.IsWaterArea;
            if (ImGui.Checkbox("Is Water Area", ref isWaterArea))
                areaParam.IsWaterArea = isWaterArea;

            // IsKoopaJr04Area
            var isKoopaJr04Area = areaParam.IsKoopaJr04Area;
            if (ImGui.Checkbox("Is Bowser Jr. 04 Fight Area", ref isKoopaJr04Area))
                areaParam.IsKoopaJr04Area = isKoopaJr04Area;

            // IsInvisibleDeadLine
            var isInvisibleDeadLine = areaParam.IsInvisibleDeadLine;
            if (ImGui.Checkbox("Is Invisible Death Line", ref isInvisibleDeadLine))
                areaParam.IsInvisibleDeadLine = isInvisibleDeadLine;

            // IsResetMarkerFlag
            var isResetMarkerFlag = areaParam.IsResetMarkerFlag;
            if (ImGui.Checkbox("Is Reset Marker Flag", ref isResetMarkerFlag))
                areaParam.IsResetMarkerFlag = isResetMarkerFlag;

            // IsVisibleOnlySameWonderPlayer
            var isVisibleOnlySameWonderPlayer = areaParam.IsVisibleOnlySameWonderPlayer;
            if (ImGui.Checkbox("Is Visible Only Same Wonder Player", ref isVisibleOnlySameWonderPlayer))
                areaParam.IsVisibleOnlySameWonderPlayer = isVisibleOnlySameWonderPlayer;

            // IsVisibleOnlySameWonderPlayer
            var isSetListenerCenter = areaParam.IsSetListenerCenter;
            if (ImGui.Checkbox("Is Set Listener Center", ref isSetListenerCenter))
                areaParam.IsSetListenerCenter = isSetListenerCenter;
        }
    }
}
