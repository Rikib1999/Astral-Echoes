using static UnityEngine.Tilemaps.Tile;

namespace Assets.Scripts.Planet
{
    public class PlanetPaletteBag
    {
        public PlanetPaletteLayer[] Palette { get; }

        public PlanetPaletteBag(PlanetPalette planetPalette)
        {
            Palette = new PlanetPaletteLayer[planetPalette.tileList.Length];

            for (int i = 0; i < planetPalette.tileList.Length; i++)
            {
                Palette[i] = new PlanetPaletteLayer
                {
                    tiles = planetPalette.tileList[i].tiles,
                    level = planetPalette.levels[i]
                };

                foreach (var tile in Palette[i].tiles)
                {
                    tile.colliderType = ColliderType.None; // planetPalette.isWalkable[i] ? ColliderType.None : ColliderType.Sprite;
                }
            }
        }
    }
}