using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(fileName = "New Team", menuName = "Gameplay/Team", order = 1)]
    public class Team : ScriptableObject
    {
        [SerializeField] private List<Team> alliedTeams;
        [SerializeField] private List<Team> enemyTeams;
    
        public List<Team> AlliedTeams => alliedTeams;
        public List<Team> EnemyTeams => enemyTeams;

        public bool IsAlly(Team team) => AlliedTeams.Contains(team) || team == this;
        public bool IsEnemy(Team team) => EnemyTeams.Contains(team);
    }
}