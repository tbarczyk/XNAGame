using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using XnaTut;


namespace GameTutorial_1
{
    // publiczna glowna  klasa programu Game1

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;


        // tworzenie instancji zawieraj¹cej teksturê t³a
        Texture2D background;

        // tworzenie prostok¹ta ograniczaj¹cego powierzchniê g³ównego ekranu gry
        Rectangle mainFrame;

        // glowny obiekt w programu
        GameObject arrow;

        //List<Weight> weightList;

        //Rectangle mouseRectangle;
        //Boolean canClick;

        //Texture2D weightTexture;



        // wspolczynniki oporow ruchu
        float ct = 0.4f; // wspolczynnik tarcia
        float cx = 0.5f; // wspolczynnik aerodynamicznych oporow ruchu

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            base.Initialize();
        }

        // zaladuj obrazki
        protected override void LoadContent()
        {
            // utworz nowy SpriteBatch do rysowania tekstur
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // zaladuj tlo
            background = Content.Load<Texture2D>("Textures\\tlo");
            font = Content.Load<SpriteFont>("SpriteFont1");

            // ustaw parametry prostokata tla
            mainFrame = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            // wczytaj teksture na obiekt arrow
            arrow = new GameObject(Content.Load<Texture2D>("Textures\\auto"));

            // ustaw obiekt arrow w centrum ekranu
            arrow.ObjectPosition = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);

            // ustaw widocznoœæ myszy
            IsMouseVisible = true;

        }


        protected override void UnloadContent()
        {

        }


        // zmien parametry obiektu
        protected override void Update(GameTime gameTime)
        {
            // wylaczanie programu
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // reakcja na nacisniecie strzalki w prawo
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                arrow.ObjectRotation += 0.1f;
            }

            // reakcja na nacisniecie strzalki w lewo
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                arrow.ObjectRotation -= 0.1f;
            }

            // reakcja na nacisniecie strzalki w gore - ustawienie flagi
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                arrow.flagUp = 1;
            }

            // w przeciwnym przypadku wartosc flagi jest rowna 0
            else
            {
                arrow.flagUp = 0;
            }

            // reakcja na nacisniecie strzalki w dol - ustawienie flagi
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                arrow.flagDown = 1;
            }

            // w przeciwnym przypadku wartosc flagi jest rowna 0
            else
            {            
                arrow.flagDown = 0;
            }

            // oblicz predkosc obiektu
            arrow.CountSpeed(ct, cx, (float)gameTime.ElapsedGameTime.TotalSeconds);
            
            // oblicz pozycje obiektu
            arrow.CountPosition();
          
            base.Update(gameTime);
        }


        // umiesc grafike na ekranie
        protected override void Draw(GameTime gameTime)
        {
            // inicjalizuj tlo
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // zacznij budowac sprite' a
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            // narysuj tlo i obiekt
            spriteBatch.Draw(background, mainFrame, Color.White);
            spriteBatch.Draw(arrow.ObjectTexture, arrow.ObjectPosition, null, Color.White,
            arrow.ObjectRotation, new Vector2(45, 45), 1.0f, SpriteEffects.None, 0);

            // Umiesc w oknie napisy dotyczace parametrow obiektu
            spriteBatch.DrawString(font, "Poduszkowiec v. 1.0",
                new Vector2(50, 10), Color.Black);
            spriteBatch.DrawString(font, "Predkosc: " + String.Format("{0:N3}", arrow.ObjectSpeed),
                new Vector2(10, 30), Color.Black);
            spriteBatch.DrawString(font, "Kat obrotu: " + String.Format("{0:N2}", arrow.ObjectRotation%3.1415),
                new Vector2(10, 50), Color.Black);
            spriteBatch.DrawString(font, "Przyspieszenie: " + arrow.ObjectAcceleration.ToString(),
                new Vector2(10, 70), Color.Black);
            spriteBatch.DrawString(font, "Wspolczynnik tarcia/oporu powietrza: " + ct.ToString() + "/" + cx.ToString(),
                new Vector2(10, 90), Color.Black);
            spriteBatch.DrawString(font, "Sila ciagu silnika: " + arrow.EngineForce.ToString(),
                new Vector2(10, 110), Color.Black);

            if (arrow.ObjectSpeed == 0f)
            {
                spriteBatch.DrawString(font, "Sila oporow: 0",
                new Vector2(10, 130), Color.Black);
            }
            else
            {
                spriteBatch.DrawString(font, "Sila oporow: " + arrow.Op.ToString(),
                new Vector2(10, 130), Color.Black);
            }


            spriteBatch.DrawString(font, "Sila wypadkowa: " + (arrow.ObjectForce).ToString(),
                new Vector2(10, 150), Color.Black);
            spriteBatch.DrawString(font, "Czas: " + String.Format("{0:N1}", arrow.czas),
                new Vector2(720, 450), Color.Black);
            if (arrow.flagUp == 1 || arrow.flagDown == 1)
            {
                spriteBatch.DrawString(font, "ENGINE ON",
                    new Vector2(700, 70), Color.Red);
            }

            // Zakoncz wyswietlanie
            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}