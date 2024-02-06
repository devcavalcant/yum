using System;

Random random = new();
Console.CursorVisible = false;
int height = Console.WindowHeight - 1;
int width = Console.WindowWidth - 5;
bool shouldExit = false;

int playerX = 0;
int playerY = 0;

int foodX = 0;
int foodY = 0;

string[] states = { "('-')", "(^-^)", "(X_X)" };
string[] foods = { "@@@@@", "$$$$$", "#####" };

string player = states[0];

int food = 0;

bool isSpeedUp = false;

InitializeGame();
while (!shouldExit)
{
    if (!TerminalResized())
    {
        Move(optionalEnd: false);
    }
    else
    {
        FinishGame();
        Console.Write("Console was resized. Program exiting.");
        break;
    }

}

bool TerminalResized()
{
    return height != Console.WindowHeight - 1 || width != Console.WindowWidth - 5;
}

void ShowFood()
{
    food = random.Next(0, foods.Length);

    foodX = random.Next(0, width - player.Length);
    foodY = random.Next(0, height - 1);

    // Display the food at the location
    Console.SetCursorPosition(foodX, foodY);
    Console.Write(foods[food]);
}

void ChangePlayer()
{
    player = states[food];
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);
}

void FreezePlayer()
{
    System.Threading.Thread.Sleep(1000);
    player = states[0];
}

void Move(bool optionalEnd = false, bool enableBuffing = false)
{
    int lastX = playerX;
    int lastY = playerY;

    switch (Console.ReadKey(true).Key)
    {
        case ConsoleKey.UpArrow:
            playerY--;
            break;
        case ConsoleKey.DownArrow:
            playerY++;
            break;
        case ConsoleKey.LeftArrow:
            if (isSpeedUp && enableBuffing)
            {
                playerX -= 3;
            }
            else
            {
                playerX--;
            }

            break;
        case ConsoleKey.RightArrow:
            if (isSpeedUp && enableBuffing)
            {
                playerX += 3;
            }
            else
            {
                playerX++;
            }
            break;
        case ConsoleKey.Backspace:
            FinishGame();
            shouldExit = true;
            break;
        default:
            if (optionalEnd)
            {
                FinishGame();
                shouldExit = true;
            }
            break;
    }

    Console.SetCursorPosition(lastX, lastY);
    for (int i = 0; i < player.Length; i++)
    {
        Console.Write(" ");
    }

    playerX = (playerX < 0) ? 0 : (playerX >= width ? width : playerX);
    playerY = (playerY < 0) ? 0 : (playerY >= height ? height : playerY);

    if (playerX == foodX && playerY == foodY)
    {
        ChangePlayer();
        ShowFood();
    }

    SetPlayerState();

    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);
}

void InitializeGame()
{
    Console.Clear();
    ShowFood();
    Console.SetCursorPosition(0, 0);
    Console.Write(player);
}

void SpeedUpPlayer()
{
    isSpeedUp = true;
}

void SetPlayerState()
{
    if (player == states[2])
    {
        FreezePlayer();
    }
    else if (player == states[1])
    {
        SpeedUpPlayer();
    }
    else
    {
        isSpeedUp = false;
    }
}

void FinishGame()
{
    Console.Clear();
    Console.CursorVisible = true;
}