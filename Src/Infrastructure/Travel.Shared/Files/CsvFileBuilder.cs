using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel.Application.Common.Interfaces;
using Travel.Application.TourLists.Queries.ExportTours;
using CsvHelper;
using System.Globalization;

namespace Travel.Shared.Files
{
    class CsvFileBuilder : ICsvFileBuilder
    {
        public byte[] BuildTourPackageFile(IEnumerable<TourPackageRecord> tourPackageRecords)
        {
            using var ms = new MemoryStream();
            using var sw = new StreamWriter(ms);

            using var csvWriter = new CsvWriter(sw, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(tourPackageRecords);

            return ms.ToArray();
        }
    }
}
