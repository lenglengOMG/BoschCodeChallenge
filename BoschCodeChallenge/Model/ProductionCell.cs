using System.Collections.Generic;

namespace BoschCodeChallenge
{
    public class ProductionCell
    {
        private readonly List<ProductionCell> _subCellsUnderCurrentCell = new List<ProductionCell>();

        public string SearialNumber { get; private set; }
        public string CellName { get; private set; }

        public ProductionCell(string serialNumber, string cellName)
        {
            SearialNumber = serialNumber;
            CellName = cellName;
        }

        /// <summary>
        /// Get sub cells under current cell
        /// </summary>
        /// <returns></returns>
        public List<ProductionCell> GetSubCellsUnderCurrentCell()
        {
            List<ProductionCell> cells = new List<ProductionCell>();
            cells.AddRange(_subCellsUnderCurrentCell);
            return cells;
        }

        /// <summary>
        /// Get all sub cells recursively
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Add sub cell to current cell
        /// </summary>
        /// <param name="cell"></param>
        public void AddSubCellToCurrentCell(ProductionCell cell)
        {
            _subCellsUnderCurrentCell.Add(cell);
        }

        /// <summary>
        /// Remove sub cell from current cell
        /// </summary>
        /// <param name="cell"></param>
        public void RemoveSubCellFromCurrentCell(ProductionCell cell)
        {
            _subCellsUnderCurrentCell.Remove(cell);
        }
    }
}
