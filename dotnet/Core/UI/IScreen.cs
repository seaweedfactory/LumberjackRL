using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LumberjackRL.Core.Monsters;
using System.Drawing;

namespace LumberjackRL.Core.UI
{
    public interface IScreen
    {
        void draw(Graphics g);
        void cycle();
        void processKey(char key, bool shift, bool alt);
        bool readyForInput();
        void displayDialog();
        void displayTrade(Monster monster);
    }
}
