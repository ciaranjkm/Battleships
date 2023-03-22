using System;

namespace Battleships
{
    public class Ship
    {
        public string StartRow { get;  set; }
        public string StartColumn { get; set; }

        public string Direction { get; set; }
        public int Length { get; }

        public string Name { get; }

        private string[] positions;
        private List<string> hits;

        public bool placed { get; set; } 

        //When creating a new ship it will set its key starting properties.
        public Ship(int l, string n)
        {
            StartRow = "";
            StartColumn = "";

            Direction = "";
            Length = l;

            Name = n;

            positions = new string[l];
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

        public void setPositions(string[] p)
        {
            positions = p;
        }
        public string[] getPositions()
        {
            return positions;
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

            positions = new string[Length];
            positions[0] = string.Join(';', StartColumn, StartRow);

            //If Direction is 0(up)
            if (Direction == "up")
            {
                int nextRow = Int32.Parse(StartRow);

                for (int i = 1; i < positions.Length; i++)
                {
                    nextRow++;

                    if (nextRow == 11)
                    {
                        Console.WriteLine("Invalid Direction.");
                        valid = false;
                    }

                    positions[i] = string.Join(';', StartColumn, nextRow.ToString());
                }
            }
            //If Direction is 1(right)
            if (Direction == "right")
            {
                char[] t = StartColumn.ToCharArray();
                int nextColumn = (int)t[0];

                for (int i = 1; i < positions.Length; i++)
                {
                    nextColumn++;

                    if (nextColumn > 106)
                    {
                        Console.WriteLine("Invalid Direction.");
                        valid = false;
                    }

                    positions[i] = string.Join(';', (char)nextColumn, StartRow);
                }
            }
            //If Direction is 2(down)
            if (Direction == "down")
            {
                int nextRow = Int32.Parse(StartRow);

                for (int i = 1; i < positions.Length; i++)
                {
                    nextRow--;

                    if (nextRow == 0)
                    {
                        Console.WriteLine("Invalid Direction.");
                        valid = false;
                    }
                    positions[i] = string.Join(';', StartColumn, nextRow.ToString());
                }
            }
            //If Direction is 3(left)
            if (Direction == "left")
            {
                char[] t = StartColumn.ToCharArray();
                int nextColumn = (int)t[0];

                for (int i = 1; i < positions.Length; i++)
                {
                    nextColumn--;

                    if (nextColumn < 97)
                    {
                        Console.WriteLine("Invalid Direction.");
                        valid = false;
                    }

                    positions[i] = string.Join(';', (char)nextColumn, StartRow);
                }
            }


            return valid;
        }
    }
}