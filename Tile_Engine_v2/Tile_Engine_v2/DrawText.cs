using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FireBall_Royale
{
    class DrawText
    {
        private SpriteFont font;
        private Vector2 position;

        public void LoadContent(ContentManager content, string assetName)
        {
            font = content.Load<SpriteFont>(assetName);
        }

        public void DrawString(SpriteBatch sb, string text, int x, int y, Color textColor)
        {
            int layer;
            position = new Vector2(x,y);

            Color backColor = new Color(0,0,0,20);
            for (layer = 0; layer < 10; layer++)
            {
                sb.DrawString(font, text, position, backColor);
                position.X++;
                position.Y++;
            }

            backColor = new Color(190, 190, 190);
            for (layer = 0; layer < 5; layer++)
            {
                sb.DrawString(font, text, position, backColor);
                position.X++;
                position.Y++;
            }

            sb.DrawString(font, text, position, textColor);
        }
    }
}
