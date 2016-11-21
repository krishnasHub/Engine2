using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using Engine2.Texture;
using Engine2.Core;
using System.Xml;

namespace Engine2.Util
{
    class ContentLoader
    {
        public static string RootFolder = "Content";
        public static Texture2D LoadTexture(string path)
        {
            if(!File.Exists(RootFolder + "/" + path))
            {
                throw new IOException("File not found: " + RootFolder + "/" + path);
            }

            int id = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, id);
            Bitmap bmp = new Bitmap(RootFolder + "/" + path);

            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bmp.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            //GL.End();

            return new Texture2D(id, bmp.Width, bmp.Height);
        }

        public static Level LoadLevel(string path)
        {

            var filePath = RootFolder + "/" + path;
            Level level;

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fs);

                int w = int.Parse(doc.DocumentElement.GetAttribute("width"));
                int h = int.Parse(doc.DocumentElement.GetAttribute("height"));

                level = new Level(w, h, filePath);

                XmlNode tileLayer = doc.DocumentElement.SelectSingleNode("layer[@name='Tile Layer 1']");
                var tiles = tileLayer.SelectSingleNode("data").InnerText.Trim().Split(',');

                // just added this block.. nothing to it. Please ignore that I did..
                {
                    int i = 0;
                    for (int y = 0; y < h; ++y)
                        for (int x = 0; x < w; x++)
                        {
                            int gid = int.Parse(tiles[i]);
                            level.SetBlock(x, y, gid);
                            i++;
                        }
                }

                XmlNode objLayer = doc.DocumentElement.SelectSingleNode("objectgroup[@name='Object Layer 1']");
                var objects = objLayer.SelectNodes("object");
                
                 
                for(int i = 0; i < objects.Count; ++i)
                {
                    int xPos = (int)float.Parse(objects[i].Attributes["x"].Value);
                    int yPos = (int)float.Parse(objects[i].Attributes["y"].Value);
                    string objName = objects[i].Attributes["name"].Value;

                    switch(objName)
                    {
                        case "playerStartPos":
                            level.PlayerStartPos = new Point((int)(xPos / (float)Constants.TILE_SIZE), (int)(yPos / (float)Constants.TILE_SIZE));
                            break;

                        default:
                            break;
                    }
                }           

            }

            return level;
        }
    }
}
