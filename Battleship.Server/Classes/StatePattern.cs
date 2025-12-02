using System.Security.Cryptography.X509Certificates;

namespace Battleship.Server.Classes
{
    public interface PlayerRankState
    {
        public List<int> getWeapons();
    }

    public class RecruitState
    {
        public List<int> getWeapons()
        {
            List<int> WeaponTypes = new List<int>();
            WeaponTypes.Add(0);
            return WeaponTypes;
        }
    }
    public class SailorState
    {
        public List<int> getWeapons()
        {
            List<int> WeaponTypes = new List<int>();
            WeaponTypes.Add(0);
            WeaponTypes.Add(1);
            return WeaponTypes;
        }
    }
    public class CaptainState
    {
        public List<int> getWeapons()
        {
            List<int> WeaponTypes = new List<int>();
            WeaponTypes.Add(0);
            WeaponTypes.Add(1);
            WeaponTypes.Add(2);
            return WeaponTypes;
        }
    }
    public class CommanderState
    {
        public List<int> getWeapons() {
            List<int> WeaponTypes = new List<int>();
            WeaponTypes.Add(0);
            WeaponTypes.Add(1);
            WeaponTypes.Add(2);
            WeaponTypes.Add(3);
            return WeaponTypes;
        }
    }
}
