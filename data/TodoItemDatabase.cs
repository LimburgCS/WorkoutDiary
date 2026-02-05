using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutDiary.Model;

namespace WorkoutDiary.data
{
    public class TodoItemDatabase
    {
        SQLiteAsyncConnection Database;

        public TodoItemDatabase()
        {
        }

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result = await Database.CreateTableAsync<BodyParts>();
        }

        public async Task<List<BodyParts>> GetInvoiceAsync()
        {
            await Init();
            return await Database.Table<BodyParts>().ToListAsync();
        }
        public async Task<BodyParts> GetInvoiceIDAsync(int id)
        {
            await Init();
            return await Database.Table<BodyParts>().FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<int> SaveInvoiceAsync(BodyParts bodyParts)
        {
            await Init();
            return await Database.InsertAsync(bodyParts);
        }

        public async Task<int> UpdateInvoiceAsync(BodyParts bodyParts)
        {
            await Init();
            return await Database.UpdateAsync(bodyParts);
        }
        public async Task<int> DeleteInvoiceAsync(BodyParts bodyParts)
        {
            await Init();
            return await Database.DeleteAsync(bodyParts);
        }
        public async Task<int> DeleteInvoiceAllAsync()
        {
            await Init();
            return await Database.DeleteAllAsync<BodyParts>();
        }

        public async Task<List<List<BodyParts>>> GetLastTwoTrainingDaysAsync()
        {
            await Init();

            // 1. Pobierz WSZYSTKO bez żadnych operacji na DateTime
            var all = await Database.Table<BodyParts>().ToListAsync();

            if (all == null || all.Count == 0)
                return new List<List<BodyParts>>();

            // 2. Operacje na datach dopiero TERAZ, w C#
            var grouped = all
                .Where(x => x.DateTime != null)
                .GroupBy(x => x.DateTime!.Date)   // ← TERAZ .Date działa, bo to C#, nie SQL
                .OrderByDescending(g => g.Key)
                .Take(2)
                .Select(g => g.ToList())
                .ToList();

            return grouped;
        }



        public async Task<List<DateTime>> GetAllTrainingDaysAsync()
        {
            await Init();

            // 1. Pobierz WSZYSTKO bez operacji na DateTime
            var all = await Database.Table<BodyParts>().ToListAsync();

            // 2. Operacje na datach dopiero TERAZ, w C#
            return all
                .Where(x => x.DateTime != null)
                .GroupBy(x => x.DateTime.Date)   // ← TERAZ działa, bo to C#, nie SQL
                .Select(g => g.Key)
                .OrderByDescending(d => d)
                .ToList();
        }


    }
}
