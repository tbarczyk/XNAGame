using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;



using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace XnaTut
{
    class Weight
    {
        Texture2D Texture;
        public Rectangle Rectangle;
        public Vector2 Position;
        public float Gravity;

        public Weight(Texture2D texture, Rectangle rectangle, Vector2 position, float gravity)
        {
            Texture = texture;
            Rectangle = rectangle;
            Position = position;
            Gravity = gravity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color.White);
        }
    }
}
