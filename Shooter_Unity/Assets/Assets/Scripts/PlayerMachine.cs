using UnityEngine;
using System.Collections;

/*
 * Example implementation of the SuperStateMachine and SuperCharacterController
 */
[RequireComponent(typeof(SuperCharacterController))]
[RequireComponent(typeof(PlayerInputController))]
public class PlayerMachine : SuperStateMachine {
    public Transform PlayerCamera;

    public float WalkSpeed = 4.0f;
    public float WalkAcceleration = 30.0f;
    public float JumpAcceleration = 5.0f;
    public float JumpHeight = 3.0f;
    public float Gravity = 25.0f;

    enum PlayerStates { Idle, Walk, Jump, Fall }

    private SuperCharacterController controller;

    // current velocity
    private Vector3 velocity;

    private PlayerInputController input;

	void Start () {
        input = gameObject.GetComponent<PlayerInputController>();
        controller = gameObject.GetComponent<SuperCharacterController>();
        currentState = PlayerStates.Idle;
	}

    protected override void EarlyGlobalSuperUpdate() {
       
    }

    protected override void LateGlobalSuperUpdate() {
        transform.position += velocity * Time.deltaTime;
    }

    // Very basic grounding function that ensures we are close enough for our "feet" to touch the ground
    // And that we are not standing right on the edge of a cliff
    private bool IsGrounded(float distance) {            
        if (controller.currentGround.Hit.distance > distance) return false;
        Vector3 projectedHitPoint = Math3d.ProjectPointOnPlane(Vector3.up, transform.position, controller.currentGround.Hit.point);
        if (Vector3.Distance(projectedHitPoint, transform.position) > controller.radius * 0.5f) return false;
        return true;
    }
	
	// More advanced grounding function. This takes into account that we may be standing on sloped
	// Ground, or walking from sloped ground onto flat surfaces. You'll want to modify this if you need
	// Specific grounding behaviour
	private bool IsGroundedAdvanced(float distance, bool currentlyGrounded) {
        if (controller.currentGround.Hit.distance > distance) return false;

        Vector3 n = controller.currentGround.FarHit.normal;
        float angle = Vector3.Angle(n, Vector3.up);

        if (angle > controller.currentGround.CollisionType.StandAngle) return false;

        float upperBoundAngle = 60.0f;
        float maxDistance = 0.96f;
        float minDistance = 0.50f;
        float angleRatio = angle / upperBoundAngle;
        float distanceRatio = Mathf.Lerp(minDistance, maxDistance, angleRatio);
        Vector3 p = Math3d.ProjectPointOnPlane(controller.up, transform.position, controller.currentGround.Hit.point);
        bool steady = Vector3.Distance(p, transform.position) <= distanceRatio * controller.radius;

        if (!steady) {
            if (!currentlyGrounded) return false;

            if (controller.currentGround.NearHit.distance < distance) {
                if (Vector3.Angle(controller.currentGround.NearHit.normal, controller.up) > controller.currentGround.CollisionType.StandAngle) return false;
            }
            else return false;
        }

        return true;
    }

    private bool AcquiringGround() {
        return IsGroundedAdvanced(0.01f, false);
    }

    private bool MaintainingGround() {
        return IsGroundedAdvanced(0.5f, true);
    }

    private Vector3 LocalMovement() {
		Vector3 right = PlayerCamera.transform.right;
		Vector3 forward = Quaternion.Euler(0, PlayerCamera.eulerAngles.y, 0) * Vector3.forward;
        Vector3 local = Vector3.zero;
        if (input.Current.MoveInput.x != 0) local += right * input.Current.MoveInput.x;
		if (input.Current.MoveInput.z != 0) local += forward * input.Current.MoveInput.z;

        return local;
    }

    private float CalculateJumpSpeed(float jumpHeight, float gravity) {
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }

    void Idle_EnterState() {
        controller.EnableSlopeLimit();
        controller.EnableClamping();
    }

    void Idle_SuperUpdate() {
        if (input.Current.JumpInput) {
            currentState = PlayerStates.Jump;
            return;
        }

        if (!MaintainingGround()) {
            currentState = PlayerStates.Fall;
            return;
        }

        if (input.Current.MoveInput != Vector3.zero) {
            currentState = PlayerStates.Walk;
            return;
        }

        velocity = Vector3.MoveTowards(velocity, Vector3.zero, 30.0f * Time.deltaTime);
    }

    void Idle_ExitState() {
        // Run once when we exit the idle state
    }

    void Walk_SuperUpdate() {
        if (input.Current.JumpInput) {
            currentState = PlayerStates.Jump;
            return;
        }

        if (!MaintainingGround()) {
            currentState = PlayerStates.Fall;
            return;
        }

        if (input.Current.MoveInput != Vector3.zero) {
            velocity = Vector3.MoveTowards(velocity, LocalMovement() * WalkSpeed, WalkAcceleration * Time.deltaTime);
        }
        else {
            currentState = PlayerStates.Idle;
            return;
        }
    }

    void Jump_EnterState() {
        controller.DisableClamping();
        controller.DisableSlopeLimit();

        velocity += Vector3.up * CalculateJumpSpeed(JumpHeight, Gravity);
    }

    void Jump_SuperUpdate() {
        Vector3 planarMoveDirection = Math3d.ProjectVectorOnPlane(Vector3.up, velocity);
        Vector3 verticalMoveDirection = velocity - planarMoveDirection;

        if (AcquiringGround()) {
            velocity = planarMoveDirection;
            currentState = PlayerStates.Idle;
            return;            
        }

        planarMoveDirection = Vector3.MoveTowards(planarMoveDirection, LocalMovement() * WalkSpeed, JumpAcceleration * Time.deltaTime);
        verticalMoveDirection -= Vector3.up * Gravity * Time.deltaTime;

        velocity = planarMoveDirection + verticalMoveDirection;
    }

    void Fall_EnterState() {
        controller.DisableClamping();
        controller.DisableSlopeLimit();
    }

    void Fall_SuperUpdate() {
        if (AcquiringGround()) {
            velocity = Math3d.ProjectVectorOnPlane(Vector3.up, velocity);
            currentState = PlayerStates.Idle;
            return;
        }

        velocity -= Vector3.up * Gravity * Time.deltaTime;
    }
}
