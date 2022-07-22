using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_state_jump : Player_base_state
{

    public Player_state_jump(Player player, Player_state_machine stateMachine, Player_data playerData, string animString) : base(player, stateMachine, playerData, animString)
    {
    }

    public override void Enter()
    {
        base.Enter();
        InputHandler.pInputActrion.Gameplay.Jump.performed -= player.PlayerJump;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Logic()
    {
        base.Logic();
        if (player.pController.isGrounded && player.IsInputingMove() && !player.isDashing)
        {
            SoundManager.Instance.PlayEffectOnce(playerData.LandSound);
            stateMachine.ChangeStage(player.stateMove);
        }
        else if (player.pController.isGrounded && player.isDashing && player.isCrouching){
            stateMachine.ChangeStage(player.stateSlide);
        }
        else if(player.pController.isGrounded)
        {
            SoundManager.Instance.PlayEffectOnce(playerData.LandSound);
            player.StopGroundVelocity();
            stateMachine.ChangeStage(player.stateIdle);
        }
        //else if(player.WallTouchedAngle > 160f 
        //    && (player.DashDir != Player.DashDirection.Forward_Left 
        //    && player.DashDir != Player.DashDirection.Forward_Right))
        //{
        //    stateMachine.ChangeStage(player.stateWallClimb);
        //}
        //else if(player.WallTouchedAngle > 160f || 
        //    (player.WallTouchedAngle > 120f 
        //    && player.WallTouchedAngle < 150f 
        //    && player.DashDir == Player.DashDirection.Forward))
        //{
        //    stateMachine.ChangeStage(player.stateWallRun);
        //}
        //player.CastRayWall();
        player.AddFriction(playerData.AirFriction);


    }
}
