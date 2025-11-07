using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Sharp_Blast
{
    public class Bricks
    {
        public static int[,] prediction = new int[8,8];

        static Squares squares;

        public static Brick[] ActiveBricks;

        public static int grabed = -1;

        public static GraphicsDevice GraphicsDevice;
        

        public Bricks(ContentManager content, GraphicsDevice graphicsDevice)
        {
            squares = new Squares(content);
            GraphicsDevice = graphicsDevice;
            generateBricks();
        }

        public void render(SpriteBatch _spriteBatch, GraphicsDevice graphicsDevice)
        {
            if (grabed > -1)
            {
                foreach (Brick bricks in ActiveBricks)
                {
                    if (bricks == ActiveBricks[grabed])
                    {
                        //graphicsDevice.SetRenderTarget(null);
                        bricks.render(_spriteBatch, true);
                        //graphicsDevice.SetRenderTarget(Game1.screen);
                    }
                    else
                    {
                        bricks.render(_spriteBatch);
                    }
                }
            }
            else
            {
                foreach (Brick bricks in ActiveBricks)
                {
                    bricks.render(_spriteBatch);
                }
            }
        }

        public static void generateBricks()
        {
            prediction = (int[,])Field.pole.Clone();
            
            ActiveBricks = [BlockFactory.PickRandomClass(0), BlockFactory.PickRandomClass(1), BlockFactory.PickRandomClass(2)];
            
            // kód pro manuální nastavení přiřazených kostek

            //ActiveBricks = [(Brick)Activator.CreateInstance(typeof(ThreHor), 0), (Brick)Activator.CreateInstance(typeof(FiveHor), 1), (Brick)Activator.CreateInstance(typeof(TwoHor), 2)];

        }

        public static void checkEmpty()
        {
            if (ActiveBricks[0].GetType() == typeof(Bricks.Empty)&& ActiveBricks[1].GetType() == typeof(Bricks.Empty) && ActiveBricks[2].GetType() == typeof(Bricks.Empty))
            {
                generateBricks();
            }
        }

        public class Squares
        {
            public static Texture2D texture1 { get; private set; }
            public static Texture2D texture2 { get; private set; }
            public static Texture2D texture3 { get; private set; }
            public static Texture2D texture4 { get; private set; }
            public static Texture2D texture5 { get; private set; }
            public static Texture2D texture6 { get; private set; }
            public static Texture2D texture7 { get; private set; }

            public Squares(ContentManager content)
            {
                //rainbow variants
                
                /*
                texture1 = content.Load<Texture2D>("bricks\\blue");
                texture2 = content.Load<Texture2D>("bricks\\dark_blue");
                texture3 = content.Load<Texture2D>("bricks\\green");
                texture4 = content.Load<Texture2D>("bricks\\orange");
                texture5 = content.Load<Texture2D>("bricks\\red");
                texture6 = content.Load<Texture2D>("bricks\\yellow");
                texture7 = content.Load<Texture2D>("bricks\\purple");
                */
                
                //wood variants


                texture1 = content.Load<Texture2D>("woodBricks\\wood1");
                texture2 = content.Load<Texture2D>("woodBricks\\wood2");
                texture3 = content.Load<Texture2D>("woodBricks\\wood3");
                texture4 = content.Load<Texture2D>("woodBricks\\wood4");
                texture5 = content.Load<Texture2D>("woodBricks\\wood5");
                texture6 = content.Load<Texture2D>("woodBricks\\wood6");
                texture7 = content.Load<Texture2D>("woodBricks\\wood7");
                
            }

            public static void render(SpriteBatch _spriteBatch, Microsoft.Xna.Framework.Vector2 cords, int color, int scale)
            {
                _spriteBatch.Begin();

                switch (color) {
                    case 0:
                        break;
                    case 1:
                        _spriteBatch.Draw(texture1, new Rectangle(Convert.ToInt32(cords.X), Convert.ToInt32(cords.Y), scale, scale), Color.White);
                        break;
                    case 2:
                        _spriteBatch.Draw(texture2, new Rectangle(Convert.ToInt32(cords.X), Convert.ToInt32(cords.Y), scale, scale), Color.White);
                        break;
                    case 3:
                        _spriteBatch.Draw(texture3, new Rectangle(Convert.ToInt32(cords.X), Convert.ToInt32(cords.Y), scale, scale), Color.White);
                        break;
                    case 4:
                        _spriteBatch.Draw(texture4, new Rectangle(Convert.ToInt32(cords.X), Convert.ToInt32(cords.Y), scale, scale), Color.White);
                        break;
                    case 5:
                        _spriteBatch.Draw(texture5, new Rectangle(Convert.ToInt32(cords.X), Convert.ToInt32(cords.Y), scale, scale), Color.White);
                        break;
                    case 6:
                        _spriteBatch.Draw(texture6, new Rectangle(Convert.ToInt32(cords.X), Convert.ToInt32(cords.Y), scale, scale), Color.White);
                        break;
                    case 7:
                        _spriteBatch.Draw(texture7, new Rectangle(Convert.ToInt32(cords.X), Convert.ToInt32(cords.Y), scale, scale), Color.White);
                        break;
                }
                _spriteBatch.End();
            }
        }

        public abstract class Brick
        {
            protected static Random random = new Random();
            public int y = 1780;

            public int Color;
            public Rectangle Rect;
            public List<List<int>> Blocks { get; protected set; }
            
            protected Brick(int Place)
            {
                Color = random.Next(1, 8);
                Rect = new Rectangle((180 + (320 * Place)), y, 100, 100);
                //Rect = new Rectangle(180 + (320 * Place), y, 100, 100);
            }

            public void setCords(int x, int y)
            {
                Rect.X = x;
                Rect.Y = y;
            }

            public void resetCords(int place)
            {
                Rect.X = 180 + (320 * place);
                Rect.Y = y;
            }

            public bool collidepoint(int x, int y)
            {
                if (Rect.Contains(new Microsoft.Xna.Framework.Vector2(x, y)))
                {
                    return true;
                }
                else
                {
                    return false;
                }  
            }

            public void render(SpriteBatch _spriteBatch, bool big = false)
            {
                List<int> cordsX = Blocks[0];
                List<int> cordsY = Blocks[1];

                if (big)
                {
                    for (int i = 0; i < cordsX.Count(); i++)
                    {
                        Squares.render(_spriteBatch, new Microsoft.Xna.Framework.Vector2(Rect.X + (115 * cordsX[i]), Rect.Y + (115 * cordsY[i])), Color, 110);
                    }
                }
                else
                {
                    for (int i = 0; i < cordsX.Count(); i++)
                    {
                        Squares.render(_spriteBatch, new Microsoft.Xna.Framework.Vector2(Rect.X + (50 * cordsX[i]), Rect.Y + (50 * cordsY[i])), Color, 50);
                    }
                }
                
            }
        }

        public class Empty : Brick
        {
            public Empty(int place) : base(place)
            {
                Blocks = new List<List<int>> { new List<int> {  }, new List<int> {  } };
            }
        }


        public class OneByOne : Brick
        {
            public OneByOne(int place) : base(place)
            {
                Blocks = new List<List<int>> { new List<int> { 0 }, new List<int> { 0 } };
            }
        }

        public class TwoHor : Brick
        {
            public TwoHor(int place) : base(place)
            {
                Blocks = new List<List<int>> { new List<int> { 0, 1 }, new List<int> { 0, 0 } };
            }
        }

        public class TwoVer : Brick
        {
            public TwoVer(int place) : base(place)
            {
                Blocks = new List<List<int>> { new List<int> { 0, 0 }, new List<int> { 0, -1 } };
            }
        }

        public class ThreHor : Brick
        {
            public ThreHor(int place) : base(place)
            {
                Blocks = new List<List<int>> { new List<int> { 0, -1, 1 }, new List<int> { 0, 0, 0 } };
            }
        }

        public class ThreVer : Brick
        {
            public ThreVer(int place) : base(place)
            {
                Blocks = new List<List<int>> { new List<int> { 0, 0 , 0}, new List<int> { 0, -1, 1 } };
            }
        }

        public class FourHor : Brick
        {
            public FourHor(int place) : base(place)
            {
                Blocks = new List<List<int>> {
            new List<int> { 0, -1, 1, -2 },
            new List<int> { 0, 0, 0, 0 }
        };
            }
        }

        public class FourVer : Brick
        {
            public FourVer(int place) : base(place)
            {
                Blocks = new List<List<int>> {
            new List<int> { 0, 0, 0, 0 },
            new List<int> { 0, -1, 1, -2 }
        };
            }
        }

        public class FiveHor : Brick
        {
            public FiveHor(int place) : base(place)
            {
                Blocks = new List<List<int>> {
            new List<int> { 0, -1, 1, -2, 2 },
            new List<int> { 0, 0, 0, 0, 0 }
        };
            }
        }

        public class FiveVer : Brick
        {
            public FiveVer(int place) : base(place)
            {
                Blocks = new List<List<int>> {
            new List<int> { 0, 0, 0, 0, 0 },
            new List<int> { 0, -1, 1, -2, 2 }
        };
            }
        }

        public class LitleEL : Brick
        {
            public LitleEL(int place) : base(place)
            {
                Blocks = new List<List<int>> {
            new List<int> { 0, -1, 0 },
            new List<int> { 0, 0, -1 }
        };
            }
        }

        public class LitleEL2 : Brick
        {
            public LitleEL2(int place) : base(place)
            {
                Blocks = new List<List<int>> {
            new List<int> { 0, 0, 1 },
            new List<int> { 0, 1, 0 }
        };
            }
        }

        public class LitleEL3 : Brick
        {
            public LitleEL3(int place) : base(place)
            {
                Blocks = new List<List<int>> {
            new List<int> { 0, 1, 0 },
            new List<int> { 0, 0, -1 }
        };
            }
        }

        public class LitleEL4 : Brick
        {
            public LitleEL4(int place) : base(place)
            {
                Blocks = new List<List<int>> {
            new List<int> { 0, 0, -1 },
            new List<int> { 0, 1, 0 }
        };
            }
        }

        public class EL : Brick
        {
            public EL(int place) : base(place)
            {
                Blocks = new List<List<int>> {
            new List<int> { 0, 1, 1, -1 },
            new List<int> { 0, 0, 1, 0 }
        };
            }
        }

        public class EL2 : Brick
        {
            public EL2(int place) : base(place)
            {
                Blocks = new List<List<int>> {
            new List<int> { 0, -1, 1, -1 },
            new List<int> { 0, 0, 0, 1 }
        };
            }
        }

        public class EL3 : Brick
        {
            public EL3(int place) : base(place)
            {
                Blocks = new List<List<int>> {
            new List<int> { 0, -1, -1, 1 },
            new List<int> { 0, 0, -1, 0 }
        };
            }
        }

        public class EL4 : Brick
        {
            public EL4(int place) : base(place)
            {
                Blocks = new List<List<int>> {
            new List<int> { 0, -1, 1, 1 },
            new List<int> { 0, 0, -1, 0 }
        };
            }
        }

        public class EL5 : Brick
        {
            public EL5(int place) : base(place)
            {
                Blocks = new List<List<int>> {
                    new List<int> { 0, 0, 1, 0 },
                    new List<int> { 0, 1, 1, -1 },    
        };
            }
        }

        public class EL6 : Brick
        {
            public EL6(int place) : base(place)
            {
                Blocks = new List<List<int>> {
                    new List<int> { 0, 0, 0, 1 },
                    new List<int> { 0, -1, 1, -1 } 
        };
            }
        }

        public class EL7 : Brick
        {
            public EL7(int place) : base(place)
            {
                Blocks = new List<List<int>> {
                    new List<int> { 0, 0, -1, 0 },
                    new List<int> { 0, -1, -1, 1 }
        };
            }
        }

        public class EL8 : Brick
        {
            public EL8(int place) : base(place)
            {
                Blocks = new List<List<int>> {
                    new List<int> { 0, 0, -1, 0 },
                    new List<int> { 0, -1, 1, 1 }
        };
            }
        }

        public class BEL : Brick
        {
            public BEL(int place) : base(place)
            {
                Blocks = new List<List<int>> {
            new List<int> { 0, -2, -1, 0, 0 },
            new List<int> { 0, 0, 0, 1, 2 }
        };
            }
        }

        public class BEL2 : Brick
        {
            public BEL2(int place) : base(place)
            {
                Blocks = new List<List<int>> {
            new List<int> { 0, 1, 2, 0, 0 },
            new List<int> { 0, 0, 0, 1, 2 }
        };
            }
        }

        public class BEL3 : Brick
        {
            public BEL3(int place) : base(place)
            {
                Blocks = new List<List<int>> {
            new List<int> { 0, 0, 0, 1, 2 },
            new List<int> { 0, -1, -2, 0, 0 }
        };
            }
        }

        public class BEL4 : Brick
        {
            public BEL4(int place) : base(place)
            {
                Blocks = new List<List<int>> {
            new List<int> { 0, -1, -2, 0, 0 },
            new List<int> { 0, 0, 0, -1, -2 }
        };
            }
        }

        public class Z1 : Brick
        {
            public Z1(int place) : base(place)
            {
                Blocks = new List<List<int>> {
            new List<int> { 0, 0, -1, 1 },
            new List<int> { 0, -1, -1, 0 }
        };
            }
        }

        public class Z2 : Brick
        {
            public Z2(int place) : base(place)
            {
                Blocks = new List<List<int>> {
            new List<int> { 0, 0, 1, -1 },
            new List<int> { 0, -1, -1, 0 }
        };
            }
        }

        public class Z3 : Brick
        {
            public Z3(int place) : base(place)
            {
                Blocks = new List<List<int>> {
            new List<int> { 0, 0, -1, -1 },
            new List<int> { 0, -1, 1, 0 }
        };
            }
        }

        public class Z4 : Brick
        {
            public Z4(int place) : base(place)
            {
                Blocks = new List<List<int>> {
            new List<int> { 0, 0, -1, -1 },
            new List<int> { 0, 1, -1, 0 }
        };
            }
        }

        public class smallT1 : Brick
        {
            public smallT1(int place) : base(place)
            {
                Blocks = new List<List<int>> {
            new List<int> { 0, -1, 1, 0 },
            new List<int> { 0, 0, 0, 1 }
        };
            }
        }

        public class smallT2 : Brick
        {
            public smallT2(int place) : base(place)
            {
                Blocks = new List<List<int>> {
            new List<int> { 0, -1, 1, 0 },
            new List<int> { 0, 0, 0, -1 }
        };
            }
        }

        public class smallT3 : Brick
        {
            public smallT3(int place) : base(place)
            {
                Blocks = new List<List<int>> {
            new List<int> { 0, 0, 0, -1 },
            new List<int> { 0, -1, 1, 0 }
        };
            }
        }

        public class smallT4 : Brick
        {
            public smallT4(int place) : base(place)
            {
                Blocks = new List<List<int>> {
            new List<int> { 0, 0, 0, 1 },
            new List<int> { 0, -1, 1, 0 }
        };
            }
        }

        public class T1 : Brick
        {
            public T1(int place) : base(place)
            {
                Blocks = new List<List<int>> {
            new List<int> { 0, -1, 1, 0, 0 },
            new List<int> { 0, 0, 0, 1, 2 }
        };
            }
        }

        public class T2 : Brick
        {
            public T2(int place) : base(place)
            {
                Blocks = new List<List<int>> {
            new List<int> { 0, -1, 1, 0, 0 },
            new List<int> { 0, 0, 0, -1, -2 }
        };
            }
        }

        public class T3 : Brick
        {
            public T3(int place) : base(place)
            {
                Blocks = new List<List<int>> {
            new List<int> { 0, 0, 0, -1, -2 },
            new List<int> { 0, -1, 1, 0, 0 }
        };
            }
        }

        public class T4 : Brick
        {
            public T4(int place) : base(place)
            {
                Blocks = new List<List<int>> {
            new List<int> { 0, 0, 0, 1, 2 },
            new List<int> { 0, -1, 1, 0, 0 }
        };
            }
        }

        public class cube22 : Brick
        {
            public cube22(int place) : base(place)
            {
                Blocks = new List<List<int>> {
            new List<int> { 0, 0, 1, 1 },
            new List<int> { 0, 1, 0, 1 }
        };
            }
        }

        public class cube32 : Brick
        {
            public cube32(int place) : base(place)
            {
                Blocks = new List<List<int>> {
            new List<int> { 0, 0, 1, 1, -1, -1 },
            new List<int> { 0, 1, 0, 1, 0, 1 }
        };
            }
        }

        public class cube33 : Brick
        {
            public cube33(int place) : base(place)
            {
                Blocks = new List<List<int>> {
            new List<int> { 0, 0, 0, 1, 1, 1, -1, -1, -1 },
            new List<int> { 0, 1, -1, 0, 1, -1, 0, 1, -1 }
        };
            }
        }

        public class blockx : Brick
        {
            public blockx(int place) : base(place)
            {
                Blocks = new List<List<int>> {
                new List<int> { 0, 1, 1, -1, -1 },
                new List<int> { 0, 1, -1, 1, -1 }
                };
            }
        }

        public class blockX : Brick
        {
            public blockX(int place) : base(place)
            {
                Blocks = new List<List<int>> {
                new List<int> { 0, 1, 1, -1, -1 , 2, 2, -2, -2},
                new List<int> { 0, -1, 1, 1, -1 , 2, -2, 2, -2}
                };
            }
        }

        public class blockSlash2_1 : Brick
        {
            public blockSlash2_1(int place) : base(place)
            {
                Blocks = new List<List<int>> {
                new List<int> { 0, -1},
                new List<int> { 0, -1}
                };
            }
        }

        public class blockSlash2_2 : Brick
        {
            public blockSlash2_2(int place) : base(place)
            {
                Blocks = new List<List<int>> {
                new List<int> { 0, 1},
                new List<int> { 0, -1}
                };
            }
        }

        public class blockslash3_1 : Brick
        {
            public blockslash3_1(int place) : base(place)
            {
                Blocks = new List<List<int>> {
                new List<int> { 0, 1, -1},
                new List<int> { 0, 1, -1}
                };
            }
        }

        public class blockslash3_2 : Brick
        {
            public blockslash3_2(int place) : base(place)
            {
                Blocks = new List<List<int>> {
                new List<int> { 0, 1, -1},
                new List<int> { 0, -1, 1}
                };
            }
        }

        public class blockSlash4_1 : Brick
        {
            public blockSlash4_1(int place) : base(place)
            {
                Blocks = new List<List<int>> {
                new List<int> { 0, 1, -1, 2},
                new List<int> { 0, 1, -1, 2}
                };
            }
        }

        public class blockSlash4_2 : Brick
        {
            public blockSlash4_2(int place) : base(place)
            {
                Blocks = new List<List<int>> {
                new List<int> { 0, 1, -1, -2},
                new List<int> { 0, -1, 1,  2}
                };
            }
        }

        public class blockSlash5_1: Brick
        {
            public blockSlash5_1(int place) : base(place)
            {
                Blocks = new List<List<int>> {
                new List<int> { 0, 1, -1, 2, -2},
                new List<int> { 0, 1, -1, 2, -2}
                };
            }
        }

        public class blockSlash5_2 : Brick
        {
            public blockSlash5_2(int place) : base(place)
            {
                Blocks = new List<List<int>> {
                new List<int> { 0, 1, -1, 2, -2},
                new List<int> { 0, -1, 1, -2, 2}
                };
            }
        }

        public static class BlockFactory
        {
            private static readonly Random random = new Random();
            private static readonly Type[] blockTypes =
            {
                // Single block
                typeof(OneByOne), 
                
                // Lines
                typeof(TwoHor), typeof(TwoVer),
                typeof(ThreHor), typeof(ThreVer),
                typeof(FourHor), typeof(FourVer),
                typeof(FiveHor), typeof(FiveVer),
                
                // EL shapes
                typeof(LitleEL), typeof(LitleEL2), typeof(LitleEL3), typeof(LitleEL4),
                typeof(EL), typeof(EL2), typeof(EL3), typeof(EL4), typeof(EL5), typeof(EL6), typeof(EL7), typeof(EL8),
                typeof(BEL), typeof(BEL2), typeof(BEL3), typeof(BEL4),
                
                // Z shapes
                typeof(Z1), typeof(Z2), typeof(Z3), typeof(Z4),
                
                // T shapes
                typeof(smallT1), typeof(smallT2), typeof(smallT3), typeof(smallT4),
                typeof(T1), typeof(T2), typeof(T3), typeof(T4),
            
                // Cubes
                typeof(cube22), typeof(cube32), typeof(cube33),

                // X shapes
                typeof(blockx), typeof(blockX),
                
                // Slash blocks
                typeof(blockSlash2_1), typeof(blockSlash2_2),
                typeof(blockslash3_1), typeof(blockslash3_2),
                typeof(blockSlash4_1), typeof(blockSlash4_2),
                typeof(blockSlash5_1), typeof(blockSlash5_2),
        };

            public static Brick PickRandomClass(int place)
            {
                int index = random.Next(blockTypes.Length);

                Brick brick = (Brick)Activator.CreateInstance(blockTypes[index], place);

                while(Field.blockfit(brick) != true)
                {
                    index = random.Next(blockTypes.Length);

                    brick = (Brick)Activator.CreateInstance(blockTypes[index], place);
                }
                return brick;
                
            }

            public static Brick getEmpty(int place)
            {
                return (Brick)Activator.CreateInstance(typeof(Empty), place);
            }
        }
    }

}
