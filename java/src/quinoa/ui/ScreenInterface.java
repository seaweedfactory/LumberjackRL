package quinoa.ui;

import java.awt.Graphics;
import quinoa.monsters.Monster;

public interface ScreenInterface
{
    public void draw(Graphics graphics);
    public void cycle();
    public void processKey(char key, boolean shift, boolean alt);
    public boolean readyForInput();
    public void displayDialog();
    public void displayTrade(Monster monster);
}
