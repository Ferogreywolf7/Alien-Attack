using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
namespace Alien_Attack
{
    //All textures are being loaded here so they can be accessed by the rest of the progrma
    internal class TextureDirectory

    {
        private static Texture2D player1Texture;
        private static Texture2D playerBulletTexture;
        private static Texture2D textBorder;
        private static Texture2D bunker;
        private static Texture2D enemyTexture;
        private static Texture2D enemyBulletTexture;
        private static SpriteFont font;


        public TextureDirectory(Texture2D textBorderTemp, Texture2D playerBulletTextureTemp, Texture2D player1TextureTemp, Texture2D bunkerTemp, Texture2D enemyTextureTemp, Texture2D enemyBulletTextureTemp, SpriteFont fontTemp)
        {

            player1Texture = player1TextureTemp;
            playerBulletTexture = playerBulletTextureTemp;
            textBorder = textBorderTemp;
            bunker = bunkerTemp;
            enemyTexture = enemyTextureTemp;
            enemyBulletTexture = enemyBulletTextureTemp;
        }


        public static Texture2D getPlayerTexture() {
            return player1Texture;
        }
        public static Texture2D getEnemyBulletTexture() {
            return enemyBulletTexture;
        }
    }
}
