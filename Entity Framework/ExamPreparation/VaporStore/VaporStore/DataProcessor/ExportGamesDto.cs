using System;
using System.Collections.Generic;
using System.Text;

namespace VaporStore.DataProcessor
{
    public class ExportGamesDto
    {
        //        s.Id,
        //Title = s.Name,
        //Developer = s.Developer.Name,
        //Tags = string.Join(", ", s.GameTags.Select(t => t.Tag.Name).ToList()),
        //Player = s.Purchases.Count()

        public int Id { get; set; }

        public string Title { get; set; }

        public string Developer { get; set; }

        public string Tags { get; set; }

        public int Players { get; set; }
    }
}
