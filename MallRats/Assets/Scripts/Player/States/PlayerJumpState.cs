namespace PlayerStates
{
    public class PlayerJumpState : PlayerState
    {
        private PlayerStateMachine _playerStateMachine;
        private PlayerController _playerController;

        public PlayerJumpState(PlayerStateMachine stateMachineContext, PlayerController playerController) : base(stateMachineContext, playerController)
        {
            this._playerStateMachine = stateMachineContext;
            this._playerController = playerController;
        }

        public override void OnStateEnter()
        {
            _playerController.SnapToGround = false;

            //Jumping is an impulse force - we only need to apply it once instead of every fixed update frame.
            _playerController.Jump(_playerController.JumpForce);
        }

        public override void OnStateUpdate()
        {
            if (_playerController.IsGrounded is false)
            {
                _playerStateMachine.SetState(new PlayerStates.PlayerFallState(_playerStateMachine, _playerController));
            }
        }

        public override void OnStateExit()
        {
            _playerController.SnapToGround = true;
        }
    }
}
