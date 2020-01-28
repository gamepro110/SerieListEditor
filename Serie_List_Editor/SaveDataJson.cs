using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie_List_Editor
{
    [Serializable]
    internal struct SaveDataJson
    {
        public List<string> Title;
        public List<int?> Episode;
        public List<int?> Season;
        public List<string> Note;

        public void AddNewEntry(string title = "Title", int season = 0, int episode = 0, string note = "Empty note")
        {
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