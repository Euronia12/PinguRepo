using System;
using System.Collections.Generic;

namespace ConsoleGame
{
    public class Dungeon
    {
        public enum Difficulty
        {
            Easy,
            Normal,
            Hard
        }

        private Character player;
        private Difficulty difficulty;
        private Random random;

        public Dungeon(Character player)
        {
            this.player = player;
            random = new Random();
        }

        private int GetBaseReward()
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

        private int CalculateAdditionalReward(int attackPower)
        {
            double percentage = random.Next(10, 21) / 100.0; // 10% ~ 20% 랜덤 값

            int additionalReward = (int)(attackPower * percentage);  // double 값을 int로 캐스팅

            return additionalReward;
        }

        private int CalculateReward(int attackPower)
        {
            int baseReward = GetBaseReward();
            int additionalReward = CalculateAdditionalReward(attackPower);

            int totalReward = baseReward + additionalReward;

            return totalReward;
        }

        public void Start(Difficulty difficulty)
        {
            this.difficulty = difficulty;

            int requiredDefense = GetRequiredDefense(difficulty);

            if (!player.HasRequiredDefense(requiredDefense))
            {
                Console.WriteLine($"방어력이 {requiredDefense} 이상이어야 {difficulty} 던전에 입장할 수 있습니다.");
                return;
            }

            bool success = random.Next(1, 101) <= 60;

            if (!success)
            {
                Console.WriteLine($"{difficulty} 던전 입장 실패! 보스를 처치하면 보상이 없으며 체력이 절반으로 감소합니다.");
                player.Health /= 2;
                return;
            }

            Console.WriteLine($"{difficulty} 던전 입장 성공!");

            Enemy enemy = GenerateEnemy(difficulty);

            Console.WriteLine($"[적 정보: {enemy.Name}, 레벨 {enemy.Level}, 체력 {enemy.Health}, 공격력 {enemy.AttackPower}]");

            while (player.Health > 0 && enemy.Health > 0)
            {
                Console.WriteLine("\n턴을 선택하세요:");
                Console.WriteLine("1. 공격");
                Console.WriteLine("2. 아이템 사용");
                Console.Write(">> ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Fight(enemy);
                        break;
                    case "2":
                        UseItem();
                        break;
                    default:
                        Console.WriteLine("잘못된 선택입니다.");
                        break;
                }

                if (enemy.Health > 0)
                {
                    EnemyAttack(enemy);
                }
            }

            if (player.Health <= 0)
            {
                Console.WriteLine("당신은 패배했습니다.");
            }
            else
            {
                Console.WriteLine("적을 물리쳤습니다!");

                int baseReward = GetBaseReward();
                int additionalReward = CalculateAdditionalReward(player.CalculateTotalAttackPower());

                Console.WriteLine($"기본 보상: {baseReward} G");
                Console.WriteLine($"공격력으로 인한 추가 보상: {additionalReward} G");

                int totalReward = baseReward + additionalReward;
                Console.WriteLine($"총 보상: {totalReward} G를 획득하였습니다.");

                player.Gold += totalReward;

                if (random.Next(1, 101) <= 20) // 15~20% 확률로 드롭
                {
                    DropHighTierItem();
                }
            }

            ClearDungeon();
        }

        private void DropHighTierItem()
        {
            List<Item> highTierItems = new List<Item>();

            highTierItems.AddRange(player.WeaponInventoryManager.GetItemsByType(Item.ItemType.Weapon));
            highTierItems.AddRange(player.ArmorInventoryManager.GetItemsByType(Item.ItemType.Armor));

            if (highTierItems.Count == 0)
            {
                Console.WriteLine("상위 무기나 방어구가 없습니다.");
                return;
            }

            Item droppedItem = highTierItems[random.Next(highTierItems.Count)];

            Console.WriteLine($"상위 아이템을 획득하였습니다: {droppedItem.Name}");
            player.AddItem(droppedItem);
        }

        private void ClearDungeon()
        {
            int damage = CalculateDamage();

            player.Health -= damage;

            Console.WriteLine($"던전 클리어! 체력 {damage} 소모됨.");
            Console.WriteLine($"남은 체력: {player.Health}");

            if (random.Next(1, 101) <= 20) // 20% 확률로 특별한 아이템 드롭
            {
                DropSpecialItem(difficulty); // difficulty를 전달
            }
        }

        private List<Item> specialItems = new List<Item>
{
    new Item("용의 검", Item.ItemType.SpecialWeapon, 10000, 40, "드래곤의 힘을 담은 무기입니다.", true),
    new Item("신의 갑옷", Item.ItemType.SpecialArmor, 12000, 50, "신들의 보호를 받는 갑옷입니다.", true),
    new Item("마법의 지팡이", Item.ItemType.SpecialWeapon, 9000, 35, "마법사의 필수 아이템입니다.", true),
    new Item("마력의 로브", Item.ItemType.SpecialArmor, 11000, 45, "마법의 힘을 강화시켜주는 로브입니다.", true),
    new Item("지혜의 서", Item.ItemType.SpecialScroll, 8000, 30, "지식과 경험을 향상시켜주는 서입니다.", true),
    new Item("천둥의 방패", Item.ItemType.SpecialArmor, 13000, 55, "천둥의 힘으로 공격을 막아주는 방패입니다.", true),
    new Item("빛의 검", Item.ItemType.SpecialWeapon, 9500, 38, "빛의 힘을 가진 검입니다.", true),
    new Item("천사의 날개", Item.ItemType.SpecialArmor, 11500, 48, "천사의 보호를 받는 날개입니다.", true)
};

        private void DropSpecialItem(Dungeon.Difficulty difficulty)
        {
            // 노말 던전부터 하드 던전까지 특별 아이템 드롭
            if (difficulty == Dungeon.Difficulty.Normal ||
                difficulty == Dungeon.Difficulty.Hard)
            {
                // 무작위로 하나의 아이템 선택
                Item droppedItem = specialItems[random.Next(specialItems.Count)];

                Console.WriteLine($"특별한 아이템을 획득하였습니다: {droppedItem.Name}");

                // 귀속 아이템이므로 Purchased 값을 true로 설정
                ItemManager.UpdateItemPurchasedStatus(droppedItem);

                // 아이템을 인벤토리의 장비 카테고리에 추가
                player.AddItem(droppedItem);
            }
        }



        private int CalculateDamage()
        {
            int baseDamage = random.Next(20, 36); // 20 ~ 35 랜덤 값

            int difference = player.CalculateTotalDefensePower() - GetRequiredDefense(difficulty);
            int extraDamage = difference > 0 ? random.Next(difference + 1) : 0;

            int totalDamage = baseDamage + extraDamage;

            return totalDamage;
        }

        public static int GetRequiredDefense(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    return 5;
                case Difficulty.Normal:
                    return 15;
                case Difficulty.Hard:
                    return 21;
                default:
                    throw new ArgumentException("Invalid difficulty");
            }
        }

        private Enemy GenerateEnemy(Difficulty difficulty)
        {
            int level;
            int health;
            int attackPower;

            switch (difficulty)
            {
                case Difficulty.Easy:
                    level = player.Level - 1;
                    health = 50 + (level * 10);
                    attackPower = 5 + (level * 2);
                    break;
                case Difficulty.Normal:
                    level = player.Level + 2;
                    health = 200 + (level * 20);
                    attackPower = 20 + (level * 10);
                    break;
                case Difficulty.Hard:
                    level = player.Level + 5;
                    health = 350 + (level * 40);
                    attackPower = 35 + (level * 40);
                    break;
                default:
                    throw new ArgumentException("Invalid difficulty");
            }

            return new Enemy(level, health, attackPower, $"적 레벨 {level}");
        }

        private void Fight(Enemy enemy)
        {
            int damage = player.CalculateTotalAttackPower();
            Console.WriteLine($"당신이 {enemy.Name}에게 {damage}의 피해를 입혔습니다.");
            enemy.Health -= damage;
        }

        private void UseItem()
        {
            Console.WriteLine("사용할 아이템을 선택하세요.");
            // 아이템 사용 로직은 구현하지 못했습니다
        }

        private void EnemyAttack(Enemy enemy)
        {
            int damage = enemy.AttackPower - player.CalculateTotalDefensePower();
            if (damage < 0)
            {
                damage = 0;
            }
            Console.WriteLine($"{enemy.Name}이(가) 당신에게 {damage}의 피해를 입혔습니다.");
            player.Health -= damage;
        }
    }

    public class Enemy
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int Health { get; set; }
        public int AttackPower { get; set; }

        public Enemy(int level, int health, int attackPower, string name)
        {
            Level = level;
            Health = health;
            AttackPower = attackPower;
            Name = name;
        }
    }
}
