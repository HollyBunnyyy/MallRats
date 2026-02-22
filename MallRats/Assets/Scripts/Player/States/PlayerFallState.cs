using UnityEngine;

namespace PlayerStates
{
    public class PlayerFallState : PlayerState
    {
        private PlayerStateMachine _playerStateMachine;
        private PlayerController _playerController;

        public PlayerFallState(PlayerStateMachine stateMachineContext, PlayerController playerController) : base(stateMachineContext, playerController)
        {
            this._playerStateMachine = stateMachineContext;
            this._playerController = playerController;
        }

        public override void OnStateEnter()
        {
            _playerController.SnapToGround = false;
        }

        public override void OnStateUpdate()
        {
            if (_playerController.IsGrounded && _playerController.GroundInfo.distance <= (_playerController.Height + 0.05f))
            {
                _playerStateMachine.SetState(new PlayerStates.PlayerWalkState(_playerStateMachine, _playerController));
            }
        }

        public override void OnStateExit()
        {
            //Zero any gravity forces that were applied the previous frame.
            _playerController.Rigidbody.linearVelocity = Vector3.Scale(_playerController.Rigidbody.linearVelocity, new Vector3(1.0f, 0.0f, 1.0f));

            _playerController.SnapToGround = true;
        }
    }
}