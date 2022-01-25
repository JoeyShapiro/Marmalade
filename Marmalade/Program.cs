using Mindmagma.Curses;
using Marmalade;

bool keepGoing = true;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var screen = NCurses.InitScreen();

NCurses.NoDelay(screen, true);
NCurses.NoEcho();

int kin = -1;
int i = 0;
int itemNo = 1;
const int SIZE = 8;
Ship ship = new Ship(4);
// map
int[,] map = new int[SIZE,SIZE];
Ship[] ships = new Ship[32]; // abritrary size

void placeShip(Ship ship, int x, int y, int rot) // smert
{
    if (rot is 0) // up(right)
    {
        // check if other
        if (x + ship.size > SIZE)
            return;
        for (int i = 0; i < ship.size; i++)
            map[x + i, y] = itemNo;
    }
    else if (rot is 1)
    {
        if(x - ship.size < 0)
            return;
        for (int i = 0; i < ship.size; i++)
            map[x - i, y] = itemNo;
    } // down
        
    else if (rot is 2)
    {
        if(y + ship.size > SIZE)
            return;
        for (int i = 0; i < ship.size; i++)
            map[x, y + i] = itemNo;
    } // left
        
    else if (rot is 3)
    {
        if(y - ship.size < 0)
            return;
        for (int i = 0; i < ship.size; i++)
            map[x, y - i] = itemNo;
    } // right
    ships[itemNo++] = ship; // store in array with its # as index
        
}

void drawMap()
{
    for (int i=0; i<SIZE; i++)
        for (int j=0; j<SIZE; j++)
            NCurses.MoveAddString(i+1, j+1, ""+map[i,j]);
}

NCurses.MoveAddString(0, 0, "press any key to continue...");
placeShip(ship, 0, 0, 0);
placeShip(ship, 0, 0, 1);
placeShip(ship, 0, 0, 2);

while(keepGoing) {
    kin = NCurses.GetChar(); // its async

    if (kin is -1) // deal with async??
        continue;

    NCurses.Clear();
    // draw layout
    drawMap();
    NCurses.Box(screen, '*', '*');
    NCurses.MoveAddChar(17,10, (char)kin);
    string str = "times: " + i++ + "char: (char)" + kin;
    NCurses.MoveAddString(18, 0, str);
    NCurses.Refresh();

    if (kin is 27) // 'esc' || '^['
        keepGoing = false;
};

NCurses.EndWin();