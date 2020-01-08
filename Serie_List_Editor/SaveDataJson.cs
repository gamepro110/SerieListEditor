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
            Title = new List<string>();
            Season = new List<int?>();
            Episode = new List<int?>();
            Note = new List<string>();
        }

        public List<string> Title;
        public List<int?> Episode;
        public List<int?> Season;
        public List<string> Note;

        public void AddNewEntry(string title, int? season, int? episode, string note)
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