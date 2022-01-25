using Mindmagma.Curses;
using Marmalade;

bool keepGoing = true;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var screen = NCurses.InitScreen();
var screenPlayer = NCurses.NewWindow(20, 30, 0, 0);
var screenEnemy = NCurses.NewWindow(21, 31, 21, 20);

NCurses.NoDelay(screen, true);
NCurses.NoEcho();

int kin = -1;
int i = 0;
Ship ship = new Ship(4);
// map

NCurses.MoveAddString(0, 0, "press any key to continue...");

while(keepGoing) {
    kin = NCurses.GetChar(); // its async

    if (kin is -1) // deal with async??
        continue;

    NCurses.Clear();
    // draw layout
    NCurses.Box(screenPlayer, '*', '*');
    NCurses.Box(screenEnemy, '*', '*');
    NCurses.MoveAddChar(5,5, (char)kin);
    string str = "times: " + i++ + "char: (char)" + kin;
    NCurses.MoveAddString(6, 6, str);
    NCurses.Refresh();

    if (kin is 27) // 'esc' || '^['
        keepGoing = false;
};

NCurses.EndWin();