using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tutor1.Models.Enum
{
    public enum ErrorCode
    {
        TodoItemNameAndNotesRequired,
        TodoItemIDInUse,
        RecordNotFound,
        CouldNotCreateItem,
        CouldNotUpdateItem,
        CouldNotDeleteItem
    }
}
