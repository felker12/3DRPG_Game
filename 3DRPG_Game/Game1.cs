using GameEngine.GameEntities.Sprites;
using GameEngine.GameWorld;
using GameEngine.PrimitiveShapes3d;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _3DRPG_Game
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Player Player { get; private set; }
        public OrbitalCamera Camera { get; private set; }
        private PrimitiveCube _playerCube;


        Model knight;

        private Vector3 _cameraTarget = Vector3.Zero;

        private Matrix _world = Matrix.Identity;

        KeyboardState _keyboardState;
        MouseState _mouseState;

        public static int ScreenWidth { get; set; } = 1280;
        public static int ScreenHeight { get; set; } = 720;
        public float DeltaTime { get; private set; }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = ScreenWidth,
                PreferredBackBufferHeight = ScreenHeight,
            };
            _graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Player = new Player(new Vector3(0, 0, 0));
            _playerCube = new PrimitiveCube(GraphicsDevice);
            Camera = new OrbitalCamera(GraphicsDevice.Viewport.AspectRatio);
        }

        protected override void Initialize()
        {
            base.Initialize();

            _keyboardState = Keyboard.GetState();
            _mouseState = Mouse.GetState();

            // Scale matrix used for the static Knight model in the world
            _world = Matrix.CreateScale(0.01f);

            knight = Content.Load<Model>("Models/Knight");

            var boneTransforms = new Matrix[knight.Bones.Count];
            knight.CopyAbsoluteBoneTransformsTo(boneTransforms);

            BoundingSphere? bounds = null;
            foreach (ModelMesh mesh in knight.Meshes)
            {
                BoundingSphere meshBounds = mesh.BoundingSphere
                    .Transform(boneTransforms[mesh.ParentBone.Index] * _world);

                bounds = bounds.HasValue
                    ? BoundingSphere.CreateMerged(bounds.Value, meshBounds)
                    : meshBounds;
            }

            float approximatePlayerRadius = bounds.HasValue ? bounds.Value.Radius : 2.0f;

            // Direct the camera initialization framework to look at the player position
            _cameraTarget = Player.Position;

            Camera.BindCameraDistanceLimits(approximatePlayerRadius * 0.5f, approximatePlayerRadius * 15f);
            Camera.SetCameraDistance(approximatePlayerRadius * 4f); // Back up slightly for a clean initial view
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _keyboardState = Keyboard.GetState();
            _mouseState = Mouse.GetState();

            Camera.HandleInput(_keyboardState, _mouseState, DeltaTime);
            Player.Update(_keyboardState, Camera, DeltaTime);
            Camera.UpdateCamera(Player.Position, Vector3.Up);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Add Drawing code here
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            Matrix playerWorldMatrix = Player.GetWorldMatrix();

            _playerCube.Draw(playerWorldMatrix, Camera.View, Camera.Projection);

            knight.Draw(_world, Camera.View, Camera.Projection);

            base.Draw(gameTime);
        }

        public Texture2D CreateTextureFromColor(Color color)
        {
            Texture2D texture = new(GraphicsDevice, 1, 1);
            texture.SetData([color]);

            return texture;
        }
    }
}
