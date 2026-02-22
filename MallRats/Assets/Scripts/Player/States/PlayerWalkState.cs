namespace PlayerStates
{
    public class PlayerWalkState : PlayerState
    {
        private PlayerStateMachine _playerStateMachine;
        private PlayerController _playerController;

        public PlayerWalkState(PlayerStateMachine stateMachineContext, PlayerController playerController) : base(stateMachineContext, playerController)
        {
            this._playerStateMachine = stateMachineContext;
            this._playerController = playerController;
        }

        public override void OnStateEnter()
        {
            _playerController.SnapToGround = true;
        }

        public override void OnStateUpdate()
        {
            if (_playerController.IsGrounded is false)
            {
                _playerStateMachine.SetState(new PlayerStates.PlayerFallState(_playerStateMachine, _playerController));
            }

            if (_playerController.InputHandler.ShouldJump)
            {
                _playerStateMachine.SetState(new PlayerStates.PlayerJumpState(_playerStateMachine, _playerController));
            }
        }

        public override void OnStateFixedUpdate()
        {
            // The Friction motor applies a correctionary force to stop momentum when not moving or receiving input, so we apply it every frame.
            _playerController.Move(_playerController.GetMovementDirection(), _playerController.MovementSpeed, _playerController.MovementFriction);
        }
    }
}