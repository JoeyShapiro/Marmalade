using Mindmagma.Curses;
using Marmalade;

bool keepGoing = true;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var screen = NCurses.InitScreen();

NCurses.NoDelay(screen, true);
NCurses.NoEcho();
NCurses.SetCursor(0);

int kin = -1;
int i = 0;
int itemNo = 1;
const int SIZE = 8;
//Ship ship = new Ship(4, "test");
// map
int[,] map = new int[SIZE,SIZE];
Ship[] ships = new Ship[32]; // abritrary size
// player
bool curRot = false;
int x = 0;
int y = 0;
Ship[] ships2Place = // need to add elsewhere, since placeShip() handles in ship, should work
{
    new Ship(2, "Patrol Boat"),
    new Ship(3, "Submarine"),
    new Ship(3, "Cruiser"),
    new Ship(4, "Battleship"),
    new Ship(5, "Carrier")
}; // the ships the player has to place // do they have names
int shipsPlaced = 0;
// logs
Queue<string> logs = new Queue<string>();

void placeShip(Ship ship, int x, int y, bool rot) // smert // with ship super smert
{
    if (!rot) // up(right)
    {
        if (x + ship.size > SIZE) // check if in range, only needs one, others are done elseware
            return;
        // check if other
        for (int i = 0; i < ship.size; i++)
            if (map[x + i, y] > 0)
                return;
        // add to map
        for (int i = 0; i < ship.size; i++)
            map[x + i, y] = itemNo;
    } 
    else if (rot) // rotated 90 degrees
    {
        if(y + ship.size > SIZE) // maybe do at handler
            return;
        // check if other
        for (int i = 0; i < ship.size; i++)
            if (map[x, y+i] > 0)
                return;
        for (int i = 0; i < ship.size; i++)
            map[x, y + i] = itemNo;
    }
    enque("Placed " + ship.name + " at (" + x + "," + y + ")");
    ships[itemNo++] = ship; // store in array with its # as index // maybe clone
    shipsPlaced++; // do here, so it only does after good, maybe have return true, but this is fine too
        
}

void drawMap()
{
    for (int i=0; i<SIZE; i++)
        for (int j=0; j<SIZE; j++)
            if (map[i, j] is not 0) // smert??, YES, dont show 0, but dont need to print ' ' for 0
                NCurses.MoveAddString(i+1, j+1, ""+map[i,j]);
}

void handleInput(int kin)
{
    if (kin is (int)'w' && x-1 > -1)
        x--;
    else if (kin is (int)'s' && x+1 < SIZE)
        x++;
    else if (kin is (int)'a' && y-1 > -1)
        y--;
    else if (kin is (int)'d' && y+1 < SIZE)
        y++;
    else if (kin is (int)' ')
    {
        if (shipsPlaced < 5)
        {
            Ship ship2place = ships2Place.ElementAt<Ship>(shipsPlaced); // i remembered how it works :) // should go through each one, and hide the last
            placeShip(ship2place, x, y, curRot);
        }

    }
    else if (kin is (int)'r')
        curRot = !curRot;
    else if (kin is 27) // 'esc' || '^['
        keepGoing = false; // TODO keep global?
}

void drawGhost(Ship sh) // use local or main when pass
{
    if (curRot)
        for (int i = 0; i < sh.size; i++)
            NCurses.MoveAddChar(x + 1, y + 1 + i, 'g'); // use "1" to account for map change with view
    else
        for (int i = 0; i < sh.size; i++)
            NCurses.MoveAddChar(x + 1+i, y + 1, 'g');
}


int curSize = 0;
int size = 6;
int logC = 0;
void enque(string msg)
{
    if (curSize >= size)
    {
        logs.Dequeue();
        logs.Enqueue(logC++ + " "+ msg);
    } // fine i guess, but learn about circle queue, that is what this needs, only show first and remove first(last one shown)
    else
    {
        logs.Enqueue(logC++ + " " + msg);
        curSize++;
    }
}

void drawLogs() // other time lucked out by passing values in c++, this doesnt, maybe
{
    Queue<string> tmpLogs = new (logs); // in higher level must be clone
    int l = 0;
    while (tmpLogs.Count != 0)
    {
        NCurses.MoveAddString(SIZE+2 + l, 1, tmpLogs.Peek()); // x actually y?
        tmpLogs.Dequeue();
        l++;
    }

}

NCurses.MoveAddString(0, 0, "press any key to continue...");
//placeShip(ship, 0, 0, false);
//placeShip(ship, 0, 0, true); // shouldnt be placed // does this add to itemNO // no because it returns early
//placeShip(ship, 0, 4, true);
enque("https://github.com/JoeyShapiro/Marmalade");

while(keepGoing) {
    kin = NCurses.GetChar(); // its async

    if (kin is -1) // deal with async??
        continue;

    NCurses.Clear();
    handleInput(kin);
    // draw layout
    drawMap();
    if (shipsPlaced < 5)
        drawGhost(ships2Place.ElementAt<Ship>(shipsPlaced)); // maybe just make nextShip and use throughout, should work
    NCurses.Box(screen, '*', '*');
    NCurses.MoveAddString(SIZE+1, 1, "******");
    NCurses.MoveAddString(1, SIZE+1, "*");
    string str = "char: (char)" + kin + " char: " + (char)kin + " times: " + i++;
    drawLogs();
    NCurses.MoveAddString(18, 0, str);
    NCurses.MoveAddChar(x+1, y+1, 'x'); // player, must be after map
    NCurses.Refresh();

    
};

NCurses.EndWin();