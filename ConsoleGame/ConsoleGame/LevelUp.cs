using System;

namespace ConsoleGame
{
    public static class LevelUp
    {
        public static void CheckLevelUp(Character player)
        {
            int requiredClears = player.Level - 1;  // 현재 레벨을 통해 필요한 클리어 횟수 계산
            if (player.DungeonClearCount >= requiredClears)
            {
                player.Level++;
                player.ResetDungeonClearCount();  // 클리어 횟수 초기화 메서드 호출

                // 기본 공격력과 방어력 증가
                player.AttackPower += 1;  // 정수로 변경
                player.DefensePower += 2;  // 정수로 변경

                Console.WriteLine($"축하합니다! 레벨 업! 현재 레벨은 Lv{player.Level} 입니다.");
                Console.WriteLine($"기본 공격력이 1, 방어력이 2 증가했습니다.");  // 메시지 수정
            }
            else
            {
                Console.WriteLine("레벨업을 위한 던전 클리어 횟수가 부족합니다.");
            }
        }
    }
}
