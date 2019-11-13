using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.BardItems;
using ThoriumMod.Items.NPCItems;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class FossilEnchant : EnchantmentItem
    {
        public const string TOOLTIP =
            @"'Beyond a forgotten age'
If you reach zero HP you cheat death, returning with 20 HP
For a few seconds after reviving, you are immune to all damage and spawn bones
Summons a pet Baby Dino";


        public FossilEnchant() : base("Fossil Enchantment", TOOLTIP, 20, 20,
            TileID.DemonAltar, Item.sellPrice(silver: 80), ItemRarityID.Green, new Color(140, 92, 59))
        {
        }


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.AddTranslation(GameCulture.Chinese, "化石魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'被遗忘的记忆'
血量为0时避免死亡, 以20点生命值重生
在复活后的几秒钟内, 免疫所有伤害, 并且可以产生骨头
召唤一只小恐龙");
        }


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().FossilEffect(10, hideVisual);
        }


        protected override void AddRecipeBase(ModRecipe recipe)
        {
            recipe.AddIngredient(ItemID.FossilHelm);
            recipe.AddIngredient(ItemID.FossilShirt);
            recipe.AddIngredient(ItemID.FossilPants);
            recipe.AddIngredient(ItemID.AntlionClaw);
            recipe.AddIngredient(ItemID.AmberStaff);
            recipe.AddIngredient(ItemID.BoneDagger, 300);

            recipe.AddIngredient(ItemID.AmberMosquito);
        }

        protected override void AddThoriumRecipe(ModRecipe recipe, Mod thorium)
        {
            recipe.AddIngredient(ModContent.ItemType<SeveredHand>(), 300);
            recipe.AddIngredient(ModContent.ItemType<Sitar>());

            recipe.AddIngredient(ItemID.BoneJavelin, 300);
        }
    }
}
