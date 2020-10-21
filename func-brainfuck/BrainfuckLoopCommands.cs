using System.Collections.Generic;

namespace func.brainfuck
{
	public class BrainfuckLoopCommands
	{
		private static Dictionary<int, int> FindBrackets(IVirtualMachine vm)
		{
			var brackets = new Dictionary<int, int>();
			var stack = new Stack<int>();

			for (var i = 0; i < vm.Instructions.Length; i++)
			{
				var bracket = vm.Instructions[i];
				switch (bracket)
				{
					case '[':
						stack.Push(i);
						break;

					case ']':
						brackets.Add(stack.Peek(), i);
						brackets.Add(i, stack.Pop());
						break;
				}
			}

			return brackets;
		}

		public static void RegisterTo(IVirtualMachine vm)
		{
			var brackets = FindBrackets(vm);

			vm.RegisterCommand('[', machine =>
			{
				if (machine.Memory[machine.MemoryPointer] == 0)
					machine.InstructionPointer = brackets[machine.InstructionPointer];
			});

			vm.RegisterCommand(']', machine =>
			{
				if (machine.Memory[machine.MemoryPointer] != 0)
					machine.InstructionPointer = brackets[machine.InstructionPointer];
			});
		}
	}
}