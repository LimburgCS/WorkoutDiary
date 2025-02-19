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
    }
}
