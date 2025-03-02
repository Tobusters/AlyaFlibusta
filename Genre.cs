using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace WpfApp1
{
    class GENRES
    {
        private static GENRES instance;
        //Update updatebox;
        private Dictionary<string, string> genres;

        public void AddGenre(ref string ID, ref string Name)
        {
            genres.Add(ID, Name);
        }
        public void AddGenre(string ID, string Name)
        {
            genres.Add(ID, Name);
        }

        public void DeleteGenre(ref int GenreID) { }
        private GENRES()
        {
            genres = new Dictionary<string, string>();
        }

        public static GENRES getInstance()
        {
            if (instance == null)
                instance = new GENRES();
            return instance;
        }
        //public List<string> GetGenresNames()
        //{
        //    List<string> strings = new List<string> { };
        //    foreach (var genre in Genres)
        //    {
        //        strings.Add(genre.Name);
        //    }
        //    return strings;
        //}
        public string[] GetGenresNames()
        {
            string[] strings = new string[genres.Count];
            int i = 0;
            foreach (string s in genres.Values)
            {
                strings[i] = s;
                i++;
            }
            return strings;
        }

        public void SetGenres(Dictionary<string, string> data)
        {
            genres = data;
        }

        public string[] GetGenresID()
        {
            string[] strings = new string[0];
            foreach (string s in genres.Values)
            {
                strings.Append(s);
                ////var b = strings.Append(genres[i].Name);
                //strings = b.ToArray();
            }
            MessageBox.Show(strings.Length.ToString());

            return strings;
        }

        public Dictionary<string, string> GetGenresDict()
        {
            Dictionary<string, string> Toreturn = genres;
            return Toreturn;
        }
        public string GetGenresIDBy(ref string Name)
        {

            return "";
        }
        public string GetGenresNameBy(ref string ID)
        {
            return "";
        }
        public string this[string ID]
        {
            get => genres[ID];
        }
        public string this[int ID]
        {
            get => genres[ID.ToString()];
        }

    }

}

