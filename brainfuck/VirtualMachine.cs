using System;
using System.Collections.Generic;

namespace func.brainfuck
{
	public class VirtualMachine : IVirtualMachine
	{
		public string Instructions { get; }
		public int InstructionPointer { get; set; }
		public byte[] Memory { get; }
		public int MemoryPointer { get; set; }

		private Dictionary<char, Action<IVirtualMachine>> _commands;

		public VirtualMachine(string program, int memorySize)
		{
			Memory = new byte[memorySize];
			Instructions = program;
			_commands = new Dictionary<char, Action<IVirtualMachine>>();
        }

		public void RegisterCommand(char symbol, Action<IVirtualMachine> execute)
		{
			_commands.Add(symbol, execute);
		}

		public void Run()
		{
            while (InstructionPointer < Instructions.Length)
			{
				if (_commands.TryGetValue(Instructions[InstructionPointer], out Action<IVirtualMachine> command))
                    command(this);
				InstructionPointer += 1;
            }
		}
	}
}