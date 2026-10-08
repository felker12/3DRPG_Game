using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.GameEntities.Sprites
{
    public class Sprite
    {
        public Texture2D Texture { get; set; }
        public Color DefaultColor { get; set; } = Color.White;
        public Color SpriteColor { get; set; } = Color.White;
        public virtual Vector2 Position { get; set; } = new();
        public float Speed { get; set; } = 1.5f;
        public int Width { get; set; } = 32;
        public int Height { get; set; } = 32;
        public Rectangle Frame { get; set; } = new(0, 0, 32, 32);
        public float RotationAngle { get; set; } = 0.0f;
        public float Scale { get; set; } = 1.0f;

        public Rectangle Rectangle => new((int)Position.X, (int)Position.Y, Width, Height);
        public Vector2 SpriteCenter => new(Width / 2, Height / 2);
        public Vector2 Center => Position + SpriteCenter;

        public Sprite() { }

        public Sprite(Texture2D texture)
        {
            Texture = texture;
        }

        public Sprite(Sprite sprite)
        {
            Texture = sprite.Texture;
            DefaultColor = sprite.DefaultColor;
            SpriteColor = sprite.SpriteColor;
            Position = sprite.Position;
            Speed = sprite.Speed;
            Width = sprite.Width;
            Height = sprite.Height;
            Frame = sprite.Frame;
            RotationAngle = sprite.RotationAngle;
            Scale = sprite.Scale;
        }

        public virtual void Update(GameTime gameTime)
        {
            // Update logic for the sprite
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Center, Frame, SpriteColor,
               RotationAngle, SpriteCenter, Scale, SpriteEffects.None, 1);
        }

        public override string ToString()
        {
            return $"Sprite at Position: {Position}";
        }

        public virtual Sprite Clone()
        {
            return new Sprite(this);
        }
    }
}
