using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fushigi.Byml;
using Fushigi.Byml.Serializer;
using Fushigi.rstb;
using Fushigi.util;

namespace Fushigi.course
{
    [Serializable]
    public class MapAnalysisInfo : BymlObject
    {
        public string BadgeId { get; set; } //
        public int FinishWonderFlowerNum { get; set; }
        public bool IsExistBigTenLuckyCoin { get; set; }
        public bool IsExistBlockSurprise { get; set; }
        public bool IsExistGoalPole { get; set; }
        public bool IsExistTreasureChest { get; set; }
        public bool IsExistWonderFinishFlower { get; set; }
        public bool IsUnlimitedBadge { get; set; }
        public int WonderFlowerNum { get; set; }

        public MapAnalysisInfo(string name)
        {
            var mapAnalysisInfoFilePath = FileUtil.FindContentPath(Path.Combine("Stage", "MapAnalysisInfo", $"{name}.game__stage__MapAnalysisInfo.bgyml"));
            var byml = new Byml.Byml(new MemoryStream(File.ReadAllBytes(mapAnalysisInfoFilePath)));

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

            //Compress and save the map analysis info           
            string levelPath = Path.Combine(folder, $"{courseName}.game__stage__MapAnalysisInfo.bgyml");
            File.WriteAllBytes(levelPath, mem.ToArray());

            //Update resource table
            // filePath is a key not an actual path so we cannot use Path.Combine
            resource_table.SetResource($"Stage/MapAnalysisInfo/{courseName}.game__stage__MapAnalysisInfo.bgyml", decomp_size);
        }
    }
}
