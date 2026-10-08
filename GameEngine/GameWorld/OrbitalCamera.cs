using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace GameEngine.GameWorld;

public class OrbitalCamera
{
    private float _cameraDistance = 15f;
    private float _cameraYaw = 0f;
    private float _cameraPitch = MathHelper.ToRadians(45f);
    private float _minCameraDistance = 1f;
    private float _maxCameraDistance = 1000f;

    private readonly float _rotationSpeed = 1.5f;

    public Vector3 Position { get; set; }
    public Matrix View { get; private set; }
    public Matrix Projection { get; private set; }
    public (float min, float max) CameraDistanceLimits => (_minCameraDistance, _maxCameraDistance);
    public float GetCameraDistance() => _cameraDistance;
    public void SetCameraDistance(float distance)
    {
        _cameraDistance = MathHelper.Clamp(distance, _minCameraDistance, _maxCameraDistance);
    }

    public OrbitalCamera(float AspectRatio)
    {
        Projection = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.PiOver4,
            AspectRatio,
            0.1f,
            10000f
        );
    }

    public void UpdateCamera(Vector3 target, Vector3 up)
    {
        float x = _cameraDistance *
            (float)Math.Cos(_cameraPitch) *
            (float)Math.Sin(_cameraYaw);

        float y = _cameraDistance *
            (float)Math.Sin(_cameraPitch);

        float z = _cameraDistance *
            (float)Math.Cos(_cameraPitch) *
            (float)Math.Cos(_cameraYaw);

        Position = target + new Vector3(x, y, z);

        View = Matrix.CreateLookAt(
            Position,
            target,
            Vector3.Up
        );
    }

    public void HandleInput(KeyboardState keyboardState, MouseState mouseState, float deltaTime)
    {
        float zoomSpeed = _cameraDistance * 2f;

        if (keyboardState.IsKeyDown(Keys.Left))
            _cameraYaw -= _rotationSpeed * deltaTime;

        if (keyboardState.IsKeyDown(Keys.Right))
            _cameraYaw += _rotationSpeed * deltaTime;

        if (keyboardState.IsKeyDown(Keys.Up))
            _cameraPitch += _rotationSpeed * deltaTime;

        if (keyboardState.IsKeyDown(Keys.Down))
            _cameraPitch -= _rotationSpeed * deltaTime;

        if (keyboardState.IsKeyDown(Keys.PageUp) ||
            keyboardState.IsKeyDown(Keys.OemPlus) ||
            keyboardState.IsKeyDown(Keys.Add))
        {
            _cameraDistance -= zoomSpeed * deltaTime;
        }

        if (keyboardState.IsKeyDown(Keys.PageDown) ||
            keyboardState.IsKeyDown(Keys.OemMinus) ||
            keyboardState.IsKeyDown(Keys.Subtract))
        {
            _cameraDistance += zoomSpeed * deltaTime;
        }

        _cameraPitch = MathHelper.Clamp(
            _cameraPitch,
            MathHelper.ToRadians(5f),
            MathHelper.ToRadians(85f)
        );

        _cameraDistance = MathHelper.Clamp(
            _cameraDistance,
            _minCameraDistance,
            _maxCameraDistance
        );
    }

    public void BindCameraDistanceLimits(float minDistance, float maxDistance)
    {
        if (minDistance <= 0 || maxDistance < minDistance)
            throw new ArgumentOutOfRangeException(nameof(minDistance));

        _minCameraDistance = minDistance;
        _maxCameraDistance = maxDistance;

        SetCameraDistance(_cameraDistance);
    }
}
