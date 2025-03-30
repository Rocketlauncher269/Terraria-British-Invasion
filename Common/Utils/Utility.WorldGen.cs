using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.WorldBuilding;
using static Terraria.WorldGen;
namespace BritishInvasion.Common.Utils
{
    public static partial class Utility
    {
        public static bool CoordinatesOutOfBounds(int i, int j) => !InWorld(i, j);

        #region BaseMod BaseWorldGen

        //------------------------------------------------------//
        //------------------- BASE WORLDGEN --------------------//
        //------------------------------------------------------//
        // Contains methods for generating various things into  //
        // the world.                                           //
        //------------------------------------------------------//
        //  Author(s): Grox the Great                           //
        //------------------------------------------------------//

        public static Tile GetTileSafely(Vector2 position)
        {
            return GetTileSafely((int)(position.X / 16f), (int)(position.Y / 16f));
        }

        public static Tile GetTileSafely(int x, int y)
        {
            if (x < 0 || x > Main.maxTilesX || y < 0 || y > Main.maxTilesY)
                return new Tile();
            return Framing.GetTileSafely(x, y);
        }
        #endregion
    }
}