using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.PrimitiveShapes3d
{
    public class PrimitiveCube
    {
        private readonly GraphicsDevice _graphicsDevice;
        private VertexPositionColor[] _vertices;
        private short[] _indices;
        private BasicEffect _effect;

        public PrimitiveCube(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            InitializeCube();
            InitializeEffect();
        }

        private void InitializeCube()
        {
            // A cube has 8 corners. We define their positions and give them a default color.
            _vertices =
            [
            new (new Vector3(-0.5f, -0.5f,  0.5f), Color.DodgerBlue), // 0: Front-Bottom-Left
            new (new Vector3( 0.5f, -0.5f,  0.5f), Color.DodgerBlue), // 1: Front-Bottom-Right
            new (new Vector3( 0.5f,  0.5f,  0.5f), Color.DodgerBlue), // 2: Front-Top-Right
            new (new Vector3(-0.5f,  0.5f,  0.5f), Color.DodgerBlue), // 3: Front-Top-Left
            new (new Vector3(-0.5f, -0.5f, -0.5f), Color.SteelBlue),  // 4: Back-Bottom-Left
            new (new Vector3( 0.5f, -0.5f, -0.5f), Color.SteelBlue),  // 5: Back-Bottom-Right
            new (new Vector3( 0.5f,  0.5f, -0.5f), Color.SteelBlue),  // 6: Back-Top-Right
            new (new Vector3(-0.5f,  0.5f, -0.5f), Color.SteelBlue)   // 7: Back-Top-Left
            ];

            // 12 triangles make up the 6 faces of a cube (2 triangles per face)
            _indices =
            [
            0, 1, 2,  0, 2, 3, // Front Face
            1, 5, 6,  1, 6, 2, // Right Face
            5, 4, 7,  5, 7, 6, // Back Face
            4, 0, 3,  4, 3, 7, // Left Face
            3, 2, 6,  3, 6, 7, // Top Face
            4, 5, 1,  4, 1, 0  // Bottom Face
            ];
        }

        private void InitializeEffect()
        {
            _effect = new BasicEffect(_graphicsDevice)
            {
                VertexColorEnabled = true,
                LightingEnabled = true
            };

            // Add basic directional light so the cube faces look distinct when rotating
            _effect.DirectionalLight0.Enabled = true;
            _effect.DirectionalLight0.Direction = new Vector3(-1, -1, -1);
            _effect.DirectionalLight0.DiffuseColor = new Vector3(0.8f, 0.8f, 0.8f);
            _effect.AmbientLightColor = new Vector3(0.2f, 0.2f, 0.2f);
        }

        public void Draw(Matrix world, Matrix view, Matrix projection)
        {
            _effect.World = world;
            _effect.View = view;
            _effect.Projection = projection;

            // Apply shader passes
            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                // Submit primitive geometry data to the GPU
                _graphicsDevice.DrawUserIndexedPrimitives(
                    PrimitiveType.TriangleList,
                    _vertices, 0, _vertices.Length,
                    _indices, 0, _indices.Length / 3
                );
            }
        }
    }
}

