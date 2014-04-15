using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;

public interface ILaserSportDataRepositoryDatasSource
{
}

public interface ILaserSportDataRepositoryWebSource : ILaserSportDataRepositoryDatasSource
{
    string URL { get; set; }
}
