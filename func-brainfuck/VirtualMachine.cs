using System;
using System.Collections.Generic;

namespace func.brainfuck
{
	public class VirtualMachine : IVirtualMachine
	{
		public VirtualMachine(string program, int memorySize)
		{
			Instructions = program;
			Memory = new byte[memorySize];
		}

		public void RegisterCommand(char symbol, Action<IVirtualMachine> execute)
		{
			registredCommands.Add(symbol, execute);
		}

		public string Instructions { get; }
		public int InstructionPointer { get; set; }
		public byte[] Memory { get; }
		public int MemoryPointer { get; set; }
		private Dictionary<char, Action<IVirtualMachine>> registredCommands = 
			new Dictionary<char, Action<IVirtualMachine>>();

		public void Run()
		{
			while (InstructionPointer < Instructions.Length)
			{
				var command = Instructions[InstructionPointer];
				if (registredCommands.ContainsKey(command))
					registredCommands[command](this);
				InstructionPointer++;
			}
		}
	}
}