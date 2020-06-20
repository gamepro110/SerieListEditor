using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie_List_Editor
{
    [Serializable]
    internal class SaveDataJson
    {
        public SaveDataJson()
        {
        }

        public SaveDataJson(bool meh)
        {
            if (meh)
            {
                Title = new List<string>(20);
                Episode = new List<int?>(20);
                Season = new List<int?>(20);
                Note = new List<string>(20);
            }
        }

        public List<string> Title;
        public List<int?> Episode;
        public List<int?> Season;
        public List<string> Note;

        public void AddNewEntry(string title = "Title", int season = 1, int episode = 1, string note = "Empty note")
        {
            if (Title == null)
            {
                Title = new List<string>(20);
                Episode = new List<int?>(20);
                Season = new List<int?>(20);
                Note = new List<string>(20);
            }

            Title.Add(title);
            Season.Add(season);
            Episode.Add(episode);
            Note.Add(note);
        }

        public void RemoveEntry(int index)
        {
            if (Title.Count >= 0)
            {
                Title.RemoveAt(index);
                Season.RemoveAt(index);
                Episode.RemoveAt(index);
                Note.RemoveAt(index);
            }
        }
    }
}