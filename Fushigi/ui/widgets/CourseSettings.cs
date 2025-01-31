using Fushigi.course;
using Fushigi.ui.modal;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Fushigi.ui.widgets
{
    internal class CourseSettings
    {
        private static readonly Dictionary<string, string> Difficulty = new Dictionary<string, string>()
        {
            { "Easy", "1 Star" },
            { "Normal", "2 Stars" },
            { "Hard", "3 Stars" },
            { "VeryHard", "4 Stars" },
            { "ExtraHard", "5 Stars" },
            { "None", "None" },
        };

        private static readonly Dictionary<string, string> PlayerMorphType = new Dictionary<string, string>()
        {
            { "", "None" },
            { "Slime", "Wubba" },
            { "Ball", "Spike Ball" },
            { "BalloonKiller", "Balloon" },
            { "Kuribo", "Goomba" },
            { "SinkBlock", "Puffy Lift" },
            { "Hoppin", "Hoppycat" },
            { "Biyon", "Sproing/Stretch" },
        };

        private static readonly Dictionary<string, string> CourseKind = new Dictionary<string, string>()
        {
            { "Normal", "Normal" },
            { "BadgeChallenge", "Badge Challenge" },
            { "BadgeHouse", "Badge House" },
            { "BadgeMedley", "Badge Medley" },
            { "Mini", "Break Time" },
            { "Bonus", "Coin Bonus" },
            { "Arena", "KO Arena" },
            { "NormalSpWorld", "Normal (Special World)" },
            { "Opening", "Opening" },
            { "DemoCourse", "Demo Course" },
            { "GeneralFacility", "Poplin House (General)" },
            { "PlayableFacility", "Poplin House (Playable)" },
            { "SecretSquare", "Search Party" },
            { "StaffCredit", "Staff Credit" },
            { "StroyTeller", "Story Teller" },
            { "Race", "Wiggler Race" },
            { "WonderPalace", "Wonder Palace" },
        };

        private static readonly Dictionary<string, string> CourseTimerType = new Dictionary<string, string>()
        {
            { "", "None" },
            { "ArenaTimer", "Area Type Timer" },
        };

        private static readonly Dictionary<string, string> DemoCourseKind = new Dictionary<string, string>()
        {
            { "", "None" },
            { "StoryTeller", "Story Teller" },
        };

        public static void Draw(ref bool continueDisplay, IPopupModalHost modalHost, CourseInfo courseInfo, MapAnalysisInfo mapAnalysisInfo, StageLoadInfo stageLoadInfo)
        {
            ImGui.SetNextWindowSize(new Vector2(500, 500), ImGuiCond.Once);

            // Window
            if (ImGui.Begin("Course Settings", ImGuiWindowFlags.NoDocking | ImGuiWindowFlags.NoCollapse))
            {
                // Close Button
                if (ImGui.Button("Close"))
                {
                    continueDisplay = false;

                    /// Remove empty strings from lists
                    //  from Suggest Badge List
                    var suggestBadgeList = courseInfo.SuggestBadgeList;
                    if (suggestBadgeList != null)
                    {
                        for (int i = 0; i < suggestBadgeList.Count; i++)
                            if (suggestBadgeList[i] == "") suggestBadgeList.Remove(suggestBadgeList[i]);

                        if (suggestBadgeList.Count == 0) suggestBadgeList = null;
                    }

                    courseInfo.SuggestBadgeList = suggestBadgeList;

                    //  from Tips Tags
                    var tipsTags = courseInfo.TipsTags;
                    if (tipsTags != null)
                    {
                        for (int i = 0; i < tipsTags.Count; i++)
                            if (tipsTags[i] == "") tipsTags.Remove(tipsTags[i]);

                        if (tipsTags.Count == 0) tipsTags = null;
                    }

                    courseInfo.TipsTags = tipsTags;

                    //  from Tips Infos
                    var tipsInfo = courseInfo.TipsInfo;
                    if (tipsInfo != null)
                    {
                        for (int i = 0; i < tipsInfo.Count; i++)
                            if ((tipsInfo[i].Cond == "" || tipsInfo[i].Cond is null) && tipsInfo[i].Label == "" || tipsInfo[i].Label is null) tipsInfo.Remove(tipsInfo[i]);

                        if (tipsInfo.Count == 0) tipsInfo = null;
                    }

                    courseInfo.TipsInfo = tipsInfo;

                }

                // Setting Tabs
                if (ImGui.BeginTabBar("CourseSettingsTypes", ImGuiTabBarFlags.None))
                {
                    if (ImGui.BeginTabItem("Level Settings"))
                    {
                        DrawLevelInfoSettings(courseInfo, mapAnalysisInfo, stageLoadInfo);
                        ImGui.EndTabItem();
                    }

                    if (ImGui.BeginTabItem("World Map Appearance"))
                    {
                        DrawWorldMapAppearanceSettings(courseInfo);
                        ImGui.EndTabItem();
                    }
                    ImGui.EndTabBar();
                }

                ImGui.End();
            }

        }

        private static void DrawWorldMapAppearanceSettings(CourseInfo courseInfo)
        {
            if (ImGui.BeginTable("##WorldMapAppearanceSettings", 2))
            {
                ImGui.TableNextRow();
                ImGui.TableSetColumnIndex(0);

                // CourseDifficulty
                {
                    ImGui.Text("Course Difficulty");
                    ImGui.TableNextColumn();

                    var courseDifficulty = courseInfo.CourseDifficulty is null ? "" : courseInfo.CourseDifficulty;
                    int index = Difficulty.Keys.ToList().IndexOf(courseDifficulty);

                    if (ImGui.Combo("##CourseDifficulty", ref index, Difficulty.Values.ToArray(), Difficulty.Count(), 6))
                        courseInfo.CourseDifficulty = Difficulty.Keys.ToArray()[index];

                    ImGui.SetItemTooltip("Difficulty of the Level.");

                    ImGui.TableNextColumn();
                }

                // CourseNameLabel
                {
                    ImGui.Text("Course Name Label");
                    ImGui.TableNextColumn();

                    var courseNameLabel = courseInfo.CourseNameLabel is null ? "" : courseInfo.CourseNameLabel;

                    if (ImGui.InputText("##CourseNameLabel", ref courseNameLabel, 1024))
                        courseInfo.CourseNameLabel = courseNameLabel;

                    ImGui.SetItemTooltip("Entry of the MSBT file containing this course's name.");

                    ImGui.TableNextColumn();
                }

                // CourseScreenCaptureMainActor
                {
                    ImGui.Text("Main Actor on Thumbnail");
                    ImGui.TableNextColumn();

                    var courseScreenCaptureMainActor = courseInfo.CourseScreenCaptureMainActor is null ? "" : courseInfo.CourseScreenCaptureMainActor;

                    if (ImGui.InputText("##CourseScreenCaptureMainActor", ref courseScreenCaptureMainActor, 1024))
                        courseInfo.CourseScreenCaptureMainActor = courseScreenCaptureMainActor;

                    ImGui.SetItemTooltip("Path to the actor file of one of the actors displayed on the course thumbnail.");

                    ImGui.TableNextColumn();
                }

                // CourseThumbnailPath
                {
                    ImGui.Text("Thumbnail Path");
                    ImGui.TableNextColumn();

                    var courseThumbnailPath = courseInfo.CourseThumbnailPath is null ? "" : courseInfo.CourseThumbnailPath;

                    if (ImGui.InputText("##CourseThumbnailPath", ref courseThumbnailPath, 1024))
                        courseInfo.CourseThumbnailPath = courseThumbnailPath;

                    ImGui.SetItemTooltip("Path to the course thumbnail.");

                    ImGui.TableNextColumn();
                }

                // CourseThumbnailPath
                {
                    ImGui.Text("Suggested Badge Replacement Label");
                    ImGui.TableNextColumn();

                    var suggestBadgeReplaceLabel = courseInfo.SuggestBadgeReplaceLabel is null ? "" : courseInfo.SuggestBadgeReplaceLabel;

                    if (ImGui.InputText("##CourseThumbnailPath", ref suggestBadgeReplaceLabel, 1024))
                        courseInfo.SuggestBadgeReplaceLabel = suggestBadgeReplaceLabel;
                }

                ImGui.EndTable();
            }

            // SuggestBadgeList
            {
                var suggestBadgeList = courseInfo.SuggestBadgeList;

                if (ImGui.TreeNode("Suggested Badge List"))
                {
                    if (ImGui.Button("Add Badge"))
                    {
                        if (suggestBadgeList is null) suggestBadgeList = new List<string>();
                        suggestBadgeList.Add("");
                        courseInfo.SuggestBadgeList = suggestBadgeList;
                    }

                    if (suggestBadgeList is not null)
                    {
                        for (int i = 0; i < suggestBadgeList.Count; i++)
                        {
                            var badge = suggestBadgeList[i];
                            if (ImGui.InputText($"##SuggestBadgeList{i}", ref badge, 1024))
                                suggestBadgeList[i] = badge;
                        }
                    }

                    ImGui.TreePop();
                }
            }

            // TipTags
            {
                var tipTags = courseInfo.TipsTags;
                if (ImGui.TreeNode("Tip Tags"))
                {
                    if (ImGui.Button("Add Tip Tag"))
                    {
                        if (tipTags is null) tipTags = new List<string>();
                        tipTags.Add("");
                        courseInfo.TipsTags = tipTags;
                    }

                    if (tipTags is not null)
                    {
                        for (int i = 0; i < tipTags.Count; i++)
                        {
                            var tipTag = tipTags[i];
                            if (ImGui.InputText($"##TipTags{i}", ref tipTag, 1024))
                                tipTags[i] = tipTag;
                        }
                    }

                    ImGui.TreePop();
                }
            }

            // TipsInfo
            {
                var tipsInfos = courseInfo.TipsInfo;
                if (ImGui.TreeNode("Tip Infos"))
                {
                    if (ImGui.Button("Add Tip Info"))
                    {
                        if (tipsInfos is null) tipsInfos = new List<CourseInfo.TipInfo>();
                        tipsInfos.Add(new CourseInfo.TipInfo());
                        courseInfo.TipsInfo = tipsInfos;
                    }

                    if (ImGui.BeginTable("##TipsInfo", 2))
                    {
                        ImGui.TableNextRow();
                        ImGui.TableSetColumnIndex(0);

                        ImGui.Text("Condition");
                        ImGui.TableNextColumn();
                        ImGui.Text("Label");

                        if (tipsInfos != null && tipsInfos.Count > 0)
                        {
                            for (int i = 0; i < tipsInfos.Count; i++)
                            {
                                ImGui.TableNextColumn();

                                var tipsInfoCond = tipsInfos[i].Cond is null ? "" : tipsInfos[i].Cond;
                                var tipsInfoLabel = tipsInfos[i].Label is null ? "" : tipsInfos[i].Label;
                                if (ImGui.InputText($"##TipsInfoCond{i}", ref tipsInfoCond, 1024))
                                    tipsInfos[i].Cond = tipsInfoCond;

                                ImGui.TableNextColumn();

                                if (ImGui.InputText($"##TipsInfoLabel{i}", ref tipsInfoLabel, 1024))
                                    tipsInfos[i].Label = tipsInfoLabel;
                            }
                        }

                        ImGui.EndTable();
                    }
                }
            }
        }

        private static void DrawLevelInfoSettings(CourseInfo courseInfo, MapAnalysisInfo mapAnalysisInfo, StageLoadInfo stageLoadInfo)
        {
            if (ImGui.BeginTable("##LevelInfoSettings", 2))
            {
                ImGui.TableNextRow();
                ImGui.TableSetColumnIndex(0);

                // GlobalCourseId
                {
                    ImGui.Text("Global Course ID");
                    ImGui.TableNextColumn();

                    var globalCourseId = courseInfo.GlobalCourseId;

                    if (ImGui.InputInt("##GlobalCourseId", ref globalCourseId))
                        courseInfo.GlobalCourseId = globalCourseId;

                    ImGui.TableNextColumn();
                }

                ImGui.EndTable();
            }

            ImGui.Separator();

            if (ImGui.BeginTable("##LevelInfoSettings2", 2))
            {
                ImGui.TableNextRow();
                ImGui.TableSetColumnIndex(0);


                // CourseKind
                {
                    ImGui.Text("Course Type");
                    ImGui.TableNextColumn();

                    var courseKind = courseInfo.CourseKind is null ? "" : courseInfo.CourseKind;
                    int indexCourseType = courseKind == "" ? 0 : CourseKind.Keys.ToList().IndexOf(courseKind);

                    if (ImGui.Combo("##CourseType", ref indexCourseType, CourseKind.Values.ToArray(), CourseKind.Count(), 10))
                        courseInfo.CourseKind = CourseKind.Keys.ToArray()[indexCourseType];

                    ImGui.TableNextColumn();
                }

                // DemoCourseKind
                {
                    ImGui.Text("Demo Course Type");
                    ImGui.TableNextColumn();

                    var demoCourseKind = courseInfo.DemoCourseKind is null ? "" : courseInfo.DemoCourseKind;
                    int indexDemoCourseType = demoCourseKind == "Invalid" ? 0 : DemoCourseKind.Keys.ToList().IndexOf(demoCourseKind);

                    if (ImGui.Combo("##DemoCourseKind", ref indexDemoCourseType, DemoCourseKind.Values.ToArray(), DemoCourseKind.Count(), 5))
                        courseInfo.DemoCourseKind = DemoCourseKind.Keys.ToArray()[indexDemoCourseType];

                    ImGui.TableNextColumn();
                }

                // RaceCourseType
                {
                    ImGui.Text("Race Course Type");
                    ImGui.TableNextColumn();
                    var raceCourseType = courseInfo.RaceCourseType is null ? "" : courseInfo.RaceCourseType;
                    if (ImGui.InputText("##RaceCourseType", ref raceCourseType, 1024))
                    {
                        courseInfo.RaceCourseType = raceCourseType;
                    }

                    ImGui.TableNextColumn();
                }

                // StartEventName
                {
                    ImGui.Text("Level Intro Event");
                    ImGui.TableNextColumn();
                    var startEventName = stageLoadInfo.StartEventName is null ? "" : stageLoadInfo.StartEventName;
                    if (ImGui.InputText("##CourseStartXLinkKey", ref startEventName, 1024))
                    {
                        stageLoadInfo.StartEventName = startEventName;
                    }

                    ImGui.TableNextColumn();
                }

                ImGui.EndTable();
            }

            ImGui.Separator();

            if (ImGui.BeginTable("##LevelInfoSettings4", 2))
            {
                ImGui.TableNextRow();
                ImGui.TableSetColumnIndex(0);

                // CourseTimer
                {
                    ImGui.Text("Course Timer");
                    ImGui.TableNextColumn();

                    var courseTimer = courseInfo.CourseTimer;

                    if (ImGui.InputInt("##CourseTimer", ref courseTimer))
                        courseInfo.CourseTimer = courseTimer;

                    ImGui.SetItemTooltip("Unused course timer.");

                    ImGui.TableNextColumn();
                }

                // CourseTimerType
                {
                    ImGui.Text("Course Timer Type");
                    ImGui.TableNextColumn();

                    var courseTimerType = courseInfo.CourseTimerType is null ? "" : courseInfo.CourseTimerType;
                    int indexTimerType = courseTimerType == "Invalid" ? 0 : CourseTimerType.Keys.ToList().IndexOf(courseTimerType);

                    if (ImGui.Combo("##CoursePlayerMorphType", ref indexTimerType, CourseTimerType.Values.ToArray(), CourseTimerType.Count(), 5))
                        courseInfo.CourseTimerType = CourseTimerType.Keys.ToArray()[indexTimerType];
                }

                ImGui.EndTable();
            }

            // IsCourseTimerAutoStart
            {
                var isCourseTimerAutoStart = courseInfo.IsCourseTimerAutoStart;

                if (ImGui.Checkbox("Auto Start Course Timer", ref isCourseTimerAutoStart))
                    courseInfo.IsCourseTimerAutoStart = isCourseTimerAutoStart;
            }

            ImGui.Separator();

            // IsExistBigTenLuckyCoin
            {
                var isExistTenLuckyCoin = mapAnalysisInfo.IsExistBigTenLuckyCoin;

                if (ImGui.Checkbox("Course has purple 10-Coin", ref isExistTenLuckyCoin))
                    mapAnalysisInfo.IsExistBigTenLuckyCoin = isExistTenLuckyCoin;
            }

            // IsExistBlockSurprise
            {
                var isExistBlockSurprise = mapAnalysisInfo.IsExistBlockSurprise;

                if (ImGui.Checkbox("Course has yellow !-Blocks", ref isExistBlockSurprise))
                    mapAnalysisInfo.IsExistBlockSurprise = isExistBlockSurprise;
            }

            // IsExistGoalPole
            {
                var isExistGoalPole = mapAnalysisInfo.IsExistGoalPole;

                if (ImGui.Checkbox("Course has Goal Pole", ref isExistGoalPole))
                    mapAnalysisInfo.IsExistGoalPole = isExistGoalPole;
            }

            // IsExistTreasureChest
            {
                var isExistTreasureChest = mapAnalysisInfo.IsExistTreasureChest;

                if (ImGui.Checkbox("Course has Teasure Chest", ref isExistTreasureChest))
                    mapAnalysisInfo.IsExistTreasureChest = isExistTreasureChest;
            }

            // IsUnlimitedBadge
            {
                var isUnlimitedBadge = mapAnalysisInfo.IsUnlimitedBadge;

                if (ImGui.Checkbox("Is Unlimited Badge", ref isUnlimitedBadge))
                    mapAnalysisInfo.IsUnlimitedBadge = isUnlimitedBadge;

                ImGui.SetItemTooltip("Never enabled.");
            }

            // IsDashMiniCourse
            {
                var isDashMiniCourse = courseInfo.IsDashMiniCourse;

                if (ImGui.Checkbox("Is Dash Break Time! Course", ref isDashMiniCourse))
                    courseInfo.IsDashMiniCourse = isDashMiniCourse;
            }

            ImGui.Separator();

            if (ImGui.BeginTable("##LevelInfoSettings5", 2))
            {
                ImGui.TableNextRow();
                ImGui.TableSetColumnIndex(0);

                // CoursePlayerMorphType
                {
                    ImGui.Text("Player Wonder Morph");
                    ImGui.TableNextColumn();

                    var coursePlayerMorphType = courseInfo.CoursePlayerMorphType is null ? "" : courseInfo.CoursePlayerMorphType;
                    int index = PlayerMorphType.Keys.ToList().IndexOf(coursePlayerMorphType);

                    if (ImGui.Combo("##CoursePlayerMorphType", ref index, PlayerMorphType.Values.ToArray(), PlayerMorphType.Count(), PlayerMorphType.Count()))
                        courseInfo.CoursePlayerMorphType = PlayerMorphType.Keys.ToArray()[index];

                    ImGui.SetItemTooltip("What the player turns into during the Wonder effect.");

                    ImGui.TableNextColumn();
                }

                // WonderFlowerNum
                {
                    ImGui.Text("Number of Wonder Seeds");
                    ImGui.TableNextColumn();

                    var wonderFlowerNum = mapAnalysisInfo.WonderFlowerNum;

                    if (ImGui.InputInt("##WonderFlowerNum", ref wonderFlowerNum))
                        mapAnalysisInfo.WonderFlowerNum = wonderFlowerNum;

                    ImGui.TableNextColumn();
                }

                // FinishWonderFlowerNum
                {
                    ImGui.Text("Number of Wonder Seeds from Wonder Flowers");
                    ImGui.TableNextColumn();

                    var finishWonderFlowerNum = mapAnalysisInfo.FinishWonderFlowerNum;

                    if (ImGui.InputInt("##FinishWonderFlowerNum", ref finishWonderFlowerNum))
                        mapAnalysisInfo.FinishWonderFlowerNum = finishWonderFlowerNum;

                    ImGui.TableNextColumn();
                }

                ImGui.EndTable();
            }

            // IsExistWonderFinishFlower
            {
                var isExistWonderFinishFlower = mapAnalysisInfo.IsExistWonderFinishFlower;

                if (ImGui.Checkbox("Course has Wonder Flower", ref isExistWonderFinishFlower))
                    mapAnalysisInfo.IsExistWonderFinishFlower = isExistWonderFinishFlower;
            }

            // IsExistWonderQuiz
            {
                var isExistWonderQuiz = courseInfo.IsExistWonderQuiz;

                if (ImGui.Checkbox("Course has Quiz Wonder", ref isExistWonderQuiz))
                    courseInfo.IsExistWonderQuiz = isExistWonderQuiz;
            }

            ImGui.Separator();

            if (ImGui.BeginTable("##LevelInfoSettings6", 2))
            {
                ImGui.TableNextRow();
                ImGui.TableSetColumnIndex(0);

                // CourseStartXLinkKey
                {
                    ImGui.Text("Course Start X Link Key");
                    ImGui.TableNextColumn();
                    var courseStartXLinkKey = courseInfo.CourseStartXLinkKey is null ? "" : courseInfo.CourseStartXLinkKey;
                    if (ImGui.InputText("##CourseStartXLinkKey", ref courseStartXLinkKey, 1024))
                    {
                        courseInfo.CourseStartXLinkKey = courseStartXLinkKey;
                    }

                    ImGui.TableNextColumn();
                }

                ImGui.EndTable();
            }

            ImGui.Separator();

            if (ImGui.BeginTable("##LevelInfoSettings7", 2))
            {
                ImGui.TableNextRow();
                ImGui.TableSetColumnIndex(0);

                // BadgeId
                {
                    ImGui.Text("Badge ID");
                    ImGui.TableNextColumn();
                    var badgeId = mapAnalysisInfo.BadgeId is null ? "" : mapAnalysisInfo.BadgeId;
                    if (ImGui.InputText("##BadgeId", ref badgeId, 1024))
                    {
                        mapAnalysisInfo.BadgeId = badgeId;
                    }

                    ImGui.SetItemTooltip("Only used in Course 500 (Badge House) and Course 590 (WONDER?).");

                    ImGui.TableNextColumn();
                }

                // NeedBadgeIdEnterCourse
                {
                    ImGui.Text("Need Badge ID Enter Course");
                    ImGui.TableNextColumn();

                    var needBadgeIdEnterCourse = courseInfo.NeedBadgeIdEnterCourse is null ? "" : courseInfo.NeedBadgeIdEnterCourse;

                    if (ImGui.InputText("##NeedBadgeIdEnterCourse", ref needBadgeIdEnterCourse, 1024))
                        courseInfo.NeedBadgeIdEnterCourse = needBadgeIdEnterCourse;

                    ImGui.TableNextColumn();
                }

                // GiveBadgeIdOnCourseClear
                {
                    ImGui.Text("Give Badge ID on Course Clear");
                    ImGui.TableNextColumn();

                    var giveBadgeIdOnCourseClear = courseInfo.GiveBadgeIdOnCourseClear is null ? "" : courseInfo.GiveBadgeIdOnCourseClear;

                    if (ImGui.InputText("##GiveBadgeIdOnCourseClear", ref giveBadgeIdOnCourseClear, 1024))
                        courseInfo.GiveBadgeIdOnCourseClear = giveBadgeIdOnCourseClear;

                    ImGui.TableNextColumn();
                }

                // UnlockBadgeIdOnCourseClear
                {
                    ImGui.Text("Unlock Badge ID on Course Clear");
                    ImGui.TableNextColumn();

                    var unlockBadgeIdOnCourseClear = courseInfo.UnlockBadgeIdOnCourseClear is null ? "" : courseInfo.UnlockBadgeIdOnCourseClear;

                    if (ImGui.InputText("##UnlockBadgeIdOnCourseClear", ref unlockBadgeIdOnCourseClear, 1024))
                        courseInfo.UnlockBadgeIdOnCourseClear = unlockBadgeIdOnCourseClear;

                    ImGui.TableNextColumn();
                }

                ImGui.EndTable();
            }

            // NoNeedRetrySuggestBadge
            {
                var noNeedRetrySuggestBadge = courseInfo.NoNeedRetrySuggestBadge;

                if (ImGui.Checkbox("Don't suggest Badge before Retry", ref noNeedRetrySuggestBadge))
                    courseInfo.NoNeedRetrySuggestBadge = noNeedRetrySuggestBadge;
            }

            // IsInvisibleBadgeSetShadow
            {
                var isInvisibleBadgeSetShadow = courseInfo.IsInvisibleBadgeSetShadow;

                if (ImGui.Checkbox("Set Shadow on Invisibility Badge", ref isInvisibleBadgeSetShadow))
                    courseInfo.IsInvisibleBadgeSetShadow = isInvisibleBadgeSetShadow;
            }

            ImGui.Separator();

            // IsUseTheEndUI
            {
                var isUseTheEndUI = courseInfo.IsUseTheEndUI;

                if (ImGui.Checkbox("Use The End UI", ref isUseTheEndUI))
                    courseInfo.IsUseTheEndUI = isUseTheEndUI;
            }
        }
    }
}
