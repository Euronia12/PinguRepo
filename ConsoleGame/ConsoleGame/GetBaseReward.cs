using System;

namespace ConsoleGame
{
    public static class GetBaseReward
    {
        public enum Difficulty
        {
            Easy,
            Normal,
            Hard
        }

        public static int Calculate(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    return 1000;
                case Difficulty.Normal:
                    return 2000;
                case Difficulty.Hard:
                    return 3500;
                default:
                    throw new ArgumentException("Invalid difficulty");
            }
        }
    }
}
