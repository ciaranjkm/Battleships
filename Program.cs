namespace Battleships;

using System.IO;
using System.Text;
using System.Linq;
using System.Diagnostics.Metrics;
using System.Xml.Linq;
using System.Numerics;

<<<<<<< HEAD
// This is a new version
=======
//This is a new version
>>>>>>> ba8b67907cbd652561dc5f16292535fe386770aa

class Program
{
    // drawship called -> calls to get coords to draw -> draws one coords moves to the next.

    static void Main(string[] args)
    {//TODO convert text file use to gameassets class
        while (true)
        {
            Console.Clear();
            //Calls menu routine as soon as the program opens, will exit while () when
            //valid choiceMade is true.

            bool vsCPU = false;
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

            ////Create a grid for each player.
            var playerOneGrid = createGrid();
            var playerTwoGrid = createGrid();

            ////Create an array of ships for each player and place them onto the grid.
            Ship[] playerOneShips = createPlayerShips();
            Ship[] playerTwoShips = createPlayerShips();

            //Create list of guesses each player has made.
            List<string> playerOneGuesses = new List<string>();
            List<string> playerTwoGuesses = new List<string>();
            List<string> cpuGuesses1 = new List<string>();
            List<string> cpuGuesses2 = new List<string>();

            int playerWon = 0;

            bool turn = false;
            switch (choice)
            {
                //1 for pass and play
                //2 for vs cpu
                //0 for cpu vs cpu testing.
                case 1:
                    {
                        //Place ships onto the board.
                        ships(playerOneGrid, playerOneShips, 1);
                        ships(playerTwoGrid, playerTwoShips, 2);

                        while (true)
                        {
                            if (turn == false)
                            {
                                gameTurn(playerOneGrid, playerTwoGrid, playerOneShips, playerTwoShips, playerOneGuesses, 1);
                            }
                            if (turn == true)
                            {
                                gameTurn(playerTwoGrid, playerOneGrid, playerTwoShips, playerOneShips, playerTwoGuesses, 2);
                            }

                            int p1Hits = 0;
                            int p2Hits = 0;

                            for (int i = 0; i < 5; i++)
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

                            turn = !turn;
                        }
                        break;
                    }
                case 2:
                    {
                        //TODO: cpu vs player.
                        cpuShips(playerTwoGrid, playerTwoShips);
                        foreach (Ship i in playerTwoShips)
                        {
                            drawShipToBoard(playerTwoGrid, i);
                        }

                        displayGame(playerTwoGrid, 2);

                        Console.ReadKey();


                        break;
                    }
                case 0:
                    {
                        sampleGame(playerOneGrid, playerTwoGrid, playerOneShips, playerTwoShips);
                        for (int i = 0; i < 5; i++)
                        {
                            drawShipToBoard(playerOneGrid, playerOneShips[i]);
                            drawShipToBoard(playerTwoGrid, playerTwoShips[i]);
                        }

                        string[] columns = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" };
                        string[] rows = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };

                        List<string> allPositions = new List<string>();
                        for (int c = 0; c < 10; c++)
                        {
                            for (int r = 0; r < 10; r++)
                            {
                                string posToAdd = string.Join(';', columns[c], rows[r]);
                                cpuGuesses1.Add(posToAdd);
                                cpuGuesses2.Add(posToAdd);
                            }
                        }

                        while (true)
                        {
                            int p1Hits = 0;
                            int p2Hits = 0;

                            cpuGuessAndUpdate(playerOneGrid, playerTwoGrid, playerOneShips, playerTwoShips, playerOneGuesses, cpuGuesses1);
                            cpuGuessAndUpdate(playerTwoGrid, playerOneGrid, playerTwoShips, playerOneShips, playerTwoGuesses, cpuGuesses2);

                            for (int i = 0; i < 5; i++)
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

                            Console.WriteLine(p1Hits.ToString());
                            Console.WriteLine(p2Hits.ToString());
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


                        displayGame(playerOneGrid, 1);
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
                        displayGame(playerTwoGrid, 2);
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
                    }
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
            choice = Int32.Parse(Console.ReadLine());

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
    }

    static void error(string messgage)
    {
        Console.WriteLine("\nError: " + messgage);
        Console.Write("Press any key to try again...");
        Console.ReadKey();
        Console.Clear();
    }

    static void displayGame(string[] grid, int titleToDisplay)
    {
        Console.Clear();
        displayTitle(titleToDisplay);
        Console.WriteLine();
        displayBoard(grid);
        Console.WriteLine();
    }

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
    }

    static void displayTitle(int titleToDisplay)
    {
        GameAssets g = new GameAssets();
        string[] title = g.Titles[titleToDisplay];

        foreach(string t in title)
        {
            Console.Write(t);
        }
    }

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
    }

    static int[] getCoordsForBoardTwo(string c, string r)
    {
        int[] coords = getCoordsForBoardOne(c, r);
        coords[0] = coords[0] + 45; // offset x only y is the same.

        return coords;
    }


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
    }

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

            int totalPositionsOfShip = ships[count].getLength();

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
    }

    //GAME -----------------------------------

    static string[] createGrid()
    {
        GameAssets g = new GameAssets();
        string[] grid = g.Grid;

        return grid;
    }

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
    }

    static void ships(string[] grid, Ship[] playerShips, int player)
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
            shipNames[i] = playerShips[i].getName();
        }

        List<string[]> forbiddenCoordinates = new List<string[]>();

        int shipsPlaced = 0;
        while (shipsPlaced < 5)
        {
            updateShipStatus(grid, playerShips);
            displayGame(grid, player);
            string[] command = new string[5];
            string commandTemp = string.Empty;

            Console.WriteLine("[Place/Move] [Shipname] [Start Position] [Directon]");
            commandTemp = Console.ReadLine().ToLower();
            command = checkCommand_placemove(commandTemp, shipNames);

            int err = Convert.ToInt32(command[0]);
            if (err < 0)
            {
                //TODO:create error system when entering commands
                Console.WriteLine(err.ToString());
                Console.ReadKey();
                continue;
            }

            string action = command[1].ToLower();
            string shipTarget = command[2].ToLower();
            string startPosition = command[3].ToLower();
            string direction = command[4].ToLower();

            int shipArrPostition = 0;
            for (int i = 0; i < 5; i++)
            {
                string nameToCheck = playerShips[i].getName().ToLower();
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
                    error("You have already placed this ship, use the 'move' command. [-2]");
                    continue;
                }

                char[] coordsTemp = startPosition.ToCharArray();
                string startColumn = coordsTemp[0].ToString();
                string startRow = "";
                for(int i = 1; i < coordsTemp.Length; i++)
                {
                    startRow = startRow + coordsTemp[i];
                }

                playerShips[shipArrPostition].setDirection(direction);
                playerShips[shipArrPostition].setStartColumn(startColumn);
                playerShips[shipArrPostition].setStartRow(startRow);

                bool validPositions = playerShips[shipArrPostition].createAllShipPositions();
                if (validPositions == false)
                {
                    error("Error creating your ship. Your direction is invalid. [-1]");
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
                    error("You already have a ship there. [-3]");
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
                Thread.Sleep(500);
                continue;
            }
            if (action.ToLower() == "move")
            {
                bool shipAlreadyPlaced = playerShips[shipArrPostition].getPlaced();
                if (shipAlreadyPlaced == false)
                {
                    Console.WriteLine("You have not placed this ship, use the 'place' command. [-4]");
                    Console.ReadKey();
                    continue;
                }

                Ship shipToRemove = new Ship(0, "");
                shipToRemove.setPositions(playerShips[shipArrPostition].getPositions());

                char[] coordsTemp = startPosition.ToCharArray();
                string startColumn = coordsTemp[0].ToString();
                string startRow = "";
                for (int i = 1; i < coordsTemp.Length; i++)
                {
                    startRow = startRow + coordsTemp[i];
                }

                playerShips[shipArrPostition].setDirection(direction);
                playerShips[shipArrPostition].setStartColumn(startColumn);
                playerShips[shipArrPostition].setStartRow(startRow); ;

                bool validPositions = playerShips[shipArrPostition].createAllShipPositions();
                if (validPositions == false)
                {
                    error("Error creating your ship. Your direction is invalid. [-1");
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
                    error("You already have a ship there. [-3]");
                    continue;
                }

                forbiddenCoordinates.Add(playerShips[shipArrPostition].getPositions());

                removeShipFromBoard(grid, shipToRemove, false);
                drawShipToBoard(grid, playerShips[shipArrPostition]);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nCommand Successful.. Any key to continue..");
                Console.ResetColor();
                Console.ReadKey();
                Thread.Sleep(500);

                continue;
            }
        }
        updateShipStatus(grid, playerShips);
        displayGame(grid, player);
    }

    static string[] checkCommand_placemove(string command, string[] shipNames)
    {
        string[] cmd = new string[5] { "-1", "-1", "-1", "-1", "-1" };
        while (true)
        {
            //returns -1 at index 0 for error. change 1-4 for each command term.
            command = command.Trim();
            string[] commandTerms = command.Split();
            if (!(commandTerms.Length == 4))
            {
                cmd[0] = "-2";
                return cmd;
            }

            for (int i = 1; i < 5; i++)
            {
                cmd[i] = commandTerms[i - 1];
            }

            string shipTarget = cmd[2];
            string coordinates = cmd[3];
            string direction = cmd[4];

            if ((shipNames.Contains(shipTarget)))
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
                case "right":
                    {
                        break;
                    }
                case "down":
                    {
                        break;
                    }
                case "left":
                    {
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
                        cmd[0] = "-6";
                        return cmd;
                    }
            }

            cmd[0] = "0";
            break;
        }

        return cmd;
    }

    static void gameTurn(string[] currentPGrid, string[] oppGrid, Ship[] currentPShips, Ship[] oppShips, List<string> playerGuesses, int turn)
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

            string g = string.Join(' ', playerGuesses);
            Console.WriteLine("My Guesses: " + g);

            string mh = "";
            foreach (Ship i in oppShips)
            {
                var htemp = i.getHits();
                mh = mh + string.Join(',', htemp);
            }


            Console.WriteLine("My Hits: " + mh);
1
            string h = "";
            foreach (Ship i in currentPShips)
            {
                var htemp = i.getHits();
                h = h + string.Join(',', htemp);
            }

            Console.WriteLine("\nOpponent Hits: " + h);

            Console.WriteLine("\n[Column][Row]");

            string cmd = Console.ReadLine();
            cmd = cmd.Trim();
            string[] command = checkCommand_guess(cmd, playerGuesses);

            int error = Convert.ToInt32(command[0]);
            if (error < 0)
            {
                //TODO errors for checking guesses.
                Console.WriteLine(error.ToString());
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
    }

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
    }

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
    }

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

                ships[i].setStartColumn(column);
                ships[i].setStartRow(row);

                bool validNonCollide = false;

                for (int d = 0; d < 4; d++)
                {
                    string dir = directions[rnd.Next(0, 4)];
                    ships[i].setDirection(dir);
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
    }

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


            bool isHitOrMiss = isShipAtPosition(guess[0], guess[1], oppShips);
            if (isHitOrMiss == true)
            {
                drawToBoardSetCoord(currentPGrid, guess[0], guess[1], ')', true);
                drawToBoardSetCoord(oppGrid, guess[0], guess[1], ')', false);

                foreach (Ship i in oppShips)
                {
                    string[] pos = i.getPositions();
                    string targetCoords = string.Join(';', guess[0], guess[1]);
                    if (pos.Contains(targetCoords) == true)
                    {
                        i.addHit(targetCoords);
                    }
                }
            }
            if (isHitOrMiss == false)
            {
                drawToBoardSetCoord(currentPGrid, guess[0], guess[1], '=', true);
                drawToBoardSetCoord(oppGrid, guess[0], guess[1], '=', false);

                updateShipStatus(currentPGrid, currentPShips);
            }
            allPositions.RemoveAt(guessPositionInList);
            previousGuesses.Add(string.Join(';', guess[0], guess[1]));

            break;
        }
    }

    //TEST -----------------------------------

    static void sampleGame(string[] p1g, string[] p2g, Ship[] p1s, Ship[] p2s)
    {
        cpuShips(p1g, p1s);
        cpuShips(p2g, p2s);
    }
}