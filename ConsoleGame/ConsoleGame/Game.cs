using System;
using System.Collections.Generic;

namespace ConsoleGame
{
    public class Game
    {
        private Character player;
        private Shop shop;
        private Random random = new Random();

        public Game(Character player)
        {
            this.player = player;

            player.WeaponInventoryManager = new InventoryManager();
            player.ArmorInventoryManager = new InventoryManager();
            player.ConsumableInventoryManager = new InventoryManager();

            shop = new Shop();
        }

        public void Run()
        {
            while (true)
            {
                DisplayMenu();

                Character player = new Character("PlayerName", "Warrior"); // 예시로 Character 객체를 생성
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ShowStatus(player); // ShowStatus 메서드 호출
                        break;
                    case "2":
                        player.InventoryManager.ShowInventory();
                        break;
                    case "3":
                        Shop.ShowShop(player);
                        break;
                    case "4":
                        EnterDungeon();
                        break;
                    case "5":
                        RestInTown.ShowRestMenu(player);
                        break;
                    case "6":
                        SaveGame();
                        break;
                    case "7":
                        Console.WriteLine("게임을 종료합니다.");
                        return;
                    default:
                        Console.WriteLine("잘못된 선택입니다.");
                        break;
                }
            }
        }

        private void SaveGame()
        {
            SaveLoadManager.Instance.SaveGame(player);
        }

        private void DisplayMenu()
        {
            Console.WriteLine("===== 게임 메뉴 =====");
            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("4. 던전 입장");
            Console.WriteLine("5. 휴식하기");
            Console.WriteLine("6. 저장하기");
            Console.WriteLine("7. 게임 종료");
            Console.WriteLine("===================");
            Console.Write("원하시는 행동을 입력해주세요: ");
        }

        private void EnterDungeon()
        {
            Console.WriteLine("1. 쉬운 던전     | 방어력 5 이상 권장");
            Console.WriteLine("2. 일반 던전     | 방어력 11 이상 권장");
            Console.WriteLine("3. 어려운 던전    | 방어력 17 이상 권장");
            Console.WriteLine("0. 나가기");
            Console.Write("원하시는 행동을 입력해주세요.\n>> ");

            string dungeonInput = Console.ReadLine();
            if (int.TryParse(dungeonInput, out int difficultyIndex) && Enum.IsDefined(typeof(Dungeon.Difficulty), difficultyIndex - 1))
            {
                Dungeon.Difficulty selectedDifficulty = (Dungeon.Difficulty)(difficultyIndex - 1);
                int requiredDefense = Dungeon.GetRequiredDefense(selectedDifficulty);

                if (!player.HasRequiredDefense(requiredDefense))
                {
                    Console.WriteLine($"방어력이 {requiredDefense} 이상이어야 {selectedDifficulty} 던전에 입장할 수 있습니다.");
                    return;
                }

                Dungeon dungeon = new Dungeon(player);
                dungeon.Start(selectedDifficulty);

                DropSpecialItem(selectedDifficulty);
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
            }
        }

        private void DropSpecialItem()
        {
            List<Item> specialItems = new List<Item>
    {
        new Item("용의 검", Item.ItemType.SpecialWeapon, 10000, 40, "드래곤의 힘을 담은 무기입니다.", true),
        new Item("신의 갑옷", Item.ItemType.SpecialArmor, 12000, 50, "신들의 보호를 받는 갑옷입니다.", true),
        new Item("마법의 지팡이", Item.ItemType.SpecialWeapon, 9000, 35, "마법사의 필수 아이템입니다.", true),
        new Item("마력의 로브", Item.ItemType.SpecialArmor, 11000, 45, "마법의 힘을 강화시켜주는 로브입니다.", true),
        new Item("지혜의 서", Item.ItemType.SpecialScroll, 8000, 30, "지식과 경험을 향상시켜주는 서입니다.", true),
        new Item("천둥의 방패", Item.ItemType.SpecialArmor, 13000, 55, "천둥의 힘으로 공격을 막아주는 방패입니다.", true),
        new Item("황혼의 반지", Item.ItemType.SpecialWeapon, 9500, 38, "황혼의 여신의 가호를 받은 반지로 태양의 힘을 사용합니다.", true),
        new Item("천사의 날개", Item.ItemType.SpecialArmor, 11500, 48, "천사의 보호를 받는 날개입니다.", true)
    };

            // 무작위로 하나의 아이템 선택
            Item droppedItem = specialItems[random.Next(specialItems.Count)];

            Console.WriteLine($"특별한 아이템을 획득하였습니다: {droppedItem.Name}");

            // 귀속 아이템이므로 Purchased 값을 true로 설정
            // 이 부분은 아이템 클래스에 Purchased 속성이 있어야 합니다.
            ItemManager.UpdateItemPurchasedStatus(droppedItem);

            // 아이템을 인벤토리의 장비 카테고리에 추가
            player.AddItem(droppedItem);
        }


        private void DropSpecialItem(Dungeon.Difficulty difficulty)
        {
            if (difficulty == Dungeon.Difficulty.Normal ||
                difficulty == Dungeon.Difficulty.Hard)
            {
                DropSpecialItem();
            }
        }
    }
}
