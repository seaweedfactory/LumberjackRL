using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using LumberjackRL.Core.Monsters;
using System.IO;

namespace LumberjackRL.Core.UI
{
    public class MenuScreen : IScreen
    {
        private DrawManager dtm;
        private Quinoa quinoa;
        private bool savedGameExists;

        public MenuScreen(Quinoa quinoa)
        {
            dtm = new DrawManager();
            this.quinoa = quinoa;
            this.savedGameExists = File.Exists(QuinoaActions.SAVED_GAME_FILENAME);

            quinoa.reset();
        }

        public void draw(Graphics g)
        {
            //clear the screen
            g.Clear(Color.Black);
            
            //Draw messages
            dtm.drawString(Quinoa.PROGRAM_NAME + " " + Quinoa.VERISON, 3, g, 20, 40, quinoa.getUI().getGraphicsManager());
            dtm.drawString("Press n to start a new game", 2, g, 20, 80, quinoa.getUI().getGraphicsManager());

            if (savedGameExists)
            {
                dtm.drawString("Press l to load a game", 2, g, 20, 100, quinoa.getUI().getGraphicsManager());
            }

            dtm.drawString("Your brother is missing!", 2, g, 20, 140, quinoa.getUI().getGraphicsManager());
            dtm.drawString("Travel to different towns and search the wilderness to find him.", 2, g, 20, 180, quinoa.getUI().getGraphicsManager());
            dtm.drawString("Perhaps he's in the forest? Or maybe a cave? Find out!", 2, g, 20, 200, quinoa.getUI().getGraphicsManager());

            dtm.drawString("Watch out for giant sponges, oozy slimes, and creepy ghosts!", 2, g, 20, 240, quinoa.getUI().getGraphicsManager());
            dtm.drawString("Keep your lumberjack hunger at bay and stay well fed.", 2, g, 20, 260, quinoa.getUI().getGraphicsManager());

            dtm.drawString("So chop down some trees, eat some flapjacks, and find your brother!", 2, g, 20, 300, quinoa.getUI().getGraphicsManager());

            dtm.drawString("Good luck!", 2, g, 20, 340, quinoa.getUI().getGraphicsManager());

            dtm.drawString("Please be patient, new games can take a few minutes to generate the world.", 2, g, 20, 600, quinoa.getUI().getGraphicsManager());

        }

        public void cycle()
        {

        }

        public bool readyForInput()
        {
            return true;
        }

        public void processKey(char key, bool shift, bool alt)
        {
            if (key == 'l' && savedGameExists)
            {
                quinoa.getActions().loadGame();
                quinoa.getUI().setInterfaceMode(InterfaceMode.ADVENTURE);
            }
            else if (key == 'n')
            {
                quinoa.getActions().newGame();
                quinoa.getUI().setInterfaceMode(InterfaceMode.ADVENTURE);
                AdventureScreen adv = (AdventureScreen)quinoa.getUI().getScreen();
                adv.setMode(AdventureScreenMode.HELP);
            }
            quinoa.cycle();
        }

        public void displayDialog()
        {
            //do nothing
        }

        public void displayTrade(Monster monster)
        {
            //do nothing
        }
    }
}
