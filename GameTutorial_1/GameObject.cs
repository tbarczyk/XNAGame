
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


namespace GameTutorial_1
{
    // klasa reprezentujaca glowny obiekt w programie
    class GameObject
    {
        // wlasciwosci obiektu
        public Texture2D ObjectTexture; // tekstura przypisana do obiektu
        public Vector2 ObjectPosition; // dwuwymiarowy wektor mowiacy o polozeniu na plaszczyznie XY
        public float ObjectRotation; // obrot obiektu dookola wlasnego srodka
        public float v1; // 1 zmienna reprezentujaca predkosc przy obliczaniu predkosci wypadkowej
        public float v2; // 2 zmienna reprezentujaca predkosc przy obliczaniu predkosci wypadkowej
        public float ObjectSpeed; // wypadkowa predkosc obiektu
        public float ObjectMass; // masa obiektu
        public float EngineForce; // sila ciagu silnika
        public float ObjectForce; // wypadkowa sila dzialajaca na obiekt
        public float ObjectAcceleration; // przyspieszenie obiektu w danej chwili
        public float ObjectSurface; // zmienna opisujaca powierzchnie czolowa obiektu
        public float czas; // czas trwania symulacji
        public float flagUp; // flaga mowiaca o wlaczeniu silnika do przodu
        public float flagDown; // flaga mowiaca o wlaczeniu silnika do tylu
        public float Op; // wartosc oporow ruchu
       

        // konstruktor obiektu
        public GameObject(Texture2D MainTexture)
        {
            ObjectRotation = 0.0f;
            ObjectPosition = Vector2.Zero;
            ObjectTexture = MainTexture;
            EngineForce = 400; // N
            ObjectMass = 40; //kg
            ObjectSurface = 10f;
            ObjectSpeed = 0f;
            v1 = 0f;
            v2 = 0f;
            flagUp = 0;
            flagDown = 0;
            ObjectAcceleration = ObjectForce/ObjectMass;            
        }

        // funkcja wyliczajaca wypadkowa sile i przyspieszenie dzialajace na obiekt
        public void CountForce(float cx, float ct)
        {

            // oblicz sile wypadkowa i przyspieszenie dla obiektu z silnikiem wlaczonym do przodu
            if (flagUp == 1)
            {
                ObjectForce = EngineForce - 0.0626f * ObjectSpeed * ObjectSpeed * ObjectSurface * cx - 9.81f * ct * ObjectMass;
                Op = 0.0626f * ObjectSpeed * ObjectSpeed * ObjectSurface * cx + 9.81f * ct * ObjectMass;
                ObjectAcceleration = ObjectForce / ObjectMass;
            }

            //oblicz sile wypadkowa i przyspieszenie dla obiektu z silnikiem wlaczonym do tylu
            else if (flagDown == 1)
            {
                ObjectForce = -(EngineForce + 0.0626f * ObjectSpeed * ObjectSpeed * ObjectSurface * cx + 9.81f * ct * ObjectMass);
                Op = 0.0626f * ObjectSpeed * ObjectSpeed * ObjectSurface * cx + 9.81f * ct * ObjectMass;
                ObjectAcceleration = ObjectForce / ObjectMass;
            }

            // oblicz sile wypadkowa i przyspieszenie dla obiektu bez wlaczonego silnika poruszajacego sie z predkoscia wieksza od minimalnej
            else if (ObjectSpeed>=0.1f)
            {
                ObjectForce = -(0.0626f * ObjectSpeed * ObjectSpeed * ObjectSurface * cx + 9.81f * ct * ObjectMass);
                ObjectAcceleration = ObjectForce / ObjectMass;
                Op = 0.0626f * ObjectSpeed * ObjectSpeed * ObjectSurface * cx + 9.81f * ct * ObjectMass;
            }

            // oblicz sile wypadkowa i przyspieszenie dla obiektu bez wlaczonego silnika poruszajacego sie z predkoscia mniejsza od minimalnej
            else if (ObjectSpeed<-0.1f)
            {
                ObjectForce = (0.0626f * ObjectSpeed * ObjectSpeed * ObjectSurface * cx + 9.81f * ct * ObjectMass);
                ObjectAcceleration = ObjectForce / ObjectMass;
                Op = 0.0626f * ObjectSpeed * ObjectSpeed * ObjectSurface * cx + 9.81f * ct * ObjectMass;
            }

            // oblicz sile wypadkowa i przyspieszenie dla pozostalych wypadkow
            else 
            {
                ObjectForce = 0;
                ObjectAcceleration = ObjectForce / ObjectMass;
            }
            
        }

        // funkcja obliczajaca wypadkowa predkosc
        public void CountSpeed(float ct, float cx, float dt)
        {
            // zwieksz wartosc czasu symulacji
            czas += dt;

            // wylicz sile wypadkowa
            CountForce(cx, ct);

            // wylicz pierwsza predkosc pomocnicza i dodaj ja do obecnej predkosci obiektu
            v1 = ObjectAcceleration * dt;
            ObjectSpeed+=v1;

            // wylicz sile wypadkowa dla nowej predkosci
            CountForce(cx, ct);
            v2 = ObjectAcceleration * dt;

            // zwieksz predkosc obiektu na podstawie poprzednich obliczen
            ObjectSpeed+=((v1+v2)/2.0f);
            
            // wyzeruj predkosci pomocnicze
            v1 = 0;
            v2 = 0;
            
            // sprawdz czy obiekt nie porusza sie z predkoscia mniejsza od minimalnej
            if (Math.Abs(ObjectSpeed) < 0.1f)
                ObjectSpeed = 0f;          
        }

        // funkcja oblicza pozycje obiektu na podstawie predkosci
        public void CountPosition()
        {

            // zabezpiecz obiekt przed wypadnieciem poza ekran
            if ((float)ObjectPosition.X > 800) 
            {
                ObjectPosition.X=0;
            }
                
            if ((float)ObjectPosition.X < 0)
            {
                ObjectPosition.X=800;
            }
            if ((float)ObjectPosition.Y > 480)
            {
                ObjectPosition.Y=0;
            }
            if ((float)ObjectPosition.Y < 0)
            {
                ObjectPosition.Y=480;
            }

            // oblicz nowa pozycje obiektu na podstawie predkosci
            ObjectPosition = ObjectPosition + new Vector2((float)ObjectSpeed * (float)(Math.Sin(ObjectRotation)),
            (float)ObjectSpeed * (float)(-Math.Cos(ObjectRotation)));
        }

    }
}

