using Mindmagma.Curses;

bool keepGoing = true;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var screen = NCurses.InitScreen();

NCurses.NoDelay(screen, true);
NCurses.NoEcho();

int kin = 0;
int i = 0;

while(keepGoing) {
    kin = NCurses.GetChar();
    NCurses.Clear();
    NCurses.MoveAddChar(5,5, (char)kin);
    string str = "times: " + i++;
    NCurses.MoveAddString(6, 6, str);
    NCurses.Refresh();

    if (kin is (int)'q')
        keepGoing = false;
};

NCurses.EndWin();