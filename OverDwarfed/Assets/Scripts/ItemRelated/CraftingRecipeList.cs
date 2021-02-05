using System.Collections.Generic;
using UnityEngine;

public class CraftingRecipeList : MonoBehaviour
{
    public static CraftingRecipeList instance;
    public List<Recipe> recipes;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        recipes = new List<Recipe>() 
        {
            new Recipe(0, 1, 0f, Item.log, new List<Cost>(){new Cost(Item.log, 8)}), // crafting table
            new Recipe(1, 1, 0f, Item.log, new List<Cost>(){new Cost(Item.rock, 20), new Cost(Item.log, 12)}),
            new Recipe(2, 1, 0f, Item.log, new List<Cost>(){new Cost(Item.stone_block, 20), new Cost(Item.plank, 12), new Cost(Item.iron_ingot, 4)}), // smelter lvl2
            new Recipe(3, 1, 0f, Item.log, new List<Cost>(){new Cost(Item.iron_ingot, 8)}), // anvil
            new Recipe(4, 1, 0f, Item.log, new List<Cost>(){new Cost(Item.log, 8), new Cost(Item.iron_ingot, 6), new Cost(Item.gear, 2)}),
            new Recipe(5, 1, 0f, Item.log, new List<Cost>(){new Cost(Item.plank, 8), new Cost(Item.gold_ingot, 1), new Cost(Item.polished_crystal, 1)}), // jewelry table
            
            // smelter lvl1
            new Recipe(6, 1, 6f, Item.stone_block, new List<Cost>(){new Cost(Item.rock, 2), new Cost(Item.log, 2)}),
            new Recipe(7, 1, 9f, Item.iron_ingot, new List<Cost>(){new Cost(Item.iron_ore, 3), new Cost(Item.log, 2)}),
            new Recipe(8, 1, 12f, Item.gold_ingot, new List<Cost>(){new Cost(Item.gold_ore, 5), new Cost(Item.log, 2)}),

            // smelter lvl2
            new Recipe(9, 1, 3f, Item.coal, new List<Cost>(){new Cost(Item.log, 2) }),
            new Recipe(10, 1, 4f, Item.stone_block, new List<Cost>(){new Cost(Item.rock, 2), new Cost(Item.coal, 1)}),
            new Recipe(11, 1, 6f, Item.iron_ingot, new List<Cost>(){new Cost(Item.iron_ingot, 3), new Cost(Item.coal, 1)}),
            new Recipe(12, 1, 8f, Item.gold_ingot, new List<Cost>(){new Cost(Item.gold_ore, 5), new Cost(Item.coal, 1)}),
            new Recipe(13, 1, 12f, Item.polished_crystal, new List<Cost>(){new Cost(Item.crystal_ore, 7), new Cost(Item.coal, 1)}),

            // crafting table
            new Recipe(14, 1, 12f, Item.stone_pickaxe, new List<Cost>(){new Cost(Item.rock, 12), new Cost(Item.log, 4) }),
            new Recipe(15, 1, 12f, Item.stone_hammer, new List<Cost>(){new Cost(Item.rock, 20), new Cost(Item.log, 4) }),
            new Recipe(16, 1, 12f, Item.iron_pickaxe, new List<Cost>(){new Cost(Item.iron_ingot, 12), new Cost(Item.plank, 4) }),
            new Recipe(17, 1, 12f, Item.iron_hammer, new List<Cost>(){new Cost(Item.iron_ingot, 20),new Cost(Item.plank, 4) }),
            new Recipe(18, 1, 6f, Item.gear, new List<Cost>(){new Cost(Item.iron_ingot, 1), new Cost(Item.stone_block, 2)}),

            // anvil
            new Recipe(19, 1, 15f, Item.iron_sword, new List<Cost>(){new Cost(Item.iron_ingot, 12), new Cost(Item.plank, 6)}),
            new Recipe(20, 1, 25f, Item.battle_axe, new List<Cost>(){new Cost(Item.iron_ingot, 18), new Cost(Item.plank, 6)}),
            new Recipe(21, 1, 10f, Item.iron_helmet, new List<Cost>(){new Cost(Item.iron_ingot, 10)}),
            new Recipe(22, 1, 20f, Item.platemail, new List<Cost>(){new Cost(Item.iron_ingot, 20)}),

            // wood cutter
            new Recipe(23, 1, 3f, Item.plank, new List<Cost>(){new Cost(Item.log, 1)}),
            new Recipe(24, 1, 20f, Item.bow, new List<Cost>(){new Cost(Item.plank, 10),new Cost(Item.string_web, 5)}),
            new Recipe(25, 1, 25f, Item.crossbow, new List<Cost>(){new Cost(Item.plank, 15), new Cost(Item.gear, 2), new Cost(Item.string_web, 5)}),
            new Recipe(26, 1, 30f, Item.shield, new List<Cost>(){new Cost(Item.plank, 20), new Cost(Item.iron_ingot, 5)}),

            // jewelry table
            new Recipe(27, 5, 5f, Item.gold_coin, new List<Cost>(){new Cost(Item.gold_ingot, 1)}),
            new Recipe(28, 1, 12f, Item.forbidden_crown, new List<Cost>(){new Cost(Item.gold_ingot, 5), new Cost(Item.polished_crystal, 3) }),
            new Recipe(29, 4, 12f, Item.polished_crystal, new List<Cost>(){new Cost(Item.lifeforce_crystal, 1)}),
            new Recipe(30, 1, 60f, Item.lifeforce_crystal, new List<Cost>(){new Cost(Item.polished_crystal, 7)}),
        };
    }
}
public struct Cost
{
    public Item item;
    public int amount;
    public Cost(Item _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }
}
public enum Item
{
    log,
    coal,
    rock,
    stone_block,
    iron_ore,
    iron_ingot,
    gold_ore,
    gold_ingot,
    crystal_ore,
    polished_crystal,
    stone_pickaxe,
    stone_hammer,
    iron_pickaxe,
    iron_hammer,
    gear,
    iron_sword,
    battle_axe,
    iron_helmet,
    platemail,
    plank,
    bow,
    crossbow,
    shield,
    gold_coin,
    forbidden_crown,
    lifeforce_crystal,
    string_web
}
public struct Recipe 
{
    public List<Cost> CostList;
    public Item resultItem;
    public int resultItemAmount, id;
    public float creationTime;
    public Recipe(int _id, int _resultsItemAmount, float _creationTime, Item _resultItem, List<Cost> _CostList)
    {
        id = _id;
        resultItemAmount = _resultsItemAmount;
        creationTime = _creationTime;
        resultItem = _resultItem;
        CostList = _CostList;
    }
}
