using System;
using System.Collections.Generic;
using System.Numerics;

namespace ConsoleGame
{
    public class Character
    {
        public string Name { get; set; }
        public string Job { get; set; }
        public int Level { get; set; }
        public int AttackPower { get; set; }
        public int DefensePower { get; set; }
        public int Health { get; set; }
        public int Gold { get; set; }

        public int DungeonClearCount { get; private set; } = 0;  // 던전 클리어 횟수 카운트

        public int MaxHealth { get; private set; } = 100;  // 최대 체력 속성 추가

        public InventoryManager InventoryManager { get; set; }
        public InventoryManager WeaponInventoryManager { get; set; }
        public InventoryManager ArmorInventoryManager { get; set; }
        public InventoryManager ConsumableInventoryManager { get; set; }
        public EquipmentManager WeaponEquipmentManager { get; set; }
        public EquipmentManager ArmorEquipmentManager { get; set; }

        public Character(string name, string job)
        {
            Name = name;
            Job = job;
            Level = 1;
            AttackPower = 10;
            DefensePower = 5;
            Health = 100;
            Gold = 1500;

            // InventoryManager 및 EquipmentManager 초기화
            InventoryManager = new InventoryManager();
            WeaponInventoryManager = new InventoryManager();
            ArmorInventoryManager = new InventoryManager();
            ConsumableInventoryManager = new InventoryManager();
            WeaponEquipmentManager = new EquipmentManager(this);
            ArmorEquipmentManager = new EquipmentManager(this);
        }

        public bool HasRequiredDefense(int requiredDefense)
        {
            return CalculateTotalDefensePower() >= requiredDefense;
        }

        public void ResetDungeonClearCount()
        {
            DungeonClearCount = 0;
        }

        public void AddItem(Item item)
        {
            InventoryManager.AddItem(item);  // 인벤토리에 아이템 추가

            switch (item.Type)
            {
                case Item.ItemType.Weapon:
                    WeaponInventoryManager.AddItem(item);
                    break;
                case Item.ItemType.Armor:
                    ArmorInventoryManager.AddItem(item);
                    break;
                case Item.ItemType.Potion:
                case Item.ItemType.Scroll:
                    ConsumableInventoryManager.AddItem(item);
                    break;
                default:
                    throw new ArgumentException($"Invalid item type: {item.Type}");
            }
        }

        public void RemoveItem(Item item)
        {
            InventoryManager.RemoveItem(item);  // 인벤토리에서 아이템 제거

            switch (item.Type)
            {
                case Item.ItemType.Weapon:
                    WeaponInventoryManager.RemoveItem(item);
                    break;
                case Item.ItemType.Armor:
                    ArmorInventoryManager.RemoveItem(item);
                    break;
                case Item.ItemType.Potion:
                case Item.ItemType.Scroll:
                    ConsumableInventoryManager.RemoveItem(item);
                    break;
                default:
                    throw new ArgumentException($"Invalid item type: {item.Type}");
            }
        }



        public int CalculateTotalAttackPower()
        {
            int totalAttackPower = AttackPower;
            foreach (var weapon in WeaponEquipmentManager.EquippedItems)
            {
                totalAttackPower += weapon.StatBonus;
            }
            return totalAttackPower;
        }

        public int CalculateTotalDefensePower()
        {
            int totalDefensePower = DefensePower;
            foreach (var armor in ArmorEquipmentManager.EquippedItems)
            {
                totalDefensePower += armor.StatBonus;
            }
            return totalDefensePower;
        }

        public static void ShowStatus(Character player)
        {
            Console.WriteLine("상태 보기");
            Console.WriteLine($"이름: {player.Name}");
            Console.WriteLine($"직업: {player.Job}");
            Console.WriteLine($"레벨: {player.Level}");
            Console.WriteLine($"체력: {player.Health}");
            Console.WriteLine($"Gold: {player.Gold}");

            double totalAttackPower = player.EquipmentManager.CalculateTotalAttackPower();
            double totalDefensePower = player.EquipmentManager.CalculateTotalDefensePower();

            Console.WriteLine($"공격력: {totalAttackPower}");
            Console.WriteLine($"방어력: {totalDefensePower}");

            // 장착한 아이템 정보 출력
            if (player.EquipmentManager.GetEquippedItems().Count > 0)
            {
                Console.WriteLine("[장착 아이템]");

                foreach (var item in player.EquipmentManager.GetEquippedItems())
                {
                    string itemTypeString = item.Type switch
                    {
                        Item.ItemType.Weapon => "무기",
                        Item.ItemType.Armor => "방어구",
                        Item.ItemType.Defense => "방어구", // Defense 타입을 "방어구"로 처리
                        _ => "기타"
                    };

                    Console.WriteLine($"- {item.Name} ({itemTypeString}) : +{item.StatBonus}");
                }
            }
            else
            {
                Console.WriteLine("장착한 아이템이 없습니다.");
            }
        }
    }

}