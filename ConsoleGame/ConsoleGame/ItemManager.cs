using System.Collections.Generic;

namespace ConsoleGame
{
    public class ItemManager
    {
        public static List<Item> ItemInfos = new List<Item>
{
    new Item("수련자 갑옷", Item.ItemType.Armor, 1000, 5, "수련에 도움을 주는 갑옷입니다.", false, false),
new Item("무쇠갑옷", Item.ItemType.Armor, 2000, 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", false, false),
new Item("스파르타의 갑옷", Item.ItemType.Armor, 3500, 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", false, false),
new Item("헤라클레스의 망토", Item.ItemType.Armor, 5000, 30, "헤라클레스가 헤르메스에게 받았다는 전설의 망토입니다.", false, false),
new Item("낡은 검", Item.ItemType.Weapon, 600, 2, "쉽게 볼 수 있는 낡은 검입니다.", false, false),
new Item("청동 도끼", Item.ItemType.Weapon, 1400, 5, "어디선가 사용됐던거 같은 도끼입니다.", false, false),
new Item("스파르타의 창", Item.ItemType.Weapon, 2500, 7, "스파르타의 전사들이 사용했다는 전설의 창입니다.", false, false),
new Item("여명의 시미터", Item.ItemType.Weapon, 3500, 20, "달의 여신의 축복을 받아 만들어진 검입니다.", false, false),
new Item("체력 회복 물약", Item.ItemType.Consumable, 100, 0, "체력을 50 회복시키는 물약입니다.", false, false),
        new Item("마나 회복 물약", Item.ItemType.Consumable, 100, 0, "마나를 30 회복시키는 물약입니다.", false, false),
        new Item("공격력 증가 물약", Item.ItemType.Consumable, 200, 0, "공격력을 5 증가시키는 물약입니다.", false, false)
    };

        public static void UpdateItemPurchasedStatus(Item item)
        {
            foreach (var itemInfo in ItemInfos)
            {
                if (itemInfo.Name == item.Name && itemInfo.Type == item.Type)
                {
                    itemInfo.Purchased = true;
                    break;
                }
            }
        }

        public static int GetIndexOfItem(Item item)
        {
            for (int i = 0; i < ItemInfos.Count; i++)
            {
                if (ItemInfos[i].Name == item.Name && ItemInfos[i].Type == item.Type)
                {
                    return i;
                }
            }
            return -1; // not found
        }
    }
}