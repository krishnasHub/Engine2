using Engine2.Core;
using Engine2.Texture;
using Engine2.Util;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine2.Actor
{
    public enum BoundingShape
    {
        BoundingBox,
        BoundingCircle
    }

    public class GameActor
    {
        private string textureName;
        Texture2D Texture;
        protected Vector2 position;


        public Vector2 Position
        {
            set
            {
                this.position = value;
                if(this.Texture.Id != -1 && Scale != null)
                    Center = new Vector2(position.X + (Texture.Width * Scale.X) / 2f, position.Y + (Texture.Height * Scale.Y) / 2f);
            }

            get { return position; }
        }
        public Vector2 Center;
        public Vector2 Velocity;
        public Vector2 Scale;
        public RectangleF BoundingBox;
        public float BoundingRadius;
        public BoundingShape BoundingShape = BoundingShape.BoundingBox;
        private bool boundingBoxSet = false;


        public IActorPhysics PhysicsComponent;
        public bool BindToView = false;

        public GameActor()
        {
            textureName = "";
        }

        public GameActor(string textureName)
        {
            this.textureName = textureName;
        }

        public void SetScale(float sx, float sy)
        {
            Scale = new Vector2(sx, sy);
        }

        public void SetScale(float value)
        {
            Scale = new Vector2(value, value);
        }

        public virtual void Init()
        {
            Texture = ContentLoader.LoadTexture(this.textureName);

            if (Scale == null)
                Scale = new Vector2(1f, 1f);

            // If it's a bounding box (the default value)
            //  then the box starts with 0, 0 to width and height
            if(BoundingShape == BoundingShape.BoundingBox)
            {
                if (!boundingBoxSet)
                    BoundingBox = new RectangleF(0, 0, Texture.Width * Scale.X, Texture.Height * Scale.Y);

                boundingBoxSet = true;
            } 

            // If the shape is that of a circle, then, we assume the texture width and height are the same
            //  also, we set the radius to the width times scale.
            else if (BoundingShape == BoundingShape.BoundingCircle)
            {
                if(!boundingBoxSet)
                    BoundingRadius = Texture.Width * Scale.X;

                boundingBoxSet = true;
            }            

            if (Velocity == null)
                Velocity = new Vector2(0f, 0f);

            if (position == null)
                position = new Vector2(0f, 0f);

            if (Center == null)
                Center = new Vector2(position.X + (Texture.Width * Scale.X) / 2f, position.Y + (Texture.Height * Scale.Y) / 2f);
        }

        public virtual void Tick()
        {
            position += Velocity;
            Center += Velocity;
        }

        public virtual void Render()
        {
            SpriteBatch.Draw(Texture, position, Scale, Color.White, Vector2.Zero);
        }

        public virtual void onHit(GameActor otherActor)
        {
            // handle post collission code here..
            // Only handle code related to this Actor.
            // The code to worry about otherActor should be written in that class and NOT here.

            Console.WriteLine("Collided with another object!");
        }

    }
}
