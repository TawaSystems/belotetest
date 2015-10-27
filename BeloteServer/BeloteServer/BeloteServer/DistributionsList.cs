using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteServer
{
    // Класс - контейнер для списка игровых раздач
    class DistributionsList
    {
        private List<Distribution> list;

        public DistributionsList()
        {
            list = new List<Distribution>();
        }

        public void AddNew()
        {
            list.Add(new Distribution());
        }

        // Ссылка на текущую раздачу
        public Distribution Current
        {
            get
            {
                if (list.Count == 0)
                    return null;
                return list[list.Count - 1];
            }
        }

        public int Count
        {
            get
            {
                return list.Count;
            }
        }

        // Очки первой команды
        public int ScoresTeam1
        {
            get
            {
                int Res = 0;
                foreach (Distribution d in list)
                {
                    if (d.Status == DistributionStatus.D_ENDED)
                        Res += d.ScoresTeam1;
                }
                return Res;
            }
        }

        // Очки второй команды
        public int ScoresTeam2
        {
            get
            {
                int Res = 0;
                foreach (Distribution d in list)
                {
                    if (d.Status == DistributionStatus.D_ENDED)
                        Res += d.ScoresTeam2;
                }
                return Res;
            }
        }
    }
}
