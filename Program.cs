namespace Battleships;

using System.IO;
using System.Text;
using System.Linq;
using System.Diagnostics.Metrics;
using System.Xml.Linq;
using System.Numerics;
using System.Data.Common;

// This is a new version

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            //Calls menu routine as soon as the program opens, will exit while () when
            //valid choiceMade is true.

            int choice = 0;
            while (true)
            {
                displayTitle(0);
                choice = menu();

                switch (choice)
                {
                    case 1:
                        {
                            break;
                        }
                    case 2:
                        {
                            break;
                        }
                    case 3:
                        {
                            Environment.Exit(0);
                            break;
                        }
                    case 0:
                        {
                            break;
                        }
                    default:
                        {
                            error("You have entered an invalid choice!");
                            continue;
                        }
                }

                break;
            }
            Console.Clear();

            //START GAME ------------------------------------------

            var playerOneGrid = createGrid(); //create a grid for each player
            var playerTwoGrid = createGrid();

            Ship[] playerOneShips = createPlayerShips(); //create ships for each player
            Ship[] playerTwoShips = createPlayerShips();

            //Create list of guesses each player has made.
            List<string> playerOneGuesses = new List<string>();
            List<string> playerTwoGuesses = new List<string>();

            int playerWon = 0;
            bool turn = false;

            string[] columns = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" };
            string[] rows = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };

            List<string> allPositions = new List<string>();
            for (int c = 0; c < 10; c++)
            {
                for (int r = 0; r < 10; r++)
                {
                    string posToAdd = string.Join(';', columns[c], rows[r]);
                    allPositions.Add(posToAdd);
                }
            } //creates a list of all possible positions

            switch (choice)
            {
                case 1:
                    {
                        ships(playerOneGrid, playerOneShips, false); //get each player to place ship onto the board
                        ships(playerTwoGrid, playerTwoShips, true);

                        while (true) //break when game is won
                        {
                            if (turn == false)
                            {
                                gameTurn(playerOneGrid, playerTwoGrid, playerOneShips, playerTwoShips, playerOneGuesses, playerTwoGuesses, turn);
                            }
                            if (turn == true)
                            {
                                gameTurn(playerTwoGrid, playerOneGrid, playerTwoShips, playerOneShips, playerTwoGuesses, playerOneGuesses, turn);
                            }

                            int p1Hits = 0;
                            int p2Hits = 0;

                            for (int i = 0; i < 5; i++) //if a player has 17 hits all their ships are gone
                            {
                                var hits1 = playerOneShips[i].getHits().Count();
                                var hits2 = playerTwoShips[i].getHits().Count();

                                p1Hits += hits1;
                                p2Hits += hits2;
                            }
                            if (p1Hits == 17)
                            {
                                playerWon = 2;
                                break;
                            }
                            if (p2Hits == 17)
                            {
                                playerWon = 1;
                                break;
                            }

                            turn = !turn; //not turn to switch to other player
                        }
                        break;
                    } //pass and plau
                case 2:
                    {
                        //TODO: cpu vs player.

                        //create grid and ships for p2(CPU)
                        //place ships PLAYER
                        //get guess from PLAYER
                        //hit miss update p1 board
                        //get guess for p2(CPU)
                        //hit miss update p1 board
                        //repeat until won

                        cpuShips(playerTwoGrid, playerTwoShips);
                        cpuShips(playerOneGrid, playerOneShips);
                        foreach(Ship s in playerOneShips)
                        {
                            drawShipToBoard(playerOneGrid, s);
                        }


                        while(true)
                        {
                            if(turn == false)
                            {
                                gameTurn(playerOneGrid, playerTwoGrid, playerOneShips, playerTwoShips, playerOneGuesses, playerTwoGuesses, turn);
                            }
                            if(turn == true)
                            {
                                cpuGuessAndUpdate(playerTwoGrid, playerOneGrid, playerTwoShips, playerOneShips, playerTwoGuesses, allPositions);
                            }

                            int p1Hits = 0;
                            int p2Hits = 0;

                            for (int i = 0; i < 5; i++) //if a player has 17 hits all their ships are gone
                            {
                                var hits1 = playerOneShips[i].getHits().Count();
                                var hits2 = playerTwoShips[i].getHits().Count();

                                p1Hits += hits1;
                                p2Hits += hits2;
                            }
                            if (p1Hits == 17)
                            {
                                playerWon = 2;
                                break;
                            }
                            if (p2Hits == 17)
                            {
                                playerWon = 1;
                                break;
                            }

                            turn = !turn; //not turn to switch to other player
                        }
                        Console.ReadKey();
                        break;
                    }
                case 0:
                    {
                        List<string> cpuGuesses1 = new List<string>();
                        List<string> cpuGuesses2 = new List<string>();

                        foreach(string i in allPositions)
                        {
                            cpuGuesses1.Add(i);
                            cpuGuesses2.Add(i);
                        }

                        sampleGame(playerOneGrid, playerTwoGrid, playerOneShips, playerTwoShips); //makes two random grids with random ships
                        for (int i = 0; i < 5; i++)
                        {
                            drawShipToBoard(playerOneGrid, playerOneShips[i]); //draws each ship to board ;optional
                            drawShipToBoard(playerTwoGrid, playerTwoShips[i]);
                        }

                        while (true)
                        {
                            int p1Hits = 0;
                            int p2Hits = 0;

                            cpuGuessAndUpdate(playerOneGrid, playerTwoGrid, playerOneShips, playerTwoShips, playerOneGuesses, cpuGuesses1); 
                            cpuGuessAndUpdate(playerTwoGrid, playerOneGrid, playerTwoShips, playerOneShips, playerTwoGuesses, cpuGuesses2);

                            for (int i = 0; i < 5; i++) //check if a player has won
                            {
                                var hits1 = playerOneShips[i].getHits().Count();
                                var hits2 = playerTwoShips[i].getHits().Count();

                                p1Hits += hits1;
                                p2Hits += hits2;
                            }
                            if (p1Hits == 17)
                            {
                                playerWon = 2;
                                break;
                            }
                            if (p2Hits == 17)
                            {
                                playerWon = 1;
                                break;
                            } 
                        }

                        List<string> p1h = new List<string>();
                        List<string> p2h = new List<string>();

                        for (int i = 0; i < 5; i++)
                        {
                            foreach (string p in playerTwoShips[i].getHits())
                            {
                                p1h.Add(p);
                            }
                            foreach (string p in playerOneShips[i].getHits())
                            {
                                p2h.Add(p);
                            }
                        }

                        displayGame(playerOneGrid, false);
                        Console.Write("\nGuesses {0}: ", playerOneGuesses.Count());
                        foreach (string s in playerOneGuesses)
                        {
                            Console.Write(s);
                        }
                        Console.Write("\n\nHits {0}: ", p1h.Count());
                        foreach (string h in p1h)
                        {
                            Console.Write(h);
                        }
                        Console.ReadKey();
                        displayGame(playerTwoGrid, true);
                        Console.Write("\nGuesses {0}: ", playerTwoGuesses.Count());
                        foreach (string s in playerTwoGuesses)
                        {
                            Console.Write(s);
                        }
                        Console.Write("\n\nHits {0}: ", p2h.Count());
                        foreach (string h in p2h)
                        {
                            Console.Write(h);
                        }

                        Console.ReadKey();

                        break;
                    } //sample game cpu vs cpu
            }

            Console.Clear();
            Console.Write("Player {0} is the winner. ", playerWon.ToString());
            Console.Write("Press any key to return to the main menu.");
            Console.ReadKey();
            continue;
        }

    }

    //DISPLAY --------------------------------

    static int menu()
    {
        Console.WriteLine("\n1. Pass and Play");
        Console.WriteLine("\n2. Vs CPU");
        Console.WriteLine("\n3. Quit");
        Console.Write("\nYour option: ");

        int choice = 0;
        try
        {
            string temp = Console.ReadLine();
            if (temp == null)
            {
                error("choice you have selected is null.");
            }
            choice = Int32.Parse(temp);

            switch (choice)
            {
                case 1:
                    return 1;
                case 2:
                    return 2;
                case 3:
                    return 3;
                case 0:
                    return 0;
                default:
                    return -1;
            }
        }
        catch
        {
            return -1;
        }
    } //display the menu and return the choice. will return -1 if error, or invalid choice.

    static void error(string messgage)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("\nError: " + messgage);
        Console.Write(" Press any key to try again...");
        Console.ResetColor();
        Console.ReadKey();
        Console.Clear();
    } //display an error message and then clear the console.

    static void displayGame(string[] grid, bool titleToDisplay)
    {
        int t = 0;
        if(titleToDisplay == false)
        {
            t = 1;
        }
        if(titleToDisplay == true)
        {
            t = 2;
        }

        Console.Clear();
        displayTitle(t);
        Console.WriteLine();
        displayBoard(grid);
        Console.WriteLine();
    } //display title and board relevant to each player.

    static void displayBoard(string[] grid)
    {
        foreach (string i in grid)
        {
            foreach (char j in i)
            {
                switch (j)
                {
                    case ')':
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Write("*");
                            Console.ResetColor();
                            break;
                        }
                    case '(':
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("*");
                            Console.ResetColor();
                            break;
                        }
                    case '&':
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("*");
                            Console.ResetColor();
                            break;
                        }
                    case '=':
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write("*");
                            Console.ResetColor();
                            break;
                        }
                    default:
                        {
                            Console.Write(j);
                            break;
                        }
                }
            }
            Console.Write("\n");
        }
    } //display the board.

    static void displayTitle(int titleToDisplay)
    {
        GameAssets g = new GameAssets();
        string[] title = g.Titles[titleToDisplay];

        foreach(string t in title)
        {
            Console.Write(t);
        }
    } //display the tile.

    //DRAW -----------------------------------

    static void drawShipToBoard(string[] grid, Ship shipToDraw)
    {
        string[] positions = shipToDraw.getPositions();

        foreach (string i in positions)
        {
            string[] split = i.Split(';');
            string c = split[0];
            string r = split[1];

            char charToDraw = '(';

            if (shipToDraw.getHits().Contains(i) == true)
            {
                charToDraw = ')';
            }

            drawToBoardSetCoord(grid, c, r, charToDraw, false);
        }
    }

    static void removeShipFromBoard(string[] grid, Ship shipToDraw, bool targetBoard)
    {
        string[] positions = shipToDraw.getPositions();

        foreach (string i in positions)
        {
            string[] split = i.Split(';');
            string c = split[0];
            string r = split[1];

            drawToBoardSetCoord(grid, c, r, ' ', targetBoard);
        }
    }

    static int[] getCoordsForBoardOne(string c, string r)
    {
        int[] coords = new int[2] { -1, -1 };

        int row = -1;
        int column = -1;

        //Switch to check the column input and change columnn variable respectively.
        switch (c.ToLower())
        {
            case "a":
                {
                    column = 2;
                    break;
                }
            case "b":
                {
                    column = 6;
                    break;
                }
            case "c":
                {
                    column = 10;
                    break;
                }
            case "d":
                {
                    column = 14;
                    break;
                }
            case "e":
                {
                    column = 18;
                    break;
                }
            case "f":
                {
                    column = 22;
                    break;
                }
            case "g":
                {
                    column = 26;
                    break;
                }
            case "h":
                {
                    column = 30;
                    break;
                }
            case "i":
                {
                    column = 34;
                    break;
                }
            case "j":
                {
                    column = 38;
                    break;
                }
            default:
                {
                    return coords;
                }
        }

        //Switch to check the row input and change row respectively.
        switch (r)
        {
            case "1":
                {
                    row = 21;
                    break;
                }
            case "2":
                {
                    row = 19;
                    break;
                }
            case "3":
                {
                    row = 17;
                    break;
                }
            case "4":
                {
                    row = 15;
                    break;
                }
            case "5":
                {
                    row = 13;
                    break;
                }
            case "6":
                {
                    row = 11;
                    break;
                }
            case "7":
                {
                    row = 9;
                    break;
                }
            case "8":
                {
                    row = 7;
                    break;
                }
            case "9":
                {
                    row = 5;
                    break;
                }
            case "10":
                {
                    row = 3;
                    break;
                }
            default:
                {
                    return coords;
                }
        }

        coords[0] = column;
        coords[1] = row;

        //returns [-1,-1] for error, returns coords to draw if valid.
        return coords;
    } //get what char to change to draw to left board.

    static int[] getCoordsForBoardTwo(string c, string r)
    {
        int[] coords = getCoordsForBoardOne(c, r);
        coords[0] = coords[0] + 45; // offset x only y is the same.

        return coords;
    } //get what char to change to draw to right board.

    static void drawToBoardSetCoord(string[] grid, string c, string r, char toDraw, bool targetBoard)
    {
        // if board one target is false.

        if (targetBoard == false)
        {
            int[] coordsToDraw = getCoordsForBoardOne(c, r);
            int column = coordsToDraw[0];
            int row = coordsToDraw[1];

            if (row == -1 || column == -1)
            {
                error("could not get coords to draw; returned -1.");
            }

            var chars = grid[row].ToCharArray();
            chars[column] = toDraw;

            grid[row] = new string(chars);
        }
        if (targetBoard == true)
        {
            int[] coordsToDraw = getCoordsForBoardTwo(c, r);
            int column = coordsToDraw[0];
            int row = coordsToDraw[1];

            if (row == -1 || column == -1)
            {
                error("could not get coords to draw; returned -1.");
            }

            var chars = grid[row].ToCharArray();
            chars[column] = toDraw;

            grid[row] = new string(chars);
        }
    } //draw a set character to a supplied coordiante letter;number a1

    static void updateShipStatus(string[] grid, Ship[] ships)
    {
        bool[] statuses = new bool[ships.Length];
        for (int i = 0; i < ships.Length; i++)
        {
            bool sts = ships[i].getPlaced();
            statuses[i] = sts;
        }

        //column to edit = row 110
        //starting at y = 9 ending at y = 17.

        int count = 0;
        for (int i = 9; i < 18; i += 2)
        {
            bool sts = ships[count].getPlaced();
            var temp = ships[count].getHits();

            int shipHits = 0;
            foreach (var j in temp)
            {
                shipHits++;
            }

            int totalPositionsOfShip = ships[count].Length;

            char[] rowToEdit = grid[i].ToCharArray();
            if (sts == true)
            {
                int health = totalPositionsOfShip - shipHits;

                if (health == totalPositionsOfShip)
                {
                    rowToEdit[110] = '(';
                    grid[i] = new string(rowToEdit);
                }
                if (health < 1)
                {
                    rowToEdit[110] = ')';
                    grid[i] = new string(rowToEdit);
                }
                if (health >= 1 && health < totalPositionsOfShip)
                {
                    rowToEdit[110] = '&';
                    grid[i] = new string(rowToEdit);
                }

            }
            count++;
        }
    } //change status of all ships

    //GAME -----------------------------------

    static string[] createGrid()
    {
        GameAssets g = new GameAssets();
        string[] grid = g.Grid;

        return grid;
    } //creates a grid from GameAssets and returns it

    static Ship[] createPlayerShips()
    {
        Ship[] playerShips = new Ship[5];

        string[] shipNames = new string[5] { "Carrier", "Battleship", "Destroyer", "Submarine", "Frigate" };
        int[] shipLengths = new int[5] { 5, 4, 3, 3, 2 };

        for (int i = 0; i < 5; i++)
        {
            Ship newShip = new Ship(shipLengths[i], shipNames[i]);
            playerShips[i] = newShip;
        }

        return playerShips;
    } //creates the ships for each player and returns then as an array

    static bool isShipAtPosition(string c, string r, Ship[] playerShips)
    {
        string targetCoord = string.Join(';', c, r);
        foreach (Ship i in playerShips)
        {
            string[] pos = i.getPositions();
            foreach (string j in pos)
            {
                if (j == targetCoord)
                {
                    return true;
                }
            }
        }

        return false;
    } //is there a ship at a coordinate letter;number a1

    //GAME : CPU

    static void cpuShips(string[] grid, Ship[] ships)
    {
        //create random row ; will always be correct (between 1-10)
        //create random col ; (between ASCII 97-106 a-j
        //get next ships length
        //get valid directions for next ship ; will it collide with edge or other ships.
        //create ship with random valid direction.
        //back to start until 5 ships placed.

        string[] directions = new string[] { "up", "right", "down", "left" };
        List<string> forbiddenCoordinates = new List<string>();

        for (int i = 0; i < 5; i++)
        {
            while (true)
            {
                Random rnd = new Random();
                char rndColumn = (char)rnd.Next(97, 107);
                int rndRow = rnd.Next(1, 11);

                string column = rndColumn.ToString();
                string row = rndRow.ToString();

                ships[i].StartColumn = column;
                ships[i].StartRow = row;

                bool validNonCollide = false;

                for (int d = 0; d < 4; d++)
                {
                    string dir = directions[rnd.Next(0, 4)];
                    ships[i].Direction = dir;
                    validNonCollide = ships[i].createAllShipPositions();
                    if (validNonCollide == true)
                    {
                        break;
                    }
                }
                if (validNonCollide == false)
                {
                    continue;
                }

                bool validShipCollide = true;
                foreach (string p in forbiddenCoordinates)
                {
                    string[] posToCheck = ships[i].getPositions();
                    if (posToCheck.Contains(p) == true)
                    {
                        validShipCollide = false;
                        break;
                    }
                }
                if (validShipCollide == false)
                {
                    continue;
                }
                string[] posToAdd = ships[i].getPositions();
                foreach (string p in posToAdd)
                {
                    forbiddenCoordinates.Add(p);
                }

                ships[i].setPlaced(true);
                break;
            }
        }
    } //will place ships randomly onto board CPU

    static void cpuGuessAndUpdate(string[] currentPGrid, string[] oppGrid,
    Ship[] currentPShips, Ship[] oppShips, List<string> previousGuesses, List<string> allPositions)
    {
        //cpu ships are p1s, player is p2s.

        //generate list of all positions.
        //get random guess.
        //check position
        //remove from list
        while (true)
        {
            if (allPositions.Count() < 1)
            {
                break;
            }
            Random rnd = new Random();
            int guessPositionInList = rnd.Next(0, allPositions.Count());
            string[] guess = allPositions[guessPositionInList].Split(';');
            string column = guess[0];
            string row = guess[1];


            bool isHitOrMiss = isShipAtPosition(column, row, oppShips);
            if (isHitOrMiss == true)
            {
                drawToBoardSetCoord(currentPGrid, column, row, ')', true);
                drawToBoardSetCoord(oppGrid, column, row, ')', false);

                foreach (Ship i in oppShips)
                {
                    string[] pos = i.getPositions();
                    string targetCoords = string.Join(';', column, row);
                    if (pos.Contains(targetCoords) == true)
                    {
                        i.addHit(targetCoords);
                    }
                }

                updateShipStatus(currentPGrid, currentPShips);
                displayGame(currentPGrid, true);

                Console.WriteLine("Hit at {0},{1}! Press any key to start the next turn...", column, row);
                Console.ReadKey();
            }
            if (isHitOrMiss == false)
            {
                drawToBoardSetCoord(currentPGrid, column, row, '=', true);
                drawToBoardSetCoord(oppGrid, column, row, '=', false);

                updateShipStatus(currentPGrid, currentPShips);
                displayGame(currentPGrid, true);

                Console.WriteLine("Miss at {0},{1}! Press any key to start the next turn...", column, row);
                Console.ReadKey();
            }
            previousGuesses.Add(string.Join(';', column, row));

            break;
            previousGuesses.Add(string.Join(';', guess[0], guess[1]));

            break;
        }
    } //get a guess from the CPU, check hit or miss and update boards accordinly

    //GAME : PLAYER

    static void ships(string[] grid, Ship[] playerShips, bool turn)
    {
        //1.create the list of ships i need to place onto the board.
        //[while all ships not place]
        //2.get each term of the command.
        //3.check shipname is valid
        //4.check coords are valid
        //5.check direction of new ship
        //6.if place/move
        //    [place]
        //    check all coords are not forbidden.
        //    place ship with shipname starting at coords with direction and length of shipname.
        //    add all coords to forbidden coords.
        //    [move]
        //    remove ship coords from forbidden coords.
        //    [place]
        //7.draw new ship to board.
        //8.update player screen.
        //9.if all ships are placed[end] else continue

        string[] shipNames = new string[5];
        for (int i = 0; i < 5; i++)
        {
            shipNames[i] = playerShips[i].Name;
        }

        List<string[]> forbiddenCoordinates = new List<string[]>();

        int shipsPlaced = 0;
        while (shipsPlaced < 5)
        {
            updateShipStatus(grid, playerShips);
            displayGame(grid, turn);
            string[] command = new string[5];

            Console.WriteLine("[Place/Move] [Shipname] [Start Position] [Directon]");
            string commandTemp = Console.ReadLine().ToLower();
            if(string.IsNullOrWhiteSpace(commandTemp) == true) //check for null
            {
                error("command supplied is null.");
                continue;
            }
            command = checkCommand_placemove(commandTemp, shipNames);

            int err = Convert.ToInt32(command[0]);
            if (err < 0)
            {
                switch(err)
                {
                    case -1:
                        {
                            error("you have not entered a valid action.");
                            break;
                        }
                    case -2:
                        {
                            error("ensure you enter all commands.");
                            break;
                        }
                    case -3:
                        {
                            error("the ship you have entered is not valid.");
                            break;
                        }
                    case -4:
                        {
                            error("the direction you have entered is not valid.");
                            break;
                        }
                }
                continue;
            }

            string action = command[1].ToLower();
            string shipTarget = command[2].ToLower();
            string startPosition = command[3].ToLower();
            string direction = command[4].ToLower();

            int shipArrPostition = 0;
            for (int i = 0; i < 5; i++)
            {
                string nameToCheck = playerShips[i].Name.ToLower();
                char firstCharOfNameToCheck = nameToCheck.ToCharArray()[0];

                if (nameToCheck == shipTarget)
                {
                    shipArrPostition = i;
                    break;
                }
                if(firstCharOfNameToCheck.ToString() == shipTarget)
                {
                    shipArrPostition = i;
                    break;
                }
            }

            if (action.ToLower() == "place")
            {
                bool shipAlreadyPlaced = playerShips[shipArrPostition].getPlaced();
                if (shipAlreadyPlaced == true)
                {
                    error("You have already placed this ship, use the 'move' command.");
                    continue;
                }

                string startColumn = startPosition.Split(';')[0];
                string startRow = startPosition.Split(';')[1];

                playerShips[shipArrPostition].Direction = direction;
                playerShips[shipArrPostition].StartColumn = startColumn;
                playerShips[shipArrPostition].StartRow = startRow;

                bool validPositions = playerShips[shipArrPostition].createAllShipPositions();
                if (validPositions == false)
                {
                    error("your ship will not fit there. Try placing somewhere else.");
                    continue;
                }

                bool collide = false;
                foreach (string i in playerShips[shipArrPostition].getPositions())
                {
                    foreach (string[] j in forbiddenCoordinates)
                    {
                        if (j.Contains(i))
                        {
                            collide = true;
                        }
                    }
                }
                if (collide == true)
                {
                    error("you already have a ship there.");
                    continue;
                }

                playerShips[shipArrPostition].setPlaced(true);
                forbiddenCoordinates.Add(playerShips[shipArrPostition].getPositions());

                drawShipToBoard(grid, playerShips[shipArrPostition]);
                shipsPlaced++;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nCommand Successful.. Any key to continue..");
                Console.ResetColor();
                Console.ReadKey();
                Thread.Sleep(200);

                continue;
            }
            if (action.ToLower() == "move")
            {
                bool shipAlreadyPlaced = playerShips[shipArrPostition].getPlaced();
                if (shipAlreadyPlaced == false)
                {
                    error("You have not placed this ship yet, use the 'place' command.");
                    continue;
                }

                Ship shipToRemove = new Ship(0, "");
                shipToRemove.setPositions(playerShips[shipArrPostition].getPositions());

                string startColumn = startPosition.Split(';')[0];
                string startRow = startPosition.Split(';')[1];

                playerShips[shipArrPostition].Direction = direction;
                playerShips[shipArrPostition].StartColumn = startColumn;
                playerShips[shipArrPostition].StartRow = startRow;

                bool validPositions = playerShips[shipArrPostition].createAllShipPositions();
                if (validPositions == false)
                {
                    error("your ship wont fit there. Try placing somewhere else.");
                    continue;
                }

                int positionsToRemove = 0;
                foreach (string[] i in forbiddenCoordinates)
                {
                    if (i[0] == shipToRemove.getPositions()[0])
                    {
                        break;
                    }
                    positionsToRemove++;
                }
                forbiddenCoordinates.RemoveAt(positionsToRemove);


                bool collide = false;
                foreach (string i in playerShips[shipArrPostition].getPositions())
                {
                    foreach (string[] j in forbiddenCoordinates)
                    {
                        if (j.Contains(i))
                        {
                            collide = true;
                        }
                    }
                }
                if (collide == true)
                {
                    error("you already have a ship there.");
                    continue;
                }

                forbiddenCoordinates.Add(playerShips[shipArrPostition].getPositions());

                removeShipFromBoard(grid, shipToRemove, false);
                drawShipToBoard(grid, playerShips[shipArrPostition]);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nCommand Successful.. Any key to continue..");
                Console.ResetColor();
                Console.ReadKey();
                Thread.Sleep(200);

                continue;
            }
        }
        updateShipStatus(grid, playerShips);
        displayGame(grid, turn);
    } //place the players ships onto the board

    static string[] checkCommand_placemove(string command, string[] shipNames)
    {
        string[] cmd = new string[5] { "-1", "-1", "-1", "-1", "-1" };
        foreach(string s in shipNames)
        {
            s.ToLower();
        }
        while (true)
        {
            //returns - at index 0 for error. change 1-4 for each command term.
            command = command.Trim();
            string[] commandTerms = command.Split();
            if (commandTerms.Length != 4) //length of command is 4
            {
                cmd[0] = "-2";
                return cmd;
            }

            for (int i = 1; i < 5; i++)
            {
                cmd[i] = commandTerms[i - 1];
            }

            string action = cmd[1];
            string shipTarget = cmd[2];
            string coordinates = cmd[3];
            string direction = cmd[4];

            switch(action)
            {
                case "place":
                    {
                        break;
                    }
                case "p":
                    {
                        action = "place";
                        break;
                    }
                case "move":
                    {
                        break;
                    }
                case "m":
                    {
                        action = "move";
                        break;
                    }
                default:
                    {
                        cmd[0] = "-1";
                        return cmd;
                    }

            }

            bool foundShipName = false;
            while (true) //is shipTarget valid.
            {
                foreach(string i in shipNames)
                {
                    string t = i.ToLower();
                    char firstChar = t[0];
                    char shipTargetFirstChar = shipTarget[0];
                    if(i.ToLower() == shipTarget)
                    {
                        foundShipName = true;
                        break;
                    }
                    if(firstChar == shipTargetFirstChar)
                    {
                        foundShipName = true;
                        break;
                    }
                }

                break;
            }
            if(foundShipName == false)
            {
                cmd[0] = "-3";
                return cmd;
            }

            switch (direction.ToLower())
            {
                case "up":
                    {
                        break;
                    }
                case "u":
                    {
                        direction = "up";
                        break;
                    }
                case "right":
                    {
                        break;
                    }
                case "r":
                    {
                        direction = "right";
                        break;
                    }
                case "down":
                    {
                        break;
                    }
                case "d":
                    {
                        direction = "down";
                        break;
                    }
                case "left":
                    {
                        break;
                    }
                case "l":
                    {
                        direction = "left";
                        break;
                    }
                default:
                    {
                        cmd[0] = "-4";
                        return cmd;
                    }

            }

            char[] coordsTemp = coordinates.ToCharArray();

            string columnCoordinate = coordsTemp[0].ToString();
            string rowCoordinate = "";
            for(int i = 1; i < coordsTemp.Length; i++)
            {
                rowCoordinate = rowCoordinate + coordsTemp[i];
            }

            switch (columnCoordinate.ToLower())
            {
                case "a":
                    {
                        break;
                    }
                case "b":
                    {
                        break;
                    }
                case "c":
                    {
                        break;
                    }
                case "d":
                    {
                        break;
                    }
                case "e":
                    {
                        break;
                    }
                case "f":
                    {
                        break;
                    }
                case "g":
                    {
                        break;
                    }
                case "h":
                    {
                        break;
                    }
                case "i":
                    {
                        break;
                    }
                case "j":
                    {
                        break;
                    }
                default:
                    {
                        cmd[0] = "-5";
                        return cmd;
                    }
            }
            switch (rowCoordinate)
            {
                case "1":
                    {
                        break;
                    }
                case "2":
                    {
                        break;
                    }
                case "3":
                    {
                        break;
                    }
                case "4":
                    {
                        break;
                    }
                case "5":
                    {
                        break;
                    }
                case "6":
                    {
                        break;
                    }
                case "7":
                    {
                        break;
                    }
                case "8":
                    {
                        break;
                    }
                case "9":
                    {
                        break;
                    }
                case "10":
                    {
                        break;
                    }
                default:
                    {
                        cmd[0] = "-5";
                        return cmd;
                    }
            }

            cmd[0] = "0";
            cmd[1] = action;
            cmd[2] = shipTarget;
            cmd[3] = string.Join(';', columnCoordinate, rowCoordinate);
            cmd[4] = direction;
            break;
        }

        return cmd;
    } //check command for placing player ships is correct, returns string[] of command terms

    static void gameTurn(string[] currentPGrid, string[] oppGrid, Ship[] currentPShips, Ship[] oppShips, List<string> playerGuesses, List<string> oppGuesses, bool turn)
    {
        //update board.
        //get a guess from the player.
        //check guess is valid.
        //check if the guess is a hit or miss.
        //add player one guess to playerOneGrid, board2.
        //add player one guess to playetTwoGrid, board1;

        while (true)
        {
            updateShipStatus(currentPGrid, currentPShips);
            displayGame(currentPGrid, turn);

            string mg = string.Join(' ', playerGuesses);
            Console.WriteLine("My Guesses: " + mg + " ");

            string mh = "";
            foreach (Ship i in oppShips)
            {
                var htemp = i.getHits();
                mh = mh + string.Join(',', htemp);
            }
            Console.WriteLine("My Hits: " + mh + " ");

            string og = string.Join(' ', oppGuesses);
            Console.WriteLine("\nOpponent Guesses: " + og + " ");

            string oh = "";
            foreach (Ship i in currentPShips)
            {
                var htemp = i.getHits();
                oh = oh + string.Join(',', htemp);
            }
            Console.WriteLine("Opponent Hits: " + oh + " ");

            Console.WriteLine("\n[Column][Row]");

            string cmd = Console.ReadLine();
            if(cmd == null)
            {
                error("command you have entered is null.");
                continue;
            }
            cmd = cmd.Trim();
            string[] command = checkCommand_guess(cmd, playerGuesses);

            int err = Convert.ToInt32(command[0]);
            if (err < 0)
            {
                //TODO errors for checking guesses.
                Console.WriteLine(err.ToString());
                Console.ReadKey();
                continue;
            }

            string column = command[1];
            string row = command[2];

            bool isHitOrMiss = isShipAtPosition(column, row, oppShips);
            if (isHitOrMiss == true)
            {
                drawToBoardSetCoord(currentPGrid, column, row, ')', true);
                drawToBoardSetCoord(oppGrid, column, row, ')', false);

                foreach (Ship i in oppShips)
                {
                    string[] pos = i.getPositions();
                    string targetCoords = string.Join(';', column, row);
                    if (pos.Contains(targetCoords) == true)
                    {
                        i.addHit(targetCoords);
                    }
                }

                updateShipStatus(currentPGrid, currentPShips);
                displayGame(currentPGrid, turn);

                Console.WriteLine("Hit at {0},{1}! Press any key to start the next turn...", column, row);
                Console.ReadKey();
            }
            if (isHitOrMiss == false)
            {
                drawToBoardSetCoord(currentPGrid, column, row, '=', true);
                drawToBoardSetCoord(oppGrid, column, row, '=', false);

                updateShipStatus(currentPGrid, currentPShips);
                displayGame(currentPGrid, turn);

                Console.WriteLine("Miss at {0},{1}! Press any key to start the next turn...", column, row);
                Console.ReadKey();
            }
            playerGuesses.Add(string.Join(';', column, row));

            break;
        }
    } //one turn PLAYER

    static string[] checkCommand_guess(string command, List<string> guesses)
    {
        string[] cmd = new string[3] { "-1", "-1", "-1" };

        while (true)
        {
            string[] commandTerms = new string[2];
            char[] temp = command.ToCharArray();
            commandTerms[0] = temp[0].ToString();
            commandTerms[1] = "";
            for(int i = 1; i < temp.Length; i++)
            {
                commandTerms[1] = commandTerms[1] + temp[i];
            }

            if (commandTerms.Length != 2)
            {
                break;
            }

            cmd[1] = commandTerms[0];
            string columnCoordinate = cmd[1];
            cmd[2] = commandTerms[1];
            string rowCoordinate = cmd[2];

            string targetCoord = string.Join(';', columnCoordinate, rowCoordinate);
            if (guesses.Contains(targetCoord) == true)
            {
                cmd[0] = "-4";
                return cmd;
            }

            switch (columnCoordinate.ToLower())
            {
                case "a":
                    {
                        break;
                    }
                case "b":
                    {
                        break;
                    }
                case "c":
                    {
                        break;
                    }
                case "d":
                    {
                        break;
                    }
                case "e":
                    {
                        break;
                    }
                case "f":
                    {
                        break;
                    }
                case "g":
                    {
                        break;
                    }
                case "h":
                    {
                        break;
                    }
                case "i":
                    {
                        break;
                    }
                case "j":
                    {
                        break;
                    }
                default:
                    {
                        cmd[0] = "-2";
                        return cmd;
                    }
            }
            switch (rowCoordinate)
            {
                case "1":
                    {
                        break;
                    }
                case "2":
                    {
                        break;
                    }
                case "3":
                    {
                        break;
                    }
                case "4":
                    {
                        break;
                    }
                case "5":
                    {
                        break;
                    }
                case "6":
                    {
                        break;
                    }
                case "7":
                    {
                        break;
                    }
                case "8":
                    {
                        break;
                    }
                case "9":
                    {
                        break;
                    }
                case "10":
                    {
                        break;
                    }
                default:
                    {
                        cmd[0] = "-3";
                        return cmd;
                    }
            }

            cmd[0] = "0";
            break;
        }

        return cmd;
    } //checks command for guessing is correct


    //TEST -----------------------------------

    static void sampleGame(string[] p1g, string[] p2g, Ship[] p1s, Ship[] p2s)
    {
        cpuShips(p1g, p1s);
        cpuShips(p2g, p2s);
    } //sets up a sample game option 0 in the menu.
}