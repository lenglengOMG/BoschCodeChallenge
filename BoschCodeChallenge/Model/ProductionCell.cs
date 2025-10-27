using System.Collections.Generic;

namespace BoschCodeChallenge
{
    public class ProductionCell
    {
        private readonly string _searialNumber;
        private readonly string _cellName;
        private List<ProductionCell> _subCellsUnderCurrentCell = new List<ProductionCell>();

        public string SearialNumber { get { return _searialNumber; } }
        public string CellName { get { return _cellName; } }

        public ProductionCell(string serialNumber, string cellName)
        {
            this._searialNumber = serialNumber;
            this._cellName = cellName;
        }

        public List<ProductionCell> GetSubCellsUnderCurrentCell()
        {
            List<ProductionCell> cells = new List<ProductionCell>();
            cells.AddRange(_subCellsUnderCurrentCell);
            return cells;
        }

        public List<ProductionCell> GetAllSubCellsRecursively()
        {
            List<ProductionCell> cells = new List<ProductionCell>();
            foreach (var cell in _subCellsUnderCurrentCell)
            {
                cells.Add(cell);
                cells.AddRange(cell.GetAllSubCellsRecursively());
            }
            return cells;
        }

        public void AddSubCellToCurrentCell(ProductionCell cell)
        {
            _subCellsUnderCurrentCell.Add(cell);
        }

        public void RemoveSubCellFromCurrentCell(ProductionCell cell)
        {
            _subCellsUnderCurrentCell.Remove(cell);
        }
    }
}
