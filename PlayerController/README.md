# PlayerController

The `PlayerController` script provides a comprehensive first-person character controller for Unity. It includes configurable settings for camera movement, player movement, sliding on slopes, jumping, and crouching.

## Features

- **Camera Control**: Smooth first-person camera movement with adjustable sensitivity and field of view (FOV).
- **Movement**: Configurable walking, sprinting, and crouching speeds.
- **Sliding**: Automatic sliding on steep slopes with configurable slide force and player input influence.
- **Jumping**: Configurable jump height, air control, and coyote time for better jump handling.
- **Crouching**: Smooth transition between standing and crouching with adjustable crouch height.

## Configuration

### Camera Settings

- **firstPersonCamera**: Reference to the first-person camera transform.
- **cameraSensitivity**: Sensitivity of the camera movement.
- **invertYAxis**: Invert the Y-axis for camera movement.
- **normalFOV**: Field of view for normal movement.
- **sprintFOV**: Field of view while sprinting.
- **fovTransitionSpeed**: Speed of transitioning between normal and sprint FOV.

### Movement Settings

- **walkSpeed**: Walking speed of the player.
- **sprintSpeed**: Sprinting speed of the player.
- **crouchSpeed**: Crouching speed of the player.
- **gravity**: Gravity force applied to the player.

### Slide Settings

- **maxClimbAngle**: Maximum angle the player can climb. If the angle is steeper, the player will slide down.
- **slideForce**: Force applied to the player when sliding down a slope.
- **playerInputInfluence**: Influence of player input on the sliding direction.

### Jump Settings

- **jumpHeight**: Height of the player's jump.
- **maxJumpTime**: Maximum time the player can hold the jump button to jump higher.
- **airControlFactor**: Control factor for the player while in the air.
- **coyoteTime**: Time allowed for the player to jump after leaving the ground (coyote time).
- **jumpCooldown**: Cooldown time between jumps.

### Crouch Settings

- **crouchHeight**: Height of the player while crouching.
- **crouchTransitionSpeed**: Speed of transitioning between standing and crouching.

## Usage

1. Attach the `PlayerController` script to a GameObject with a `CharacterController` component.
2. Assign the `firstPersonCamera` in the Inspector.
3. Adjust the settings in the Inspector to fit your game's requirements.

## Example

Here's an example of how to set up the `PlayerController` in the Unity Editor:

1. Create an empty GameObject and name it "Player".
2. Add a `CharacterController` component to the "Player" GameObject.
3. Attach the `PlayerController` script to the "Player" GameObject.
4. Create a new Camera and set its position and rotation to match the player's viewpoint.
5. Assign the Camera to the `firstPersonCamera` field in the `PlayerController` script.
6. Adjust the various settings in the Inspector to configure the player's movement, camera, sliding, jumping, and crouching behavior.

## Notes

- Ensure the `CharacterController` component is properly configured with the desired height, center, and radius.
- The script assumes the player will be using a first-person perspective.
