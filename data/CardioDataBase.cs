using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutDiary.Model;

namespace WorkoutDiary.data
{
    public class CardioDataBase
    {
        SQLiteAsyncConnection Database;

        public CardioDataBase()
        {
        }

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(PersonConstants.DatabasePath, CardioConstants.Flags);
            var result = await Database.CreateTableAsync<Cardio>();
        }

        public async Task<List<Cardio>> GetCardioAsync()
        {
            await Init();
            return await Database.Table<Cardio>().ToListAsync();
        }
        public async Task<Cardio> GetCardioIDAsync(int id)
        {
            await Init();
            return await Database.Table<Cardio>().FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<int> SaveCardioAsync(Cardio cardio)
        {
            await Init();
            return await Database.InsertAsync(cardio);
        }

        public async Task<int> UpdateCardioAsync(Cardio cardio)
        {
            await Init();
            return await Database.UpdateAsync(cardio);
        }
        public async Task<int> DeleteCardioAsync(Cardio cardio)
        {
            await Init();
            return await Database.DeleteAsync(cardio);
        }
        public async Task<int> DeleteCardioAllAsync()
        {
            await Init();
            return await Database.DeleteAllAsync<Cardio>();
        }
    }
}
