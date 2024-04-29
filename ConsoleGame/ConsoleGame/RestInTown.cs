using System;

namespace ConsoleGame
{
    public static class RestInTown
    {
        public static void ShowRestMenu(Character player)
        {
            Console.WriteLine($"휴식하기");
            Console.WriteLine($"500 G 를 내면 체력을 회복할 수 있습니다. (보유 골드 : {player.Gold} G)");
            Console.WriteLine();
            Console.WriteLine("1. 휴식하기");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.Write("원하시는 행동을 입력해주세요.\n>> ");

            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    Rest(player);
                    break;
                case "0":
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    break;
            }
        }

        public static void Rest(Character player)
        {
            const int restCost = 500;

            if (player.Gold >= restCost)
            {
                player.Gold -= restCost;
                player.Health = Math.Min(player.Health + player.MaxHealth, player.MaxHealth);
                Console.WriteLine($"휴식을 완료했습니다. 체력이 {player.Health}까지 회복되었습니다.");
            }
            else
            {
                Console.WriteLine("Gold가 부족합니다.");
            }
        }
    }
}
