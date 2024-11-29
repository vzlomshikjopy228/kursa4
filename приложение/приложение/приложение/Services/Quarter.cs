using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace приложение.Services
{
    public enum Quarter
    {
        [Description("Первая четверть")]
        First,
        [Description("Вторая четверть")]
        Second,
        [Description("Третья четверть")]
        Third,
        [Description("Четвертая четверть")]
        Fourth
    }
}
