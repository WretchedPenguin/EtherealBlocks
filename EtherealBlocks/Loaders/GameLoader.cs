using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pipliz;

namespace EtherealBlocks
{
    [ModLoader.ModManager]
    public static class GameLoader
    {
        [ModLoader.ModCallback(ModLoader.EModCallbackType.OnAssemblyLoaded, ModInfo.Namespace + ".OnAssemblyLoaded")]
        public static void OnAssemblyLoaded(string path)
        {
            ModInfo.ModFolder = Path.GetDirectoryName(path);
            ModInfo.GamedataFolder = Path.Combine(ModInfo.ModFolder, "gamedata");
            ModInfo.LocalizationFolder = Path.Combine(ModInfo.GamedataFolder, "localization");
            ModInfo.IconFolder = Path.Combine(ModInfo.GamedataFolder, "icons");

            
            Log.Write("Hello there");
        }
    }
}
