using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
namespace PuzzlescriptFlickUtil
{
    class Program
    {
        static void Main(string[] args)
        {
            //string inputFile = args[0] + ".txt";
            //string outputFile = args[1] + ".txt";

            string inputFile = "input.txt";
            string outputFile = "output.txt";

            string read = File.ReadAllText(inputFile);
            string result = Work(read);
            File.WriteAllText(outputFile, result);
            //Console.ReadLine();
        }

        static string Work(string input)
        {
            string oldData;
            string rest;

            //Here, we check if this is a proper puzzlescript file, or just some arbitrary levels floating in the void.
            string regex = @"=+\s*[lL][eE][vV][eE][lL][sS]\s*=+";
            Match m = Regex.Match(input, regex);
            if (!m.Success)
            {
                return AreWeSplitting(input + Environment.NewLine);
            }
            else
            {
                oldData = input.Substring(0, m.Index + m.Length) + Environment.NewLine + Environment.NewLine;
                rest = input.Substring(m.Index + m.Length + 1);
                string result = AreWeSplitting(rest + Environment.NewLine);
                return oldData + result;
            }

        }


        enum LevelParseMode
        {
            SeekingMessage,
            SeekingLevelData
        }

        class Level
        {
            public char[,] symbols;
            public int w;
            public int h;
        }


        private static readonly string levelValue = @"\(\s*level\s*w([1-9][0-9]*)\s*h([1-9][0-9]*)\)";
        private static readonly string splitValue = @"\(\s*flickscreen\s*([1-9][0-9]*)x([1-9][0-9]*)\s*\)";

        static string AreWeSplitting(string levelInput)
        {
            if (Regex.IsMatch(levelInput, splitValue))
            {
                return SplitWork(levelInput);
            }
            else
                return LevelWork(levelInput);
        }
        static string SplitWork(string levelInput)
        {
            string[] splitter = { Environment.NewLine , "\n" };
            string[] lines = levelInput.Split(splitter, StringSplitOptions.None);

            bool waiting = true;
            int flickX = 0;
            int flickY = 0;
            List<Char> chars = new List<char>();
            int lineLength = -1;
            for(int z = 0; z < lines.Length;z++)
            {
                if(waiting)
                {
                    Match m = Regex.Match(lines[z].Trim(), splitValue);
                    if(m.Success)
                    {
                        flickX = int.Parse(m.Groups[1].Value);
                        flickY = int.Parse(m.Groups[2].Value);
                        waiting = false;
                    }
                }
                else
                {
                    string line = lines[z].Trim();
                    if(lines[z] == "")  //if we have reached an empty line, its time to parse.
                    {
                        if(lineLength == -1)    //if we haven't read even a single line, uh oh.
                        {
                            //todo.
                            return "UH OH, ERROR, it seems";
                        }
                        int numLines = chars.Count / lineLength;
                        char[,] result = new char[numLines, lineLength];    //0,0 bottom left.
                        for(int i = 0; i < numLines;i++)
                            for(int j = 0; j < lineLength;j++)
                            {
                                result[i, j] = chars[(numLines - 1 - i) * lineLength + j];
                            }
                        return ConvertCharsToLevel(result, flickX, flickY);
                    }
                    if(lines[z] != "")  //if this is a line with characters,
                    {
                        lines[z].Trim();
                        if (lineLength == -1)
                            lineLength = line.Length;
                        if(lineLength != line.Length)
                        {
                            return "UH OH, ERROR, it seems that your splitted level doesn't have lines of the same length. Fix that!";
                        }
                        chars.AddRange(line);


                    }
                }
            }
            if (waiting)
                return "";
            return "";
            //1: get just the level itself. All that good level data.
        }

        static string ConvertCharsToLevel(char[,] levelInput, int levelWidth, int levelHeight)
        {
            int height = levelInput.GetLength(0);
            int width = levelInput.GetLength(1);
            int numLevelsWidth = width / levelWidth;
            int numLevelsHeight = height / levelHeight;
            string building = "";
            for (int i = 0; i < numLevelsWidth; i++)
                for (int j = 0; j < numLevelsHeight; j++)
            {
                    string level = "(level w" + (i + 1) + " h" + (j + 1) + ")" + Environment.NewLine;
                    char[,] symbols = new char[levelHeight, levelWidth];    //0,0 bottom left 
                    for(int y = levelHeight * j; y < levelHeight * (j + 1); y++)
                        for(int x = levelWidth * i; x < levelWidth * (i + 1);x++)
                        {
                            int a = y - levelHeight * j;
                            int b = x - levelWidth * i;
                            symbols[a, b] = levelInput[y,x];
                        }
                    string next = ToMyString(symbols) + Environment.NewLine + Environment.NewLine;
                    building = building + level + next;
            }
            return building;
        }
        static string LevelWork(string levelInput)
        {
            string[] splitter = { Environment.NewLine };
            string[] lines = levelInput.Split(splitter, StringSplitOptions.None);

            List<Level> levels = new List<Level>();
            Level currentLevel = new Level();
            List<char> currentLevelChars = new List<char>();
            int levelWidth = -1;
            LevelParseMode mode = LevelParseMode.SeekingMessage;
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (mode == LevelParseMode.SeekingMessage)
                {
                    Match m = Regex.Match(line, levelValue);
                    //if string is empty, keep looking for message.
                    if (line == "")
                    {
                        continue;
                    }
                    if(m.Success)
                    {
                        string w = m.Groups[1].Value;
                        string h = m.Groups[2].Value;
                        mode = LevelParseMode.SeekingLevelData;
                        currentLevel.w = int.Parse(w) - 1;
                        currentLevel.h = int.Parse(h) - 1;
                    }
                    else
                    {
                        
                        Console.Write(i + ": we found line " + line + "; this line isn't a level message. We want a level message. ");
                        return "";
                    }
                }
                else if (mode == LevelParseMode.SeekingLevelData)
                {
                    //if we find a line.
                    if(line != "")
                    {
                        if(levelWidth == -1)
                        {
                            levelWidth = line.Length;
                            for (int a = 0; a < line.Length; a++)
                                currentLevelChars.Add(line[a]);
                        }
                        else
                        {
                            if(levelWidth != line.Length)
                            {
                                Console.Write("when parsing a level, this line here: " + lines[i] + "; length didn't match what we wanted ");
                                return "";
                            }
                            else
                            {
                                for (int a = 0; a < line.Length; a++)
                                    currentLevelChars.Add(line[a]);
                            }
                        }
                    }
                    //if we find nothing.
                    if (line == "" || i == lines.Length - 1)
                    {
                        if(i == lines.Length - 1)
                        {
                            Console.Write("HERE. WE. ARE");
                        }
                        //check that we have a level to submit.
                        if (levelWidth == -1)
                        {
                            continue;
                        }
                        else
                        {
                            int numLines = currentLevelChars.Count / levelWidth;
                            char[,] temp = new char[numLines, levelWidth];
                            for (int a = 0; a < numLines; a++)
                                for (int b = 0; b < levelWidth; b++)
                                {
                                    int aAlter = numLines - 1 - a;
                                    temp[aAlter, b] = currentLevelChars[a * levelWidth + b];
                                }

                            currentLevel.symbols = temp;
                            levels.Add(currentLevel);
                            currentLevel = new Level();
                            mode = LevelParseMode.SeekingMessage;
                            currentLevelChars.Clear();
                        }

                    }
                }
            }


            return TurnLevelsIntoBigString(levels.ToArray());
        }

        static string TurnLevelsIntoBigString(Level[] levels)
        {
            int xSize = levels[0].symbols.GetLength(1);
            int ySize = levels[0].symbols.GetLength(0);
            int xMax = levels[0].w;
            int yMax = levels[0].h;
            for(int i = 1; i < levels.Length;i++)
            {
                if (xSize != levels[i].symbols.GetLength(1)
                    || ySize != levels[i].symbols.GetLength(0))
                {
                    Console.Write("Umm. One of the levels doesn't have the right number of rows. Fix that!");
                    return "";
                }
                xMax = Math.Max(xMax, levels[i].w);
                yMax = Math.Max(yMax, levels[i].h);
            }

            int height = (yMax + 1) * ySize;
            int width = (xMax + 1) * xSize;
            char[,] result = new char[height,width];


            for(int i = 0; i < height;i++)
                for(int j = 0; j < width;j++)
                {
                    result[i, j] = '.';
                }

            for(int i = 0; i < levels.Length;i++)
            {
                int xStart = levels[i].w * xSize;
                int yStart = levels[i].h * ySize;
                
                for(int y = yStart; y < yStart + ySize;y++)
                    for(int x = xStart; x < xStart + xSize;x++)
                    {
                        int a = y - yStart;
                        int b = x - xStart;
                        char toPlace = levels[i].symbols[a, b];
                        result[y, x] = toPlace;
                    }
            }
            string flickResult = "( flickscreen " + xSize + "x" + ySize + ")\n";
            return flickResult + ToMyString(result);
        }

        static string ToMyString(Char[,] array)
        {
            List<char> result = new List<char>();   //0,0 is top right.
            int h = array.GetLength(0);
            int w = array.GetLength(1);
            for(int i = 0; i < h;i++)  //h
            {
                for (int j = 0; j < w; j++) //w
                {
                    int y = h - 1 - i;
                    result.Add(array[y, j]);
                }
                result.AddRange(Environment.NewLine);
            }

            return new string(result.ToArray());
        }
    }

}
