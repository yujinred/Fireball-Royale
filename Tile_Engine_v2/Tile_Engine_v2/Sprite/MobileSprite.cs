using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FireBall_Royale
{
    class MobileSprite
    {
        // The SpriteAnimation object that holds the graphical and animation data for this object
        SpriteAnimation asSprite;

        // Determine the status of the sprite.  An inactive sprite will not be updated but will be drawn.
        bool bActive = true;

        // If true, the sprite will be drawn to the screen
        bool bVisible = true;

        public SpriteAnimation Sprite
        {
            get { return asSprite; }
        }

        public Vector2 Position
        {
            get { return asSprite.Position; }
            set { asSprite.Position = value; }
        }

        public bool IsVisible
        {
            get { return bVisible; }
            set { bVisible = value; }
        }

        public bool IsActive
        {
            get { return bActive; }
            set { bActive = value; }
        }

        public Rectangle BoundingBox
        {
            get { return asSprite.BoundingBox; }
        }

        public MobileSprite(Texture2D texture)
        {
            asSprite = new SpriteAnimation(texture);
        }

        public void Update(GameTime gameTime)
        {

            if (bActive)
                asSprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (bVisible)
            {
                asSprite.Draw(spriteBatch, 0, 0);
            }
        }
    }
}
