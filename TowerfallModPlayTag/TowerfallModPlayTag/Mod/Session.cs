using System;
using Microsoft.Xna.Framework;
using Patcher;
using TowerFall;

namespace TowerfallModPlayTag
{
  [Patch]
  public class ModSession : Session {
    Action originalOnLevelLoadFinish;


    //Copy parent...
    public bool IsInOvertime
    {
      get
      {
        if (this.RoundIndex >= 0 && (this.MatchSettings.Mode == Modes.HeadHunters || this.MatchSettings.Mode == Modes.PlayTag))
        {
          for (int index = 0; index < this.Scores.Length; ++index)
          {
            if (this.Scores[index] >= this.MatchSettings.GoalScore)
              return true;
          }
        }
        return false;
      }
    }

    public ModSession(MatchSettings matchSettings) : base(matchSettings) {
      var ptr = this.GetType().GetMethod("$original_OnLevelLoadFinish").MethodHandle.GetFunctionPointer();
      originalOnLevelLoadFinish = (Action)Activator.CreateInstance(typeof(Action), this, ptr);
    }

    public override void LevelLoadStart(Level level)
    {
      try {
        base.LevelLoadStart(level);
      } catch (Exception e) {
        if (e.Message == "No defined Round Logic for that mode!" && this.MatchSettings.Mode == Modes.PlayTag) {
          this.RoundLogic = new PlayTagRoundLogic(this);
        } else {
          throw e;
        }
      }
    }

    public override void OnLevelLoadFinish()
    {
      originalOnLevelLoadFinish();
    }
  }
}
