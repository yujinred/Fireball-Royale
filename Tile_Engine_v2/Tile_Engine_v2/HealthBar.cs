using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FireBall_Royale
{
    class HealthBar
    {
        private Texture2D hbBar;
        private int hX;
        private int hY;
        private int hWidth;
        private int hHeight;
        private Color hbColor;
        private int health = 0;

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public HealthBar(Texture2D bar, int x, int y, int width, int height, Color color)
        {
            hbBar = bar;
            hX = x;
            hY = y;
            hWidth = width;
            hHeight = height;
            hbColor = color;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(hbBar, new Rectangle(hX, hY, hWidth +health, hHeight), hbColor);
        }
    }
}
