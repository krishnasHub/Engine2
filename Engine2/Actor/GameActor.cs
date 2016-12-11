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
    /// <summary>
    /// The basic shapes that the bounding boxes use are either a Bounding Box (a rectangle) or a circle.
    /// </summary>
    public enum BoundingShape
    {
        BoundingBox,
        BoundingCircle
    }

    /// <summary>
    /// This is the base class for all Actors that you want on the level.
    /// This isn't an abstract class. You can directly use it hands down without wasting any time.
    /// Just add a texture and see it on the screen.
    /// The more ideal way of building actors is to derive your own Actor class from this class where you defin how your actor behaves.
    /// </summary>
    public class GameActor
    {
        public GameActor ParentActor;
        private List<GameActor> ChildActors = new List<GameActor>();
        public GameActor RootActor = null;
        public bool IsCollidable
        {
            get { return isCollidable; }
            set { isCollidable = value; }
        }
        public bool CanInit
        {
            get { return canInit; }
        }
        public GameLevel ParentLevel;
        public Vector2 Position
        {
            set
            {
                if (ParentLevel != null)
                {
                    if (!ParentLevel.CanActorMoveTo(this, value))
                    {
                        //this.position -= value;
                        return;
                    }

                }

                this.position = value;
                if (this.Texture.Id != -1 && Scale != null)
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
        public bool ReadyToBeDestroyed = false;
        public bool InAir = false;        
        public bool BindToView = false;
        public Vector2 JumpVector = new Vector2(0f, -5f);
        public float BounceFactor = 2f;
        public TileSheetManager TileSheet
        {
            set
            {
                tileSheet = value;
                useTileSheetManager = true;
            }

            get { return tileSheet; }
        }

        public bool IsAffectedByGravity = true;
        protected bool isCollidable = true;
        protected Vector2 position;
        protected bool canInit = true;

        private string textureName;
        private TileSheetManager tileSheet;
        private bool useTileSheetManager = false;
        private IActorPhysics physicsComponent;
        private bool boundingBoxSet = false;

        Texture2D Texture;
        protected PawnSensing PawnSensing;

        public void AddPawnSensing(float defaultDist = 100.0f)
        {
            PawnSensing = new PawnSensing(this);
            PawnSensing.PawnSensed = PawnSensed;
            PawnSensing.SensingRadius = defaultDist;
        }

        public PawnSensing GetPawnSensingComponent()
        {
            return PawnSensing;
        }

        public virtual void OnDestroyed()
        {
            // Do the needed cleanup..
        }

        public void MarkReadyToBeDestroyed()
        {
            this.ReadyToBeDestroyed = true;
            foreach(var a in ChildActors)
            {
                a.MarkReadyToBeDestroyed();
            }

            OnDestroyed();
        }

        public virtual void PawnSensed(GameActor otherActor)
        {
            // Need to be handled by your own code.
        }

        public void AddChild(GameActor child)
        {
            if (child == null)
                return;

            ChildActors.Add(child);
            child.ParentActor = this;
            child.ParentLevel = this.ParentLevel;

            if(RootActor == null)
            {
                child.RootActor = this;
            }
            else
            {
                child.RootActor = this.RootActor;
            }
        }

        public List<GameActor> GetChildActors()
        {
            return ChildActors;
        }

        public void Jump()
        {
            if (InAir)
                return;

            if (!ParentLevel.CanActorMoveTo(this, this.Position + this.Velocity + 2f * JumpVector))
                return;

            InAir = true;
            this.Velocity += JumpVector;
            this.Position += JumpVector;
        }

        public IActorPhysics PhysicsComponent
        {
            set
            {
                physicsComponent = value;
                physicsComponent.SetActor(this);
            }

            get { return physicsComponent; }
        }

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
            if (!canInit)
                return;

            if (Velocity == null)
                Velocity = new Vector2(0f, 0f);

            if (position == null)
                position = new Vector2(0f, 0f);

            if (useTileSheetManager)
            {
                tileSheet.Init();

                if (BoundingShape == BoundingShape.BoundingBox)
                {
                    if (!boundingBoxSet)
                        BoundingBox = new RectangleF(0, 0, 
                            (tileSheet.SpriteWidth - tileSheet.SpriteBuffer.X - tileSheet.SpriteBuffer.Width) * Scale.X, 
                            (tileSheet.SpriteHeight - tileSheet.SpriteBuffer.Y - tileSheet.SpriteBuffer.Height) * Scale.Y);

                    boundingBoxSet = true;
                }
            }
            else
            {
                Texture = ContentLoader.LoadTexture(this.textureName);

                if (Scale == null)
                    Scale = new Vector2(1f, 1f);

                // If it's a bounding box (the default value)
                //  then the box starts with 0, 0 to width and height
                if (BoundingShape == BoundingShape.BoundingBox)
                {
                    if (!boundingBoxSet)
                        BoundingBox = new RectangleF(0, 0, Texture.Width * Scale.X, Texture.Height * Scale.Y);

                    boundingBoxSet = true;
                }

                // If the shape is that of a circle, then, we assume the texture width and height are the same
                //  also, we set the radius to the width times scale.
                else if (BoundingShape == BoundingShape.BoundingCircle)
                {
                    if (!boundingBoxSet)
                        BoundingRadius = Texture.Width * Scale.X;

                    boundingBoxSet = true;
                }

                // MOstly used in case of a circle as a bounding box
                if (Center == null)
                    Center = new Vector2(position.X + (Texture.Width * Scale.X) / 2f, position.Y + (Texture.Height * Scale.Y) / 2f);
            }

            // Finally Init the children
            ChildActors.ForEach(a =>
            {
                if (a == null)
                    return;

                if (a.CanInit)
                    a.Init();
            });
            
        }

        public virtual void Tick()
        {
            position += Velocity;
            Center += Velocity;

            // Finally Tick the children
            ChildActors.ForEach(a =>
            {
                if (a == null)
                    return;

                a.Tick();
            });

            if(PawnSensing != null)
            {
                PawnSensing.CheckPawn();
            }
        }

        public virtual void Render()
        {
            if(useTileSheetManager)
            {
                tileSheet.Render(position, Scale);
            }
            else
            {
                SpriteBatch.Draw(Texture, position, Scale, Color.White, Vector2.Zero);
            }

            // Finally Render the children
            ChildActors.ForEach(a =>
            {
                if (a == null)
                    return;

                a.Render();
            });
        }

        public virtual void onHit(GameActor otherActor)
        {
            // handle post collission code here..
            // Only handle code related to this Actor.
            // The code to worry about otherActor should be written in that class and NOT here.           
        }

    }
}
