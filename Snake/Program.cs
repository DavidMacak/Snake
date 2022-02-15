namespace Snake
{
    class Snake
    {
        int height = 20;
        int width = 60;
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

        Random random = new Random();

        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            while (true)
            {
                Snake snake = new Snake();
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
            //
            DrawFruit();
            if (snakeParts != 0)
            {
                DrawSnakeParts();
            }
            DrawSnake();
            Thread.Sleep(100);

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
            fruitPosX = random.Next(1, width);
            fruitPosY = random.Next(1, height);
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