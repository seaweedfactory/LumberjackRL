using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LumberjackRL.Core.Items;
using LumberjackRL.Core.Utilities;
using LumberjackRL.Core.Map;

namespace LumberjackRL.Core.Monsters
{
    public class MonsterAI
    {
        public static void think(Monster monster, Quinoa quinoa)
        {
            switch (monster.monsterCode)
            {
                case MonsterClassType.HUMAN: humanAI(monster, quinoa); break;
                case MonsterClassType.GHOST: followerAI(monster, quinoa); break;
                case MonsterClassType.SPONGE: spongeAI(monster, quinoa); break;
                case MonsterClassType.SLIME: followerAI(monster, quinoa); break;
                case MonsterClassType.SMALL_SLIME: followerAI(monster, quinoa); break;
                case MonsterClassType.TINY_SLIME: followerAI(monster, quinoa); break;
                case MonsterClassType.DEER: deerAI(monster, quinoa); break;
                case MonsterClassType.PIG: pigAI(monster, quinoa); break;
            }
        }


        public static void pigAI(Monster monster, Quinoa quinoa)
        {
            if (monster.readyForCommand())
            {
                MonsterActionManager.setMoveCommand(monster, EnumUtil.RandomEnumValue<Direction>());
            }
        }


        public static void deerAI(Monster monster, Quinoa quinoa)
        {
            if (monster.readyForCommand())
            {
                MonsterActionManager.setMoveCommand(monster, EnumUtil.RandomEnumValue<Direction>());
            }
        }

        public static void humanAI(Monster monster, Quinoa quinoa)
        {
            MonsterActionManager.cashOutInventory(monster);
        }

        public static void spongeAI(Monster monster, Quinoa quinoa)
        {
            MonsterActionManager.leaveWaterTrail(monster, quinoa, 0.02);

            //pick up any items which the sponge passes over
            Item newItem = quinoa.getCurrentRegionHeader().getRegion().getItem(monster.x, monster.y);
            if (newItem != null && !monster.inventory.Full && monster.readyForCommand())
            {
                MonsterActionManager.setPickupCommand(monster);
            }
            else
            {
                followerAI(monster, quinoa);
            }
        }

        public static void pathingAI(Monster monster, Quinoa quinoa)
        {
            if (monster.readyForCommand())
            {
                String searchIntervalStr = monster.getAIParameter("searchInterval");
                if (searchIntervalStr != null)
                {
                    int searchInterval = Int32.Parse(searchIntervalStr);
                    if (searchInterval > 0)
                    {
                        searchInterval = searchInterval - 1;
                        monster.setAIParameter("searchInterval", searchInterval + "");
                        //followerAI(monster, quinoa);
                        return;
                    }
                    else
                    {
                        monster.setAIParameter("searchInterval", "3");
                    }
                }
                else
                {
                    monster.setAIParameter("searchInterval", "3");
                }


                List<Position> path = quinoa.getPathFinder().findPath(quinoa.getCurrentRegionHeader().getRegion(), 50, monster, monster.x, monster.y, quinoa.getPlayer().x, quinoa.getPlayer().y);
                if (path != null && path.Count > 1)
                {
                    Position pos = path[1];

                    if (pos.y > monster.y)
                    {
                        MonsterActionManager.setMoveCommand(monster, Direction.S);
                    }
                    else if (pos.y < monster.y)
                    {
                        MonsterActionManager.setMoveCommand(monster, Direction.N);
                    }
                    else if (pos.x > monster.x)
                    {
                        MonsterActionManager.setMoveCommand(monster, Direction.E);
                    }
                    else if (pos.x < monster.x)
                    {
                        MonsterActionManager.setMoveCommand(monster, Direction.W);
                    }
                }
            }
        }

        public static void followerAI(Monster monster, Quinoa quinoa)
        {
            int targetX = quinoa.getPlayer().x;
            int targetY = quinoa.getPlayer().y;
            if (monster.readyForCommand())
            {
                //check if the player is adjacent, if so then attack
                if (quinoa.monsterIsAdjacent(monster.x, monster.y, MonsterActionManager.PLAYER_ID))
                {
                    MonsterActionManager.setAttackCommand(monster, MonsterActionManager.PLAYER_ID);
                }
                else
                {

                    if (monster.getAIParameter("altmove") != null)
                    {
                        if (monster.getAIParameter("altmove").Equals("0"))
                        {
                            monster.setAIParameter("altmove", "1");
                        }
                        else
                        {
                            monster.setAIParameter("altmove", "0");
                        }
                    }
                    else
                    {
                        monster.setAIParameter("altmove", "0");
                    }


                    if (monster.getAIParameter("altmove").Equals("0"))
                    {
                        if (monster.x < targetX)
                        {
                            MonsterActionManager.setMoveCommand(monster, Direction.E);
                        }
                        else if (monster.x > targetX)
                        {
                            MonsterActionManager.setMoveCommand(monster, Direction.W);
                        }
                        else if (monster.y < targetY)
                        {
                            MonsterActionManager.setMoveCommand(monster, Direction.S);
                        }
                        else if (monster.y > targetY)
                        {
                            MonsterActionManager.setMoveCommand(monster, Direction.N);
                        }
                    }
                    else
                    {
                        if (monster.y < targetY)
                        {
                            MonsterActionManager.setMoveCommand(monster, Direction.S);
                        }
                        else if (monster.y > targetY)
                        {
                            MonsterActionManager.setMoveCommand(monster, Direction.N);
                        }
                        else if (monster.x < targetX)
                        {
                            MonsterActionManager.setMoveCommand(monster, Direction.E);
                        }
                        else if (monster.x > targetX)
                        {
                            MonsterActionManager.setMoveCommand(monster, Direction.W);
                        }
                    }
                }
            }
        }
    }
}
