using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace LumberjackRL.Core.UI
{

    public interface IQuinoaUI
    {
        void refresh();
        void register(Quinoa quinoa);
        IScreen getScreen();
        void setScreen(IScreen screen);
        GraphicsManager getGraphicsManager();
        void setInterfaceMode(InterfaceMode mode);
        Form getForm();
    }
}
