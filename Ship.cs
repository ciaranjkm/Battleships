using System;

namespace Battleships
{
    public class Ship
    {
        private string startRow = string.Empty;
        private string startColumn = string.Empty;
        private string direction;
        private int length;
        private string name;

        private string[] positions;
        private List<string> hits;

        bool placed;

        //When creating a new ship it will set its key starting properties.
        public Ship(int l, string n)
        {
            length = l;
            name = n;
            hits = new List<string>();

            placed = false;
        }

        public void addHit(string h)
        {
            hits.Add(h);
        }
        public List<string> getHits()
        {
            return hits;
        }

        public void setStartRow(string r)
        {
            startRow = r;
        }
        public string getStartRow()
        {
            return startRow;
        }

        public void setStartColumn(string c)
        {
            startColumn = c;
        }
        public string getStartColumn()
        {
            return startColumn;
        }

        public void setPositions(string[] p)
        {
            positions = p;
        }
        public string[] getPositions()
        {
            return positions;
        }

        public void setLength(int l)
        {
            length = l;
        }
        public int getLength()
        {
            return length;
        }

        public void setName(string n)
        {
            name = n;
        }
        public string getName()
        {
            return name;
        }

        public void setDirection(string d)
        {
            direction = d;
        }
        public string getDirection()
        {
            return direction;
        }

        public void setPlaced(bool s)
        {
            placed = s;
        }
        public bool getPlaced()
        {
            return placed;
        }

        //Method to create an array of string for the positions of the ship.
        //Returns true if ship created, returns false when invalid.
        public bool createAllShipPositions()
        {
            bool valid = true;

            positions = new string[length];
            positions[0] = string.Join(';', startColumn, startRow);

            //If direction is 0(up)
            if (direction == "up")
            {
                int nextRow = Int32.Parse(startRow);

                for (int i = 1; i < positions.Length; i++)
                {
                    nextRow++;

                    if (nextRow == 11)
                    {
                        Console.WriteLine("Invalid direction.");
                        valid = false;
                    }

                    positions[i] = string.Join(';', startColumn, nextRow.ToString());
                }
            }
            //If direction is 1(right)
            if (direction == "right")
            {
                char[] t = startColumn.ToCharArray();
                int nextColumn = (int)t[0];

                for (int i = 1; i < positions.Length; i++)
                {
                    nextColumn++;

                    if (nextColumn > 106)
                    {
                        Console.WriteLine("Invalid direction.");
                        valid = false;
                    }

                    positions[i] = string.Join(';', (char)nextColumn, startRow);
                }
            }
            //If direction is 2(down)
            if (direction == "down")
            {
                int nextRow = Int32.Parse(startRow);

                for (int i = 1; i < positions.Length; i++)
                {
                    nextRow--;

                    if (nextRow == 0)
                    {
                        Console.WriteLine("Invalid direction.");
                        valid = false;
                    }
                    positions[i] = string.Join(';', startColumn, nextRow.ToString());
                }
            }
            //If direction is 3(left)
            if (direction == "left")
            {
                char[] t = startColumn.ToCharArray();
                int nextColumn = (int)t[0];

                for (int i = 1; i < positions.Length; i++)
                {
                    nextColumn--;

                    if (nextColumn < 97)
                    {
                        Console.WriteLine("Invalid direction.");
                        valid = false;
                    }

                    positions[i] = string.Join(';', (char)nextColumn, startRow);
                }
            }


            return valid;
        }
    }
}