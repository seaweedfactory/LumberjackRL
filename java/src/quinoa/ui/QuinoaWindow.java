package quinoa.ui;

import java.awt.BorderLayout;
import java.awt.Dimension;
import java.awt.Graphics;
import java.awt.Image;
import java.awt.Toolkit;
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;
import java.awt.image.BufferedImage;
import javax.swing.JFrame;
import quinoa.Quinoa;

/**
 *
 * @author User
 */
public class QuinoaWindow extends JFrame implements KeyListener, QuinoaUIInterface
{
    public static final int UI_PIXEL_WIDTH=960;
    public static final int UI_PIXEL_HEIGHT=640;
    
    private Quinoa quinoa;                  //reference to the main program
    private ScreenInterface screen;         //currently displayed screen
    private Image buffer;                   //double-buffering buffer
    private GraphicsManager graphicsManager;//holds tile graphics

    public QuinoaWindow()
    {
        if(!Quinoa.DEBUG_MODE)
        {
            this.setTitle(Quinoa.PROGRAM_NAME + " " + Quinoa.VERISON);
        }
        else
        {
            this.setTitle(Quinoa.PROGRAM_NAME + " " + Quinoa.VERISON + " - DEBUG MODE");
        }
        this.setLayout(new BorderLayout());
        this.setSize(QuinoaWindow.UI_PIXEL_WIDTH, QuinoaWindow.UI_PIXEL_HEIGHT);
        this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        this.quinoa = null;
        buffer = new BufferedImage(QuinoaWindow.UI_PIXEL_WIDTH,QuinoaWindow.UI_PIXEL_HEIGHT, BufferedImage.TYPE_INT_ARGB);
        graphicsManager = new GraphicsManager();
    }

    public void register(Quinoa quinoa)
    {
        this.quinoa = quinoa;
        this.addKeyListener(this);
    }

    public void setInterfaceMode(InterfaceMode mode)
    {
        switch(mode)
        {
            case MENU:
            setScreen(new MenuScreen(quinoa));
            break;

            case ADVENTURE:
            setScreen(new AdventureScreen(quinoa));
            break;
        }
    }

    public GraphicsManager getGraphicsManager()
    {
        return graphicsManager;
    }

    public void setScreen(ScreenInterface screen)
    {
        this.screen = screen;
    }

    public ScreenInterface getScreen()
    {
        return screen;
    }

    public void display()
    {
        //show the screen
        this.setVisible(true);

        // Move the screen to the middle of the display
        Dimension dim = Toolkit.getDefaultToolkit().getScreenSize();
        this.setLocation((dim.width-this.getSize().width)/2, (dim.height-this.getSize().height)/2 - 17);

    }

    public void refresh()
    {
        this.repaint();
    }

    public void paint(Graphics graphics)
    {
        screen.draw(buffer.getGraphics());
        graphics.drawImage(buffer, 0, 0, null);
    }

    public void keyTyped(KeyEvent e)
    {
        quinoa.processKey(e.getKeyChar(), e.isShiftDown(), e.isAltDown());
    }

    public void keyPressed(KeyEvent e)
    {
        //ignore
    }

    public void keyReleased(KeyEvent e)
    {
        //ignore
    }

}
