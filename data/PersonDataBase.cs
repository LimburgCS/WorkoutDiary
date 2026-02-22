using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutDiary.Model;

namespace WorkoutDiary.data
{
    public class PersonDataBase
    {
        SQLiteAsyncConnection Database;

        public PersonDataBase()
        {
        }

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(PersonConstants.DatabasePath, PersonConstants.Flags);
            var result = await Database.CreateTableAsync<Person>();
        }

        public async Task<List<Person>> GetPersonAsync()
        {
            await Init();
            return await Database.Table<Person>().ToListAsync();
        }
        public async Task<Person> GetPersonIDAsync(int id)
        {
            await Init();
            return await Database.Table<Person>().FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<int> SavePersonAsync(Person person)
        {
            await Init();
            return await Database.InsertAsync(person);
        }

        public async Task<int> UpdatePersonAsync(Person person)
        {
            await Init();
            return await Database.UpdateAsync(person);
        }
        public async Task<int> DeleteInvoiceAllAsync()
        {
            await Init();
            return await Database.DeleteAllAsync<Person>();
        }

        public async Task SaveNotesAsync(string text)
        {
            await Init();

            // pobierz istniejącą notatkę
            var existing = await Database.Table<Person>().FirstOrDefaultAsync();

            if (existing == null)
            {
                // tworzymy nową
                var note = new Person
                {
                    Notes = text,
                };

                await Database.InsertAsync(note);
            }
            else
            {
                // aktualizujemy istniejącą
                existing.Notes = text;

                await Database.UpdateAsync(existing);
            }
        }

        public async Task<Person> LoadNotes()
        {
            await Init();

            return await Database.Table<Person>().FirstOrDefaultAsync();
        }


    }
}
