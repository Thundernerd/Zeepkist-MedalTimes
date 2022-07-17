using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore;

namespace TNRD.Zeepkist.MedalTimes
{
    public static class TextMeshProUtilities
    {
        private static PropertyInfo versionProperty =
            typeof(TMP_SpriteAsset).GetProperty("version", BindingFlags.Instance | BindingFlags.Public);

        private static PropertyInfo spriteCharacterTableProperty =
            typeof(TMP_SpriteAsset).GetProperty("spriteCharacterTable", BindingFlags.Instance | BindingFlags.Public);
        
        private static PropertyInfo spriteGlyphTableProperty =
            typeof(TMP_SpriteAsset).GetProperty("spriteGlyphTable", BindingFlags.Instance | BindingFlags.Public);

        public static TMP_SpriteAsset CreateSpriteAsset(Sprite sprite)
        {
            // Create new Sprite Asset
            TMP_SpriteAsset spriteAsset = ScriptableObject.CreateInstance<TMP_SpriteAsset>();

            spriteAsset.name = sprite.name + "SpriteAsset";
            versionProperty.SetValue(spriteAsset, "1.1.0");
            // spriteAsset.version = "1.1.0";

            // Compute the hash code for the sprite asset.
            spriteAsset.hashCode = TMP_TextUtilities.GetSimpleHashCode(spriteAsset.name);

            List<TMP_SpriteGlyph> spriteGlyphTable = new List<TMP_SpriteGlyph>();
            List<TMP_SpriteCharacter> spriteCharacterTable = new List<TMP_SpriteCharacter>();

            PopulateSpriteTables(sprite, ref spriteCharacterTable, ref spriteGlyphTable);

            spriteCharacterTableProperty.SetValue(spriteAsset, spriteCharacterTable);
            // spriteAsset.spriteCharacterTable = spriteCharacterTable;

            spriteGlyphTableProperty.SetValue(spriteAsset, spriteGlyphTable);
            // spriteAsset.spriteGlyphTable = spriteGlyphTable;

            // Add new default material for sprite asset.
            AddDefaultMaterial(spriteAsset);
            
            spriteAsset.UpdateLookupTables();

            return spriteAsset;
        }

        private static void PopulateSpriteTables(
            Sprite sprite,
            ref List<TMP_SpriteCharacter> spriteCharacterTable,
            ref List<TMP_SpriteGlyph> spriteGlyphTable
        )
        {
            TMP_SpriteGlyph spriteGlyph = new TMP_SpriteGlyph();
            spriteGlyph.index = 0;
            spriteGlyph.metrics = new GlyphMetrics(sprite.rect.width,
                sprite.rect.height,
                -sprite.pivot.x,
                sprite.rect.height - sprite.pivot.y,
                sprite.rect.width);
            spriteGlyph.glyphRect = new GlyphRect(sprite.rect);
            spriteGlyph.scale = 1.0f;
            spriteGlyph.sprite = sprite;

            spriteGlyphTable.Add(spriteGlyph);

            TMP_SpriteCharacter spriteCharacter = new TMP_SpriteCharacter(0xFFFE, spriteGlyph);
            spriteCharacter.name = sprite.name;
            spriteCharacter.scale = 1.0f;

            spriteCharacterTable.Add(spriteCharacter);
        }

        private static void AddDefaultMaterial(TMP_SpriteAsset spriteAsset)
        {
            Shader shader = Shader.Find("TextMeshPro/Sprite");
            Material material = new Material(shader);
            material.SetTexture(ShaderUtilities.ID_MainTex, spriteAsset.spriteSheet);

            spriteAsset.material = material;
            material.hideFlags = HideFlags.HideInHierarchy;
        }
    }
}
