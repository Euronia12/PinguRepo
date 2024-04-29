using System;
using System.Collections.Generic;

namespace ConsoleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            LoadOrStartGame();
        }

        static void LoadOrStartGame()
        {
            List<string> savedGames = SaveLoadManager.Instance.GetSavedGames();

            if (savedGames.Count > 0)
            {
                SaveLoadManager.Instance.DisplaySavedGames(savedGames);
                int selectedIndex = SaveLoadManager.Instance.GetValidSelectedIndex(savedGames.Count);

                string selectedGame = savedGames[selectedIndex - 1];
                Character player = SaveLoadManager.Instance.LoadGame(selectedGame);

                if (player == null)
                {
                    player = CreateNewCharacter();
                }

                RunGame(player);
            }
            else
            {
                Console.WriteLine("저장된 게임 파일이 없습니다.");
                Character player = CreateNewCharacter();
                RunGame(player);
            }
        }

        private static void SaveGame(Character player)
        {
            SaveLoadManager.Instance.SaveGame(player);
        }

        static Character CreateNewCharacter()
        {
            Console.Write("캐릭터 이름을 입력하세요: ");
            string name = Console.ReadLine();

            Console.Write("직업을 선택하세요 (전사, 마법사, 도적 등): ");
            string job = Console.ReadLine();

            return new Character(name, job);
        }

        static void RunGame(Character player)
        {
            Game game = new Game(player);
            game.Run();
        }
    }
}
