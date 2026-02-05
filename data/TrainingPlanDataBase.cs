using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutDiary.Model;

namespace WorkoutDiary.data
{
    public class TrainingPlanDataBase
    {
        SQLiteAsyncConnection Database;

        public TrainingPlanDataBase()
        {
        }

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(TrainingPlanConstans.DatabasePath, TrainingPlanConstans.Flags);
            var result = await Database.CreateTableAsync<TrainingPlan>();
        }

        public async Task<List<TrainingPlan>> GetTrainingAsync()
        {
            await Init();
            return await Database.Table<TrainingPlan>().ToListAsync();
        }
        public async Task<TrainingPlan> GetTrainingIDAsync(int id)
        {
            await Init();
            return await Database.Table<TrainingPlan>().FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<int> SaveTrainingAsync(TrainingPlan training)
        {
            await Init();
            return await Database.InsertAsync(training);
        }

        public async Task<int> UpdateTrainingAsync(TrainingPlan training)
        {
            await Init();
            return await Database.UpdateAsync(training);
        }

        public async Task<int> DeletePlanAsync(TrainingPlan training)
        {
            await Init();
            return await Database.DeleteAsync(training);
        }
        public async Task<int> DeleteTraingingAllAsync()
        {
            await Init();
            return await Database.DeleteAllAsync<TrainingPlan>();
        }
    }
}
