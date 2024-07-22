using System;
using Microsoft.Xna.Framework;
using Patcher;
using TowerFall;

namespace TowerfallModPlayTag
{
  [Patch]
  public class ModPlayer : Player
  {
    public static Monocle.Collider[] wasColliders = new Monocle.Collider[4];

    // Play Tag var
    public bool playTag = false;
    public PlayTagHUD PlayTagHUD;
    public int playTagDelay = 10; 
    public int playTagDelayModePlayTag = 15; 
    public int playTagCountDown = 0;
    public int previousPlayTagCountDown = 0;
    public bool playTagCountDownOn = false;
    public readonly DateTime creationTime;
    // End Play Tag var

    public ModPlayer(
      int playerIndex,
      Vector2 position,
      Allegiance allegiance,
      Allegiance teamColor,
      PlayerInventory inventory,
      Player.HatStates hatState,
      bool frozen,
      bool flash,
      bool indicator)
      : base(playerIndex, position, allegiance, teamColor, inventory, hatState, frozen, flash, indicator)
    {
      this.Add((Monocle.Component)(this.PlayTagHUD = new PlayTagHUD()));
    }

    public static void PlayerOnPlayer(Player a, Player b)
    {
      Player.PlayerOnPlayer(a, b); 
      if (a.playTag && !b.HasShield)
      {
        b.playTag = true;
        b.playTagCountDown = a.playTagCountDown;
        b.creationTime = a.creationTime;

        a.playTag = false;
      }
      else if (b.playTag && !a.HasShield)
      {
        a.playTag = true;
        a.playTagCountDownOn = true;
        a.playTagCountDown = b.playTagCountDown;
        a.creationTime = b.creationTime;

        b.playTag = false;
      }
    }
    public override void HUDRender(bool wrapped)
    {
      if (!playTagCountDownOn && this.Level.Session.MatchSettings.Mode != Modes.PlayTag)
      {
        //hide arrow
        base.HUDRender(wrapped);
      }

      // Active the arrows just after the explosion in case the tag is a survivor
      if (this.Level.Session.MatchSettings.Mode == Modes.PlayTag && !playTagCountDownOn && previousPlayTagCountDown > playTagCountDown) {
        base.HUDRender(wrapped);
      }

      if (this.playTag)
      {
        PlayTagHUDRender();
      } 
    }

    public void PlayTagHUDRender()
    {
      PlayTagHUD.Render();
      if (!(bool)(Monocle.Component)Indicator)
        return;
      Indicator.Render();
    }

    public override void ShootArrow() 
    {
      base.ShootArrow();
    }

    public override void HurtBouncedOn(int bouncerIndex)
    {
      if (playTagCountDownOn)
        return;
      // When MatchSettings.Mode == Modes.PlayTag we can Hurt people juste after the bomb explose ^^, it's a feature!
      base.HurtBouncedOn(bouncerIndex);
    }

    public override void Update()
    {
      base.Update();
      if (playTagCountDownOn)
      {
        this.Aiming = false; 
        int delay;
        if (this.Level.Session.MatchSettings.Mode == Modes.PlayTag) {
          delay = playTagDelayModePlayTag;
        } else {
          delay = playTagDelay;
        }
        previousPlayTagCountDown = playTagCountDown;
        playTagCountDown = delay - (int)(DateTime.Now - creationTime).TotalSeconds;
      }
    }
  }
}
