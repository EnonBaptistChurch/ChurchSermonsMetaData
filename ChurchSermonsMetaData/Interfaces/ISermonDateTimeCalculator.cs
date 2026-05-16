using ChurchSermonsMetaData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChurchSermonsMetaData.Interfaces
{
    public interface ISermonDateTimeCalculator
    {

        SermonServiceAndDate GetSermonServiceInfoAsync(FileInfo file);
    }
}
