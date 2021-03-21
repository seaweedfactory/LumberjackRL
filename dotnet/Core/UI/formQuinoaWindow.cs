using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LumberjackRL.Core.UI
{
    public partial class formQuinoaWindow : Form, IQuinoaUI
    {   
        private Quinoa quinoa;  //reference to the main program
        private IScreen screen; //currently displayed screen
        private GraphicsManager graphicsManager;//holds tile graphics

        public formQuinoaWindow()
        {
            InitializeComponent();

            this.Text = (Quinoa.PROGRAM_NAME + " " + Quinoa.VERSION);

            this.quinoa = null;
            graphicsManager = new GraphicsManager(this);
        }

        public void register(Quinoa quinoa)
        {
            this.quinoa = quinoa;
        }

        public void setInterfaceMode(InterfaceMode mode)
        {
            switch(mode)
            {
                case InterfaceMode.MENU:
                setScreen(new MenuScreen(quinoa, this));
                break;

                case InterfaceMode.ADVENTURE:
                setScreen(new AdventureScreen(quinoa, this));
                break;
            }
        }

        public GraphicsManager getGraphicsManager()
        {
            return graphicsManager;
        }

        public void setScreen(IScreen screen)
        {
            this.screen = screen;
            refresh();
        }

        public IScreen getScreen()
        {
            return screen;
        }

        public void refresh()
        {
            //pnlScreen.Invalidate();
            //pnlScreen.Update();

            Invalidate();
            Update();
        }

        public Form getForm()
        {
            return this;
        }

        private void formQuinoaWindow_KeyPress(object sender, KeyPressEventArgs e)
        {
            quinoa.processKey(e.KeyChar, false, false);
        }

        private void formQuinoaWindow_Paint(object sender, PaintEventArgs e)
        {
            screen.draw(e.Graphics);
        }
    }
}
