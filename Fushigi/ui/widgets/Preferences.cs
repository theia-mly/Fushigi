﻿using Fushigi.gl;
using Fushigi.param;
using Fushigi.ui.modal;
using Fushigi.util;
using ImGuiNET;
using System.Numerics;

namespace Fushigi.ui.widgets
{
    class Preferences
    {
        static readonly Vector4 errCol = new Vector4(1f, 0, 0, 1);
        static bool romfsTouched = false;
        static bool modRomfsTouched = false;

        public static void Draw(ref bool continueDisplay, GLTaskScheduler glTaskScheduler,
            IPopupModalHost modalHost)
        {
            ImGui.SetNextWindowSize(new Vector2(700, 300), ImGuiCond.Once);
            if (ImGui.Begin("Preferences", ImGuiWindowFlags.NoDocking))
            {
                var romfs = UserSettings.GetRomFSPath();
                var mod = UserSettings.GetModRomFSPath();
                var renderCustomModels = UserSettings.GetRenderCustomModels();
                var useGameShaders = UserSettings.UseGameShaders();
                var useAstcTextureCache = UserSettings.UseAstcTextureCache();
                var hideDeletingLinkedActorsPopup = UserSettings.HideDeletingLinkedActorsPopup();
                var useNewCamera = UserSettings.GetUseNewCamera();
                var backupFreqMinutes = UserSettings.GetBackupFreqMinutes();

                ImGui.Indent();

                if (PathSelector.Show(
                    "RomFS Game Path",
                    ref romfs,
                    RomFS.IsValidRoot(romfs))
                    )
                {
                    romfsTouched = true;
 
                    UserSettings.SetRomFSPath(romfs);

                    if (!RomFS.IsValidRoot(romfs))
                    {
                        return;
                    }

                    Task.Run(async () =>
                    {
                        await ProgressBarDialog.ShowDialogForAsyncAction(modalHost,
                        $"Preloading Thumbnails",
                        async (p) =>
                        {
                            await glTaskScheduler.Schedule(gl => RomFS.SetRoot(romfs, gl));
                        });

                        ChildActorParam.Load();

                        /* if our parameter database isn't set, set it */
                        if (!ParamDB.sIsInit)
                        {
                            await MainWindow.LoadParamDBWithProgressBar(modalHost);
                        }
                    });
                    
                }

                Tooltip.Show("The game files which are stored under the romfs folder.\nIf you are using v1.0.1 of Super Mario Bros. Wonder, use a RomFS Game Path with v65536 files in it.");

                if (romfsTouched && !RomFS.IsValidRoot(romfs))
                {
                    ImGui.TextColored(errCol,
                        "The path you have selected is invalid. Please select a RomFS path that contains BancMapUnit, Model, and Stage.");
                }

                if (PathSelector.Show("Save Directory", ref mod, !string.IsNullOrEmpty(mod)))
                {
                    modRomfsTouched = true;

                    UserSettings.SetModRomFSPath(mod);
                }   

                Tooltip.Show("The save output where to save modified romfs files");

                if (modRomfsTouched && string.IsNullOrEmpty(mod))
                {
                    ImGui.TextColored(errCol,
                        "The path you have selected is invalid. Directory must not be empty.");
                }

                if (ImGui.Checkbox("Use Game Shaders", ref useGameShaders))
                {
                    UserSettings.SetGameShaders(useGameShaders);
                }

                Tooltip.Show("Displays models using the shaders present in the game. This may cause a performance drop but will look more visually accurate.");

                if (ImGui.Checkbox("Render Models from mod RomFS", ref renderCustomModels))
                {
                    UserSettings.SetRenderCustomModels(renderCustomModels);
                }

                Tooltip.Show("Uses the models from the mod directory. WARNING: Rendering of custom models using game shaders is broken.");

                if (ImGui.Checkbox("Use Astc Texture Cache", ref useAstcTextureCache))
                {
                    UserSettings.SetAstcTextureCache(useAstcTextureCache);
                }

                Tooltip.Show("Saves ASTC textures to disk which takes up disk space, but improves loading times and ram usage significantly.");

                if (ImGui.Checkbox("Hide Deleting Linked Actors Popup", ref hideDeletingLinkedActorsPopup))
                {
                    UserSettings.SetHideDeletingLinkedObjectsPopup(hideDeletingLinkedActorsPopup);
                }

                Tooltip.Show("Hides the warning popup when you delete actors with links.");

                if (ImGui.Checkbox("Use New Camera [BETA!]", ref useNewCamera))
                    UserSettings.SetUseNewCamera(useNewCamera);

                Tooltip.Show("Uses a new camera system that aims to be more accurate.\nWARNING: in beta and might cause some issues");

                if (ImGui.InputFloat("Backup Frequency (in minutes)", ref backupFreqMinutes))
                    UserSettings.SetBackupFreqMinutes(backupFreqMinutes);

                Tooltip.Show("How long between each backup, in minutes.\nBackups are stored wherever Fushigi is installed to.");

                ImGui.Unindent();

                if (ImGui.Button("Close"))
                {
                    continueDisplay = false;
                }

                ImGui.End();
            }
        }
    }
}
