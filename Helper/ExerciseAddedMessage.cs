using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutDiary.Helper
{
    public class ExerciseAddedMessage : ValueChangedMessage<int>
    {
        public ExerciseAddedMessage(int numberWeek) : base(numberWeek) { }
    }

}
