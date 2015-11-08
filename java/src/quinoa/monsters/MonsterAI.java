package quinoa.monsters;

import java.util.ArrayList;
import quinoa.Quinoa;
import quinoa.items.Item;
import quinoa.region.Position;
import quinoa.region.Terrain;
import quinoa.region.TerrainManager;

public class MonsterAI
{
    public static void think(Monster monster, Quinoa quinoa)
    {
        switch(monster.getMonsterCode())
        {
            case HUMAN: humanAI(monster, quinoa); break;
            case GHOST: followerAI(monster, quinoa); break;
            case SPONGE: spongeAI(monster, quinoa); break;
            case SLIME: followerAI(monster, quinoa); break;
            case SMALL_SLIME: followerAI(monster, quinoa); break;
            case TINY_SLIME: followerAI(monster, quinoa); break;
            case DEER: deerAI(monster, quinoa); break;
            case PIG: pigAI(monster, quinoa); break;
        }
    }


    public static void pigAI(Monster monster, Quinoa quinoa)
    {
        if(monster.readyForCommand())
        {
            MonsterActionManager.setMoveCommand(monster, Monster.Direction.values()[(int)(Math.random() * Monster.Direction.values().length)]);
        }
    }


    public static void deerAI(Monster monster, Quinoa quinoa)
    {
        if(monster.readyForCommand())
        {
            MonsterActionManager.setMoveCommand(monster, Monster.Direction.values()[(int)(Math.random() * Monster.Direction.values().length)]);
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
        Item newItem = quinoa.getCurrentRegionHeader().getRegion().getItem(monster.getX(), monster.getY());
        if(newItem != null && !monster.getInventory().isFull() && monster.readyForCommand())
        {
            MonsterActionManager.setPickupCommand(monster);
        }
        else
        {
            followerAI(monster,quinoa);
        }
    }

    public static void pathingAI(Monster monster, Quinoa quinoa)
    {
        if(monster.readyForCommand())
        {
            String searchIntervalStr = monster.getAIParameter("searchInterval");
            if(searchIntervalStr != null)
            {
                int searchInterval = Integer.parseInt(searchIntervalStr);
                if(searchInterval > 0)
                {
                    searchInterval = searchInterval - 1;
                    monster.setAIParameter("searchInterval", searchInterval +"");
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


            ArrayList<Position> path = quinoa.getPathFinder().findPath(quinoa.getCurrentRegionHeader().getRegion(), 50, monster, monster.getX(), monster.getY(), quinoa.getPlayer().getX(), quinoa.getPlayer().getY());
            if(path != null && path.size() > 1)
            {
                Position pos = path.get(1);

                if(pos.y > monster.getY())
                {
                    MonsterActionManager.setMoveCommand(monster, Monster.Direction.S);
                }
                else if(pos.y < monster.getY())
                {
                    MonsterActionManager.setMoveCommand(monster, Monster.Direction.N);
                }
                else if(pos.x > monster.getX())
                {
                    MonsterActionManager.setMoveCommand(monster, Monster.Direction.E);
                }
                else if(pos.x < monster.getX())
                {
                    MonsterActionManager.setMoveCommand(monster, Monster.Direction.W);
                }
            }
        }
    }

    public static void followerAI(Monster monster, Quinoa quinoa)
    {
        int targetX = quinoa.getPlayer().getX();
        int targetY = quinoa.getPlayer().getY();
        if(monster.readyForCommand())
        {
            //check if the player is adjacent, if so then attack
            if(quinoa.monsterIsAdjacent(monster.getX(), monster.getY(), MonsterActionManager.PLAYER_ID))
            {
                MonsterActionManager.setAttackCommand(monster, MonsterActionManager.PLAYER_ID);
            }
            else
            {

                if(monster.getAIParameter("altmove") != null)
                {
                    if(monster.getAIParameter("altmove").equals("0"))
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


                if(monster.getAIParameter("altmove").equals("0"))
                {
                    if(monster.getX() < targetX)
                    {
                        MonsterActionManager.setMoveCommand(monster, Monster.Direction.E);
                    }
                    else if(monster.getX() > targetX)
                    {
                        MonsterActionManager.setMoveCommand(monster, Monster.Direction.W);
                    }
                    else if(monster.getY() < targetY)
                    {
                        MonsterActionManager.setMoveCommand(monster, Monster.Direction.S);
                    }
                    else if(monster.getY() > targetY)
                    {
                        MonsterActionManager.setMoveCommand(monster, Monster.Direction.N);
                    }
                }
                else
                {
                    if(monster.getY() < targetY)
                    {
                        MonsterActionManager.setMoveCommand(monster, Monster.Direction.S);
                    }
                    else if(monster.getY() > targetY)
                    {
                        MonsterActionManager.setMoveCommand(monster, Monster.Direction.N);
                    }
                    else if(monster.getX() < targetX)
                    {
                        MonsterActionManager.setMoveCommand(monster, Monster.Direction.E);
                    }
                    else if(monster.getX() > targetX)
                    {
                        MonsterActionManager.setMoveCommand(monster, Monster.Direction.W);
                    }
                }
            }
        }
    }
}
