using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValorantTGBot.Models
{
    public class WeaponsModel
    {
        public string WeaponName { get; set; }
        public string Photo { get; set; }
        public string FireRate { get; set; }
        public string MagazineSize { get; set; }
        public string RunSpeedMultiplier { get; set; }
        public string EquipTimeSeconds { get; set; }
        public string ReloadTimeSeconds { get; set; }
        public List<DamageRanges> Damage { get; set; }
        public string Cost { get; set; }
        public string WeaponType { get; set; }
    }

    public class DamageRanges
    {
        public string RangeStartMeters { get; set; }
        public string RangeEndMeters { get; set; }
        public string HeadDamage { get; set; }
        public string BodyDamage { get; set; }
        public string LegDamage { get; set; }
    }
}
