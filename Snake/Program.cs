namespace Snake
{
    class Snake
    {
        int height;
        int width;
        int speed;
        bool gameover;

        //Get Input
        ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();
        char key;

        //Snake
        int snakePosX;
        int snakePosY;
        char snakeDirection;
        int snakePrevPosX;
        int snakePrevPosY;

        //Snake parts
        int snakeParts;
        List<int> snakePartsPosX = new List<int>();
        List<int> snakePartsPosY = new List<int>();

        //fruit
        int fruitPosX;
        int fruitPosY;
        int fruitDiff = 0;

        Random random = new Random();

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            while (true)
            {
                Snake snake = new Snake();
                snake.ChooseDifficulty();
                Console.Clear();
                snake.gameover = false;
                snake.DrawArena();
                snake.CreateSnake();
                snake.CreateFirstFruit();
                snake.snakeDirection = 's';

                while (!snake.gameover)
                {
                    snake.Update();
                }

                Console.SetCursorPosition(snake.width + 10, snake.height / 2);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("GAME OVER");
                Console.ResetColor();
                Countdown(snake.width, snake.height,5);
            }
        }

        void ChooseDifficulty()
        {
            int userInput;
            bool success = true;

            while (success)
            {
                Console.Clear();
                Console.SetCursorPosition(0, 0);

                Console.WriteLine("Choose difficulty:");
                Console.WriteLine("1 - Easy");
                Console.WriteLine("2 - Normal");
                Console.WriteLine("3 - Pro");
                Console.WriteLine("4 - Cheater");

                Int32.TryParse(Console.ReadLine(), out userInput);
                switch (userInput)
                {
                    case 1:
                        SetDifficulty(20, 15, 150);
                        success = false;
                        break;
                    case 2:
                        SetDifficulty(40, 20, 100);
                        success = false;
                        break;
                    case 3:
                        SetDifficulty(40, 20, 75);
                        success = false;
                        break;
                    case 4:
                        SetDifficulty(40, 20, 25);
                        success = false;
                        break;
                    case 69:
                        SetDifficulty(30, 15, 69);
                        fruitDiff = 69;
                        success = false;
                        break;
                    case 420:
                        SetDifficulty(20, 10, 500);
                        fruitDiff = 420;
                        success = false;
                        break;
                    default:
                        break;
                }
            }
        }
        void SetDifficulty(int arenaX, int arenaY, int speed)
        {
            width = arenaX;
            height = arenaY;
            this.speed = speed;
        }
        void Update()
        {
            GetInput();
            DrawScore();

            //if we can play again, logic will return true
            if (!Logic())   
            {
                gameover = true;
                return;
            }
            if (snakeParts != 0)
            {
                DrawSnakeParts();
            }
            DrawFruit();
            DrawSnake();
            Thread.Sleep(speed);

        }
        bool Logic()
        {
            //next snake move
            switch (key)
            {
                case 'w':
                    if(snakeDirection != 's')
                    {
                        snakeDirection = 'w';
                    }
                    break;
                case 's':
                    if (snakeDirection != 'w')
                    {
                        snakeDirection = 's';
                    }
                    break;
                case 'd':
                    if (snakeDirection != 'a')
                    {
                        snakeDirection = 'd';
                    }
                    break;
                case 'a':
                    if (snakeDirection != 'd')
                    {
                        snakeDirection = 'a';
                    }
                    break;
            }

            //fruit
            if(snakePosX == fruitPosX && snakePosY == fruitPosY)
            {
                snakeParts++;
                CreateFruit();
            }

            //Bounds
            if(snakePosX == 0 || snakePosX == width || snakePosY == 0 || snakePosY == height)
            {
                Console.SetCursorPosition(snakePosX, snakePosY);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("O");
                Console.ForegroundColor = ConsoleColor.Gray;
                return false;
            }

            //Suicide
            for(int i = 0; i < snakeParts - 1; i++)
            {
                if (snakePosX == snakePartsPosX[i] && snakePosY == snakePartsPosY[i])
                {
                    return false;
                }
            }
            

            return true;
        }
        void DrawSnakeParts()
        {
            //přidá part

            snakePartsPosX.Add(snakePosX);
            snakePartsPosY.Add(snakePosY);

            if (snakeParts < snakePartsPosX.Count)
            {
                for (int i = 0; i < (snakePartsPosX.Count - 1); i++)
                {
                    snakePartsPosX[i] = snakePartsPosX[i+1];
                    snakePartsPosY[i] = snakePartsPosY[i+1];
                }
                snakePartsPosX.RemoveAt(snakePartsPosX.Count-1);
                snakePartsPosY.RemoveAt(snakePartsPosY.Count-1);
            }

            //Draw parts
            for (int i = 0; i < snakeParts; i++)
            {
                    Console.SetCursorPosition(snakePartsPosX[i], snakePartsPosY[i]);
                    Console.Write('o');
            }

            //Clear last part
            Console.SetCursorPosition(snakePartsPosX[0], snakePartsPosY[0]);
            Console.Write(' ');
            

        }
        void DrawSnake()
        {
            snakePrevPosX = snakePosX;
            snakePrevPosY = snakePosY;

            if (snakeDirection == 'w')
            {
                snakePosY--;
            }
            else if (snakeDirection == 's')
            {
                snakePosY++;
            }
            else if (snakeDirection == 'd')
            {
                snakePosX++;
            }
            else if (snakeDirection == 'a')
            {
                snakePosX--;
            }
            Console.SetCursorPosition(snakePosX, snakePosY);
            Console.Write('O');

            //Clears path
            if (snakeParts == 0)
            {
                Console.SetCursorPosition(snakePrevPosX, snakePrevPosY);
                Console.Write(' ');
            }
        }
        void DrawScore()
        {
            Console.SetCursorPosition(width + 10, height/2 + 2);
            Console.Write($"Score: {snakeParts - 1}");
        }
        void DrawFruit()
        {
            Console.SetCursorPosition(fruitPosX, fruitPosY);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write('b');
            Console.ResetColor();
        }
        void CreateFirstFruit()
        {
            fruitPosX = width / 3;
            fruitPosY = height / 3;
        }
        void CreateFruit()
        {
            switch (fruitDiff)
            {
                case 0:
                    fruitPosX = random.Next(1, width);
                    fruitPosY = random.Next(1, height);
                    break;
                case 69:
                    //TODO
                    break;
                case 420:
                    int[] arrPosX = { 1, width - 1 };
                    int[] arrPosY = { 1, height - 1 };
                    fruitPosX = arrPosX[random.Next(0, 2)];
                    fruitPosY = arrPosY[random.Next(0, 2)];
                    break;
            }
        }
        void CreateSnake()
        {
            Console.SetCursorPosition(width / 2, height / 2);
            snakePosX = width / 2;
            snakePosY = height / 2;
            snakeParts++;
            snakePartsPosX.Add(snakePosX);
            snakePartsPosY.Add(snakePosY);
            Console.Write('O');
        }
        void DrawArena()
        {

            //top
            for (int i = 0; i <= width; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("#");
            }

            //left
            for (int i = 0; i <= height; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("#");
            }

            //right
            for (int i = 0; i <= height; i++)
            {
                Console.SetCursorPosition(width, i);
                Console.Write("#");
            }

            //bottom
            for (int i = 0; i <= width; i++)
            {
                Console.SetCursorPosition(i, height);
                Console.Write("#");
            }

        }
        void GetInput()
        {

            if (Console.KeyAvailable)
            {
                keyInfo = Console.ReadKey(true);
                key = keyInfo.KeyChar;
            }
        }
        static void Countdown(int width, int height, int seconds)
        {
            int secondsLeft = seconds;
            
            for(int i = 0; i < seconds; i++)
            {
                Console.SetCursorPosition(width + 10, height / 2 + 6);
                Console.Write(secondsLeft--);
                Thread.Sleep(500);
            }
        }

    }
}