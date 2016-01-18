using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace FireBall_Royale
{
    class FogOfWar
    {
        Texture2D texFog;
        int iMapHeight;
        int iMapWidth;
        int iTileWidth;
        int iTileHeight;
        public float fStartFade; //the distance in tiles to where fog starts to apear from the player
        public float fEndFade; //the distance in tiles to where fog is completly dark
        public byte[,] by_ary_Fog;
        byte byRevealedTerrainFog;

        //calculation variable(s)
        float fDistance = 0f;
        byte byAlpha = 0;
        Color cDrawColor = Color.White;

        //constructor
        public FogOfWar(Texture2D par_texFog, int par_iMapWidth, int par_iMapHeight, int par_iTileWidth,
            int par_iTileHeight, float par_fStartFade, float par_fEndFade,
            byte par_byInitialFogDensity, byte par_byRevealedTerrainFog)
        {
            texFog = par_texFog;
            iMapWidth = par_iMapWidth;
            iMapHeight = par_iMapHeight;
            iTileWidth = par_iTileWidth;
            iTileHeight = par_iTileHeight;
            fStartFade = par_fStartFade;
            fEndFade = par_fEndFade;
            byRevealedTerrainFog = par_byRevealedTerrainFog;
            by_ary_Fog = new byte[iMapWidth, iMapHeight];

            for (int x = 0; x < iMapWidth; x++)
            {
                for (int y = 0; y < iMapHeight; y++)
                {
                    by_ary_Fog[x, y] = par_byInitialFogDensity;
                }
            }
        }

        public void Update(Vector2 par_vPlayer, int par_iStartXTile, int par_iXTiles, int par_iStartYTile, int par_iYTiles, bool par_bUpdateAll)
        {
            if (par_bUpdateAll)
            {
                for (int x = 0; x < iMapWidth; x++)
                {
                    for (int y = 0; y < iMapHeight; y++)
                    {
                        //find the distance from the player to the current block
                        fDistance = (new Vector2(x * iTileWidth + iTileWidth / 2, y * iTileHeight + iTileHeight / 2) - par_vPlayer).Length();
                        if (fDistance >= fEndFade * iTileWidth)
                        {
                            by_ary_Fog[x, y] = (byte)255;
                            continue;
                        }
                        else if (fDistance < fStartFade * iTileWidth)
                        {
                            by_ary_Fog[x, y] = (byte)0;
                            continue;
                        }
                        //apply our formula to calculate the proper alpha for this tile
                        by_ary_Fog[x, y] = (byte)(((fDistance - (fStartFade * iTileWidth)) * 255) / ((fEndFade * iTileWidth) - fStartFade * iTileWidth));
                    }
                }
            }
            else
            {
                for (int x = par_iStartXTile; x < par_iStartXTile + par_iXTiles; x++)
                {
                    for (int y = par_iStartYTile; y < par_iStartYTile + par_iYTiles; y++)
                    {
                        //find the distance from the player to the current block
                        fDistance = (new Vector2(x * iTileWidth + iTileWidth / 2, y * iTileHeight + iTileHeight / 2) - par_vPlayer).Length();

                        //apply our formula to calculate the proper alpha for this tile
                        byAlpha = (byte)(((fDistance - (fStartFade * 48)) * 255) / ((fEndFade * 48) - fStartFade * 48));
                        //if we are increasing the fog darkness
                        if (byAlpha > by_ary_Fog[x, y])
                        {
                            //if the fog is darkening past our darkness level for revealed terrain,
                            if (byAlpha > byRevealedTerrainFog)
                            {
                                //don't let this tile get any darker than revealed terrain
                                by_ary_Fog[x, y] = byRevealedTerrainFog;
                            }
                            else
                            {
                                //else we can go ahead and set the tile fog to the calculated amount as this tile is
                                //not too dark yet
                                by_ary_Fog[x, y] = byAlpha;
                            }
                        }
                        else //else if we are decreasing the fog darkness
                        {
                            //go ahead and decrease
                            by_ary_Fog[x, y] = byAlpha;
                        }
                    }
                }
            }

        }

        public void DrawFog(SpriteBatch spriteBatch, int par_iStartXTile, int par_iXTiles, int par_iStartYTile, int par_iYTiles, bool par_bDrawAll)
        {
            if (par_bDrawAll)
            {
                for (int x = 0; x < iMapWidth; x++)
                {
                    for (int y = 0; y < iMapHeight; y++)
                    {
                        cDrawColor.A = by_ary_Fog[x,y];
                        spriteBatch.Draw(texFog, new Vector2(x * iTileWidth, y * iTileHeight), cDrawColor);
                    }
                }
            }
            else
            {
                for (int x = par_iStartXTile; x < par_iStartXTile + par_iXTiles; x++)
                {
                    for (int y = par_iStartYTile; y < par_iStartYTile + par_iYTiles; y++)
                    {
                        spriteBatch.Draw(texFog, new Vector2(x * iTileWidth, y * iTileHeight), new Color(255f, 255f, 255f, (float)by_ary_Fog[x, y]));
                    }
                }
            }
        }
    }
}
