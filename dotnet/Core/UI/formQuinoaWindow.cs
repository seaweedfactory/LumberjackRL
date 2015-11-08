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
        public const int UI_PIXEL_WIDTH=1000;
        public const int UI_PIXEL_HEIGHT=700;
    
        private Quinoa quinoa;  //reference to the main program
        private IScreen screen; //currently displayed screen
        private GraphicsManager graphicsManager;//holds tile graphics

        public formQuinoaWindow()
        {
            InitializeComponent();

            this.Text = (Quinoa.PROGRAM_NAME + " " + Quinoa.VERISON);
          
            this.Size = new Size(UI_PIXEL_WIDTH, UI_PIXEL_HEIGHT);
            this.quinoa = null;
            graphicsManager = new GraphicsManager();
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
                setScreen(new MenuScreen(quinoa));
                break;

                case InterfaceMode.ADVENTURE:
                setScreen(new AdventureScreen(quinoa));
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
            pnlScreen.Invalidate();
            pnlScreen.Update();
        }

        public Form getForm()
        {
            return this;
        }

        private void formQuinoaWindow_KeyPress(object sender, KeyPressEventArgs e)
        {
            quinoa.processKey(e.KeyChar, false, false);
        }

        private void pnlScreen_Paint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);
            screen.draw(e.Graphics);
        }
    }
}
