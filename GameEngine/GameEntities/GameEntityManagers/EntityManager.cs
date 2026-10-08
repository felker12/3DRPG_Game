using GameEngine.GameEntities.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameEngine.GameEntities.GameEntityManagers
{
    public class EntityManager
    {
        private List<Sprite> Sprites { get; set; } = [];

        public EntityManager() { }

        public void Add(Sprite sprite)
        {
            Sprites.Add(sprite);
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var sprite in Sprites)
            {
                sprite.Draw(spriteBatch);
            }
        }
    }
}
