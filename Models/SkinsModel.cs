using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValorantTGBot.Models
{
    public class SkinsModel
    {
        public string WeaponName { get; set; }
        public List<Skins> WeaponSkins { get; set; }
    }

    public class Skins
    {
        public string DisplayName { get; set; }
        public string DisplayIcon { get; set; }
    }
}
