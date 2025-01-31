using Fushigi.Bfres;
using Fushigi.Byml;
using Fushigi.Byml.Serializer;
using Fushigi.env;
using Fushigi.rstb;
using Fushigi.util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Fushigi.course
{
    [Serializable]
    public class AreaParam : BymlObject
    {   
        public string EnvSetName { get; set; }
        public string BgmType { get; set; }
        public string WonderBgmType { get; set; }
        public float WonderBgmStartOffset { get; set; }
        public bool BgmInterlock { get; set; }
        public string EnvironmentSound { get; set; } 
        public string WonderEnvironmentSound { get; set; }
        public bool IsNotCallWaterEnvSE { get; set; }
        public string BackGroundAreaType { get; set; }
        public bool BgmInterlockOfWonder { get; set; }
        public string PlayerRhythmJumpTiming { get; set; }
        public string ExternalSoundAsset { get; set; }
        public string PlayerRhythmJumpBadgeTiming { get; set; }
        public string WonderEnvironmentSoundEfx { get; set; }
        public string EnvironmentSoundEfx { get; set; }
        public string DynamicResolutionQuality { get; set; }
        public bool IsInvisibleDeadLine { get; set; }
        public bool IsWaterArea { get; set; }
        public string WonderSEKeyForTag { get; set; }
        public bool IsNeedCallWaterInSE { get; set; }
        public bool IsVisibleOnlySameWonderPlayer { get; set; }
        public bool IsResetMarkerFlag { get; set; }
        public string WonderBgmEfx { get; set; }
        public string BgmString { get; set; }
        public bool UseMetalicPlayerSoundAsset { get; set; }
        public int RemotePlayerSEPriority { get; set; }
        public bool IsKoopaJr04Area { get; set; }
        public bool IsSetListenerCenter { get; set; }
        public string BadgeMedleyEquipBadgeId { get; set; }
        public string ExternalRhythmPatternSet { get; set; }

        public AreaSkinParam SkinParam { get; set; } = new AreaSkinParam();
        public AreaEnvPaletteSetting EnvPaletteSetting { get; set; } = new AreaEnvPaletteSetting();

        public AreaParam(Byml.Byml byml)
        {
            this.Load((BymlHashTable)byml.Root);
        }

        public void Save(RSTB resource_table, string folder, string areaName)
        {
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            BymlHashTable root = this.Serialize();

            var byml = new Byml.Byml(root);
            var mem = new MemoryStream();
            byml.Save(mem);

            var decomp_size = (uint)mem.Length;

            //Compress and save the course area           
            string levelPath = Path.Combine(folder, $"{areaName}.game__stage__AreaParam.bgyml");
            File.WriteAllBytes(levelPath, mem.ToArray());

            //Update resource table
            // filePath is a key not an actual path so we cannot use Path.Combine
            resource_table.SetResource($"Stage/AreaParam/{areaName}.game__stage__AreaParam.bgyml", decomp_size);
        }

        public void Save(RSTB resource_table, string areaName)
        {
            Save(resource_table, Path.Combine(UserSettings.GetModRomFSPath(), "Stage", "AreaParam"), areaName);
        }

        public bool ContainsParam(string param)
        {
            return ((BymlHashTable)this.HashTable).ContainsKey(param);
        }

        public object GetParam(BymlHashTable node, string paramName, string paramType)
        {
            switch (paramType)
            {
                case "String":
                    return ((BymlNode<string>)node[paramName]).Data;
                case "Bool":
                    return ((BymlNode<bool>)node[paramName]).Data;
                case "Float":
                    return ((BymlNode<float>)node[paramName]).Data;
            }

            return null;
        }

        public BymlHashTable GetRoot()
        {
            return (BymlHashTable)this.HashTable;
        }

        [Serializable]
        public class AreaEnvPaletteSetting
        {
            public string InitPaletteBaseName { get; set; }
            public List<string> WonderPaletteList { get; set; }
            public List<string> TransPaletteList { get; set; }
            public List<string> EventPaletteList { get; set; }

            public BymlHashTable Serialize()
            {
                BymlHashTable evnPaletteSettings = new();
                evnPaletteSettings.AddNode(BymlNodeId.String, BymlUtil.CreateNode<string>(InitPaletteBaseName), "InitPaletteBaseName");

                BymlArrayNode wonderPalette = new();
                BymlArrayNode transPalette = new();
                BymlArrayNode eventPalette = new();

                if (WonderPaletteList is not null && WonderPaletteList.Count > 0)
                {
                    foreach (string palette in WonderPaletteList)
                    {
                        if (palette is not null)
                            wonderPalette.AddNodeToArray(BymlUtil.CreateNode<string>(palette));
                    }

                    evnPaletteSettings.AddNode(BymlNodeId.Array, wonderPalette, "WonderPaletteList");
                }

                if (TransPaletteList is not null && TransPaletteList.Count > 0)
                {
                    foreach (string palette in TransPaletteList)
                    {
                        if (palette is not null)
                            transPalette.AddNodeToArray(BymlUtil.CreateNode<string>(palette));
                    }
                    evnPaletteSettings.AddNode(BymlNodeId.Array, transPalette, "TransPaletteList");
                }

                if (EventPaletteList is not null && EventPaletteList.Count > 0)
                {
                    foreach (string palette in EventPaletteList)
                    {
                        if (palette is not null)
                            eventPalette.AddNodeToArray(BymlUtil.CreateNode<string>(palette));
                    }
                    evnPaletteSettings.AddNode(BymlNodeId.Array, eventPalette, "EventPaletteList");
                }

                return evnPaletteSettings;
            }
        }

        [Serializable]
        public class AreaSkinParam
        {
            public bool DisableBgUnitDecoA { get; set; }
            public string FieldA { get; set; } = "";
            public string FieldB { get; set; } = "";
            public string Object { get; set; } = "";

            public BymlHashTable Serialize()
            {
                BymlHashTable skinParam = new();

                if (FieldA is not null && FieldA != "")
                    skinParam.AddNode(BymlNodeId.String, BymlUtil.CreateNode<string>(FieldA), "FieldA");

                if (FieldB is not null && FieldB != "")
                    skinParam.AddNode(BymlNodeId.String, BymlUtil.CreateNode<string>(FieldB), "FieldB");

                if (Object is not null && Object != "")
                    skinParam.AddNode(BymlNodeId.String, BymlUtil.CreateNode<string>(Object), "Object");

                skinParam.AddNode(BymlNodeId.Bool, BymlUtil.CreateNode<bool>(DisableBgUnitDecoA), "DisableBgUnitDecoA");

                return skinParam;
            }
        }
    }
}
