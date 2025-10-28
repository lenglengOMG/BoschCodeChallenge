using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace BoschCodeChallenge
{
    /// <summary>
    /// Register as singleton to manage all machines
    /// 1. Communicate with MessageRouter to receive/send messages
    /// 2. Manage machine, production cells and parts processed
    /// 3. Provide methods to start/stop/maintain machines and cells
    /// 4. Communicate with data source
    /// </summary>
    public class MachineManager
    {
        private readonly Dictionary<ProductionCell, List<string>> _cellMachineMapping = new Dictionary<ProductionCell, List<string>>();
        private readonly List<Machine> _machines = new List<Machine>();
        private readonly List<Part> _parts = new List<Part>();

        private const string StatusQueueName = "machine_status_queue";
        private const string StatusRoutingKey = "machine.status";
        private const string PartQueueName = "machine_part_queue";
        private const string PartRoutingKey = "machine.part";

        public MachineManager()
        {
            // Load machines and production cells from data source
            LoadMachines();
            LoadProductionCells();

            // Subscribe to messages from all machines
            MessageRouter.Instance.SubScribeStatus(StatusQueueName, StatusRoutingKey, HandleStatus);
            MessageRouter.Instance.SubScribePart(PartQueueName, PartRoutingKey, HandlePart);
        }

        /// <summary>
        /// Handle machine status message from machine
        /// </summary>
        /// <param name="message"></param>
        private void HandleStatus(string message)
        {
            // Handle status message from machine
            // Parse message and update machine state accordingly
            var statusMessage = JsonConvert.DeserializeObject<StatusMessage>(message);
            if (statusMessage != null)
            {
                var machineId = statusMessage.MachineId;
                var status = statusMessage.State;
                var machine = _machines.FirstOrDefault(m => m.MachineId == machineId);
                if (machine != null)
                {
                    switch (status)
                    {
                        case StateType.Running:
                            machine.SetOperationState(new RunningState(machine, this));
                            break;
                        case StateType.Idle:
                            machine.SetOperationState(new IdleState(machine, this));
                            break;
                        case StateType.UnderMaintenance:
                            machine.SetOperationState(new UnderMantainenceState(machine, this));
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Handle part production message from machine
        /// </summary>
        /// <param name="message"></param>
        private void HandlePart(string message)
        {
            // Handle part message from machine
            // Parse message and update part info accordingly
            // The part info should include production completion time.
            var partProductionInfo = JsonConvert.DeserializeObject<PartProductionMessage>(message);
            if (partProductionInfo != null)
            {
                _parts.Add(partProductionInfo.PartInfo);
                // Save part info to database or perform other actions
            }
        }

        /// <summary>
        /// Load machines from data source
        /// </summary>
        public void LoadMachines()
        {
            _machines.Clear();
            // Load machines from data source
            Machine fakeMachine = new Machine("Machine_001", new List<PartType> { PartType.Gear });
            fakeMachine.SetOperationState(new IdleState(fakeMachine, this));
            _machines.Add(fakeMachine);
        }

        /// <summary>
        /// Load production cells from data source
        /// </summary>
        public void LoadProductionCells()
        {
            _cellMachineMapping.Clear();
            // Load production cells and map machines to cells
            ProductionCell fakeCell = new ProductionCell("Cell_001", "Assembly Cell 1");
            _cellMachineMapping.Add(fakeCell, new List<string> { _machines[0].MachineId });
        }

        /// <summary>
        /// Save machines to data source
        /// </summary>
        public void SaveMachines()
        {
            // Save machines to data source
        }

        /// <summary>
        /// Save production cells to data source
        /// </summary>
        public void SaveProductionCells()
        {
            // Save production cells to data source
        }

        /// <summary>
        /// Get all registered machines
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Machine> GetAllMachines()
        {
            List<Machine> machines = new List<Machine>();
            foreach (var machine in _machines) {
                machines.Add(machine);
            }
            return machines;
        }

        /// <summary>
        /// Get machines under a specific production cell
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public IEnumerable<Machine> GetMachinesUnderCurrentCell(ProductionCell cell)
        {
            List<Machine> machines = new List<Machine>();
            if (_cellMachineMapping.ContainsKey(cell))
            {
                machines.AddRange(_machines.Where(m => _cellMachineMapping[cell].Contains(m.MachineId)));
            }
            return machines;
        }

        /// <summary>
        /// Get all machines under a production cell recursively
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public IEnumerable<Machine> GetAllMachinesByCellRecursively(ProductionCell cell)
        {
            List<Machine> machines = new List<Machine>();
            if (_cellMachineMapping.ContainsKey(cell))
            {
                machines.AddRange(_machines.Where(m => _cellMachineMapping[cell].Contains(m.MachineId)));
            }
            var subCells = cell.GetAllSubCellsRecursively();
            foreach (var subCell in subCells)
            {
                if (_cellMachineMapping.ContainsKey(subCell))
                {
                    machines.AddRange(_machines.Where(m => _cellMachineMapping[cell].Contains(m.MachineId)));
                }
            }
            return machines;
        }

        /// <summary>
        /// Add machine to a production cell
        /// </summary>
        /// <param name="machineId"></param>
        /// <param name="cell"></param>
        /// <returns></returns>
        public bool AddMachineToCell(string machineId, ProductionCell cell)
        {
            if (!_cellMachineMapping.ContainsKey(cell))
            {
                _cellMachineMapping[cell] = new List<string>();
            }
            if (!_cellMachineMapping[cell].Contains(machineId))
            {
                _cellMachineMapping[cell].Add(machineId);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remove machine from a production cell
        /// </summary>
        /// <param name="machineId"></param>
        /// <param name="cell"></param>
        /// <returns></returns>
        public bool RemoveMachineFromCell(string machineId, ProductionCell cell)
        {
            if (_cellMachineMapping.ContainsKey(cell) && _cellMachineMapping[cell].Contains(machineId))
            {
                _cellMachineMapping[cell].Remove(machineId);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remove a production cell
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public bool RemoveCell(ProductionCell cell)
        {
            return _cellMachineMapping.Remove(cell);
        }

        /// <summary>
        /// Add a production cell
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public bool AddCell(ProductionCell cell)
        {
            if (!_cellMachineMapping.ContainsKey(cell))
            {
                _cellMachineMapping[cell] = new List<string>();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Start all machines under a production cell recursively
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="partType"></param>
        /// <returns></returns>
        public bool StartCell(ProductionCell cell, IEnumerable<Part> parts)
        {
            if (!_cellMachineMapping.ContainsKey(cell))
            {
                return false;
            }

            var machines = GetAllMachinesByCellRecursively(cell);

            var result = true;
            foreach (var machine in machines)
            {
                if (!machine.Start(parts))
                {
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// Stop all machines under a production cell recursively
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public bool StopCell(ProductionCell cell)
        {
            if (!_cellMachineMapping.ContainsKey(cell))
            {
                return false;
            }

            var machines = GetAllMachinesByCellRecursively(cell);

            var result = true;
            foreach (var machine in machines)
            {
                if (!machine.Stop())
                {
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// Send instruction to a specific machine
        /// </summary>
        public void SendInstructionToMachine(InstructionMessage instructionMessage)
        {
            // contruct message
            // send message via rabbitmq client
            var message = JsonConvert.SerializeObject(instructionMessage);
            MessageRouter.Instance.SendInstruciton(message);
        }
    }
}
