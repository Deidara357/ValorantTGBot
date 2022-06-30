using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValorantTGBot.Models
{
    public class AgentsModel
    {

        public string AgentName { get; set; }
        public string Description { get; set; }
        public string FullPortrait { get; set; }
        public string Role { get; set; }
        public string RoleDescription { get; set; }
        public List<Abilities> AgentsAbilities { get; set; }

        public class Abilities
        {
            public string Slot { get; set; }
            public string DisplayName { get; set; }
            public string Description { get; set; }
        }
    }
}
