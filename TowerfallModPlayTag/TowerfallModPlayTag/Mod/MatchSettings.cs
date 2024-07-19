using Patcher;
using System;
using TowerFall;

namespace TowerfallModPlayTag
{
    [Patch]
    public class MyMatchSettings : MatchSettings
    {
        public int GoalScore
        {
          get
          {
            int num;
        
            switch (this.Mode)
            {
              case Modes.LastManStanding:
                num = base.GoalScore;
                break;
              case Modes.HeadHunters:
              case Modes.Warlord:
                 num = base.GoalScore;
                break;
              case Modes.TeamDeathmatch:
                 num = base.GoalScore;
                break;
              case Modes.Trials:
              case Modes.LevelTest:
                  num = base.GoalScore;
                break;
              case Modes.PlayTag:
                 num = this.PlayerGoals(5, 4, 3);
                 break;
              default:
                throw new Exception("No Goal value defined for this mode!");
            }
            return (int)Math.Ceiling((double)num * (double)MatchSettings.GoalMultiplier[(int)this.MatchLength]);
          }
        }

    public MyMatchSettings(LevelSystem levelSystem, Modes mode, MatchLengths matchLength) : base(levelSystem, mode, matchLength)
    {
      this.LevelSystem = levelSystem;
      this.Mode = mode;
      this.MatchLength = matchLength;
      this.Teams = new MatchTeams(Allegiance.Neutral);
      this.Variants = new MatchVariants(false);
    }
  }
}
