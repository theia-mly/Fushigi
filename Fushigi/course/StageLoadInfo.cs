using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Fushigi.Byml;
using Fushigi.Byml.Serializer;
using Fushigi.rstb;
using Fushigi.util;

namespace Fushigi.course
{
    [Serializable]
    public class StageLoadInfo : BymlObject
    {
        public string StartEventName { get; set; }

        public StageLoadInfo(string name)
        {
            var stageLoadInfoFilePath = FileUtil.FindContentPath(Path.Combine("Stage", "StageLoadInfo", $"{name}.game__stage__StageLoadInfo.bgyml"));
            var byml = new Byml.Byml(new MemoryStream(File.ReadAllBytes(stageLoadInfoFilePath)));

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

            //Compress and save the stage load info           
            string levelPath = Path.Combine(folder, $"{courseName}.game__stage__StageLoadInfo.bgyml");
            File.WriteAllBytes(levelPath, mem.ToArray());

            //Update resource table
            // filePath is a key not an actual path so we cannot use Path.Combine
            resource_table.SetResource($"Stage/StageLoadInfo/{courseName}.game__stage__StageLoadInfo.bgyml", decomp_size);
        }
    }
}
