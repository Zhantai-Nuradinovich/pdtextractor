using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pdftextractor.Data.Models
{
    public enum Decision
    {
        Absent,
        Agreed,
        Rejected,
        Initiator
    }
}