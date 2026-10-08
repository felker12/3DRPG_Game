using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace GameEngine.GameWorld;

public class Player
{
    // Transform parameters
    public Vector3 Position { get; private set; }
    public float Heading { get; private set; } // Yaw orientation in radians

    // Movement configurations
    private readonly float _moveSpeed = 8.0f;
    private readonly float _turnSpeed = 10.0f; // Interpolation speed for smooth turning

    public Player(Vector3 startingPosition)
    {
        Position = startingPosition;
        Heading = 0f;
    }

    public void Update(KeyboardState keyboardState, OrbitalCamera camera, float deltaTime)
    {
        // Gather directional input relative to the screen
        Vector3 movementInput = Vector3.Zero;

        if (keyboardState.IsKeyDown(Keys.W)) movementInput.Z += 1f; // Forward
        if (keyboardState.IsKeyDown(Keys.S)) movementInput.Z -= 1f; // Backward
        if (keyboardState.IsKeyDown(Keys.A)) movementInput.X -= 1f; // Left
        if (keyboardState.IsKeyDown(Keys.D)) movementInput.X += 1f; // Right

        // Only move and rotate if there is active directional input
        if (movementInput != Vector3.Zero)
        {
            movementInput.Normalize();

            // Extract camera vectors and flatten them onto the XZ (ground) plane
            Vector3 camForward = camera.Position - Position; // Vector pointing back from target to camera
            camForward.Y = 0; // Flatten
            if (camForward != Vector3.Zero) camForward.Normalize();

            // Since camera looks *at* the player, camForward points backward. Flip it for actual forward.
            Vector3 playerForward = -camForward;
            Vector3 playerRight = Vector3.Cross(playerForward, Vector3.Up);

            // Translate screen-relative input into 3D world-space directions
            Vector3 moveDirection = (playerForward * movementInput.Z) + (playerRight * movementInput.X);
            if (moveDirection != Vector3.Zero) moveDirection.Normalize();

            // Update Position
            Position += moveDirection * _moveSpeed * deltaTime;

            // Smoothly rotate character toward the moving direction
            float targetHeading = (float)Math.Atan2(moveDirection.X, moveDirection.Z);

            // Handle radian wrapping seamlessly for smooth lerp interpolation
            float angleDifference = MathHelper.WrapAngle(targetHeading - Heading);
            Heading += angleDifference * _turnSpeed * deltaTime;
        }
    }

    public Matrix GetWorldMatrix()
    {
        // Generates the transform matrix required to render the 3D player mesh
        return Matrix.CreateRotationY(Heading) * Matrix.CreateTranslation(Position);
    }
}