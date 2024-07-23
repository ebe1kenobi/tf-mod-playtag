using System;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Monocle;
using Patcher;
using TowerFall;

namespace TowerfallModPlayTag
{
  [Patch]
  public class ModPauseMenu : PauseMenu
  {
    public readonly DateTime creationTime;

    public ModPauseMenu(Level level, Vector2 position, MenuType menuType, int controllerDisconnected = -1) : base(level, position, menuType, controllerDisconnected) 
    { 
      creationTime = DateTime.Now;
    }

    public override void Resume()
    {
      int pauseDuration = (int)(DateTime.Now - creationTime).TotalSeconds;

      for (var i = 0; i < TFGame.Players.Length; i++)
      {
        Player p = level.Session.CurrentLevel.GetPlayer(i);
        if (p != null)
        {
          p.pauseDuration += pauseDuration;
        }
      }
      base.Resume();
    }
  }
}
