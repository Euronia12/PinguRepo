using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace ConsoleGame
{
    public sealed class SaveLoadManager
    {
        private const string SAVE_FOLDER = "SaveGames";

        private static readonly SaveLoadManager instance = new SaveLoadManager();

        private SaveLoadManager() { }  // private 생성자로 외부에서 인스턴스화 방지

        public static SaveLoadManager Instance
        {
            get { return instance; }
        }

        public List<string> GetSavedGames()
        {
            string saveFolderPath = Path.Combine(Directory.GetCurrentDirectory(), SAVE_FOLDER);

            if (!Directory.Exists(saveFolderPath))
            {
                Console.WriteLine("저장된 게임 파일이 없습니다.");
                return new List<string>();
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(saveFolderPath);
            FileInfo[] files = directoryInfo.GetFiles("*.json");

            List<string> savedGames = new List<string>();
            foreach (var file in files)
            {
                savedGames.Add(file.Name);
            }

            return savedGames;
        }

        public void SaveGame(Character player)
        {
            string saveFolderPath = Path.Combine(Directory.GetCurrentDirectory(), SAVE_FOLDER);

            if (!Directory.Exists(saveFolderPath))
            {
                Directory.CreateDirectory(saveFolderPath);
            }

            string saveName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm");
            string filePath = Path.Combine(saveFolderPath, $"{saveName}.json");

            string json = JsonConvert.SerializeObject(player, Formatting.Indented);
            File.WriteAllText(filePath, json);

            Console.WriteLine($"게임이 저장되었습니다. ({saveName}.json)");
        }


        public Character LoadGame(string fileName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), SAVE_FOLDER, fileName);
            if (File.Exists(filePath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    Character player = JsonConvert.DeserializeObject<Character>(json);
                    Console.WriteLine("게임 불러오기 완료");
                    return player;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"게임 불러오기 실패: {e.Message}");
                    return null;
                }
            }
            else
            {
                Console.WriteLine("선택한 게임 파일이 없습니다.");
                return null;
            }
        }

        public void DeleteSavedGame(string fileName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), SAVE_FOLDER, fileName);
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                    Console.WriteLine($"게임 파일 {fileName}을(를) 삭제하였습니다.");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"게임 파일 삭제 실패: {e.Message}");
                }
            }
            else
            {
                Console.WriteLine("선택한 게임 파일이 없습니다.");
            }
        }

        public void DisplaySavedGames(List<string> savedGames)
        {
            Console.WriteLine("불러올 게임을 선택해주세요:");
            for (int i = 0; i < savedGames.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {savedGames[i]}");
            }
        }

        public int GetValidSelectedIndex(int maxIndex)
        {
            int selectedIndex;
            while (true)
            {
                Console.Write("번호를 입력하세요: ");
                if (int.TryParse(Console.ReadLine(), out selectedIndex) && selectedIndex >= 1 && selectedIndex <= maxIndex)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("유효한 번호를 입력해주세요.");
                }
            }
            return selectedIndex;
        }
    }
}
