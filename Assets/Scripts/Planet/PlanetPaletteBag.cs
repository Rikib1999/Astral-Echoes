using static UnityEngine.Tilemaps.Tile;

namespace Assets.Scripts.Planet
{
    public class PlanetPaletteBag
    {
        public PlanetPaletteLayer[] Palette { get; }

        public PlanetPaletteBag(PlanetPalette planetPalette)
        {
            Palette = new PlanetPaletteLayer[planetPalette.tiles.Length];

            for (int i = 0; i < planetPalette.tiles.Length; i++)
            {
                Palette[i] = new PlanetPaletteLayer
                {
                    tile = planetPalette.tiles[i],
                    level = planetPalette.levels[i]
                };

                Palette[i].tile.m_DefaultColliderType = planetPalette.isWalkable[i] ? ColliderType.None : ColliderType.Sprite;
            }
        }
    }
}