﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class RainEnchant : ModItem
    {        
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rain Enchantment");
            Tooltip.SetDefault(
@"Grants immunity to Wet
A miniature storm will appear after heavily damaging enemies
Shooting it will make it grow
At maximum size, attacks will turn into lightning bolts
'Come again some other day'");
            DisplayName.AddTranslation(GameCulture.Chinese, "云雨魔石");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 6;
            item.value = 150000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.buffImmune[BuffID.Wet] = true;
            player.GetModPlayer<FargoPlayer>().RainEnchant = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.RainHat);
            recipe.AddIngredient(ItemID.RainCoat);
            recipe.AddIngredient(ItemID.UmbrellaHat);
            //inner tube
            recipe.AddIngredient(ItemID.Umbrella);
            //tragic umbrella
            recipe.AddIngredient(ItemID.NimbusRod);
            recipe.AddIngredient(ItemID.WaterGun);
            recipe.AddIngredient(ItemID.RainbowBrick, 50);
            //volt bunny pet

            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
