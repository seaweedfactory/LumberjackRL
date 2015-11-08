package quinoa.ui;

import quinoa.Quinoa;


public interface QuinoaUIInterface
{
    public enum InterfaceMode {MENU, ADVENTURE};
    public void display();
    public void refresh();
    public void register(Quinoa quinoa);
    public ScreenInterface getScreen();
    public void setScreen(ScreenInterface screen);
    public GraphicsManager getGraphicsManager();
    public void setInterfaceMode(InterfaceMode mode);
}
