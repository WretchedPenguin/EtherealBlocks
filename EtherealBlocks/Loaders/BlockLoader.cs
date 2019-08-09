using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Chatting;
using Pipliz;
using Pipliz.JSON;
using Recipes;
using UnityEngine;

namespace EtherealBlocks
{
    [ModLoader.ModManager]
    public static class BlockLoader
    {
        private static List<EtherealBlock> etherealBlocks = new List<EtherealBlock>();

        [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterAddingBaseTypes,
            ModInfo.Namespace + ".AfterAddingBaseTypes")]
        [ModLoader.ModCallbackDependsOn("pipliz.blocknpcs.addlittypes")]
        public static void AfterAddingBaseTypes(Dictionary<string, ItemTypesServer.ItemTypeRaw> items)
        {
            foreach (KeyValuePair<string, ItemTypesServer.ItemTypeRaw> item in items.ToList())
            {
                if (!item.Value.Categories?.Contains("decorative") ?? true)
                    continue;
                JSONNode etherealBlockNode = new JSONNode();
                etherealBlockNode.SetAs("isDestructible", item.Value.IsDestructible);
                etherealBlockNode.SetAs("isRotatable", item.Value.IsRotatable);
                etherealBlockNode.SetAs("icon", item.Value.Icon);
                etherealBlockNode.SetAs("isSolid", item.Value.IsSolid);
                etherealBlockNode.SetAs("isFertile", item.Value.IsFertile);
                etherealBlockNode.SetAs("isPlaceable", item.Value.IsPlaceable);
                etherealBlockNode.SetAs("needsBase", item.Value.NeedsBase);
                etherealBlockNode.SetAs("maxStackSize", item.Value.MaxStackSize);
                etherealBlockNode.SetAs("onRemoveAudio", item.Value.OnRemoveAudio);
                etherealBlockNode.SetAs("onPlaceAudio", item.Value.OnPlaceAudio);
                etherealBlockNode.SetAs("destructionTime", item.Value.DestructionTime);
                etherealBlockNode.SetAs("blocksPathing", "false");
                etherealBlockNode.SetAs("color", $"#{ColorUtility.ToHtmlStringRGBA(item.Value.Color)}");
                etherealBlockNode.SetAs("sideall", item.Value.SideAll);
                string name = "EtherealBlocks.Ethereal" + item.Value.name;
                var etherealBlock =
                    new ItemTypesServer.ItemTypeRaw(name, etherealBlockNode);
                items.Add(etherealBlock.name, etherealBlock);
                etherealBlocks.Add(new EtherealBlock
                {
                    OriginalBlockName = item.Value.name,
                    OriginalBlockIndex = item.Value.ItemIndex,
                    EtherealBlockName = name,
                    EtherealBlockIndex = etherealBlock.ItemIndex
                });
            }
        }

        [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterItemTypesDefined,
            ModInfo.Namespace + ".AfterItemTypesDefined")]
        public static void AfterItemTypesDefined()
        {
            foreach (EtherealBlock block in etherealBlocks)
            {
                InventoryItem etherealBlockRequirements = new InventoryItem(block.OriginalBlockIndex, 1);
                InventoryItem originalBlockRequirements = new InventoryItem(block.EtherealBlockIndex, 1);

                Recipe originalToEtherealRecipe = new Recipe(block.EtherealBlockName, etherealBlockRequirements,
                    new RecipeResult(block.EtherealBlockIndex, 1));
                Recipe etherealToOriginalRecipe = new Recipe(block.OriginalBlockName, originalBlockRequirements,
                    new RecipeResult(block.OriginalBlockIndex, 1));

                ServerManager.RecipeStorage.AddPlayerRecipe(originalToEtherealRecipe);
                ServerManager.RecipeStorage.AddPlayerRecipe(etherealToOriginalRecipe);
            }
        }

        private struct EtherealBlock
        {
            public string OriginalBlockName { get; set; }
            public ushort OriginalBlockIndex { get; set; }

            public string EtherealBlockName { get; set; }
            public ushort EtherealBlockIndex { get; set; }
        }
    }
}