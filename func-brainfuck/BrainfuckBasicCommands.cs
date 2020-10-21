using System;

namespace func.brainfuck
{
	public class BrainfuckBasicCommands
	{
		private static void WriteByte(IVirtualMachine vm, Action<char> write)
		{
			write((char)vm.Memory[vm.MemoryPointer]);
		}

		private static void ReadByte(IVirtualMachine vm, Func<int> read)
		{
			vm.Memory[vm.MemoryPointer] = (byte)read();
		}

		private static void ShiftMemoryPointerToLeft(IVirtualMachine vm)
		{
			vm.MemoryPointer = (vm.MemoryPointer - 1 + vm.Memory.Length) % vm.Memory.Length;
		}

		private static void ShiftMemoryPointerToRight(IVirtualMachine vm)
		{
			vm.MemoryPointer = (vm.MemoryPointer + 1) % vm.Memory.Length;
		}

		private static void IncrementByte(IVirtualMachine vm)
		{
			if (vm.Memory[vm.MemoryPointer] != 255) vm.Memory[vm.MemoryPointer]++;
			else vm.Memory[vm.MemoryPointer] = 0;
		}

		private static void DecrementByte(IVirtualMachine vm)
		{
			if (vm.Memory[vm.MemoryPointer] != 0) vm.Memory[vm.MemoryPointer]--;
			else vm.Memory[vm.MemoryPointer] = 255;
		}

		private static void RegisterConstant(IVirtualMachine vm, char c)
		{
			vm.RegisterCommand(c, m => m.Memory[m.MemoryPointer] = (byte)c);
		}

		private static void WriteAllSymbols(IVirtualMachine vm)
		{
			for (var i = '0'; i <= '9'; i++)
				RegisterConstant(vm, i);
			for (var i = 'a'; i <= 'z'; i++)
				RegisterConstant(vm, i);
			for (var i = 'A'; i <= 'Z'; i++)
				RegisterConstant(vm, i);
		}

		public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
		{
			vm.RegisterCommand('.', machine => WriteByte(machine, write));
			vm.RegisterCommand(',', machine => ReadByte(machine, read));
			vm.RegisterCommand('+', machine => IncrementByte(machine));
			vm.RegisterCommand('-', machine => DecrementByte(machine));
			vm.RegisterCommand('>', machine => ShiftMemoryPointerToRight(machine));
			vm.RegisterCommand('<', machine => ShiftMemoryPointerToLeft(machine));
			WriteAllSymbols(vm);
		}
	}
}