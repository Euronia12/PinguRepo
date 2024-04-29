using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleGame
{
    public class EquipmentManager
    {
        public List<Item> EquippedItems { get; set; }
        private Character Owner { get; set; }

        public EquipmentManager(Character owner)
        {
            EquippedItems = new List<Item>();
            Owner = owner;
        }

        public void EquipItem(Item item)
        {
            if (item.Equipped)
            {
                Console.WriteLine($"{item.Name}은(는) 이미 장착되어 있습니다.");
                return;
            }

            switch (item.Type)
            {
                case Item.ItemType.Weapon:
                    if (EquippedItems.Any(i => i.Type == Item.ItemType.Weapon))
                    {
                        UnequipItem(EquippedItems.Find(i => i.Type == Item.ItemType.Weapon));
                    }
                    Owner.AttackPower += item.StatBonus;
                    break;
                case Item.ItemType.Defense:
                    if (EquippedItems.Any(i => i.Type == Item.ItemType.Defense))
                    {
                        UnequipItem(EquippedItems.Find(i => i.Type == Item.ItemType.Defense));
                    }
                    Owner.DefensePower += item.StatBonus;
                    break;
            }

            item.Equipped = true;
            EquippedItems.Add(item);  // 여기에 장착한 아이템을 추가합니다.
        }

        public void UnequipItem(Item item)
        {
            if (!item.Equipped)
            {
                Console.WriteLine($"{item.Name}은(는) 장착되어 있지 않습니다.");
                return;
            }

            switch (item.Type)
            {
                case Item.ItemType.Weapon:
                    Owner.AttackPower -= item.StatBonus;
                    break;
                case Item.ItemType.Armor:
                    Owner.DefensePower -= item.StatBonus;  // DefensePower 감소
                    break;
            }

            item.Equipped = false;
            EquippedItems.Remove(item);  // EquippedItems에서 아이템 제거
        }

        public int CalculateTotalAttackPower()
        {
            return GetTotalAttackPower();
        }

        public int CalculateTotalDefensePower()
        {
            return GetTotalDefensePower();
        }

        public int GetTotalAttackPower()
        {
            int totalAttackPower = Owner.AttackPower;
            foreach (var item in EquippedItems)
            {
                if (item.Type == Item.ItemType.Weapon)
                {
                    totalAttackPower += item.StatBonus;
                }
            }
            return totalAttackPower;
        }

        public int GetTotalDefensePower()
        {
            int totalDefensePower = Owner.DefensePower;
            foreach (var item in EquippedItems)
            {
                if (item.Type == Item.ItemType.Defense)
                {
                    totalDefensePower += item.StatBonus;
                }
            }
            return totalDefensePower;
        }

        public List<Item> GetEquippedItems()
        {
            return EquippedItems;
        }
    }
}