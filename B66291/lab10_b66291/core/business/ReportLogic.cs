
using core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace core.Business{
public sealed class ReportLogic
{
    public ReportLogic() { }

   public Ienumerable<Report>> TransformarDatos(List<Report> sales) 
   {
        if (sales == null)
        {
            throw new ArgumentNullException(nameof(sales), "Las ventas estan vacias");
        }

        List<Report> responseData = new List<Report();

        foreach (var report in sales)
        {
            if (report.purchaseNumber != null && report.purchase_date != null && report.total != null)
            {
                var data = new
                {
                    purchaseNumber = report.purchaseNumber,
                    purchaseDate = report.purchase_date,
                    total = report.total
                };

                responseData.Add(data);
            }
            else
            {
                throw new ArgumentException("Uno o mas campos requeridos estan vacios");
            }
        }

        return responseData;
    }
}
}