using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BeloteClient
{
    public class TablesList
    {
        private List<Table> tables;

        public TablesList()
        {
            tables = new List<Table>();
        }

        // Индексатор - обращение к столам по идентификатору
        public Table this[int ID]
        {
            get
            {
                return tables.Find(t => (t.ID == ID));
            }
        }

        // Метод добавления стола
        public void AddTable(Table table)
        {
            tables.Add(table);
        }

        public Table GetTableAt(int Index)
        {
            return tables[Index];
        }

        // Метод удаления игрового стола по его идентификатору
        public void DeleteTable(int ID)
        {
            this.DeleteTable(this[ID]);
        }

        // Метод удаления игрового стола по ссылке на объект
        public void DeleteTable(Table table)
        {
            if (table != null)
            {
                tables.Remove(table);
            }
        }

        public void Clear()
        {
            tables.Clear();
        }

        // Количество столов в списке
        public int Count
        {
            get
            {
                return tables.Count;
            }
        }
    }
}
