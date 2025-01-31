using Fushigi.Byml;
using Fushigi.Byml.Serializer;
using Fushigi.rstb;
using Fushigi.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fushigi.course
{
    [Serializable]
    public class CourseInfo : BymlObject
    {
        public string CourseDifficulty { get; set; } //
        public string CourseKind { get; set; } //
        public string CoursePlayerMorphType { get; set; } //
        public string CourseNameLabel { get; set; } //
        public string CourseScreenCaptureMainActor { get; set; } //
        public string CourseStartXLinkKey { get; set; } //
        public string CourseThumbnailPath { get; set; } //
        public int CourseTimer { get; set; } //
        public string CourseTimerType { get; set; } //
        public string DemoCourseKind { get; set; } //
        public string GiveBadgeIdOnCourseClear { get; set; } //
        public int GlobalCourseId { get; set; } //
        public bool IsCourseTimerAutoStart { get; set; } //
        public bool IsDashMiniCourse {  get; set; } //
        public bool IsExistWonderQuiz { get; set; } //
        public bool IsInvisibleBadgeSetShadow { get; set; } //
        public bool IsUseTheEndUI { get; set; } //
        public string NeedBadgeIdEnterCourse { get; set; } //
        public bool NoNeedRetrySuggestBadge { get; set; } //
        public string RaceCourseType { get; set; } //
        public List<string> SuggestBadgeList { get; set; } //
        public string SuggestBadgeReplaceLabel { get; set; } //
        public List<string> TipsTags { get; set; } //
        public List<TipInfo> TipsInfo { get; set; } //
        public string UnlockBadgeIdOnCourseClear { get; set; } //

        public CourseInfo(string name)
        {
            var courseFilePath = FileUtil.FindContentPath(Path.Combine("Stage", "CourseInfo", $"{name}.game__stage__CourseInfo.bgyml"));
            var byml = new Byml.Byml(new MemoryStream(File.ReadAllBytes(courseFilePath)));

            this.Load((BymlHashTable)byml.Root);
        }

        public void Save(RSTB resource_table, string folder, string courseName)
        {
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var root = this.Serialize();

            var byml = new Byml.Byml(root);
            var mem = new MemoryStream();
            byml.Save(mem);

            var decomp_size = (uint)mem.Length;

            //Compress and save the course area           
            string levelPath = Path.Combine(folder, $"{courseName}.game__stage__CourseInfo.bgyml");
            File.WriteAllBytes(levelPath, mem.ToArray());

            //Update resource table
            // filePath is a key not an actual path so we cannot use Path.Combine
            resource_table.SetResource($"Stage/CourseInfo/{courseName}.game__stage__CourseInfo.bgyml", decomp_size);
        }

        [Serializable]
        public class TipInfo
        {
            public string Cond { get; set; }
            public string Label { get; set; }
        }
    }
}
