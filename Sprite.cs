using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Tipsy_bartender
{
    public abstract class Sprite
    {
        public Texture2D texture;
        public Vector2 position;
        public float layer;


        public Sprite(Texture2D texture, Vector2 position, float layer)
        {
            this.texture = texture;
            this.position = position;
            this.layer = layer;
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
