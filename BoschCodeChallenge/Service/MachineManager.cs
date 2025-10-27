using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace BoschCodeChallenge
{
    /// <summary>
    /// Register as singleton to manage all machines
    /// 1. Communicate with MessageRouter to receive/send messages
    /// 2. Manage machine states and production cells
    /// 3. Provide methods to start/stop/maintain machines and cells
    /// 4. Communicate with data source
    /// A kind of adapter pattern
    /// </summary>
    public class MachineManager
    {
        private readonly List<Machine> _machines = new List<Machine>();
        private readonly Dictionary<ProductionCell, List<Machine>> _cellMachineMapping = new Dictionary<ProductionCell, List<Machine>>();
        private readonly List<Part> _parts = new List<Part>();

        private const string StatusQueueName = "machine_status_queue";
        private const string StatusRoutingKey = "machine.status";
        private const string PartQueueName = "machine_part_queue";
        private const string PartRoutingKey = "machine.part";

        public MachineManager()
        {
            // Subscribe to messages from all machines
            MessageRouter.Instance.SubScribeStatus(StatusQueueName, StatusRoutingKey, HandleStatus);
            MessageRouter.Instance.SubScribeStatus(PartQueueName, PartRoutingKey, HandlePart);
            LoadMachines();
            LoadProductionCells();
        }

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

        public void LoadMachines()
        {
            _machines.Clear();
            // Load machines from data source
            Machine fakeMachine = new Machine("Machine_001", new List<PartType> { PartType.Gear });
            fakeMachine.SetOperationState(new IdleState(fakeMachine, this));
            _machines.Add(fakeMachine);
        }

        public void LoadProductionCells()
        {
            _cellMachineMapping.Clear();
            // Load production cells and map machines to cells
            ProductionCell fakeCell = new ProductionCell("Cell_001", "Assembly Cell 1");
            _cellMachineMapping.Add(fakeCell, new List<Machine> { _machines[0] });
        }

        public void SaveMachines()
        {
            // Save machines to data source
        }

        public void SaveProductionCells()
        {
            // Save production cells to data source
        }

        public IEnumerable<Machine> GetAllMachines()
        {
            List<Machine> machines = new List<Machine>();
            foreach (var machine in _machines) {
                machines.Add(machine);
            }
            return machines;
        }

        public IEnumerable<Machine> GetMachinesUnderCurrentCell(ProductionCell cell)
        {
            List<Machine> machines = new List<Machine>();
            if (_cellMachineMapping.ContainsKey(cell))
            {
                machines.AddRange(_cellMachineMapping[cell]);
            }
            return machines;
        }

        public IEnumerable<Machine> GetAllMachinesByCellRecursively(ProductionCell cell)
        {
            List<Machine> machines = new List<Machine>();
            if (_cellMachineMapping.ContainsKey(cell))
            {
                machines.AddRange(_cellMachineMapping[cell]);
            }
            var subCells = cell.GetAllSubCellsRecursively();
            foreach (var subCell in subCells)
            {
                if (_cellMachineMapping.ContainsKey(subCell))
                {
                    machines.AddRange(_cellMachineMapping[subCell]);
                }
            }
            return machines;
        }

        public bool AddMachineToCell(Machine machine, ProductionCell cell)
        {
            if (!_cellMachineMapping.ContainsKey(cell))
            {
                _cellMachineMapping[cell] = new List<Machine>();
            }
            if (!_cellMachineMapping[cell].Contains(machine))
            {
                _cellMachineMapping[cell].Add(machine);
                return true;
            }
            return false;
        }

        public bool RemoveMachineFromCell(Machine machine, ProductionCell cell)
        {
            if (_cellMachineMapping.ContainsKey(cell) && _cellMachineMapping[cell].Contains(machine))
            {
                _cellMachineMapping[cell].Remove(machine);
                return true;
            }
            return false;
        }

        public bool RemoveCell(ProductionCell cell)
        {
            return _cellMachineMapping.Remove(cell);
        }

        public bool AddCell(ProductionCell cell)
        {
            if (!_cellMachineMapping.ContainsKey(cell))
            {
                _cellMachineMapping[cell] = new List<Machine>();
                return true;
            }
            return false;
        }

        public bool StartCell(ProductionCell cell, PartType partType)
        {
            if (!_cellMachineMapping.ContainsKey(cell))
            {
                return false;
            }

            var machines = GetAllMachinesByCellRecursively(cell);

            var result = true;
            foreach (var machine in machines)
            {
                if (!machine.Start(partType))
                {
                    result = false;
                }
            }

            return result;
        }

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
