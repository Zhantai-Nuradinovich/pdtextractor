using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pdftextractor.Interfaces
{
    public interface IOperationStrategy
    {
        int Id { get; }
        bool AddToDb();
    }
}
