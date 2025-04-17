using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using static Sharp_Blast.Bricks;
using static Android.Provider.UserDictionary;
namespace Sharp_Blast
{
    public class Field
    {
        public static int[,] pole = new int[8, 8];

        public static Texture2D pole_texture;

        public Field(ContentManager content)
        {
            pole_texture = content.Load<Texture2D>("pole");

            /*
             * Pokrytí pole kostkami pro kontrolu renderu všech
             *      nutnost vypnout kontrolu plnosti 
             *      
            Random rand = new Random();
            
            for (int i = 0; i < pole.GetLength(0); i++)
            {
                for (int j = 0; j < pole.GetLength(1); j++)
                {
                    pole[j, i] = rand.Next(1,7);
                }
            }
            */
        }

        public void render(SpriteBatch _spriteBatch)
        {

            _spriteBatch.Begin();
            _spriteBatch.Draw(pole_texture, new Rectangle(Convert.ToInt32(50), Convert.ToInt32(500), 1000, 1000), Color.White);
            _spriteBatch.End();

            for (int i = 0; i < pole.GetLength(0); i++)
            {
                for (int j = 0; j < pole.GetLength(1); j++)
                {
                    Bricks.Squares.render(_spriteBatch, new Vector2(90 + (115 * j), 545 + (115 * i)), pole[j, i], 110);
                }
            }

            /*
             * Kód pro vyrenderování collision pointů (levý horní roh čtverců)
             *
            for (int i = 150; i < 990; i += 115)
            {
                for (int j = 600; j < 1500; j += 115)
                {
                    Bricks.Squares.render(_spriteBatch, new Vector2(i, j), 3, 10);

                }
            }
            */
        }

        // Funkce pro umístění kostky na pole po pustění hráčem
        public void place()
        {
            Bricks.Brick brick = Bricks.ActiveBricks[Bricks.grabed];

            for (int i = 150; i < 990; i += 115)
            {
                for (int j = 600; j < 1500; j += 115)
                {
                    if (brick.collidepoint(i, j))
                    {
                        (bool, int[,]) result = brick_fill(((i - 150) / 115), ((j - 600) / 115), pole, brick.Blocks[0], brick.Blocks[1], brick.Color);

                        if (!result.Item1)
                        {
                            return;
                        }

                        pole = result.Item2;
                        Bricks.ActiveBricks[Bricks.grabed] = Bricks.BlockFactory.getEmpty(Bricks.grabed);
                        Bricks.checkEmpty();
                        return;
                    }
                }
            }
        }

        //ověření zda se kostka vejde na pole při vygenerování

        public static bool blockfit(Bricks.Brick brick)
        {

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Bricks.prediction[i, j] == 0)
                    {
                        (bool, int[,]) result = brick_fill(i, j, Bricks.prediction, brick.Blocks[0], brick.Blocks[1], brick.Color);

                        if (result.Item1)
                        {
                            Bricks.prediction = result.Item2;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        // Funkce pro naplnění pole kostkou
        public static (bool, int[,]) brick_fill(int i, int j, int[,] pole_check, List<int> cordsX, List<int> cordsY, int color)
        {
            int[,] pole_filed = (int[,])pole_check.Clone();

            for (int k = 0; k < cordsX.Count(); k++)
            {
                try
                {
                    if (pole_check[i + cordsX[k], j + cordsY[k]] == 0)
                    {
                        pole_filed[i + cordsX[k], j + cordsY[k]] = color;
                    }
                    else
                    {
                        return (false, pole_check);
                    }
                }
                catch
                {
                    return (false, pole_check);
                }
            }
            return (true, pole_filed);
        }

        //funkce pro kontrolu plnosti pole
        public int checkField()
        {
            //tvorba potřebných proměných
            int[,] clearedField = (int[,])pole.Clone();

            bool contains = false;
            int score = 0;

            // Kontrola řádků            
            for (int i = 0; i < pole.GetLength(0); i++)
            {
                for (int j = 0; j < pole.GetLength(1); j++)
                {
                    if (pole[i, j] == 0)
                    {
                        contains = true;
                        break;
                    }
                    else
                    {
                        contains = false;
                    }
                }

                if (contains == false)
                {
                    for (int j = 0; j < pole.GetLength(1); j++)
                    {
                        clearedField[i, j] = 0;
                    }
                    score += 80;
                    contains = true;
                }
            }

            contains = false;

            // Kontrola sloupců
            for (int i = 0; i < pole.GetLength(0); i++)
            {

                for (int j = 0; j < pole.GetLength(1); j++)
                {
                    if (pole[j, i] == 0)
                    {
                        contains = true;
                        break;
                    }
                    else
                    {
                        contains = false;
                    }
                }

                if (contains == false)
                {
                    for (int j = 0; j < pole.GetLength(1); j++)
                    {
                        clearedField[j, i] = 0;
                        score += 90;
                        contains = true;
                    }
                }
            }

            pole = clearedField;
            return score;
        }
    }
}
