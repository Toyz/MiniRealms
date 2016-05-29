using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace MiniRealms.Engine.Audio.Music
{
    public class GameSong
    {
        //actual sound object
        private Song _song;

        public GameSong(ContentManager content, string fileName, bool isPhysicalPath = false)
        {
            _song = isPhysicalPath ? content.Load<Song>(fileName) : content.Load<Song>("Music/" + fileName);
        }

        public void Play()
        {
            MediaPlayer.Play(_song);
        }

        public void Stop()
        {
           MediaPlayer.Stop();
        }

        public void Unload()
        {
            _song = null;
        }

    }
}
